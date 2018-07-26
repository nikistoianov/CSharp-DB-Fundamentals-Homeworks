namespace Office.App.Core.Writers
{
    using System;
    using Contracts;

    public class ConsoleWriter : IWriter
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
