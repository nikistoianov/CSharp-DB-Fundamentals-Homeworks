﻿namespace Office.App.Core.Contracts
{
    using System;
    using DTOs;

    public interface IEmployeeController
    {
        void AddEmployee(EmployeeDto employeeDto);

        void SetBirthday(int employeeId, DateTime date);

        void SetAddress(int employeeId, string address);

        EmployeeDto GetEmployeeInfo(int employeeId);

        EmployeeFullInfoDto GetEmployeePersonalInfo(int employeeId);
    }
}
