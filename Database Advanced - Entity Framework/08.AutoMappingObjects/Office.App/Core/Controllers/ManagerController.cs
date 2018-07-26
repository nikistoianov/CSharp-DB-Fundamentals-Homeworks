namespace Office.App.Core.Controllers
{
    using System;
    using System.Linq;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Contracts;
    using DTOs;
    using Data;
    using Models;

    public class ManagerController : IManagerController
    {
        private readonly OfficeContext db;
        private readonly IMapper mapper;

        public ManagerController(OfficeContext context, IMapper mapper)
        {
            this.db = context;
            this.mapper = mapper;
        }

        public void SetManager(int employeeId, int managerId)
        {
            var employee = GetEmployee(employeeId);
            var manager = GetEmployee(managerId);
            employee.Manager = manager;
            db.SaveChanges();
        }

        public ManagerDto ManagerInfo(int employeeId)
        {
            var manager = GetEmployee(employeeId);
            return this.mapper.Map<ManagerDto>(manager);
        }

        private Employee GetEmployee(int employeeId)
        {
            var employee = this.db.Employees
                .Where(x => x.Id == employeeId)
                .Include(x => x.ManagerEmployees)
                .FirstOrDefault();

            if (employee == null)
            {
                throw new ArgumentException($"Employee with id: {employeeId} not found!");
            }

            return employee;
        }

    }
}
