namespace Office.App.Core.Commands
{
    using System;
    using Contracts;

    public class BaseCommandManager
    {
        protected readonly IManagerController managerController;

        public BaseCommandManager(IManagerController managerController)
        {
            this.managerController = managerController;
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
