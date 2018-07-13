using Common.Generators;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Generators
{
    public class CourseGenerator
    {
        internal static void InitialCourseSeed(StudentSystemContext context, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var date = DateGenerator.RandomDate();
                var course = new Course()
                {
                    Name = TextGenerator.FirstName(),
                    StartDate = date,
                    EndDate = DateGenerator.DateAfter(date),
                    Price = PriceGenerator.GeneratePrice()
                };

                context.Courses.Add(course);
            }

            context.SaveChanges();
        }
    }
}
