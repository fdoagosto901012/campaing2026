using Newtonsoft.Json;
using radiotaxi.Model.v2.Openpay;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.API.Providers
{
    public static class RestServiceCall<T>
    {
        // ❗ Usa HTTPS en prod
        private static readonly string BASE_URL = "http://apidev.taxistascancunoficial.com/";
        private static readonly HttpClient Client = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(60)
        };

        // ---------- Helpers básicos ----------

        private static HttpRequestMessage BuildRequest(HttpMethod method, string endPoint, string token = null, HttpContent content = null)
        {
            var url = BASE_URL.TrimEnd('/') + "/" + endPoint.TrimStart('/');
            var req = new HttpRequestMessage(method, url);
            if (!string.IsNullOrWhiteSpace(token))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            if (content != null) req.Content = content;
            return req;
        }

        private static async Task<TResp> SendAsync<TResp>(HttpRequestMessage req)
        {
            using (var resp = await Client.SendAsync(req))
            {
                var payload = await resp.Content.ReadAsStringAsync();

                // Acepta 2xx (200, 201, 204, etc.)
                if (!resp.IsSuccessStatusCode)
                    throw new Exception("HTTP " + (int)resp.StatusCode + " " + resp.ReasonPhrase + (string.IsNullOrWhiteSpace(payload) ? "" : ": " + payload));

                if (typeof(TResp) == typeof(string))
                    return (TResp)(object)payload;

                if (string.IsNullOrWhiteSpace(payload))
                    return default(TResp);

                return JsonConvert.DeserializeObject<TResp>(payload);
            }
        }

        public static async Task<TResp> GetJson<TResp>(string endPoint, string token = null)
        {
            var req = BuildRequest(HttpMethod.Get, endPoint, token);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await SendAsync<TResp>(req);
        }

        public static async Task<TResp> PostJson<TResp>(string endPoint, object body, string token = null)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var req = BuildRequest(HttpMethod.Post, endPoint, token, content);
            req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await SendAsync<TResp>(req);
        }

        // ---------- Compat (tu firma anterior; opcional mantener) ----------

        public static async Task<object> Get(string endPoint, Action<T> onSuccess, Action<Exception> onError, string token = null)
        {
            try
            {
                var data = await GetJson<T>(endPoint, token);
                onSuccess?.Invoke(data);
                return data;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return null;
            }
        }

        public static async Task<object> Post(string endPoint, StringContent content, Action<T> onSuccess, Action<Exception> onError, string token = null)
        {
            try
            {
                var req = BuildRequest(HttpMethod.Post, endPoint, token, content);
                var data = await SendAsync<T>(req);
                onSuccess?.Invoke(data);
                return data;
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
                return null;
            }
        }

        // ---------- MÉTODOS ESPECÍFICOS OPENPAY (tu backend) ----------

        // 1) Ensure customer → { CustomerId }
        public static Task<EnsureRes> EnsureOpenpayCustomerAsync(EnsureReq body, string token = null)
            => PostJson<EnsureRes>("api/openpay/customer/ensure", body, token);

        // 2) Guardar tarjeta → { Id, Brand, Type, Last4 }
        public static Task<CardSavedDto> SaveOpenpayCardAsync(string customerId, SaveCardDto body, string token = null)
            => PostJson<CardSavedDto>($"api/openpay/customers/{customerId}/cards", body, token);

        // 3) Crear cargo → { ChargeId, Status, ... } (ajusta DTO según tu API)
        public static Task<ChargeRes> CreateChargeAsync(ChargeReq body, string token = null)
            => PostJson<ChargeRes>("api/openpay/charges", body, token);
    }
}
