using System;
using System.Data;
using System.Data.SqlClient;
using _01.InitialSetup;

namespace _09.IncreaseAgeStoredProcedure
{
    class StartUp
    {
        static void Main()
        {
            int inputId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase(Configuration.DatabaseName);

                //using (SqlCommand command = new SqlCommand("usp_GetOlder", connection))
                //{
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.Add(new SqlParameter("@minionId", SqlDbType.Int, 0, "Id"));
                //    command.Parameters[0].Value = inputId;
                //    command.ExecuteNonQuery();
                //}

                string getMinionSql = "SELECT [Name], Age FROM Minions WHERE Id = @id";

                using (SqlCommand command = new SqlCommand(getMinionSql, connection))
                {
                    command.Parameters.AddWithValue("@id", inputId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                    }
                }

                connection.Close();
            }
        }
    }
}
