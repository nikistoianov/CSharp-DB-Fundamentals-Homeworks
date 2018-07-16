using System;
using System.IO;

namespace P03_FootballBetting.Data
{
    internal static class Configuration
    {
        internal static string ConnectionString => String.Format(File.ReadAllText(@"C:\Program Files\Microsoft SQL Server\MSSQL_connectionString.txt"), "FootballBetting");
    }
}
