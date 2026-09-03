using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.Partner
{
    public  class PartnerT
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
        public int partnerTypeId { get; set; }
        public string birthPlace { get; set; }
        public int sex { get; set; }
        public DateTime birthDate { get; set; }
        public int userTypeId { get; set; }
        public DateTime? dtLastUpdate { get; set; }
        public int relationShipStatusId { get; set; }
        public int? sectionId { get; set; }
        public bool cafecude { get; set; }
        public bool at { get; set; }
        public bool active { get; set; }
        public string reason { get; set; }
        public string createdBy { get; set; }
        public string editBy { get; set; }
        public int? meeting { get; set; }
        public bool death { get; set; }
        public bool enemy { get; set; }
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

        /// Objeto soc_datpersonal
        public Nullable<int> Id_Op { get; set; }
        public int Numero { get; set; }
        public string Status { get; set; }
        public string Asignacion { get; set; }
        public string EcoDelRespon { get; set; }
        public string Responsable { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Telefono { get; set; }
        public Nullable<short> Edad { get; set; }
        public string Sexo { get; set; }
        public string EdoCivil { get; set; }
        public Nullable<System.DateTime> Fch_Nac { get; set; }
        public bool FondDef { get; set; }
        public string Beneficiario { get; set; }
        public string Tutor { get; set; }
        public string Alergias { get; set; }
        public string Enfermedades { get; set; }
        public string TipoSangre { get; set; }
        public string Oficio { get; set; }
        public string Escolaridad { get; set; }
        public string LugarNac { get; set; }
        public string Conyuge { get; set; }
        public string Ocupacion { get; set; }
        public Nullable<short> Hijos { get; set; }
        public string CredElector { get; set; }
        public string Distrito { get; set; }
        public Nullable<System.DateTime> Fch_Op { get; set; }
        public string Id_Usuario { get; set; }
        public Nullable<System.DateTime> Fch_Edit { get; set; }
        public string id_Usuario_edit { get; set; }
        public Nullable<System.DateTime> FechaRenFdi { get; set; }
        public string MayaCaribe { get; set; }
        public string UltTaxi { get; set; }
        public string ULTTURNO { get; set; }
        public string SECRETARIA { get; set; }
        public string PUESTO { get; set; }
        public string fraccionamiento { get; set; }
        public string sm { get; set; }
        public string mz { get; set; }
        public string lote { get; set; }
        public string calle { get; set; }
        public string num { get; set; }
        public string localidad { get; set; }
        public string municipio { get; set; }
        public string ubicacion { get; set; }
        public string celular { get; set; }
        public string email { get; set; }
        public Nullable<System.DateTime> fechanacimiento { get; set; }
        public Nullable<System.DateTime> fechaingreso { get; set; }
        public string socemplacamiento { get; set; }
        public string obsaya { get; set; }
        public bool valido { get; set; }
        public bool imprimirenpadron { get; set; }
        public Nullable<bool> fallecidoPadron { get; set; }

        public int TotalRows { get; set; }
    }
}
