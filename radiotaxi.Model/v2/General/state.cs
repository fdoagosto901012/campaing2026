using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public partial class state : baseModel
    {
        public List<state> get()
        {
            return this.db.states.OrderBy(x => x.id).ToList();
        }

        public List<state> get(int id)
        {
            return this.db.states.Where(x => x.id == id)
                .OrderBy(x => x.id).ToList();
        }
    }
}
