using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace radiotaxi.API.Hubs
{

    public class CarsHub : BaseHub
    {
        public CarsHub() { }

        public void Sendbroadcast(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

    }
}