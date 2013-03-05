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
                DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda < 3 OR Spelare2 = " + UserName.userID + " AND Runda < 3");

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
                        // Kollar vad den inloggades motståndare har för användarnamn.
                        DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare2);
                        DBconnect.DataReader.Read();
                        txt_spelare[i].Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                        DBconnect.Connection.Close();


                        DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + matchList[i]);
                        DBconnect.DataReader.Read();
                        // Skriver ut ställningen.
                        txt_resultat[i].Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]));
                    }

                    else
                    {
                        spelare2 = Convert.ToInt16(DBconnect.DataReader["Spelare1"]);
                        // Kollar vad den inloggades motståndare har för användarnamn.
                        DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare2);
                        DBconnect.DataReader.Read();
                        txt_spelare[i].Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                        DBconnect.Connection.Close();

                        DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + matchList[i]);
                        DBconnect.DataReader.Read();
                        // Skriver ut ställningen.
                        txt_resultat[i].Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]));
                    }
                }
            }

            catch
            {

            }

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

        private void VemsTur()
        {

            // Titta vems tur det är.
            DBconnect.openDB("SELECT * FROM `Match` WHERE MatchID = " + UserName.MatchID);
            DBconnect.DataReader.Read();

            if ((Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID && Convert.ToString(DBconnect.DataReader["Spelare1Spelat"]) != "1")
                || (Convert.ToString(DBconnect.DataReader["Spelare2"]) == UserName.userID && Convert.ToString(DBconnect.DataReader["Spelare2Spelat"]) != "1"))
            {
                if (Convert.ToInt16(DBconnect.DataReader["Runda"]) == 0 || Convert.ToInt16(DBconnect.DataReader["Runda"]) == 2)
                {

                    if (Convert.ToString(DBconnect.DataReader["Spelare1"]) == UserName.userID)
                    {

                        DBconnect.Connection.Close();

                        UserName.spelarNummer = "1";
                        Switcher.Switch(new kategori());

                    }

                    else
                    {
                        if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) != "TOM")
                        {
                            DBconnect.Connection.Close();

                            UserName.spelarNummer = "2";
                            Switcher.Switch(new match());
                        }

                        else
                        {
                            MessageBox.Show("Inte din tur än!");
                        }

                    }
                }

                else
                {
                    if (Convert.ToString(DBconnect.DataReader["Spelare2"]) == UserName.userID)
                    {
                        DBconnect.Connection.Close();

                        UserName.spelarNummer = "2";
                        Switcher.Switch(new kategori());
                    }
                    else
                    {
                        if (Convert.ToString(DBconnect.DataReader["FragorRunda" + Convert.ToString(DBconnect.DataReader["Runda"])]) != "TOM")
                        {
                            DBconnect.Connection.Close();

                            UserName.spelarNummer = "1";
                            Switcher.Switch(new match());
                        }
                        else
                        {
                            MessageBox.Show("Inte din tur än!");
                        }
                    }

                    DBconnect.Connection.Close();

                }
            }

            else {

                MessageBox.Show("Inte din tur");
            
            }
        }
    }
}
