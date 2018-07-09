using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace P02_DatabaseFirst
{
    public class StartUp
    {
        static void Main()
        {
            using (SoftUniContext context = new SoftUniContext())
            {
                var outputFile = "../output.txt";
                using (var writer = new StreamWriter(outputFile))
                {
                    EmployeesAndProjects(context, writer);
                }
                Process.Start("notepad.exe", outputFile);
            }
        }

        private static void EmployeesFullInformation(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees.ToArray();
            foreach (var employee in employees.OrderBy(x => x.EmployeeId))
            {
                writer.WriteLine($"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }
        }

        private static void EmployeesSalaryOver50000(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees
                .Where(x => x.Salary > 50000)
                .OrderBy(x => x.FirstName)
                .Select(x => new
                {
                    x.FirstName
                })
                .ToArray();
            foreach (var employee in employees)
            {
                var line = employee.FirstName;
                writer.WriteLine(line);
            }
        }

        private static void EmployeesFromResearchAndDevelopment(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees
                .Include(x => x.Department)
                .Where(x => x.Department.Name == "Research and Development")
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    DepartmentName = x.Department.Name,
                    x.Salary
                })
                .ToArray();

            foreach (var employee in employees)
            {
                var line = $"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}";
                writer.WriteLine(line);
            }
        }

        private static void AddNewAddressUpdateEmployee(SoftUniContext context, StreamWriter writer)
        {
            var address = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var employee = context.Employees
                .FirstOrDefault(x => x.LastName == "Nakov");
            employee.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Include(x => x.Address)
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .Select(x => new
                {
                    x.Address.AddressText
                })
                .ToArray();

            foreach (var e in employees)
            {
                var line = $"{e.AddressText}";
                writer.WriteLine(line);
            }
        }
        
        private static void EmployeesAndProjects(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees
                .Include(x => x.EmployeesProjects)
                .Where(x => x.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                //.Select(x => new
                //{
                //    x.FirstName
                //})
                .ToArray();

            foreach (var employee in employees)
            {
                var line = employee.FirstName;
                writer.WriteLine(line);
            }
        }

    }
}
