using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.DTOs
{
    public class CreatePurchasedRequestDTO
    {
        [Required(ErrorMessage = "Amount Hour is required")]
        public double AmountHour { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }

        [Required(ErrorMessage = "CustomerID is required")]
        public int CustomerId { get; set; }
        public int PaymentId { get; set; }
    }
}