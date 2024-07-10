using System.ComponentModel.DataAnnotations;

namespace BadmintonReservationData.DTOs;

public class UpdateFrameRequestDTO
{
    [Required(ErrorMessage = "Id is required")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Label is required")]
    public string Label { get; set; }
    [Required(ErrorMessage = "Note is required")]
    public string Note { get; set; }
    [Required(ErrorMessage = "Time From is required")]
    [DataType(DataType.Time)]
    public TimeSpan TimeFrom { get; set; }

    [Required(ErrorMessage = "Time To is required")]
    [DataType(DataType.Time)]
    public TimeSpan TimeTo { get; set; }

    [Required(ErrorMessage = "Time From is required")]
    public int OldTimeFrom { get; set; }

    [Required(ErrorMessage = "Time To is required")]
    public int OldTimeTo { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public int Status { get; set; }

    [Required(ErrorMessage = "Price is required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be > 0")]
    public double Price { get; set; }

    [Required(ErrorMessage = "Court is required")]
    public int CourtId { get; set; }
    [Required(ErrorMessage = "Court is required")]
    public int OldCourtId { get; set; }
}