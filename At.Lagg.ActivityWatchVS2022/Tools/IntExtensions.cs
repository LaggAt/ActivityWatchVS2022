namespace At.Lagg.ActivityWatchVS2022.Tools
{
    internal static class IntExtensions
    {
        #region Methods

        public static string Ordinal(this int number)
        {
            var ones = number % 10;
            var tens = Math.Floor(number / 10f) % 10;
            if (tens == 1)
            {
                return $"{number}th";
            }
            switch (ones)
            {
                case 1: return $"{number}st";
                case 2: return $"{number}nd";
                case 3: return $"{number}rd";
                default: return $"{number}th";
            }
        }

        #endregion Methods
    }
}
