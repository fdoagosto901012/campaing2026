using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class RTYPE : baseModel
    {
        public async Task<RTYPE> getRtype(string partnerReference) {
			try
			{
				using (var db = new radiotaxiEntities())
				{
					// Obtenemos primero el valor de las faltas. 
					string query = @"select top 1 PrecioConIva from Inventario where 
									Id_Familia = '0' and Id_Proveedor = '" + partnerReference + "';";
					decimal price  = await db.Database.SqlQuery<decimal>(query).FirstAsync(); // Una vez que obtenemos el precio, lo buscamos en la tabla.
					RTYPE response = db.RTYPEs.Where(x => x.simple == price).FirstOrDefault(); // Obtenemos la tarifa
					if (response == null)
					{
						response = new RTYPE() { 
							simple = price,
							doble = price*2,
						};
						return response;
					}
					return response;
				}
                return null;
			}
			catch (Exception ex)
			{
				return null;
			}
			finally { 
			
			}
        }
    }
}