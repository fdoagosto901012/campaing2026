using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Convenio : baseModel
    {
        public ConvenioResponse get(string id_op) {
			try
			{
                ConvenioResponse convenioResponse = new ConvenioResponse();
                convenioResponse.partidas = this.db.ConveniosDetalles.Where( x => x.Id_Op == id_op).ToList();
                convenioResponse.convenio = this.db.Convenios.Where( x => x.Id_Op == id_op).FirstOrDefault();
                return convenioResponse;
            }
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
        }
    }
}
