﻿namespace Common.Generators
{
    using System;
    using System.IO;
    
    public class TextGenerator
    {
        private static string[] firstNames = { "Petur", "Ivan", "Georgi", "Alexander", "Stefan", "Vladimir", "Svetoslav", "Kaloyan", "Mihail", "Stamat" };
        //private static string[] firstNames = File.ReadAllLines("<INSERT DIR HERE>");
        private static string[] lastNames = { "Ivanov", "Georgiev", "Stefanov", "Alexandrov", "Petrov", "Stamatkov", };
        //private static string[] lastNames = File.ReadAllLines("<INSERT DIR HERE>");

        public static string FirstName => GenerateText(firstNames);
        public static string LastName => GenerateText(lastNames);
        public static string Text(params string[] texts) => GenerateText(texts);
        public static string FromFile(string file) => GenerateText(File.ReadAllLines(file));

        public string FirstNam => GenerateText(firstNames);

        private static string GenerateText(string[] names)
        {
            Random rnd = new Random();

            int index = rnd.Next(0, names.Length);

            string name = names[index];

            return name;
        }

        public static string Password(int passwordLength)
        {
            Random rnd = new Random();
            var result = "";
            for (int i = 0; i < passwordLength; i++)
            {
                var ch = (char)rnd.Next(48, 125);
                result += ch;
            }
            return result;
        }
    }
}
