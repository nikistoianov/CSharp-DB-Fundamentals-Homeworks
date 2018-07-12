using System;
using System.IO;

namespace P01_StudentSystem.Data
{
    internal static class Configuration
    {
        internal static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "StudentSystem");
    }
}
