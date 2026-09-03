using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace radiotaxi.API.Hubs
{
    public class CreditCardHub : BaseHub
    {
        public CreditCardHub() { }

        public void Sendbroadcast(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

    }
}