namespace BadmintonReservationData.DTO
{
    public class UpdatePatchBookingRequestDTO
    {
        public int? Status { get; set; } // 1: Pending, 2: Successful, 3: Failed, 4: Cancelled
        public int? PaymentStatus { get; set; } // 1: NotYet, 2: Paid, 3: Failed, 4: Refund
        public int? CustomerId { get; set; }
        public int? BookingTypeId { get; set; }
        public double? PromotionAmount { get; set; }
        public int? PaymentType { get; set; }
    }
}
