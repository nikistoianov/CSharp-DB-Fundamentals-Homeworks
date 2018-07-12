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
            using (var db = new StudentSystemContext())
            {
                db.Database.EnsureCreated();

                AddResource(db);

                AddHomework(db);

                var resourses = db.Resources.Include(x => x.Course).ToArray();
                Console.WriteLine("Resources:");
                foreach (var res in resourses)
                {
                    var line = $"  --> Name: {res.Name}, Type: {res.ResourceType}, Course Name: {res.Course.Name}";
                    Console.WriteLine(line);
                }

                var homeworks = db.HomeworkSubmissions.ToArray();
                Console.WriteLine("Homeworks:");
                foreach (var h in homeworks)
                {
                    var line = $"  --> Content: {h.Content}, Type: {h.ContentType}";
                    Console.WriteLine(line);
                }
            }
        }

        private static void AddHomework(StudentSystemContext db)
        {
            var homework = new Homework()
            {
                Content = "My first homework",
                ContentType = ContentType.Pdf,
                SubmissionTime = DateTime.Now,
                Student = new Student()
                {
                    Name = "Pesho",
                    RegisteredOn = new DateTime(2015, 5, 30)
                },
                Course = new Course()
                {
                    Name = "Java",
                    StartDate = DateTime.Now,
                    EndDate = new DateTime(2020, 1, 2),
                    Price = 9.99m
                }
            };

            db.HomeworkSubmissions.Add(homework);
            db.SaveChanges();
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
