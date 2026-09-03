using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Openpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace radiotaxi.API.Controllers.Base
{
    public abstract class BaseControllerWithHub<THub> : BaseController where THub : IHub
    {
        Lazy<IHubContext> hub = new Lazy<IHubContext>(
            () => GlobalHost.ConnectionManager.GetHubContext<THub>()
        );

        protected IHubContext Hub
        {
            get { return hub.Value; }
        }
        // Operaciones generales.

    }
}
