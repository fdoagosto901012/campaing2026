using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;

namespace radiotaxi.Model
{
    public partial class userGeolocation : baseModel
    {
        public userGeolocation() { 
        
        }

        private DbGeography CrearUbicacion(double? lat, double? lng)
        {
            if (lat == null || lng == null)
                return null;

            string wkt = $"POINT({lng.Value} {lat.Value})";
            return DbGeography.PointFromText(wkt, 4326);
        }

        public List<userGeolocation> get(int time = 20) {
            try
            {
                string query = @"
                WITH UltimosRegistros AS (
                    SELECT 
                        Id,
                        UserId,
                        Latitud,
                        Longitud,
                        DateCreate,
		                Speed,
		                geolocation,
                        ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY DateCreate DESC) AS rn
                    FROM userGeolocation
                    WHERE dateCreate >= DATEADD(MINUTE, -" + time + @", GETDATE())
                )
                SELECT 
                    Id,
                    UserId,
                    Latitud,
                    Longitud,
                    DateCreate,
	                Speed,
	                geolocation
                FROM UltimosRegistros
                WHERE rn = 1;
                ";
                List<userGeolocation> Object = db.Database.SqlQuery<userGeolocation>(query).ToList<userGeolocation>();
                return Object;
            }
            catch (Exception ex)
            {
                return new List<userGeolocation>();
            }
        }

        public bool save() {
            try
            {
                DbGeography ubicacion = CrearUbicacion(this.Latitud, this.Longitud);
                this.geolocation = ubicacion;
                this.db.userGeolocations.Add(this);
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                return false;
            }
        }

        public userGeolocation getLast(int id) {
            try
            {
                this.db.userGeolocations.Where( x => x.UserId == id)
                    .OrderByDescending(x => x.DateCreate).FirstOrDefault();
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
