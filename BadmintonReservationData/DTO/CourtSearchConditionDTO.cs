namespace BadmintonReservationWebApp.Models
{
    public class CourtSearchConditionDTO
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string? SearchText { get; set; }
        public int SearchMode { get; set; }
        public int StatusFilter { get; set; }
        public int SurfaceTypeFilter { get; set; }
        public int CourtType { get; set; }
        public int OpeningHours { get; set; }
        public int CloseHours { get; set; }
    }
}
