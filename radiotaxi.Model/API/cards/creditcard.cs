using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class creditcard : baseModel
    {
        public bool save()
        {
            try
            {
                this.db.creditcards.Add(this);
                this.db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<creditcard> get(int userId)
        {
            try
            {
                return this.db.creditcards.Where(x => x.userId == userId).ToList();            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<creditcard>();
            }
        }
    }
}
