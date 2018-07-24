namespace Common.Generators
{
    using System;

    public class IntGenerator
    {
        public static int GenerateInt(int min, int max)
        {
            Random rnd = new Random();
            var num = rnd.Next(min, max);

            return num;
        }
    }
}
