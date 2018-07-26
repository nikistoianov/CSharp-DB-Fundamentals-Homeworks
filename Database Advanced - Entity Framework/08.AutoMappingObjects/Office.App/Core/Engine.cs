namespace Office.App.Core
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Contracts;
    using Services.Contracts;
    using AutoMapper;
    using Models;
    using DTOs;

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

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeInfoDto>()
                   .ForMember(dest => dest.ManagerLastName, opt => opt.MapFrom(e => e.Manager.LastName))
                   .ReverseMap();
            });

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
