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



        // Läser in frågor + svarsalternativ från databasen
        private void readQuestion()
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            myOleDbCommand.CommandText = "SELECT FragorID FROM Fragor";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            myOleDbDataReader.Read();
            while (myOleDbDataReader.Read())
            {
            
            
            
            
            }



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
