using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Utils
{
    public interface iAwsS3
    {
        string getImage(string BUCKETNAME, string objectKey = "");
    }
}
