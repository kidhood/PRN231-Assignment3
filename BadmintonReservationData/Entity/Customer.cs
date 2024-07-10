using BadmintonReservationData.Entity;
using System;
using System.Collections.Generic;

namespace BadmintonReservationData
{
    public partial class Customer : BaseEntity
    {
        public Customer()
        {
            PurchasedHoursMonthlies = new HashSet<PurchasedHoursMonthly>();
        }

        public int Id { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public double TotalHoursMonthly { get; set; }
        public int? AccountId { get; set; }
        public virtual ICollection<PurchasedHoursMonthly> PurchasedHoursMonthlies { get; set; }
    }
}
