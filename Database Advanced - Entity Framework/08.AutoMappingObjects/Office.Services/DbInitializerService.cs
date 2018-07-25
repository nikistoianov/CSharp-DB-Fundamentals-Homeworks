namespace Office.Services
{
    using Data;
    using Contracts;
    using Microsoft.EntityFrameworkCore;

    public class DbInitializerService : IDbInitializerService
    {
        private readonly OfficeContext context;

        public DbInitializerService(OfficeContext context)
        {
            this.context = context;
        }

        public void InitializeDatabase()
        {
            this.context.Database.Migrate();
        }
    }
}
