using System;

namespace Varisten
{
    public class Tool
    {
        private static Random R = new Random();

        public static double NextDouble(double max, double min = 0)
        {
            return min + (R.NextDouble() * (max - min));
        }
    }
}
