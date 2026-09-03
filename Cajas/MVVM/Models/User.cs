using Cajas.Client;
using Cajas.MVVM.Models.@base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Cajas.MVVM.Models
{
    public partial class User : BaseModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {

        }
        public string Name { get; set; }
        public string Image { get; set; }
        public Color Color { get; set; }
        public int id { get; set; }
        public string partnerReference { get; set; }
        public string firstName { get; set; }
        public string lastNameF { get; set; }
        public string lastNameM { get; set; }
        public DateTime createdDt { get; set; }
        public int partnerTypeId { get; set; }
        public string birthPlace { get; set; }
        public int sex { get; set; }
        public DateTime? birthDate { get; set; }
        public int userTypeId { get; set; }
        public DateTime? dtLastUpdate { get; set; }
        public int relationShipStatusId { get; set; }
        public short statusId { get; set; }
        public bool? cafecude { get; set; }
        public bool? at { get; set; }
        public bool? active { get; set; }
        public string reason { get; set; }
        public string createdBy { get; set; }
        public string editBy { get; set; }
        public int? meeting { get; set; }
        public bool? death { get; set; }
        public bool? enemy { get; set; }
        public bool? payroll { get; set; }
        public bool? ttesoc { get; set; }
        public int? employee { get; set; }
        public bool? employees { get; set; }
        public bool? noVote { get; set; }
        public int? sectionId { get; set; }
        public int? candidateId { get; set; }
        public int? candidateHardvoteId { get; set; }
        public int candidateQuizID { get; set; }
        public int? gps_id { get; set; }
        public bool? tsc { get; set; }
        public bool? rt { get; set; }
        public string googleplus { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string twitter { get; set; }
        public string ttc_soc_eco { get; set; }
        public string comments { get; set; }
        public string rfc { get; set; }
        public string ine { get; set; }
        public string curp { get; set; }
        public string token { get; set; }

        public bool isLoged { get; set; }

        public Partner partner { get; set; }
        public string role { get; set; }
        public string username { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string userId { get; set; }
        public string padronId { get; set; }




        protected void onDataLoaded(string token)
        {
            // AQUI SIGANMOS LA INFORMACION
            JwtSecurityToken objJwtSecurityToken = new JwtSecurityToken(token);
            try
            {
                username = objJwtSecurityToken.Claims.Where(x => x.Type == "username").FirstOrDefault().Value;
                firstName = objJwtSecurityToken.Claims.Where(x => x.Type == "firstName").FirstOrDefault().Value;
                lastName = objJwtSecurityToken.Claims.Where(x => x.Type == "lastName").FirstOrDefault().Value;
                email = objJwtSecurityToken.Claims.Where(x => x.Type == "email").FirstOrDefault().Value;
                partnerReference = objJwtSecurityToken.Claims.Where(x => x.Type == "partnerReference").FirstOrDefault().Value;
                phone = objJwtSecurityToken.Claims.Where(x => x.Type == "phone").FirstOrDefault().Value;
                userId = objJwtSecurityToken.Claims.Where(x => x.Type == "userId").FirstOrDefault().Value;
                padronId = objJwtSecurityToken.Claims.Where(x => x.Type == "padronId").FirstOrDefault().Value;
                role = objJwtSecurityToken.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
                string User = objJwtSecurityToken.Claims.Where(x => x.Type == "User").FirstOrDefault().Value;
                if (User != null && User != "" && role == "client")
                {
                    partner = JsonConvert.DeserializeObject<Partner>(User);
                }
                Console.WriteLine("JWT");
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<string> Login(string user, string password)
        {

            using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(new
                                {
                                    userName = user,
                                    password
                                }),
                                Encoding.UTF8,
                                "application/json");
            string respnose = (string)await RestServiceCall<string>.Post("api/users/token", jsonContent, onDataLoaded, onDataLoadFailed);


            return respnose;
        }


        protected void onPartnerDataLoaded(PartnerGlobalDTO obj)
        {
            try
            {
                Console.WriteLine(obj);
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<PartnerGlobalDTO> getPartnerInfo()
        {
            using StringContent jsonContent = new(
                                JsonConvert.SerializeObject(new { }),
                                Encoding.UTF8,
                                "application/json");

            PartnerGlobalDTO respnose = (PartnerGlobalDTO)await RestServiceCall<PartnerGlobalDTO>.Get("api/partner/" + partner.partnerReference, onPartnerDataLoaded, onDataLoadFailed);
            return respnose;
        }

    }
}
