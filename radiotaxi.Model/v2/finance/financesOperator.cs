using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model.v2.finance
{
    public  class financesOperator
    {
        public int faltas { get; set; }
        public decimal cuotas_fdi { get; set; }
        public decimal otros_cargos { get; set; }
        public double porcent
        {
            get
            {
                var year = DateTime.Now.Year;
                var month = DateTime.Now.Month;
                double days = DateTime.DaysInMonth(year, month);
                double porcentUnit = (days / 100.0);
                double _porcent = this.faltas / porcentUnit;
                double _percentTotal = 100 - _porcent;
                if (_percentTotal >= 0)
                {
                    return Math.Round(_percentTotal);
                }
                return 0;
            }
            private set
            {
            }
        }
    }
}
