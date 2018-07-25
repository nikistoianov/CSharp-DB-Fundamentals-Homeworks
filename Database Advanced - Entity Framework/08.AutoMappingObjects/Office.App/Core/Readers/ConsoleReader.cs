namespace Office.App.Core.Readers
{
    using System;
    using Office.App.Core.Contracts;

    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}
