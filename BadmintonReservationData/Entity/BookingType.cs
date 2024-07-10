using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class BookingType : BaseEntity
    {
        public BookingType()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public double PromotionAmount { get; set; }
    }
}
