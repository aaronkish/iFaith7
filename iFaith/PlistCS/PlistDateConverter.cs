namespace iFaith.PlistCS
{
    using System;

    public sealed class PlistDateConverter
    {
        public static long timeDifference = 0x3a4fc880L;

        private PlistDateConverter()
        {
        }

        public static DateTime ConvertFromAppleTimeStamp(double timestamp)
        {
            DateTime time = new DateTime(0x7d1, 1, 1, 0, 0, 0, 0);
            return time.AddSeconds(timestamp);
        }

        public static double ConvertToAppleTimeStamp(DateTime date)
        {
            DateTime time = new DateTime(0x7d1, 1, 1, 0, 0, 0, 0);
            TimeSpan span = (TimeSpan) (date - time);
            return Math.Floor(span.TotalSeconds);
        }

        public static long GetAppleTime(long unixTime)
        {
            return (unixTime - timeDifference);
        }

        public static long GetUnixTime(long appleTime)
        {
            return (appleTime + timeDifference);
        }
    }
}

