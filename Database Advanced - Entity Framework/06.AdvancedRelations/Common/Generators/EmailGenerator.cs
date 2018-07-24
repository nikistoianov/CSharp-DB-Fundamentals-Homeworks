namespace Common.Generators
{
	using System;
	
    public class EmailGenerator
    {
        private static Random rnd = new Random();

        private static string[] domains = { "mail.bg", "abv.bg", "gmail.com", "hotmail.com", "softuni.bg", "students.softuni.bg" };
        //private static string[] domains = File.ReadAllLines("<INSERT DIR HERE>");

        public static string NewEmail(string name)
        {
            string domain = domains[rnd.Next(domains.Length)];
            int number = rnd.Next(1, 2000);

            return $"{name.ToLower()}{number}@{domain}";
        }
    }
}
