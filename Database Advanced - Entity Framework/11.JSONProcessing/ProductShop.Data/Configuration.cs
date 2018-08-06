namespace ProductShop.Data
{
    using System;
    using System.IO;

    public class Configuration
    {
        public static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "ProductShop");
    }
}
