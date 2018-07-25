namespace Office.App.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Contracts;

    public class CommandInterpreter : ICommandInterpreter
    {
        private const string InvalidCommandMessage = "Invalid Command!";

        private readonly IServiceProvider serviceProvider;

        public CommandInterpreter(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public string Read(string[] inputTokens)
        {
            if (inputTokens.Length == 0)
            {
                throw new ArgumentException("No input!");
            }

            var commandName = inputTokens[0] + "Command";
            var commandArgs = inputTokens.Skip(1).ToArray();

            var commandType = Assembly.GetCallingAssembly()
                .GetTypes()
                .FirstOrDefault(c => c.Name == commandName);

            if (commandType == null)
            {
                throw new ArgumentException(InvalidCommandMessage);
            }

            var constructor = commandType.GetConstructors().First();

            var constructorParams = constructor.GetParameters()
                .Select(x => x.ParameterType)
                .ToArray();

            var service = constructorParams
                .Select(serviceProvider.GetService)
                .ToArray();

            var command = (ICommand)constructor.Invoke(service);
            var result = command.Execute(commandArgs);

            return result;
        }
    }
}
