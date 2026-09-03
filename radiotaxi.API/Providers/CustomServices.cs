using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using radiotaxi.Model;
using radiotaxi.Model.v2.Openpay;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace radiotaxi.API.Providers
{
    public class CustomServices
    {
        private static readonly HttpClient _http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };

        private readonly string _merchantId;
        private readonly string _privateKey;
        private readonly bool _production;

        public CustomServices()
        {
            _merchantId = ConfigurationManager.AppSettings["Openpay.MerchantId"];
            _privateKey = ConfigurationManager.AppSettings["Openpay.PrivateKey"];
            _production = bool.Parse(ConfigurationManager.AppSettings["Openpay.Production"] ?? "false");
        }

        private string BaseUrl
            => _production ? "https://api.openpay.mx" : "https://sandbox-api.openpay.mx";
        private AuthenticationHeaderValue BuildBasicAuth()
        {
            var basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(_privateKey + ":")); // user=privateKey, pass vacío
            return new AuthenticationHeaderValue("Basic", basic);
        }
        private OpenpayAPI CreateApi()
        {
            var api = new OpenpayAPI(_privateKey, _merchantId) { Production = _production };
            return api;
        }
        // -------------------- 1) ENSURE CUSTOMER --------------------

        /// Busca por external_id en Openpay (REST) y retorna el customer_id si existe (o null)
        public async Task<string> FindCustomerIdByExternalIdAsync(string externalId)
        {
            var url = string.Format("{0}/v1/{1}/customers?external_id={2}",
                                    BaseUrl, _merchantId, Uri.EscapeDataString(externalId));

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = BuildBasicAuth();

            using (var resp = await _http.SendAsync(req))
            {
                resp.EnsureSuccessStatusCode();
                var json = await resp.Content.ReadAsStringAsync();
                var arr = JArray.Parse(json);
                if (arr.Count > 0)
                {
                    return (string)arr[0]["id"];
                }
                return null;
            }
        }

        /// Idempotente: si existe el Customer (por external_id), lo devuelve; si no, lo crea y retorna su Id.
        public async Task<string> EnsureOpenpayCustomerAsync(
            string userId, string name, string lastName, string email, string phone,
            string countryCode = "MX", string city = null, string state = null,
            string line1 = null, string postalCode = null)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            // 1) intentar recuperar por external_id (REST)
            var found = await FindCustomerIdByExternalIdAsync(userId);
            if (!string.IsNullOrEmpty(found))
                return found;
            // 2) crear con SDK .NET
            var api = CreateApi();
            var customer = new Openpay.Entities.Customer
            {
                ExternalId = userId,
                Name = string.IsNullOrWhiteSpace(name) ? null : name,
                LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName,
                Email = string.IsNullOrWhiteSpace(email) ? null : email,
                PhoneNumber = string.IsNullOrWhiteSpace(phone) ? null : phone,
                RequiresAccount = false,
                Address = new Address
                {
                    CountryCode = string.IsNullOrWhiteSpace(countryCode) ? "MX" : countryCode,
                    City = city,
                    State = state,
                    Line1 = line1,
                    PostalCode = postalCode
                }
            };
            var created = api.CustomerService.Create(customer);
            return created.Id;
        }

        // -------------------- 2) SAVE CARD --------------------

        /// Guarda una tarjeta (One-Click) para un customer con token + device + address opcional.
        public async Task<CardSavedDto> SaveCardAsync(
            string customerId,
            string tokenId,
            string deviceSessionId,
            string email = null,
            string phone = null,
            AddressDto address = null)
        {

            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                var api = CreateApi();
                var req = new Card
                {
                    TokenId = tokenId,
                    DeviceSessionId = deviceSessionId,
                    Address = (address == null) ? null : new Address
                    {
                        Line1 = address.line1,
                        Line2 = address.line2,
                        Line3 = address.line3,
                        City = address.city,
                        State = address.state,
                        PostalCode = address.postal_code,
                        CountryCode = address.country_code
                    }
                };

                // SDK .NET es síncrono; envolvemos en Task.Run para evitar bloquear.
                var card = await Task.Run(() => api.CardService.Create(customerId, req));

                var last4 = (card.CardNumber != null && card.CardNumber.Length >= 4)
                    ? card.CardNumber.Substring(card.CardNumber.Length - 4)
                    : null;

                return new CardSavedDto
                {
                    Id = card.Id,
                    Brand = card.Brand,
                    Type = card.Type,
                    Last4 = last4
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // -------------------- 3) CREATE CHARGE --------------------

        /// Crea un cargo con tarjeta guardada (sourceId=card_id) o con un token_id directo.
        public async Task<ChargeResultDto> CreateChargeAsync(ChargeCreateDto dto)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var api = CreateApi();
            var req = new ChargeRequest
            {
                Method = "card",
                SourceId = dto.SourceId,                 // card_id guardada o token_id
                Amount = dto.Amount,
                Currency = string.IsNullOrWhiteSpace(dto.Currency) ? "MXN" : dto.Currency,
                Description = dto.Description,
                OrderId = dto.OrderId,
                DeviceSessionId = dto.DeviceSessionId
            };

            // Cargo a nombre del Customer (recomendado)
            var charge = await Task.Run(() => api.ChargeService.Create(dto.CustomerId, req));

            return new ChargeResultDto
            {
                ChargeId = charge.Id,
                Status = charge.Status,
                OperationType = charge.OperationType,
                TransactionType = charge.TransactionType,
                Authorization = charge.Authorization,
                CreationDate = charge.CreationDate,
                Amount = charge.Amount,
                Currency = dto.Currency,
                OrderId = charge.OrderId,
                Description = charge.Description
            };
        }


        /// <summary>
        /// Obtiene TODAS las tarjetas guardadas de un Customer en Openpay (paginando).
        /// </summary>
        /// <param name="customerId">Id del cliente en Openpay (acfw...)</param>
        /// <param name="pageSize">Tamaño de página para la API (default 100)</param>
        public async Task<List<CardListItemDto>> GetCustomerCardsAsync(string customerId, int pageSize = 100)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            if (string.IsNullOrWhiteSpace(customerId))
                throw new ArgumentException("customerId requerido.", "customerId");

            var api = CreateApi();
            var result = new List<CardListItemDto>();

            int offset = 0;
            while (true)
            {
                var sp = new SearchParams
                {
                    Limit = pageSize,
                    Offset = offset
                };

                // El SDK es sincrónico; lo envolvemos para no bloquear hilos de ASP.NET
                var page = await Task.Run(() => api.CardService.List(customerId, sp));

                if (page == null || page.Count == 0)
                    break;

                foreach (var c in page)
                {
                    var last4 = (c.CardNumber != null && c.CardNumber.Length >= 4)
                        ? c.CardNumber.Substring(c.CardNumber.Length - 4)
                        : null;

                    result.Add(new CardListItemDto
                    {
                        Id = c.Id,
                        customer_id = customerId,
                        Brand = c.Brand,
                        Type = c.Type,
                        Last4 = last4,
                        HolderName = c.HolderName,
                        ExpirationMonth = c.ExpirationMonth,
                        ExpirationYear = c.ExpirationYear,
                        BankName = c.BankName,
                        // estos flags pueden estar o no según versión del SDK
                        AllowsCharges = c.AllowsCharges,
                        AllowsPayouts = c.AllowsPayouts
                    });
                }

                // si la página llegó "incompleta", ya terminamos
                if (page.Count < pageSize)
                    break;
                offset += pageSize;
            }
            return result;
        }


    }
}