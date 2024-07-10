using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Holiday : BaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
