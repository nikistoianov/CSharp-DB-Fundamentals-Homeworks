using System.Linq;
using P01_StudentSystem.Data;
using P01_StudentSystem.Generators;

namespace P01_StudentSystem
{
    public class DatabaseInitializer
    {
        public static void ResetDatabase()
        {
            using (var context = new StudentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //context.Database.Migrate();

                InitialSeed(context);
            }
        }

        public static void InitialSeed(StudentSystemContext context)
        {
            StudentGenerator.InitialStudentSeed(context, 5);
            CourseGenerator.InitialCourseSeed(context, 10);

            var students = context.Students.ToList();
            var courses = context.Courses.ToList();
            HomeworkGenerator.InitialHomeworkSeed(context, 30, courses, students);
            
        }

    }
}
