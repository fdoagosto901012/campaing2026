using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Operadores_Baja_Datos : baseModel
    {
        public List<Operadores_Baja_Datos> get(Operadores_Baja_Datos data){
            return db.Operadores_Baja_Datos.Where(x => x.NOMBRE == data.NOMBRE).ToList<Operadores_Baja_Datos>();
        }
    }
}
