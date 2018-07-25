namespace Office.Data
{
    using System;
    using System.IO;

    public static class DbContextConfiguration
    {
        public static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "Office");
    }
}
