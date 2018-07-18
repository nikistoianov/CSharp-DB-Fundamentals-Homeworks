namespace Common.Generators
{
    using System;

    public class DateGenerator
    {
        public static DateTime PastDate() => GenerateDate(DateTime.Now, false);
        public static DateTime FutureDate() => GenerateDate(DateTime.Now, true);
        public static DateTime DateAfter(DateTime date) => GenerateDate(date, true);
        public static DateTime RandomDate() => GenerateDate(DateTime.Now.AddYears(-5), true);

        private static DateTime GenerateDate(DateTime initialDate, bool afterInitialDate)
        {
            Random rnd = new Random();

            long minutes = rnd.Next(0, 60 * 24 * 365 * 10);
            var date = initialDate;
            var result = date.AddMinutes(afterInitialDate ? minutes : -minutes);

            return result;
        }
    }
}
