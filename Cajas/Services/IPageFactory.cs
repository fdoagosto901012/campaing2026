using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public interface IPageFactory
    {
        T Create<T>() where T : Page;
    }
}
