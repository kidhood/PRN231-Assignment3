

using System.ComponentModel.DataAnnotations;

namespace BadmintonReservationData.DTOs
{
    public class CustomFrameDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Time From is required")]
        public int FrameId { get; set; }
        public double Price { get; set; }
        public DateTime SpecificDate { get; set; }
        public int DateType { get; set; }
        public string DateTypeName { get; set; }
        public int Status { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string CourtName { get; set; }
        public DateTime UpdatedDate { get; set; }
    } 
}
