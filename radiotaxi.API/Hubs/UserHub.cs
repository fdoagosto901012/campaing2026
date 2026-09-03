using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace radiotaxi.API.Hubs
{
    public class UserHub : BaseHub
    {
        public UserHub() { }

        public void Sendbroadcast(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
        }

    }
}