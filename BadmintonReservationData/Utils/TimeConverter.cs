namespace BadmintonReservationData.Utils;

public static class TimeConverter
{
    public static TimeSpan ConvertToTimeSpan(int time)
    {
        var hours = time / 100;
        var minutes = time % 100;
        return new TimeSpan(hours, minutes, 0);
    }

    public static int ConvertToInt(TimeSpan timeSpan)
    {
        return timeSpan.Hours * 100 + timeSpan.Minutes;
    }

    public static string ConvertIntTime(int hhmm)
    {
        string timeString = hhmm.ToString().PadLeft(4, '0');
        string hours = timeString.Substring(0, 2);
        string minutes = timeString.Substring(2, 2);
        return $"{hours}:{minutes}";
    }
}