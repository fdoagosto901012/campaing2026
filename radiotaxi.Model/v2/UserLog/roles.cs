using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class role : baseModel
    {
        public List<role> get() {
            return this.db.roles.ToList();
        }

        public role get(int id)
        {
            return this.db.roles.Where(x => x.id == id).ToList().FirstOrDefault();
        }
    }
}
