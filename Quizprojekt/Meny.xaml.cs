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

        public Meny()
        {
            InitializeComponent();
         
            // Användarnamn hämtas från det du loggat in med
            anvNamn = UserName.userName;
            userID = UserName.userID;

            // Lägger till ditt användarnamn efter "Logga ut"
            btn_LoggaUt.Content = "Logga ut " + anvNamn;
            btn_LoggaUt.Width = ("Logga ut " + anvNamn).Length * 8;



            kollaMatcher();
            

        }

        // Kollar vilka matcher en spelare har igång och skriver ut dom.
        private void kollaMatcher()
        {
            try
            {
                // Kollar vilka matcher som den inloggade har igång.
                DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda < 4");
                DBconnect.DataReader.Read();

                spelare2 = Convert.ToInt16(DBconnect.DataReader["Spelare2"]);

                // Skriver ut båda spelarnas resultat i matchen. (Ställningen)
                txtbox_Resultat1.Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]));


                // Kollar vad den inloggades motståndare har för användarnamn.
                DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare2);
                DBconnect.DataReader.Read();


                txtbox_Spelare1.Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                DBconnect.Connection.Close();
            }


            catch
            {
                txtbox_Spelare1.Text = "Ingen match hittades";
                txtbox_Spelare2.Text = "Ingen match hittades";
                txtbox_Spelare3.Text = "Ingen match hittades";
            }

            try
            {
                // Kollar vilka matcher som den inloggade har igång.
                DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare2 = " + UserName.userID + " AND Runda < 4");
                DBconnect.DataReader.Read();

                spelare1 = Convert.ToInt16(DBconnect.DataReader["Spelare1"]);

                // Skriver ut båda spelarnas resultat i matchen. (Ställningen)
                txtbox_Resultat2.Text = (Convert.ToString(DBconnect.DataReader["ResultatSpelare2"]) + "-" + Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]));


                // Kollar vad den inloggades motståndare har för användarnamn.
                DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + spelare1);
                DBconnect.DataReader.Read();


                txtbox_Spelare2.Text = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                DBconnect.Connection.Close();
            }


            catch
            {

                txtbox_Spelare2.Text = "Ingen match hittades";
                txtbox_Spelare3.Text = "Ingen match hittades";
            }
            txtbox_Spelare3.Text = "Ingen match hittades";
                
        }


        // Loggar ut
        private void btn_LoggaUt_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainWindow());
        }

        // Spelar första matchen
        private void btn_Spela1_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new kategori());
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

        private void btn_Spela2_Click(object sender, RoutedEventArgs e)
        {
          
        }

    

    }
}
