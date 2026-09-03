using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.Services
{
    public interface IQrScannerService
    {
        event EventHandler<string>? CodeScanned;
        event EventHandler<string>? FocuScanned;
        void StartListening();
        void StopListening();
    }
}
