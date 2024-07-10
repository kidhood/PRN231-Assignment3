using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class BookingDetail : BaseEntity
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime BookDate { get; set; }
        public int FrameId { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public double Price { get; set; }
        public int Status { get; set; }
        public DateTime? CheckinTime { get; set; }
        public DateTime? CheckoutTime { get; set; }
        public virtual Frame Frame { get; set; } = null!;
    }
}
