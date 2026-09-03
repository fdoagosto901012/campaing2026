using radiotaxi.Model;
using System;
using System.Collections.ObjectModel;

namespace Cajas.MVVM.ViewModels.cupones
{
    // =====================================================
    // OFFCANVAS - HISTORIAL DE PAGOS
    // MODELO DUMMY PARA PRUEBAS DE FRONT-END
    // =====================================================

    public class PaymentHistoryItem
    {
        public int PaymentId { get; set; }

        public string OperatorName { get; set; } = string.Empty;

        public int CouponsCount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime PaymentDate { get; set; }

        // =====================================================
        // DETALLE DEL PAGO - CUPONES DUMMY DEL FOLIO
        // =====================================================

        public ObservableCollection<CouponPayment> payments { get; set; } = new();
    }

}