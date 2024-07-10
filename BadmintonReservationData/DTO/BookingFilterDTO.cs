using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.DTO
{
    public class BookingFilterDTO
    {
        public string SearchText { get; set; }
        public int Status { get; set; }
        public int PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public int BookingType { get; set; }
        public DateTime? BookingDateFrom { get; set; }
        public DateTime? BookingDateTo { get; set; }
    }
}
