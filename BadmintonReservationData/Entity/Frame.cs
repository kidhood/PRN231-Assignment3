using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Frame : BaseEntity
    {
        public Frame()
        {
        }

        public int Id { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public int Status { get; set; }
        public double Price { get; set; }
        public string Label { get; set; } = null!;
        public string Note { get; set; } = null!;
        public int CourtId { get; set; }
            
        public virtual Court Court { get; set; } = null!;

        public override string ToString()
        {
            string timeFromFormatted = $"{TimeFrom / 100:00}:{TimeFrom % 100:00}";
            string timeToFormatted = $"{TimeTo / 100:00}:{TimeTo % 100:00}";

            return $"{Court.Name} - {timeFromFormatted} - {timeToFormatted}";
        }
    }
}
