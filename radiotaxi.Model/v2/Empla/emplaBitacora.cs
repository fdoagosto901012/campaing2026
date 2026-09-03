using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class emplaBitacora : baseModel
    {

        public Emplacamiento prebitaco {
            get { return JsonConvert.DeserializeObject<Emplacamiento>(this.objPre) ; }
            set
            {
                
            }
        }

        public Emplacamiento postbitacoa
        {
            get {
                if (string.IsNullOrWhiteSpace(this.objPos))
                {
                    return null;
                }

                try
                {
                    return JsonConvert.DeserializeObject<Emplacamiento>(this.objPos);
                }
                catch (JsonException)
                {
                    // Manejar el error de deserialización si el JSON es inválido
                    return null;
                }
            }
            set
            {

            }
        }
    }
}
