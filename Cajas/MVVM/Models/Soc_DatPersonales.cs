using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class Soc_DatPersonales
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Soc_DatPersonales()
        {
        }

        public int? Id_Op { get; set; }
        public int Numero { get; set; }
        public string Status { get; set; }
        public string Asignacion { get; set; }
        public string EcoDelRespon { get; set; }
        public string Responsable { get; set; }
        public string Nombre { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Telefono { get; set; }
        public short? Edad { get; set; }
        public string Sexo { get; set; }
        public string EdoCivil { get; set; }
        public DateTime? Fch_Nac { get; set; }
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
        public short? Hijos { get; set; }
        public string CredElector { get; set; }
        public string Distrito { get; set; }
        public DateTime? Fch_Op { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime? Fch_Edit { get; set; }
        public string id_Usuario_edit { get; set; }
        public DateTime? FechaRenFdi { get; set; }
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
        public DateTime? fechanacimiento { get; set; }
        public DateTime? fechaingreso { get; set; }
        public string socemplacamiento { get; set; }
        public string obsaya { get; set; }
        public bool valido { get; set; }
        public bool imprimirenpadron { get; set; }
        public bool? fallecidoPadron { get; set; }
        public bool? Fiscal { get; set; }

    }
}
