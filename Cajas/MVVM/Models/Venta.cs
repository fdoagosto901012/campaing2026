using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public partial class Venta
    {
        public string Id_Tienda { get; set; }
        public string Id_Op { get; set; }
        public string Id_Cliente { get; set; }
        public string Id_Vendedor { get; set; }
        public decimal Rate { get; set; }
        public string No_Doc { get; set; }
        public string Status { get; set; }
        public decimal Importe { get; set; }
        public decimal Desporcen { get; set; }
        public decimal DescImp { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Ips { get; set; }
        public decimal Total { get; set; }
        public string Id_Usuario { get; set; }
        public DateTime FechaOp { get; set; }
        public DateTime HoraOp { get; set; }
        public DateTime FechaEdit { get; set; }
        public string Id_UsuarioEdit { get; set; }
        public decimal CostoTotal { get; set; }
        public decimal TIPOCAMBIO { get; set; }
    }
}
