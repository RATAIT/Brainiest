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
using System.Media;
using MySql.Data.MySqlClient;
using System.Windows.Threading;

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

            // Vid load körs denna metod, för att starta timer
            Loaded += new RoutedEventHandler(match_Loaded);

            On1.Visibility = Visibility.Visible;
            Off1.Visibility = Visibility.Hidden;
        }

        string[] idFragaArray = new string[3]; // Array för pickThreeQuestions
        public string[] arrayFragorRunda = new string[3]; // Array för att fylla frågor, valda av en spelare, från databasen för att sedan visas
        int kategoriID = UserName.kategoriID; // Hämtar vilken kategori som valts
        string corAns; // Rätta svaret
        int fraga = 0; // Kollar vilken fråga man är på just nu
        List<string> idList = new List<string>(); // Lista med frågor från en viss kategori
        int antalID = 0; // Används för att kolla hur många frågor som är lagda i listan
        int questAnswered = 0; // Variabel för att kolla om frågan är svarad
        int mute = 0; // För mute-funktion
        int fragaNummer; // Kollar vilken fråga du är på just nu ( till indikatorn )
        int antalRatt; // Kollar hur många rätt man har just nu i denna rundan, för att fylla databasen med antal rätt

       
        // Läser in alla frågeIDn från en specifik kategori.
        private void readQuestion()
        {
            //Query för att hämta alla för en viss kategori
            DBconnect.openDB("SELECT * FROM Fragor WHERE KategoriID = " + kategoriID);

            // Läser alla rader i databasen med kommandot givet ovan.
            while (DBconnect.DataReader.Read())
            {
                // Lägger till ett ID i idList.
                idList.Add(Convert.ToString(DBconnect.DataReader["FragorID"]));
                antalID++;
            }

            DBconnect.Connection.Close();

            // Skickar vidare idList, antalID och fraga (vilken av de tre frågorna som ska skickas) till pickThreeQuestions metoden
            pickThreeQuestions(idList, antalID, fraga);
        }

        // Väljer ut tre frågor till en array från en list som läses in i readQuestion()
        private void pickThreeQuestions(List<string> idList, int antalID, int fraga)
        {
  
            // Om man svarat på tre frågor stängs fönstret.
            if (fraga > 2)
            {
                fraga = 0;
                questAnswered = 0;

                timer.Stop();

                // En lång if-sats för att kolla vem som har spelat och vem som inte har, allt för logiken när vi spelar
                DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                DBconnect.DataReader.Read();
             
                if (UserName.spelarNummer == "1")
                {
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("UPDATE `Match` SET Spelare1Spelat = 1 WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                }

                else // Spelare2
                {
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("UPDATE `Match` SET Spelare2Spelat = 1 WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                
                }

                DBconnect.DataReader.Close();
                DBconnect.Connection.Close();
                DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                DBconnect.DataReader.Read();

                if (Convert.ToString(DBconnect.DataReader["Spelare1Spelat"]) == "1" && Convert.ToString(DBconnect.DataReader["Spelare2Spelat"]) == "1")
                {
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("UPDATE `Match` SET Runda = Runda+" + 1 + " WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    changeRating();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("UPDATE `Match` SET Spelare1Spelat = 0 WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("UPDATE `Match` SET Spelare2Spelat = 0 WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();
                
                }

                Switcher.Switch(new Meny());
            }

            // Hämtar ny fråga om man svarat på första frågan.
            else if (fraga > 0)
            {
                setQuestion(arrayFragorRunda[fraga]);
            }

            // Slumpar ut tre frågor från en lista som skapats i readQuestion()
            else
            {
                DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                DBconnect.DataReader.Read();

                if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) != "TOM")
                {
                    DBconnect.DataReader.Close();
                    splitFragor();          
                }
                else if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) == "TOM")
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

                    DBconnect.openDB("UPDATE `Match` SET FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"]) + " = '" + idFragaArray[0] + "_" + idFragaArray[1] + "_" + idFragaArray[2] + "' WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();
                    splitFragor();
                }

                DBconnect.DataReader.Close();
                DBconnect.Connection.Close();

                setQuestion(arrayFragorRunda[fraga]);
            }
        }

        // Metod för att ändra rating
        private void changeRating()
        {
            DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
            DBconnect.DataReader.Read();

            if (Convert.ToString(DBconnect.DataReader["Runda"]) == "4")
            {

                if (Convert.ToInt16(DBconnect.DataReader["ResultatSpelare1"]) > Convert.ToInt16(DBconnect.DataReader["ResultatSpelare2"]))
                {

                    // Lägg till rating till spelare1
                    DBconnect.openDB("UPDATE `Medlemmar` SET Rating = Rating+" + (Convert.ToInt16(DBconnect.DataReader["ResultatSpelare1"]) - Convert.ToInt16(DBconnect.DataReader["ResultatSpelare2"])) * 10 + " WHERE MedlemmarID = " + Convert.ToInt16(DBconnect.DataReader["Spelare1"]));
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();

                    // Tar bort rating från spelare2
                    DBconnect.openDB("UPDATE `Medlemmar` SET Rating = Rating-" + (Convert.ToInt16(DBconnect.DataReader["ResultatSpelare1"]) - Convert.ToInt16(DBconnect.DataReader["ResultatSpelare2"])) * 10 + " WHERE MedlemmarID = " + Convert.ToInt16(DBconnect.DataReader["Spelare2"]));
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                }
                else
                {
                    // Lägg till rating till spelare2
                    DBconnect.openDB("UPDATE `Medlemmar` SET Rating = Rating+" + (Convert.ToInt16(DBconnect.DataReader["ResultatSpelare2"]) - Convert.ToInt16(DBconnect.DataReader["ResultatSpelare1"])) * 10 + " WHERE MedlemmarID = " + Convert.ToInt16(DBconnect.DataReader["Spelare2"]));
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();

                    // Tar bort rating från spelare1
                    DBconnect.openDB("UPDATE `Medlemmar` SET Rating = Rating-" + (Convert.ToInt16(DBconnect.DataReader["ResultatSpelare2"]) - Convert.ToInt16(DBconnect.DataReader["ResultatSpelare1"])) * 10 + " WHERE MedlemmarID = " + Convert.ToInt16(DBconnect.DataReader["Spelare1"]));
                    DBconnect.DataReader.Read();

                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                }
            }
        }

        private void splitFragor()
        {
            DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
            DBconnect.DataReader.Read();

            string fragorTillArray = Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]);

            arrayFragorRunda = fragorTillArray.Split('_');

            DBconnect.DataReader.Close();
            DBconnect.Connection.Close();
        }

        // Skriver ut frågan + svarsalternativen
        private void setQuestion(string idFraga)
        {
            changeBtnColReset();

            //Query för att hämta alla för en viss kategori
            DBconnect.openDB("SELECT * FROM Fragor WHERE FragorID = " + idFraga);
            DBconnect.DataReader.Read();

            // Lägger in frågan
            string fraga = Convert.ToString(DBconnect.DataReader["Fraga"]);
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
            fragArray[first] = Convert.ToString(DBconnect.DataReader["Korrekt"]);
            fragArray[second] = Convert.ToString(DBconnect.DataReader["Fel1"]);
            fragArray[third] = Convert.ToString(DBconnect.DataReader["Fel2"]);
            fragArray[fourth] = Convert.ToString(DBconnect.DataReader["Fel3"]);

            // Sparar det rätta svaret i corAns, för att kunna kollas senare
            corAns = Convert.ToString(DBconnect.DataReader["Korrekt"]);

            // Fyller knapparna med svaren
            btn_Svar1.Content = fragArray[0];
            btn_Svar2.Content = fragArray[1];
            btn_Svar3.Content = fragArray[2];
            btn_Svar4.Content = fragArray[3];

            DBconnect.DataReader.Close();
            DBconnect.Connection.Close();

            //Startar om progressbaren
            startAniProg();
        }

        // Startar progressbaren.
        private void startAniProg()
        {
            questAnswered = 0;
            BeginStoryboard progBarStart = (BeginStoryboard)gridMatch.Resources["progBarStart"];
            progBarStart.Storyboard.Begin(progressBar1);
        }

        // Sätter alla knappar till röda om man inte redan svarat på frågan. Detta händer när timern är ute.
        private void SetAllRed()
        {
            if (questAnswered == 0)
            {
                btn1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                btn4Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                btn4Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));

                FargaknapparRoda();
            }

            fragaOver();
        }

        // Sätter röd färg på poängvisare
        private void FargaknapparRoda()
        {
            fragaNummer++;

            if (fragaNummer == 1)
            {
                bt1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                bt1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }

            else if (fragaNummer == 2)
            {
                bt2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                bt2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }

            else if (fragaNummer == 3)
            {
                bt3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FFF65E5E"));
                bt3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF7C1E1E"));
            }
        }

        // Sätter grön färg på poängvisare
        private void FargaknapparnaGrona()
        {
            fragaNummer++;

            if (fragaNummer == 1)
            {
                bt1Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
                bt1Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));
                antalRatt++;

            }

            else if (fragaNummer == 2)
            {
                bt2Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
                bt2Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));
                antalRatt++;
            }

            else if (fragaNummer == 3)
            {
                bt3Grad1.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF5EF677"));
                bt3Grad2.SetValue(GradientStop.ColorProperty, (Color)ColorConverter.ConvertFromString("#FF1E7C30"));
                antalRatt++;
            }
        }

        private DispatcherTimer timer;

        // När match-objectet skapas startar ett timerobject på 15 sek
        void match_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(15);
            timer.Tick += timer1_Tick;

            timer.Start();
        }

        // När timern har gått till 15 sec stoppas timern och alla knappar sätts till röda
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            SetAllRed();
        }
      
        // När man svarat på en fråga ska detta hända
        private void fragaOver()
        {
            // Om du svarat på 3 frågor kommer du tillbaka till Menyn vid klick på knappen
            if (fraga < 3)
            {
                // Om man ännu inte svarat på 3 frågor visas "Nästa fråga"-knappen
                if (fraga < 2)
                {
                    // Progressbar och "Tid kvar:" försvinner och en Nästa Fråga-knapp kommer fram
                    progressBar1.Visibility = Visibility.Collapsed;
                    btn_NastaFraga.Visibility = Visibility.Visible;
                    lbl_TidKvar.Visibility = Visibility.Hidden;
                }
                else // Knappens content ändras till tillbaka
                {
                    // Lägger till resultatet i databasen för de båda spelarna
                    DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                    DBconnect.DataReader.Read();
                    if (Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID)
                    {
                        DBconnect.DataReader.Close();
                        DBconnect.openDB("UPDATE `Match` SET ResultatSpelare1 = ResultatSpelare1+" + antalRatt + " WHERE MatchID = " + UserName.MatchID);
                        DBconnect.DataReader.Close();
                    }
                    else
                    {
                        DBconnect.DataReader.Close();
                        DBconnect.openDB("UPDATE `Match` SET ResultatSpelare2 = ResultatSpelare2+" + antalRatt + " WHERE MatchID = " + UserName.MatchID);
                        DBconnect.DataReader.Close();
                    }


                    // Progressbar och "Tid kvar:" försvinner och en Tillbaka-knapp kommer fram
                    progressBar1.Visibility = Visibility.Hidden;
                    btn_NastaFraga.Visibility = Visibility.Visible;
                    lbl_TidKvar.Visibility = Visibility.Hidden;
                    btn_NastaFraga.Content = "Tillbaka";



                    DBconnect.Connection.Close();
                }


            }
            // Annars går man tillbaka till kategori
            else
            {
                questAnswered = 0;
                fraga = 0;
                Switcher.Switch(new Meny());
            }

        }

        // Klick på "Nästa Fråga"-knappen
        private void btn_NastaFraga_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            timer.Start();

            // Knappen döljs och progressbar+tidkvar kommer tillbaka
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            btn_NastaFraga.Visibility = System.Windows.Visibility.Collapsed;
            lbl_TidKvar.Visibility = Visibility.Visible;

            // Nummer på fråga ökar
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

        //Beroende på vad man svarat så skickas denna text till checkAnswer()
        private void btn_Svar2_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar2.Content);
            checkAnswer(content, "2");
        }

        //Beroende på vad man svarat så skickas denna text till checkAnswer()
        private void btn_Svar3_Click(object sender, RoutedEventArgs e)
        { 
            string content = Convert.ToString(btn_Svar3.Content);
            checkAnswer(content, "3");
        }

        //Beroende på vad man svarat så skickas denna text till checkAnswer()
        private void btn_Svar4_Click(object sender, RoutedEventArgs e)
        {
            string content = Convert.ToString(btn_Svar4.Content);
            checkAnswer(content, "4");
        }


        // Knapp som sätter värdet till ett och ändrar till mute
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            mute = 1;
            Off1.Visibility = Visibility.Visible;
            On1.Visibility = Visibility.Hidden;
     
        
        }

        
        //Kollar om ditt svar och det rätta svaret är samma och spelar ljud därefter
        private void checkAnswer(string btnAnswer, string btnID)
        {
            timer.Stop();

            // Är värdet noll spelas något av ljuden

                if (questAnswered == 0)
                {
                    if (btnAnswer == corAns)
                    {
                        if (mute == 0)
                        {
                            System.IO.Stream correct = GetType().Assembly.GetManifestResourceStream("Quizprojekt.correct.wav");
                            SoundPlayer player1 = new SoundPlayer(correct);
                        player1.Play();
                        }

                        FargaknapparnaGrona();

                    }
                    else
                    {
                        if (mute == 0)
                        {
                        System.IO.Stream wrong = GetType().Assembly.GetManifestResourceStream("Quizprojekt.wrong.wav");
                        SoundPlayer player2 = new SoundPlayer(wrong);
                        player2.Play();
                        }

                        FargaknapparRoda();
                    }
                }

            questAnswered = 1;

            // Ändrar färgerna på fel svar till rött och rätt svar till grönt
            changeBtnCol(corAns);

            // Läser in en ny fråga
            fragaOver();
        }


        // Ändrar färgerna på fel svar till rött och rätt svar till grönt
        private void changeBtnCol(string svar)
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

        // Ljud på igen (muteknapp)
        private void Off1_Click(object sender, RoutedEventArgs e)
        {
            mute = 0;
            On1.Visibility = Visibility.Visible;
            Off1.Visibility = Visibility.Hidden;
        }
    
    }

    }

