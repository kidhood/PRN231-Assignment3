using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class DateType : BaseEntity
    {
        public DateType()
        {
            CustomFrames = new HashSet<CustomFrame>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<CustomFrame> CustomFrames { get; set; }
    }
}
