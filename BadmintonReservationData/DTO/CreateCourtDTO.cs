using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonReservationData.DTO
{
    public class CreateCourtDTO
    {
        [Required(ErrorMessage = "Court Name is required")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Court status is required")]
        public int Status { get; set; }
        [Required(ErrorMessage = "Court Surface is required")]
        public int SurfaceType { get; set; }
        [Required(ErrorMessage = "Court opening hours is required")]
        public int OpeningHours { get; set; }
        [Required(ErrorMessage = "Court close hours is required")]
        public int CloseHours { get; set; }
        public string? Amentities { get; set; }
        [Required(ErrorMessage = "Court capacity is required")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Court type is required")]
        public int CourtType { get; set; }
    }
}
