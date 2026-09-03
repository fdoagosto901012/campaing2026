using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public class PageFactory : IPageFactory
    {
        private readonly IServiceProvider _services;

        public PageFactory(IServiceProvider services)
        {
            _services = services;
        }

        public T Create<T>() where T : Page
        {
            return _services.GetRequiredService<T>();
        }
    }
}
