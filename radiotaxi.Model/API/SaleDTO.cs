using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.API
{
    public class SaleDTO
    {
        public string taxi { get; set; }
        public string reference { get; set; }
        public string chashcashier { get; set; }
        public List<_caja_cargos_DTO> pays { get; set; }
    }
}
