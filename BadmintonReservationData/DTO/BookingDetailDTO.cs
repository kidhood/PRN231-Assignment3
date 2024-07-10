namespace BadmintonReservationData.DTO
{
    public class BookingDetailDTO
    {
        public int Id { get; set; }
        public DateTime BookDate { get; set; }
        public int FrameId { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }
        public double Price { get; set; }
    }
}
