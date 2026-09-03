using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models.@base
{
    public class BaseModel
    {
        public string Message { get; set; }

        protected void onDataLoadFailed(Exception exception)
        {
            Message = exception?.Message;
        }


    }
}
