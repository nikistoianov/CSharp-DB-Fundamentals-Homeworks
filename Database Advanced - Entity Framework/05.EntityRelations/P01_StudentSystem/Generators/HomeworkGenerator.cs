using Common.Generators;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using P01_StudentSystem.Data.Models.Enums;
using System.Collections.Generic;

namespace P01_StudentSystem.Generators
{
    public class HomeworkGenerator
    {
        internal static void InitialHomeworkSeed(StudentSystemContext context, int count, List<Course> courses, List<Student> students)
        {
            for (int i = 0; i < count; i++)
            {
                context.HomeworkSubmissions.Add(GenerateHomework(courses, students));
            }

            context.SaveChanges();
        }

        internal static Homework GenerateHomework(List<Course> courses, List<Student> students)
        {
            var homework = new Homework()
            {
                Content = TextGenerator.Text("Lab", "Exercise", "Exam", "Other stuff"),
                SubmissionTime = DateGenerator.RandomDate(),
                ContentType = (ContentType)IntGenerator.GenerateInt(1, 3),
                Course = courses[IntGenerator.GenerateInt(0, courses.Count - 1)],
                Student = students[IntGenerator.GenerateInt(0, students.Count - 1)]
            };
            return homework;
        }
    }
}
