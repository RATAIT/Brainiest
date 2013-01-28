using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace UnitTest
{
    class Program
    {


        static void Main(string[] args)
        {
       

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT * FROM Fragor WHERE KategoriID = 2";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();


            List<string> idList = new List<string>(); // Deklarerar en lista.

            // Läser alla rader i databasen med kommandot givet ovan.
            while (myOleDbDataReader.Read())
            {

                // Lägger till ett ID i idList.
                idList.Add(Convert.ToString(myOleDbDataReader["FragorID"]));
            }

        
            foreach (string idNumber in idList) // Display for verification
            {
                Console.WriteLine("ID: " + idNumber);
            }


            Console.WriteLine("Det specifika på den plats du angivit är " + idList.ElementAtOrDefault(5));

            Console.ReadKey();




         
   


            




          
            }
        }
    }

