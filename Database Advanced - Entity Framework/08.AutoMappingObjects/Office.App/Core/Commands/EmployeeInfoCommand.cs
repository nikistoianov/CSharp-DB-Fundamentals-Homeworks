namespace Office.App.Core.Commands
{
    using Contracts;

    public class EmployeeInfoCommand : BaseCommandEmployee, ICommand
    {
        public EmployeeInfoCommand(IEmployeeController employeeController) : base(employeeController)
        {}

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 1);

            var id = int.Parse(args[0]);
            var employeeDto = this.employeeController.GetEmployeeInfo(id);

            return $"ID: {employeeDto.Id} - {employeeDto.FirstName} {employeeDto.LastName} - ${employeeDto.Salary:F2}";
        }
    }
}
