namespace Office.App.Core.Commands
{
    using Contracts;

    public class SetManagerCommand : BaseCommandManager, ICommand
    {
        public SetManagerCommand(IManagerController managerController) : base(managerController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 2);

            var employeeId = int.Parse(args[0]);
            var managerId = int.Parse(args[1]);
            this.managerController.SetManager(employeeId, managerId);

            return "Manager set.";
        }
    }
}
