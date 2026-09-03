using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class PackageDTO
    {
        public int id { get; set; }
        public string description { get; set; }
        public float amount { get; set; }
        public int vaidDays { get; set; }

        public override string ToString()
        {
            if (this.vaidDays == 1)
            {
                return vaidDays.ToString() + " día - $" +
                amount.ToString() + " MXN.";

            }
            else
            {
                return vaidDays.ToString() + " dias - $" +
                amount.ToString() + " MXN.";
            }
        }
    }
}
