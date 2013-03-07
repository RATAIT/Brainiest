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
using System.Windows.Media.Animation;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for Meny.xaml
    /// </summary>
    public partial class Meny : UserControl, ISwitchable
    {
        public string anvNamn;
        public string userID;
        public int spelare1;
        public int spelare2;
        int match1, match2, match3;
        TextBox[] txt_spelare = new TextBox[3];
        TextBox[] txt_resultat = new TextBox[3];

        List<string> matchList = new List<string>(); // Lista med frågor från en viss kategori

        public Meny()
        {



            InitializeComponent();

            txt_spelare[0] = txtbox_Spelare1;
            txt_spelare[1] = txtbox_Spelare2;
            txt_spelare[2] = txtbox_Spelare3;

            txt_resultat[0] = txtbox_Resultat1;
            txt_resultat[1] = txtbox_Resultat2;
            txt_resultat[2] = txtbox_Resultat3;


            // Användarnamn hämtas från det du loggat in med
            anvNamn = UserName.userName;
            userID = UserName.userID;

            // Lägger till ditt användarnamn efter "Logga ut"
            btn_LoggaUt.Content = "Logga ut " + anvNamn;
            btn_LoggaUt.Width = ("Logga ut " + anvNamn).Length * 8;

            kollaMatcher();
            DBconnect.Connection.Close();
            DBconnect.DataReader.Close();

        }

        // Kollar vilka matcher en spelare har igång och skriver ut dom.
        private void kollaMatcher()
        {
            int antalMatcher = 0;
            try
            {
                // Kollar vilka matcher som den inloggade har igång.
                DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda < 4 OR Spelare2 = " + UserName.userID + " AND Runda < 4");

                // Läser alla rader i databasen med kommandot givet ovan.
                while (DBconnect.DataReader.Read())
                {
                    // Lägger till ett ID i idList.
                    matchList.Add(Convert.ToString(DBconnect.DataReader["MatchID"]));
                    antalMatcher++;
                }

                DBconnect.Connection.Close();


                try
                {
                    match1 = Convert.ToInt16(matchList[0]);
                }
                catch { txtbox_Spelare1.Text = "Ingen match hittades"; }
                try
                {
                    match2 = Convert.ToInt16(matchList[1]);
                }
                catch { txtbox_Spelare2.Text = "Ingen match hittades"; }
                try
                {
                    match3 = Convert.ToInt16(matchList[2]);
                }
                catch { txtbox_Spelare3.Text = "Ingen match hittades"; }



                // Skriver ut användarnamn på alla mostståndare.
                for (int i = 0; i < antalMatcher; i++)
                {

                    DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + matchList[i]);
                    DBconnect.DataReader.Read();

                    if (Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID)
                    {
                        spelare2 = Convert.ToInt16(DBconnect.DataReader["Spelare2"]);
                        DBconnect.DataReader.Close();

                        // Kollar vad den inloggades motståndare har för användarnamn.
                        DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare2);
                        DBconnect.DataReader.Read();

                        txt_spelare[i].Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);

                        DBconnect.DataReader.Close();
                        DBconnect.Connection.Close();


                        DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + matchList[i]);
                        DBconnect.DataReader.Read();

                        // Skriver ut ställningen.
                        txt_resultat[i].Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]));
                        DBconnect.DataReader.Close();
                    }

                    else
                    {
                        spelare2 = Convert.ToInt16(DBconnect.DataReader["Spelare1"]);
                        DBconnect.DataReader.Close();

                        // Kollar vad den inloggades motståndare har för användarnamn.
                        DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare2);
                        DBconnect.DataReader.Read();

                        txt_spelare[i].Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                        DBconnect.DataReader.Close();
                        DBconnect.Connection.Close();

                        DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + matchList[i]);
                        DBconnect.DataReader.Read();
                        // Skriver ut ställningen.
                        txt_resultat[i].Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]));
                        DBconnect.DataReader.Close();
                    }
                }
            }

            catch
            {

            }

            DBconnect.DataReader.Close();
            DBconnect.Connection.Close();

        }

        // Loggar ut
        private void btn_LoggaUt_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainWindow());
        }



        // Nytt spel tar dig till "NyttSpel"-fönstret
        private void btn_NyttSpel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new NyttSpel());
        }

        // Hur man spelar.
        private void btn_HurSpelarMan_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Info());
        }

        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion


        // Spelar första matchen
        private void btn_Spela1_Click(object sender, RoutedEventArgs e)
        {
            UserName.MatchID = Convert.ToString(match1);
            VemsTur();
        }

        private void btn_Spela2_Click(object sender, RoutedEventArgs e)
        {
            UserName.MatchID = Convert.ToString(match2);
            VemsTur();
        }

        private void btn_Spela3_Click(object sender, RoutedEventArgs e)
        {
            UserName.MatchID = Convert.ToString(match3);
            VemsTur();
        }

        // Titta vems tur det är.
        private void VemsTur()
        {

            try
            {
                DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
                DBconnect.DataReader.Read();

                // Kollar om någon av spelarna inte har spelat nuvarande runda
                if ((Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID && Convert.ToString(DBconnect.DataReader["Spelare1Spelat"]) != "1")
                    || (Convert.ToString(DBconnect.DataReader["Spelare2"]) == UserName.userID && Convert.ToString(DBconnect.DataReader["Spelare2Spelat"]) != "1"))
                {
                    // Om runda är 0 eller 2 är det Spelare1 som ska välja kategori
                    if (Convert.ToInt16(DBconnect.DataReader["Runda"]) == 0 || Convert.ToInt16(DBconnect.DataReader["Runda"]) == 2)
                    {
                        if (Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID)
                        {
                            DBconnect.DataReader.Close();
                            DBconnect.Connection.Close();

                            UserName.spelarNummer = "1";
                            Switcher.Switch(new kategori());

                        }
                        else // Om spelare2
                        {
                            // Om frågor redan valts till den nuvarande rundan får Spelare2 spela dessa
                            if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) != "TOM")
                            {
                                DBconnect.DataReader.Close();
                                DBconnect.Connection.Close();

                                UserName.spelarNummer = "2";
                                Switcher.Switch(new match());
                            }
                            else // Om frågor inte är valda så måste Spelare1 välja dessa först innan Spelare2 kan spela
                            {
                                DBconnect.DataReader.Close();
                                DBconnect.Connection.Close();

                                MessageBox.Show("Inte din tur än!");
                            }

                        }
                    }

                    else // Om runda är 1 eller 3 är det Spelare2 som ska välja kategori
                    {
                        if (Convert.ToString(DBconnect.DataReader["Spelare2"]) == UserName.userID)
                        {
                            DBconnect.DataReader.Close();
                            DBconnect.Connection.Close();

                            UserName.spelarNummer = "2";
                            Switcher.Switch(new kategori());
                        }
                        else // Om spelare1
                        {
                            // Om frågor redan valts till den nuvarande rundan får Spelare1 spela dessa
                            if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) != "TOM")
                            {
                                DBconnect.DataReader.Close();
                                DBconnect.Connection.Close();

                                UserName.spelarNummer = "1";
                                Switcher.Switch(new match());
                            }
                            else // Om frågor inte är valda så måste Spelare1 välja dessa först innan Spelare2 kan spela
                            {
                                DBconnect.DataReader.Close();
                                DBconnect.Connection.Close();
                                MessageBox.Show("Inte din tur än!");
                            }
                        }
                        DBconnect.DataReader.Close();
                        DBconnect.Connection.Close();
                    }
                }

                else
                {
                    DBconnect.DataReader.Close();
                    DBconnect.Connection.Close();
                    MessageBox.Show("Inte din tur än!");
                }

            }
            catch
            {
                MessageBox.Show("Vänligen starta en match först!");
            }
  
    }
        // Gå till statistikfönstret
        private void btn_Statistik_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Statistik());
        }

        // Knapp för att uppdatera fönstret.
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Skapar en animation för uppdateringen
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = -360;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.6));

            RotateTransform rt = new RotateTransform();
            img_Refresh.RenderTransform = rt;
            rt.CenterX = 14;
            rt.CenterY = 14.5;

            rt.BeginAnimation(RotateTransform.AngleProperty, da);

            // Metoden för att uppdatera matcher
            kollaMatcher();
        }
    }
}
