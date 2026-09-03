using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cajas.Client;
using Cajas.MVVM.Models.@base;

namespace Cajas.MVVM.Models
{
    public class EmplacamientoDTO : BaseModel
    {

        public EmplacamientoDTO()
        {

        }
        public decimal Id_Op { get; set; }
        public decimal No_Economico { get; set; }
        public string Nombre { get; set; }
        public string Nombre2 { get; set; }
        public string Paterno { get; set; }
        public string Materno { get; set; }
        public string Propiedad { get; set; }
        public string Operacion { get; set; }
        public string Id_Mov { get; set; }
        public string Id_Marca { get; set; }
        public string Id_Modelo { get; set; }
        public int AutoAno { get; set; }
        public string Motor { get; set; }
        public string No_Serie { get; set; }
        public string Placas { get; set; }
        public int Capacidad { get; set; }
        public string EconoResp { get; set; }
        public string Responsable { get; set; }
        public string TitularEmpla { get; set; }
        public string TitularRentas { get; set; }
        public string TitularCT { get; set; }
        public string TitularOtro { get; set; }
        public string Observaciones { get; set; }
        public string Status { get; set; }
        public bool Bloqueado { get; set; }
        public bool? Protegido { get; set; }
        public DateTime? FechaOp { get; set; }
        public DateTime? FechaBaja { get; set; }
        public string Id_Usuario { get; set; }
        public string Id_UsuarioEdit { get; set; }
        public string resp_telefono { get; set; }
        public string resp_celular { get; set; }
        public string poliza { get; set; }
        public string tipopoliza { get; set; }
        public string aseguradora { get; set; }
        public DateTime? vencepoliza { get; set; }

        public string Modelo { get; set; }
        public string Marca { get; set; }

        public static implicit operator EmplacamientoDTO(ObservableCollection<EmplacamientoDTO> v)
        {
            throw new NotImplementedException();
        }
        protected void onDataLoaded(EmplacamientoDTO obj)
        {

        }

        public async Task<EmplacamientoDTO> get(string id)
        {
            EmplacamientoDTO a = (EmplacamientoDTO)await RestServiceCall<EmplacamientoDTO>.Get("api/cars/" + id, onDataLoaded, onDataLoadFailed);
            return a;
        }



    }
}
