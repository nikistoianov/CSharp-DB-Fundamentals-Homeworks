namespace P01_BillsPaymentSystem
{
    using System.Linq;
    using Data;
    using Generators;

    public class DatabaseInitializer
    {
        public static void ResetDatabase()
        {
            using (var context = new BillsPaymentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                //context.Database.Migrate();

                InitialSeed(context);
            }
        }

        public static void InitialSeed(BillsPaymentSystemContext context)
        {
            UserGenerator.Seed(context, 20);

            var users = context.Users.ToList();
            PaymentMethodGenerator.Seed(context, 30, users);
        }
    }
}
