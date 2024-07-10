using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Booking : BaseEntity
    {
        public Booking()
        {
            BookingDetails = new HashSet<BookingDetail>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BookingTypeId { get; set; }
        public int Status { get; set; }
        public double PromotionAmount { get; set; }
        public int PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentId { get; set; }

        public virtual BookingType BookingType { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
        public virtual Payment Payment { get; set; } = null!;
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
    }
}
