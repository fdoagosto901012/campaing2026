using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace radiotaxi.Model
{
    public class MessageDTO
    {
        public int Id { get; set; } 
        public string Message { get; set; }
        public int Type { get; set; }
    
        // 200 ok
        // 500 error
    }
}
