using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cajas.Client;
using Cajas.MVVM.Models.@base;

namespace Cajas.MVVM.Models
{
    public partial class card : BaseModel
    {
        public card()
        {
        }

        public int id { get; set; }
        public int userId { get; set; }
        public string partnerReference { get; set; }
        public string taxi { get; set; }
        public DateTime? asignedDate { get; set; }
        public string asgnedBy { get; set; }
        public string note { get; set; }
        public DateTime? lastEditedDate { get; set; }
        public string editedBy { get; set; }
        public bool? isActive { get; set; }
        public int? turn { get; set; }
        public bool? deliver { get; set; }
        public DateTime? deliverDate { get; set; }
        public DateTime? expiration_date { get; set; }
        public double? id_Op { get; set; }
        public bool? low { get; set; }
        public DateTime? lowDate { get; set; }

        public virtual User user { get; set; }
        protected void onDataLoaded(CardResponse obj)
        {

        }
        public async Task<CardResponse> get(string id)
        {
            CardResponse a = (CardResponse)await RestServiceCall<CardResponse>.Get("api/cards/" + id, onDataLoaded, onDataLoadFailed);
            return a;
        }

    }
}