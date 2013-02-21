using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Quizprojekt
{
    public class DBconnect
    {
        // hemlig KODDDD
        // tåbbe är en liten fisk


        public static MySqlCommand Command;
        public static MySqlDataReader DataReader;
        public static MySqlConnection Connection;


        public static void openDB(string cmdText)
        {

            // Connectionstringen för att connecta till databasen.
            string connectionString = @"Server=83.168.226.169;Database=db1131745_BrainiestDB;Uid=u1131745_admin;Pwd=kidco[0lao;Port=3306;";

            Connection = new MySqlConnection(connectionString);

            Command = Connection.CreateCommand();
            Command.CommandText = cmdText;
            Connection.Open();
            DataReader = Command.ExecuteReader();



        }
    }
}
