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
        int kategoriID = kategori.kategoriID.id;
        string corAns;

        // Läser in alla frågeIDn från en specifik kategori.
        private void readQuestion()
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            //Query för att hämta alla för en viss kategori
            myOleDbCommand.CommandText = "SELECT * FROM Fragor WHERE KategoriID = " + kategoriID;
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();



            List<string> idList = new List<string>(); // Deklarerar en lista.

            int antalID = 0;
            // Läser alla rader i databasen med kommandot givet ovan.
            while (myOleDbDataReader.Read())
            {
                // Lägger till ett ID i idList.
                idList.Add(Convert.ToString(myOleDbDataReader["FragorID"]));
                antalID++;
            }

            Random random = new Random();
            int num = random.Next(antalID);

            //Tar FragorID på positionen num
            idFraga = idList.ElementAtOrDefault(num);

            setQuestion();
        }

        // Skriver ut frågan + svarsalternativen
        private void setQuestion()
        {
            changeBtnColReset();

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            //Query för att hämta allt från en viss fråga
            myOleDbCommand.CommandText = "SELECT * FROM Fragor WHERE FragorID = " + idFraga;
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            myOleDbDataReader.Read();

            // Lägger in frågan
            string fraga = Convert.ToString(myOleDbDataReader["Fraga"]);
            txtbox_Fraga.Text = fraga;

            string[] fragArray = new string[4];

            // Slumpar talen 1-4 in i en array
            var rng = new Random();
            var values = Enumerable.Range(0, 4).OrderBy(x => rng.Next()).ToArray();
            int first = values[0];
            int second = values[1];
            int third = values[2];
            int fourth = values[3];

            // Lägger in svaren från databasen in på den slumpade platsen
            fragArray[first] = Convert.ToString(myOleDbDataReader["Korrekt"]);
            fragArray[second] = Convert.ToString(myOleDbDataReader["Fel1"]);
            fragArray[third] = Convert.ToString(myOleDbDataReader["Fel2"]);
            fragArray[fourth] = Convert.ToString(myOleDbDataReader["Fel3"]);

            // Sparar det rätta svaret i corAns, för att kunna kollas senare
            corAns = Convert.ToString(myOleDbDataReader["Korrekt"]);

            // Fyller knapparna med svaren
            btn_Svar1.Content = fragArray[0];
            btn_Svar2.Content = fragArray[1];
            btn_Svar3.Content = fragArray[2];
            btn_Svar4.Content = fragArray[3];

            myOleDbDataReader.Close();
            myOleDbConnection.Close();

            //Startar om progressbaren
            progBarStart.Begin(progressBar1);
        }


        // När tiden gått ut i progressbaren:
        private void progressBarAnimation_Completed(object sender, EventArgs e)
        {

            //MessageBox.Show("Tiden är ute.");
        }

        //Beroende på vad man svarat så skickas denna text till checkAnswer()
        private void btn_Svar1_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar1.Content);
            checkAnswer(content, "1");
        }

        private void btn_Svar2_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar2.Content);
            checkAnswer(content, "2");
        }

        private void btn_Svar3_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar3.Content);
            checkAnswer(content, "3");
        }

        private void btn_Svar4_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar4.Content);
            checkAnswer(content, "4");
        }

        //Kollar om ditt svar och det rätta svaret är samma
        private void checkAnswer(string btnAnswer, string btnID)
        {
            // Ändrar färgerna på fel svar till rött och rätt svar till grönt
            changeBtnCol(btnID, corAns);

            // Berättar om vi fått fel eller rätt
            if (corAns == btnAnswer)
                MessageBox.Show("RÄTT");
            else
                MessageBox.Show("FEL");

            // Läser in en ny fråga
            readQuestion();
        }

        // Ändrar färgerna på fel svar till rött och rätt svar till grönt
        private void changeBtnCol(string btnID, string svar)
        {
            string content1 = Convert.ToString(btn_Svar1.Content);
            string content2 = Convert.ToString(btn_Svar2.Content);
            string content3 = Convert.ToString(btn_Svar3.Content);
            string content4 = Convert.ToString(btn_Svar4.Content);
            
            if (content1 == svar)
            {
                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (content2 == svar)
            {
                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (content3 == svar)
            {
                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (content4 == svar)
            {
                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
        }

        // Återställer färgerna på knapparna
        private void changeBtnColReset()
        {
            btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
            btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));

            btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
            btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));

            btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
            btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));

            btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
            btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));
        }


    }
}
