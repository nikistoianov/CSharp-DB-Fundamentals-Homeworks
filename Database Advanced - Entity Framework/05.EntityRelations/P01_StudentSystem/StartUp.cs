using Common.Generators;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Data.Models.Enums;
using System;
using System.Linq;

namespace P01_StudentSystem
{
    class StartUp
    {
        static void Main()
        {
            //DatabaseInitializer.ResetDatabase();

            using (var db = new StudentSystemContext())
            {
                //DatabaseInitializer.InitialSeed(db);

                //AddResource(db);
                
                Output(db);
            }
        }

        private static void Output(StudentSystemContext db)
        {
            var resourses = db.Resources.Include(x => x.Course).ToArray();
            Console.WriteLine("Resources:");
            foreach (var res in resourses)
            {
                var line = $"  --> Name: {res.Name}, Type: {res.ResourceType}, Course Name: {res.Course.Name}";
                Console.WriteLine(line);
            }
            
            var students = db.Students
                .Include(x => x.HomeworkSubmissions)
                .ToArray();
            Console.WriteLine("Students:");
            foreach (var s in students)
            {
                var line = $"  --> Name: {s.Name}, RegisteredOn: {s.RegisteredOn.ToString()}";
                Console.WriteLine(line);
                foreach (var homework in s.HomeworkSubmissions)
                {
                    Console.WriteLine($"     --> Homework: {homework.Content}, {homework.ContentType}");
                }
            }

            var courses = db.Courses
                .ToArray();
            Console.WriteLine();
            Console.WriteLine("Courses:");
            foreach (var c in courses)
            {
                Console.WriteLine($"  --> StartDate: {c.StartDate}, EndDate: {c.EndDate}");
            }
        }
        
        private static void AddResource(StudentSystemContext db)
        {
            var res = new Resource()
            {
                Name = "Book",
                ResourceType = ResourceType.Other,
                Url = "https://softuni.bg/trainings/resources/officedocument/33522/exercise-problem-descriptions-databases-advanced-entity-framework-june-2018",
                Course = new Course()
                {
                    Name = "C#",
                    StartDate = DateTime.Now,
                    EndDate = new DateTime(2018, 11, 21),
                    Price = 19.99m
                }
            };

            db.Resources.Add(res);
            db.SaveChanges();
        }

    }
}
