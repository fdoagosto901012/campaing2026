using radiotaxi.Model.v2.Operator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Emplacamiento : baseModel
    {
        public String Modelo { get; set; }
        public String Marca { get; set; }

		// Obtenemos el emplacamientos.
        public EmplacamientoDTO get(string gafet){
			try
			{
				string sql = @"fdo_getEmplacamiento " + gafet;
                EmplacamientoDTO Object = db.Database.SqlQuery<EmplacamientoDTO>(sql).ToList<EmplacamientoDTO>().FirstOrDefault<EmplacamientoDTO>();
				return Object;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw;
			}
        }

		public List<Emplacamiento> getHistory(string gafet){
            try
            {
                using ( var db = new radiotaxiEntities()) {
                    string sql = @"fdo_getEmplacamientoHistory " + gafet;
                    List<Emplacamiento> emplacados = db.Database.SqlQuery<Emplacamiento>(sql).ToList<Emplacamiento>();
                    foreach (Emplacamiento item in emplacados)
                    {
                        EmplaMarca emplaMarca = db.EmplaMarcas.Where(x => x.Id_Marca == item.Id_Marca).FirstOrDefault<EmplaMarca>();
                        EmplaModelo emplaModelo = db.EmplaModeloes.Where(x => x.Id_Modelo == item.Id_Modelo).FirstOrDefault<EmplaModelo>();
                        item.Marca = emplaMarca.Marca;
                        item.Modelo = emplaModelo.Modelo;
                    }
                    return emplacados;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<emplaBitacora> getBitacora(string gafet)
        {
            try
            {
                string sql = @"select eb.* from Emplacamiento as e
                            inner join emplaBitacora as eb on eb.id_emplacamiento = e.Id_Op
                            where No_Economico = " + gafet + @"
                            order by movDate desc;";
                List<emplaBitacora> Objects = db.Database.SqlQuery<emplaBitacora>(sql).ToList<emplaBitacora>();
                return Objects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public List<Emplacamiento> get() {
            try
            {
                string sql = @"select * from Emplacamiento as E where Operacion = 'EMPLACADO';";
                List<Emplacamiento> Objects = db.Database.SqlQuery<Emplacamiento>(sql).ToList<Emplacamiento>();
                return Objects;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Emplacamiento> VINLock() {
            try
            {
                // Regresamos todos los Vins bloqueados y donde estan bloqueados.
                return this.db.Emplacamientoes.Where(x => 
                    x.No_Serie == this.No_Serie && 
                    x.Bloqueado == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<Emplacamiento>();
            }
        }

        public Emplacamiento FindVin(string vin)
        {
            try
            {
                // Regresamos todos los Vins bloqueados y donde estan bloqueados.
                return this.db.Emplacamientoes
                    .Where(x =>
                        x.No_Serie == vin &&
                        x.Bloqueado == true
                    ).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Emplacamiento> EcoLock()
        {
            try
            {
                // Regresamos todos los Vins bloqueados y donde estan bloqueados.
                return this.db.Emplacamientoes.Where(x =>
                    x.No_Economico == this.No_Economico &&
                    x.Protegido == true)
                    .ToList();
            }
            catch (Exception ex)
            {
                return new List<Emplacamiento>();
            }
        }

    }
}
