

using System.ComponentModel.DataAnnotations;

namespace BadmintonReservationData.DTOs
{
    public class CreateCustomFrameRequestDTO
    {

        [Required(ErrorMessage = "Frame id is required")]
        public int FrameId { get; set; }    

        [Required(ErrorMessage = "Price is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be > 0")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Specific Date is required")]
        public DateTime SpecificDate { get; set; }

        [Required(ErrorMessage = "Date Type is required")]
        public int DateType { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public int Status { get; set; }
    } 
}
