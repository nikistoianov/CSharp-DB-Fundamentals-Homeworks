using Common.Generators;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Generators
{
    public class StudentGenerator
    {
        internal static void InitialStudentSeed(StudentSystemContext context, int count)
        {            
            for (int i = 0; i < count; i++)
            {
                var student = new Student()
                {
                    Name = TextGenerator.FirstName() + " " + TextGenerator.LastName(),
                    RegisteredOn = DateGenerator.PastDate()
                };

                context.Students.Add(student);
            }

            context.SaveChanges();
        }
    }
}
