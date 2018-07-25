namespace Office.App.Core
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Core.Contracts;
    using Office.Services.Contracts;

    public class Engine : IEngine
    {
        private readonly IServiceProvider serviceProvider;

        public Engine(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Run()
        {
            var initializeDb = this.serviceProvider.GetService<IDbInitializerService>();
            initializeDb.InitializeDatabase();

            var commandInterpreter = this.serviceProvider.GetService<ICommandInterpreter>();
            var reader = this.serviceProvider.GetService<IReader>();
            var writer = this.serviceProvider.GetService<IWriter>();

            string input = "";
            while ((input = reader.ReadLine()).ToLower() != "exit")
            {
                var inputTokens = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    var result = commandInterpreter.Read(inputTokens);
                    writer.WriteLine(result);
                }
                catch (ArgumentException ae)
                {
                    writer.WriteLine(ae.Message);
                }
            }
        }
    }
}
