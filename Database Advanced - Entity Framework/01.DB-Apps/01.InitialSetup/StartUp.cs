namespace _01.InitialSetup
{
    using System.Data.SqlClient;

    class StartUp
    {
        static void Main()
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionString))
            {
                connection.Open();

                // Creating Database
                ExecNonQuerry("CREATE DATABASE " + Configuration.databaseName, connection);
                connection.ChangeDatabase(Configuration.databaseName);

                // Creating Tables
                ExecNonQuerry("CREATE TABLE Countries (Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50))", connection);
                ExecNonQuerry("CREATE TABLE Towns(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(50), CountryCode INT FOREIGN KEY REFERENCES Countries(Id))", connection);
                ExecNonQuerry("CREATE TABLE Minions(Id INT PRIMARY KEY IDENTITY,Name VARCHAR(30), Age INT, TownId INT FOREIGN KEY REFERENCES Towns(Id))", connection);
                ExecNonQuerry("CREATE TABLE EvilnessFactors(Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50))", connection);
                ExecNonQuerry("CREATE TABLE Villains (Id INT PRIMARY KEY IDENTITY, Name VARCHAR(50), EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id))", connection);
                ExecNonQuerry("CREATE TABLE MinionsVillains (MinionId INT FOREIGN KEY REFERENCES Minions(Id),VillainId INT FOREIGN KEY REFERENCES Villains(Id),CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId))", connection);

                // Filling tables with data
                ExecNonQuerry("INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')", connection);
                ExecNonQuerry("INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)", connection);
                ExecNonQuerry("INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)", connection);
                ExecNonQuerry("INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')", connection);
                ExecNonQuerry("INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)", connection);
                ExecNonQuerry("INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)", connection);

                connection.Close();
            }
        }

        private static void ExecNonQuerry(string commandString, SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand(commandString, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
