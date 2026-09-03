using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace radiotaxi.Model
{
    public partial class ChatMessage
    {
        [NotMapped]
        public string Message { get; set; }
        [NotMapped]
        public string Group { get; set; }
        [NotMapped]
        public string _latitude { get; set; }
        [NotMapped]
        public string _longitude { get; set; }
    }
}
