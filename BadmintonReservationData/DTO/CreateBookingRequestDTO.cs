namespace BadmintonReservationData.DTO
{
    public class CreateBookingRequestDTO
    {
        public int CustomerId { get; set; }
        public int BookingTypeId { get; set; }
        public int Status { get; set; }
        public int PaymentType { get; set; }
        public int PaymentStatus { get; set; }
        public double PromotionAmount { get; set; }
        
        public List<BookingDetailDTO> BookingDetails { get; set; }
    }
}
