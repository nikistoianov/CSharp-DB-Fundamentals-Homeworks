using MiniORM.App.Data;
using MiniORM.App.Data.Entities;
using System.IO;
using System.Linq;

namespace MiniORM.App
{
    class StartUp
    {
        static void Main()
        {
            var connectionString = File.ReadAllText(@"F:\base\MSSQL_connectionString.txt");
            var context = new SoftUniDbContext(connectionString);

            context.Employees.Add(new Employee
            {
                FirstName = "King",
                LastName = "Zerkan",
                IsEmployed = true
            });

            context.Projects.Add(new Project
            {
                Name = "JavaScript Project"
            });
            
            //context.EmployeesProjects.Add(new EmployeeProject
            //{
            //    ProjectId = context.Projects.Last().Id,
            //    EmployeeId = context.Employees.Last().Id
            //});

            var employee = context.Employees.Last();
            employee.DepartmentId = context.Departments.First().Id;

            context.SaveChanges();
        }
    }
}
