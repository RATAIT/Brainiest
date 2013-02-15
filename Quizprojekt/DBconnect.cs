using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Quizprojekt
{
    public class DBconnect
    {

        public static MySqlCommand Command;
        public static MySqlDataReader DataReader;
        public static MySqlConnection Connection;


        public static void openDB(string cmdText)
        {

            // Connectionstringen för att connecta till databasen.
            string connectionString = @"Server=projweb.hj.se;Database=test;Uid=liad11am;Pwd=Pnlx717;Port=3306;";

            Connection = new MySqlConnection(connectionString);

            Command = Connection.CreateCommand();
            Command.CommandText = cmdText;
            Connection.Open();
            DataReader = Command.ExecuteReader();



        }
    }
}
