using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class PurchasedHoursMonthly : BaseEntity
    {
        public int Id { get; set; }
        public double AmountHour { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
    }
}
