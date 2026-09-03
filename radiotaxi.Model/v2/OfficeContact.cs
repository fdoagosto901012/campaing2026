using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    using System;
    using System.Collections.Generic;

    public partial class OfficeContact : baseModel
    {
        public OfficeContact() { }

        public List<OfficeContact> get() {
            try
            {
                return this.db.OfficeContacts.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
