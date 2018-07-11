using P01_HospitalDatabase.Data;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase
{
    public class StartUp
    {
        static void Main()
        {
            using (var context = new HospitalContext())
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                var patient = new Patient()
                {
                    FirstName = "Pesho",
                    LastName = "Peshev",
                };

                context.Patients.Add(patient);
                context.SaveChanges();
            }
        }
    }
}
