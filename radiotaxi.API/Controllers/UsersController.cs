using Microsoft.AspNet.SignalR.Hosting;
using radiotaxi.API.Controllers.Base;
using radiotaxi.API.Hubs;
using radiotaxi.Model;
using radiotaxi.Model.API.cards;
using radiotaxi.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace radiotaxi.API.Controllers
{
    [Authorize(Roles = "soc")]
    [RoutePrefix("api/creditcards")]
    public class UsersController : BaseControllerWithHub<CreditCardHub>
    {
        
    }
}
