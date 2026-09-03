using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class Hotel : baseModel
    {
        public List<Hotel> get() {
			try
			{
				return this.db.Hotels
                    .OrderBy(x => x.shortname)
                    .ToList();
			}
			catch (Exception ex)
			{
				return null;
			}
        }

        public List<Hotel> getWithTarif()
        {
            try
            {
                return this.db.Hotels.Where(h => h.Couponrates.Any(r => r.HotelID == h.HotelID)).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Hotel getWithTarif(int id)
        {
            try
            {
                Hotel hotel = this.db.Hotels.Where(h => h.Couponrates.Any(r => r.HotelID == id)).Include(x => x.Couponrates.Select(r => r.Cupontype)).FirstOrDefault();
                return hotel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string DisplayName =>
        $"{shortname} ({Name})";

        
    }
}
