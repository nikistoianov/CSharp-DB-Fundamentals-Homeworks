namespace Office.App
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using AutoMapper;
    using Data;
    using Services;
    using Services.Contracts;
    using Core;
    using Core.Contracts;
    using Core.Controllers;
    using Core.Readers;
    using Core.Writers;

    public class StartUp
    {
        static void Main()
        {
            var service = ConfigureService();
            IEngine engine = new Engine(service);
            engine.Run();
        }

        private static IServiceProvider ConfigureService()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<OfficeContext>(opts => opts.UseSqlServer(DbContextConfiguration.ConnectionString));

            serviceCollection.AddTransient<IDbInitializerService, DbInitializerService>();
            serviceCollection.AddTransient<ICommandInterpreter, CommandInterpreter>();
            serviceCollection.AddTransient<IEmployeeController, EmployeeController>();
            serviceCollection.AddTransient<IReader, ConsoleReader>();
            serviceCollection.AddTransient<IWriter, ConsoleWriter>();
            serviceCollection.AddTransient<IManagerController, ManagerController>();

            serviceCollection.AddAutoMapper(conf => conf.AddProfile<OfficeProfile>());

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
