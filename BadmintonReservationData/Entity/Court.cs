using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Court : BaseEntity
    {
        public Court()
        {
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int SurfaceType { get; set; }
        public int Status { get; set; }
        public string TotalBooking { get; set; } = null!;
        public int OpeningHours { get; set; }
        public int CloseHours { get; set; }
        public string? Amentities { get; set; }
        public int Capacity { get; set; }
        public int CourtType { get; set; }
    }
}
