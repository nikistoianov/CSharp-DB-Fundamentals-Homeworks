namespace Office.App.Core.Commands
{
    using System;
    using System.Globalization;
    using Contracts;

    public class SetBirthdayCommand : BaseCommandEmployee, ICommand
    {
        public SetBirthdayCommand(IEmployeeController employeeController) : base(employeeController)
        { }

        public string Execute(string[] args)
        {
            ValidateArgsLength(args, 2);

            var id = int.Parse(args[0]);
            var date = DateTime.ParseExact(args[1], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            employeeController.SetBirthday(id, date);

            return $"Birthday set.";
        }
    }
}
