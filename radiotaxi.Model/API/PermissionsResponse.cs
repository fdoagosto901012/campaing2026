using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API
{
    public class PermissionsResponse
    {
        public List<C_permissionsDTO> permissions { get; set; }
        public C_permissions current { get; set; }

        public int Exist { get; set; }

        public PermissionsResponse() {
            this.Exist = -1;
        }
    }
}