namespace Office.App.Core.Commands
{
    using System;
    using Contracts;

    public class SetAddressCommand : BaseCommandEmployee, ICommand
    {
        public SetAddressCommand(IEmployeeController employeeController) : base(employeeController)
        {}

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 2);

            var id = int.Parse(args[0]);
            var address = args[1];
            this.employeeController.SetAddress(id, address);

            return $"Address set.";
        }
    }
}
