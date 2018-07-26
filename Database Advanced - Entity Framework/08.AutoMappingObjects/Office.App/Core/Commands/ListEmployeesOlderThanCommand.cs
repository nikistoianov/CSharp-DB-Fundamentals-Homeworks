namespace Office.App.Core.Commands
{
    using System.Text;
    using Contracts;

    public class ListEmployeesOlderThanCommand : BaseCommandEmployee, ICommand
    {
        public ListEmployeesOlderThanCommand(IEmployeeController employeeController) : base(employeeController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 1);

            var age = int.Parse(args[0]);
            var employees = this.employeeController.ListEmployeesOlderThan(age);

            var result = new StringBuilder();
            if (employees.Count == 0)
            {
                result.AppendLine($"No employees older than {age} found!");
            }
            foreach (var e in employees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} - ${e.Salary:F2} - Manager: {(e.ManagerLastName == null ? "[no manager]" : e.ManagerLastName)}");
            }
            return result.ToString().TrimEnd();
        }
    }
}
