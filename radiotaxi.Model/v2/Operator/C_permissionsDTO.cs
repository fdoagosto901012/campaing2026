using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using radiotaxi.Model.v2;

namespace radiotaxi.Model
{
    public class C_permissionsDTO
    {
        public int id { get; set; }
        public System.DateTime dateCreated { get; set; }
        public string CreatedBy { get; set; }

        
        public Nullable<System.DateTime> dateEdited { get; set; }
        public string EditedBy { get; set; }
        public string id_Operacion { get; set; }
        public Nullable<System.DateTime> dateStart { get; set; }
        public Nullable<System.DateTime> dateEnd { get; set; }
        public int id_user { get; set; }
        public string gafet { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<System.DateTime> dateDisabled { get; set; }
        public string DisableBy { get; set; }
       
        public string taxi { get; set; }
        public Nullable<decimal> control { get; set; }
        
        public int Id_Tienda { get; set; }
        public string Id_Producto { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public string Id_Proveedor { get; set; }
        public string Id_Depto { get; set; }
        public string Id_Subfamilia { get; set; }
        public string Id_Familia { get; set; }
        public string Id_Logo { get; set; }
        public bool Consignado { get; set; }
        public int TipoComision { get; set; }
        public string UnidadEnt { get; set; }
        public string UnidadSal { get; set; }
        public int Factor { get; set; }
        public int Exist { get; set; }
        public int TiempoSurtido { get; set; }
        public int Minimo { get; set; }
        public int Maximo { get; set; }
        public int TasaIva { get; set; }
        public int TasaIvaCompra { get; set; }
        public decimal CostoPromedio { get; set; }
        public decimal CostoUltimo { get; set; }
        public int TasaImpEsp { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioConIva { get; set; }
        public int Descuento { get; set; }
        public System.DateTime FechaOp { get; set; }
        public Nullable<System.DateTime> FechaEdit { get; set; }
        public string Id_Usuario { get; set; }
        public string Id_UsuarioEdit { get; set; }
        public string Color { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> F_UltimoRep { get; set; }
        public Nullable<int> Reportes { get; set; }
        public Nullable<System.DateTime> F_CubiertoHasta { get; set; }


        public string Gafete { get; set; }
        public string CHOFER { get; set; }
        public string partnerReference { get; set; }
        public DateTime FechaReporte { get; set; }
        public int userid { get; set; }
        public string ticket { get; set; }
        public string EditBy { get; set; }



        public string firstName { get; set; }
		public string lastNameF { get; set; } 
		public string lastNameM { get; set; }


    }
}
