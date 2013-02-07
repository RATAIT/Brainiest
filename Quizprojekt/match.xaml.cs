﻿using System;
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
    public partial class match : UserControl, ISwitchable
    {

        public match()
        {
            InitializeComponent();
            
            // Läser in fråga när man öppnar fönstret
            readQuestion();
        }

        string[] idFragaArray = new string[3]; // Array för pickThreeQuestions
        int kategoriID = kategori.id;
        string corAns; // Rätta svaret
        int fraga = 0; // Kollar vilken fråga man är på just nu
        List<string> idList = new List<string>(); // Lista med frågor från en viss kategori
        int antalID = 0;
        

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

            // Läser alla rader i databasen med kommandot givet ovan.
            while (myOleDbDataReader.Read())
            {
                // Lägger till ett ID i idList.
                idList.Add(Convert.ToString(myOleDbDataReader["FragorID"]));
                antalID++;
            }

            // Skickar vidare idList, antalID och fraga (vilken av de tre frågorna som ska skickas) till pickThreeQuestions metoden
            pickThreeQuestions(idList, antalID, fraga);
        }

        // Väljer ut tre frågor till en array från en list som läses in i readQuestion()
        private void pickThreeQuestions(List<string> idList, int antalID, int fraga)
        {
  
            // Om man svarat på tre frågor stängs fönstret.
            if (fraga > 2)
            {
                Switcher.Switch(new kategori());
            }
            // Hämtar ny fråga om man svarat på första frågan
            else if (fraga > 0)
                setQuestion(idFragaArray[fraga]);
            // Slumpar ut tre frågor från en lista som skapats i readQuestion()
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    Random random = new Random();
                    int num = random.Next(antalID);

                    //Tar FragorID på positionen num
                    idFragaArray[i] = idList.ElementAtOrDefault(num);
                    idList.RemoveAt(num);
                    antalID--;
                }
                setQuestion(idFragaArray[fraga]);
            }
        }

        // Skriver ut frågan + svarsalternativen
        private void setQuestion(string idFraga)
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
            startAniProg();
        }

        int aniOn = 0;
        private void startAniProg()
        {
            BeginStoryboard progBarStart = (BeginStoryboard)gridMatch.Resources["progBarStart"];
            PauseStoryboard progBarStop = (PauseStoryboard)gridMatch.Resources["progBarStop"];

            if (aniOn == 0)
            {
                
                progBarStart.Storyboard.Begin(progressBar1);
                aniOn++;
            }
            else if (aniOn == 1)
            {
                progBarStart.Storyboard.Pause(progressBar1);
                aniOn--;
            }
        }

        // När tiden gått ut i progressbaren sätts alla knappar till rött:
        private void progressBarAnimation_Completed(object sender, EventArgs e)
        {
            btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
            btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

            btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
            btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

            btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
            btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

            btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
            btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

            fragaOver();
        }

        // När man svarat på en fråga ska detta hända
        private void fragaOver()
        {
     
            // Om man ännu inte svarat på 3 frågor visas "Nästa fråga"-knappen
            if (fraga < 3)
            {
                if (fraga < 2)
                {
                    progressBar1.Visibility = Visibility.Visible;
                    btn_NastaFraga.Visibility = Visibility.Visible;
                    lbl_TidKvar.Visibility = Visibility.Hidden;
                }
                else
                {
                    progressBar1.Visibility = Visibility.Hidden;
                    btn_NastaFraga.Visibility = Visibility.Visible;
                    lbl_TidKvar.Visibility = Visibility.Hidden;
                    btn_NastaFraga.Content = "Tillbaka";
                }


            }
            // Annars går man tillbaka till kategori
            else
            {
                Switcher.Switch(new kategori());
             
            }

        }

        // Klick på "Nästa Fråga"-knappen
        private void btn_NastaFraga_Click(object sender, RoutedEventArgs e)
        {
            // Knappen döljs och progressbar+tidkvar kommer tillbaka
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            btn_NastaFraga.Visibility = System.Windows.Visibility.Collapsed;
            lbl_TidKvar.Visibility = Visibility.Visible;

            fraga++;

            // En ny fråga kommer fram
            pickThreeQuestions(idList, antalID, fraga);

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

            startAniProg();
            
            // Ändrar färgerna på fel svar till rött och rätt svar till grönt
            changeBtnCol(btnID, corAns);

            // Läser in en ny fråga
            fragaOver();
        }

        // Ändrar färgerna på fel svar till rött och rätt svar till grönt
        private void changeBtnCol(string btnID, string svar)
        {
            if (Convert.ToString(btn_Svar1.Content) == svar)
            {
                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (Convert.ToString(btn_Svar2.Content) == svar)
            {
                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (Convert.ToString(btn_Svar3.Content) == svar)
            {
                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
            else if (Convert.ToString(btn_Svar4.Content) == svar)
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


        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
