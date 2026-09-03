using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.Models
{
    public sealed class PrintTicketResult
    {
        public bool Success { get; init; }

        public string? ErrorMessage { get; init; }

        public static PrintTicketResult Ok()
            => new() { Success = true };

        public static PrintTicketResult Error(string message)
            => new()
            {
                Success = false,
                ErrorMessage = message
            };
    }
}
