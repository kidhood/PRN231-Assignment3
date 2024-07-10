using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Operation : BaseEntity
    {
        public int Id { get; set; }
        public DateTime OpenTimeFrom { get; set; }
        public DateTime OpenTimeTo { get; set; }
        public int Status { get; set; }
    }
}
