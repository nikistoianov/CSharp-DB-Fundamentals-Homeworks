using Microsoft.EntityFrameworkCore;
using P02_DatabaseFirst.Data.Models;
using System;
using System.Diagnostics;
using System.Globalization;
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
                    DeleteProjectById(context, writer);
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
                //.Include(x => x.EmployeesProjects)
                .Where(x => x.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001 && ep.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    managerFirstName = x.Manager.FirstName,
                    managerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        p.Project.Name,
                        p.Project.StartDate,
                        p.Project.EndDate
                    })
                })
                .Take(30)
                .ToArray();

            foreach (var employee in employees)
            {
                var line = $"{employee.FirstName} {employee.LastName} - Manager: {employee.managerFirstName} {employee.managerLastName}";
                writer.WriteLine(line);
                foreach (var project in employee.Projects)
                {
                    string startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    string endDate = project.EndDate == null ? "not finished" : project.EndDate?.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    line = $"--{project.Name} - {startDate} - {endDate}";
                    writer.WriteLine(line);
                }
            }
        }

        private static void AddressesByTown(SoftUniContext context, StreamWriter writer)
        {
            var addresses = context.Addresses
                //.Where(x => x.Department.Name == "Research and Development")
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count
                })
                .Take(10)
                .ToArray();

            foreach (var a in addresses)
            {
                var line = $"{a.AddressText}, {a.TownName} - {a.EmployeeCount} employees";
                writer.WriteLine(line);
            }
        }

        private static void Employee147(SoftUniContext context, StreamWriter writer)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects
                    .Select(p => new
                    {
                        p.Project.Name
                    })
                    .OrderBy(p => p.Name)
                })
                .FirstOrDefault();

            var line = $"{employee.FirstName} {employee.LastName} - {employee.JobTitle}";
            writer.WriteLine(line);
            foreach (var project in employee.Projects)
            {
                line = $"{project.Name}";
                writer.WriteLine(line);
            }
        }

        private static void DepartmentWithMoreThan5Employees(SoftUniContext context, StreamWriter writer)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    Employees = d.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                })
                .ToArray();

            foreach (var d in departments)
            {
                var line = $"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}";
                writer.WriteLine(line);
                foreach (var e in d.Employees)
                {
                    line = $"{e.FirstName} {e.LastName} - {e.JobTitle}";
                    writer.WriteLine(line);
                }
                writer.WriteLine(new string('-', 10));
            }
        }

        private static void FindLatest10Projects(SoftUniContext context, StreamWriter writer)
        {
            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                //.Where(p => p.StartDate < DateTime.Now)
                .OrderBy(p => p.Name)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .ToList();

            foreach (var project in projects)
            {
                writer.WriteLine(project.Name);
                writer.WriteLine(project.Description);
                writer.WriteLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }
        }

        private static void IncreaseSalaries(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                            || e.Department.Name == "Tool Design"
                            || e.Department.Name == "Marketing"
                            || e.Department.Name == "Information Services")
                .ToArray();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12m;
            }
            context.SaveChanges();

            var updEmployees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                            || e.Department.Name == "Tool Design"
                            || e.Department.Name == "Marketing"
                            || e.Department.Name == "Information Services")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in updEmployees)
            {
                var line = $"{e.FirstName} {e.LastName} (${e.Salary:F2})";
                writer.WriteLine(line);
            }
        }

        private static void FinfEmployeeByFirstName(SoftUniContext context, StreamWriter writer)
        {
            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToArray();

            foreach (var e in employees)
            {
                var line = $"{e.FirstName} {e.LastName} - {e.JobTitle} - (${e.Salary:F2})";
                writer.WriteLine(line);
            }
        }

        private static void DeleteProjectById(SoftUniContext context, StreamWriter writer)
        {
            var employeesProjects = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);
            context.EmployeesProjects.RemoveRange(employeesProjects);

            var project = context.Projects.Find(2);
            context.Projects.Remove(project);

            context.SaveChanges();

            var projects = context.Projects.Take(10);
            foreach (var p in projects)
            {
                writer.WriteLine(p.Name);
            }
        }
    }
}
