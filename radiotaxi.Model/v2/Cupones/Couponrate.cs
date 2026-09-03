using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static radiotaxi.Model.client;

namespace radiotaxi.Model
{
    [MetadataType(typeof(MetadataCouponrate))]
    public partial class Couponrate : baseModel
    {
        public Couponrate get(int id) {
			try
			{
				return this.db.Couponrates.Where(x => x.CouponrateID == id).FirstOrDefault();
			}
			catch (Exception)
			{
				return null;
			}
        }
    }

	public class MetadataCouponrate {
        [JsonIgnore]
        public virtual ICollection<Cupon> Cupons { get; set; }
    }

}
