using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Servicio : baseModel
    {
        public Servicio() { }

        public List<Servicio> get() {
			try
			{
				List<Servicio> serv = new List<Servicio>();
				serv = db.Servicios.ToList();
				return serv;
			}
			catch (Exception ex)
			{
				return null;
			}
        }
    }
}
