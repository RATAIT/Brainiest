using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Media.Animation;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for match.xaml
    /// </summary>
    public partial class match : Window
    {




        public match()
        {
            InitializeComponent();

            // Läser in fråga när man öppnar fönstret
            readQuestion();
        }

        string idFraga;


        // Läser in alla frågeIDn från en specifik kategori.
        private void readQuestion()
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT * FROM Fragor WHERE KategoriID = 5";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();



            List<string> idList = new List<string>(); // Deklarerar en lista.

            // Läser alla rader i databasen med kommandot givet ovan.
            while (myOleDbDataReader.Read())
            {

                // Lägger till ett ID i idList.
                idList.Add(Convert.ToString(myOleDbDataReader["FragorID"]));
            }

            Random random = new Random();
            int num = random.Next(10);

            idFraga = idList.ElementAtOrDefault(num);


            setQuestion();

        }



        // Skriver ut frågan + svarsalternativen
        private void setQuestion()
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT * FROM Fragor WHERE FragorID = " + idFraga;
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            myOleDbDataReader.Read();


            string fraga = Convert.ToString(myOleDbDataReader["Fraga"]);
            txtbox_Fraga.Text = fraga;

            string korrekt = Convert.ToString(myOleDbDataReader["Korrekt"]);
            btn_Svar1.Content = korrekt;

            string fel1 = Convert.ToString(myOleDbDataReader["Fel1"]);
            btn_Svar2.Content = fel1;

            string fel2 = Convert.ToString(myOleDbDataReader["Fel2"]);
            btn_Svar3.Content = fel2;

            string fel3 = Convert.ToString(myOleDbDataReader["Fel3"]);
            btn_Svar4.Content = fel3;

            myOleDbDataReader.Close();
            myOleDbConnection.Close();
        }


        // När tiden gått ut i progressbaren:
        private void progressBarAnimation_Completed(object sender, EventArgs e)
        {
            MessageBox.Show("Tiden är ute.");
        }

    }
}
