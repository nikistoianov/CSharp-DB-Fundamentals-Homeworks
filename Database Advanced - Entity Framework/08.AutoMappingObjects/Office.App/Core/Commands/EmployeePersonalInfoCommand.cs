namespace Office.App.Core.Commands
{
    using System;
    using System.Text;
    using Contracts;

    public class EmployeePersonalInfoCommand : BaseCommandEmployee, ICommand
    {
        public EmployeePersonalInfoCommand(IEmployeeController employeeController) : base(employeeController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 1);

            var id = int.Parse(args[0]);
            var e = this.employeeController.GetEmployeePersonalInfo(id);
            var dateString = e.Birthday == null ? "" : e.Birthday.Value.ToString("dd-MM-yyyy");

            var result = new StringBuilder();
            result.AppendLine($"ID: {e.Id} - {e.FirstName} {e.LastName} - ${e.Salary:F2}");
            result.AppendLine($"Birthday: {dateString}");
            result.AppendLine($"Address: {e.Address}");

            return result.ToString().TrimEnd();
        }
    }
}
