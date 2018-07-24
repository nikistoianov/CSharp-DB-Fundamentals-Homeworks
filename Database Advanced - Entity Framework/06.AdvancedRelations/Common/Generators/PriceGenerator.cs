namespace Common.Generators
{
    using System;

    public class PriceGenerator
    {
        public static decimal GeneratePrice()
        {
            Random rnd = new Random();
            var num = rnd.Next(0, 10000);
            var result = (decimal)num / 100;

            return result;
        }
    }
}
