namespace Office.App.Core.Commands
{
    using System;
    using Contracts;
    using DTOs;

    public class AddEmployeeCommand : BaseCommandEmployee, ICommand
    {
        public AddEmployeeCommand(IEmployeeController employeeController) : base(employeeController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 3);

            var employeeDto = new EmployeeDto()
            {
                FirstName = args[0],
                LastName = args[1],
                Salary = decimal.Parse(args[2])
            };

            this.employeeController.AddEmployee(employeeDto);

            return $"Employee {employeeDto.FirstName} {employeeDto.LastName} added!";
        }
    }
}
