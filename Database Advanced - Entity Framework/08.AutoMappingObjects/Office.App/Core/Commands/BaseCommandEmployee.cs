namespace Office.App.Core.Commands
{
    using Contracts;
    using System;

    public abstract class BaseCommandEmployee
    {
        protected readonly IEmployeeController employeeController;

        public BaseCommandEmployee(IEmployeeController employeeController)
        {
            this.employeeController = employeeController;
        }

        protected void ValidateArgsLength(string[] args, int length)
        {
            if (args.Length < length)
            {
                throw new ArgumentException("Not enough arguments!");
            }
        }
    }
}
