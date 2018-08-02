namespace ProductShop.Data
{
    using System;
    using System.IO;

    internal static class Configuration
    {
        internal static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "ProductShop");
    }
}
