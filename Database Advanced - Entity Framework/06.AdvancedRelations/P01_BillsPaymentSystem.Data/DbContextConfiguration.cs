using System;
using System.IO;

namespace P01_BillsPaymentSystem.Data
{
    internal static class DbContextConfiguration
    {
        internal static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "BillsPaymentSystem");
    }
}
