using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class reports_DTO
    {
        public ResultsRervicesCounter days_report { get; set; } // Tablas de assignaciones, pendientes, cancelaciones por dia.
        public IEnumerable<totalReportUser> asigned_by_user { get; set; } // Asignados por centralistas
        public IEnumerable<totalReportUser> Created_by_user { get; set; } // Creados por centralistas

    }
}
