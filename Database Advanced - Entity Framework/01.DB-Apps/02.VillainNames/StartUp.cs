namespace _02.VillainNames
{
    using _01.InitialSetup;
    using System;
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                connection.ChangeDatabase(Configuration.databaseName);

                var commandString = "SELECT v.[Name], COUNT(mv.MinionId) AS MinionsCount FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Id, v.[Name] HAVING COUNT(mv.MinionId) >= 3 ORDER BY MinionsCount DESC";

                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string villainName = reader[0].ToString();
                            int minionsCount = (int)reader[1];

                            Console.WriteLine($"{villainName} - {minionsCount}");
                        }
                    }
                }

                connection.Close();
            }
        }
    }
}
