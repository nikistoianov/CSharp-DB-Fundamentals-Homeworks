using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Office.Models;

namespace Office.Data
{
    public class OfficeContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public OfficeContext(DbContextOptions options) : base(options)
        {}

        public OfficeContext()
        {}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer(DbContextConfiguration.ConnectionString);
        //    }
        //}
    }
}
