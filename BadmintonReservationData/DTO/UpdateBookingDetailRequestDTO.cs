namespace BadmintonReservationData.DTOs
{
    public class UpdateBookingDetailRequestDTO
    {
        public int? Status { get; set; } // 1: Pending, 2: Successful, 3: Failed, 4: Cancelled
    }
}
