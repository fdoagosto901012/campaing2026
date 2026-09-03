using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class Partner : Soc_DatPersonales
    {
        public int userId { get; set; }
        public string urlImage { get; set; }
        public string bucket { get; set; }
        public string key { get; set; }
        public string curp { get; set; }
        public string ine { get; set; }
        public bool? organDonor { get; set; }
        public string bloodType { get; set; }
        public string bloodTypeId { get; set; }
        public string partnerReference { get; set; }
        public string firstName { get; set; }
        public string lastNameF { get; set; }
        public string lastNameM { get; set; }
        public DateTime createdDt { get; set; }
        public int? partnerTypeId { get; set; }
        public string birthPlace { get; set; }
        public int? sex { get; set; }
        public DateTime birthDate { get; set; }
        public int? userTypeId { get; set; }
        public DateTime? dtLastUpdate { get; set; }
        public int? relationShipStatusId { get; set; }
        public int? sectionId { get; set; }
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
        public bool? noVote { get; set; }
        public int? candidateId { get; set; }
        public int? candidateHardvoteId { get; set; }
        public int? candidateQuizID { get; set; }
        public int? gps_id { get; set; }
        public string googleplus { get; set; }
        public string facebook { get; set; }
        public string instagram { get; set; }
        public string twitter { get; set; }
        public string comments { get; set; }
        public string urfc { get; set; }
        public string relationship { get; set; }
        public string relationShipStatus { get; set; }
        public string allergies { get; set; }
        public string realtionship { get; set; }

        public ICollection<userAddress> userAddresses { get; set; }
        public ICollection<email> emails { get; set; }
        public ICollection<userPhone> userPhones { get; set; }
        public ICollection<userHealthInformation> userHealthInformation { get; set; }
        public ICollection<amazonPicture> amazonPictures { get; set; }

        public Partner()
        {

        }

    }
}
