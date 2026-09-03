using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class ResultsRervicesCounter
    {
        public List<ListAsignedDTO> assigments { get; set; }
        public List<ListPendingDTO> peding { get; set; }
        public List<ListCanceledDTO> canceled { get; set; }
        public List<ListFinishedDTO> finished { get; set; }

        public List<totalReportUser> throwned { get; set; }
        public List<totalReportUser> received { get; set; }

        public Dictionary<string, string> dictionary_assigend { get; set; }
        public Dictionary<string, string> dictionary_canceled { get; set; }
        public Dictionary<string, string> dictionary_finished { get; set; }
        public Dictionary<string, string> dictionary_pending { get; set; }

        public List<rtUnidNumberServices> numberServicesRTbyUnid_servicesCount { get; set; }
        public List<rtUnidNumberServices> numberServicesRTbyUnid_rtNumber { get; set; }

        public List<ServicesByTurn> ServicesByTurns { get; set; }
    }
}
