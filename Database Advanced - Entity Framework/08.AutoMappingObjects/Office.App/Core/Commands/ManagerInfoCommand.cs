namespace Office.App.Core.Commands
{
    using System.Text;
    using Contracts;

    public class ManagerInfoCommand : BaseCommandManager, ICommand
    {
        public ManagerInfoCommand(IManagerController managerController) : base(managerController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 1);

            var id = int.Parse(args[0]);
            var manager = this.managerController.ManagerInfo(id);

            var result = new StringBuilder();
            result.AppendLine($"{manager.FirstName} {manager.LastName} | Employees: {manager.Employees.Count}");
            foreach (var employee in manager.Employees)
            {
                result.AppendLine($" - {employee.FirstName} {employee.LastName} - ${employee.Salary:F2}");
            }
            return result.ToString().TrimEnd();
        }
    }
}
