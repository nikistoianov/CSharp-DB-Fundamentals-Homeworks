namespace Office.App.Core.Contracts
{
    using DTOs;

    public interface IManagerController
    {
        void SetManager(int employeeId, int managerId);
        ManagerDto ManagerInfo(int employeeId);
    }
}
