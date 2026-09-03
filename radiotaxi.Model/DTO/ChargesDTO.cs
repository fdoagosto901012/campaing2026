using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.DTO
{
    public  class ChargesDTO 
    {
        public Charge Charge { get; set; }
        public List<ChargesItem> Items { get; set; }

        public void save() {
            try
            {
                using (var db = new radiotaxiEntities())
                {
                    db.Configuration.LazyLoadingEnabled = false;
                    db.Configuration.ProxyCreationEnabled = false;
                    Charge.ChargesItems = Items;
                    db.Charges.Add(Charge);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
