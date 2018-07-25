using AutoMapper;
using Office.App.Core.Contracts;
using Office.App.Core.DTOs;
using Office.Data;
using Office.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Office.App.Core.Controllers
{
    public class EmployeeController : IEmployeeController
    {
        private readonly OfficeContext db;
        private readonly IMapper mapper;

        public EmployeeController(OfficeContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public void AddEmployee(EmployeeDto employeeDto)
        {
            var employee = this.mapper.Map<Employee>(employeeDto);
            this.db.Employees.Add(employee);
            this.db.SaveChanges();
        }

        public EmployeeDto GetEmployeeInfo(int employeeId)
        {
            Employee employee = GetEmployee(employeeId);
            return this.mapper.Map<EmployeeDto>(employee);
        }

        public EmployeeFullInfoDto GetEmployeePersonalInfo(int employeeId)
        {
            Employee employee = GetEmployee(employeeId);
            return this.mapper.Map<EmployeeFullInfoDto>(employee);
        }

        public void SetAddress(int employeeId, string address)
        {
            Employee employee = GetEmployee(employeeId);
            employee.Address = address;
            this.db.SaveChanges();
        }

        public void SetBirthday(int employeeId, DateTime date)
        {
            var employee = GetEmployee(employeeId);
            employee.Birthday = date;
            this.db.SaveChanges();
        }

        private Employee GetEmployee(int employeeId)
        {
            var employee = this.db.Employees.Find(employeeId);
            if (employee == null)
            {
                throw new ArgumentException($"Employee with id: {employeeId} not found!");
            }

            return employee;
        }

    }
}
