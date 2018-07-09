namespace _03.MinionNames
{
    using System;
    using System.Data.SqlClient;
    using _01.InitialSetup;

    class StartUp
    {
        static void Main()
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase(Configuration.DatabaseName);

                var villainName = GetVillainName(connection, villainId);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villian: {villainName}");

                    string minionNamesSql = "SELECT m.[Name], m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id WHERE mv.VillainId = @id ORDER BY m.[Name] ASC";

                    using (SqlCommand command = new SqlCommand(minionNamesSql, connection))
                    {
                        command.Parameters.AddWithValue("id", villainId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                int num = 1;

                                while (reader.Read())
                                {
                                    string minionName = reader[0].ToString();
                                    string minionAge = reader[1].ToString();

                                    Console.WriteLine($"{num}. {minionName} {minionAge}");

                                    num++;
                                }
                            }
                            else
                            {
                                Console.WriteLine("(no minions)");
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        private static string GetVillainName(SqlConnection connection, int villainId)
        {
            var villainName = String.Empty;
            var query = "SELECT [Name] FROM Villains WHERE Id = @id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", villainId);
                villainName = (string) command.ExecuteScalar();
            }
            return villainName;
        }
    }
}
