using System;

public static class TimeFormatter
{
    public static string Format(DateTime time)
    {
        TimeSpan span = DateTime.Now - time;
        if (span.TotalMinutes < 60)
            return Math.Round(span.TotalMinutes) + " minutes ago";
        if (span.TotalHours < 24)
            return Math.Round(span.TotalHours) + " hours ago";
        if (span.TotalDays < 5)
            return Math.Round(span.TotalDays) + " days ago";
        return time.ToString("dd. MMM yyyy", System.Threading.Thread.CurrentThread.CurrentUICulture);
    }
}
