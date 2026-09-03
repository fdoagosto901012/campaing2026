using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class taxiServiceDTO
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Requiredo")]
        [Display(Name = "Origen")]
        public string startPoint { get; set; }
        [Display(Name = "Destino")]
        public string endPoint { get; set; }
        [Display(Name = "Fecha solicitado")]
        public System.DateTime dateRequested { get; set; }
        [Display(Name = "Creado Por")]
        public string createdBy { get; set; }
        [Display(Name = "Editado Por")]
        public string editedBy { get; set; }
        [Display(Name = "Teléfono Cliente")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "El número telefónico debe contener 10 dígitos")]
        [Required(ErrorMessage = "Requiredo")]
        //[RegularExpression(@"^[0-9]$", ErrorMessage = "Solamente número con clave lada")]
        //[Range(0, int.MaxValue, ErrorMessage = "Error de rango")]
        public string phoneId { get; set; }
        [Display(Name = "Estado")]
        public int statusServiceId { get; set; }
        [Display(Name = "Observación")]
        public string note { get; set; }
        [Display(Name = "Hora del Servicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime datePickUp { get; set; }
        [Display(Name = "Renovación Automática")]
        public bool autoRenew { get; set; }
        [Display(Name = "Base")]
        public int baseServiceId { get; set; }
        [Display(Name = "Tipo de servicio")]
        public int catalogServiceId { get; set; }
        [Display(Name = "´Días de renovación")]
        public string days { get; set; }

        
        public virtual client client { get; set; }
        public virtual List<rtNumberService> rtNumberServices { get; set; }
        public virtual statusService statusService { get; set; }
        public virtual baseService baseService { get; set; }
    }
}
