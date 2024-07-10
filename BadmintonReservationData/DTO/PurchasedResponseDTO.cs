using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.DTOs
{
    public class PurchasedResponseDTO
    {
        public int Id { get; set; }
        public double AmountHour { get; set; }
        public int Status { get; set; }
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
    }
}
