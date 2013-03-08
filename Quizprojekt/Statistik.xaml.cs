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
    /// Interaction logic for Statistik.xaml
    /// </summary>
    public partial class Statistik : UserControl, ISwitchable
    {
        public Statistik()
        {
            InitializeComponent();

            StatistikRatt();
            DinRating();
            ToppLista();
            MedelVarde();
            SpeladeMatcher();
        
        }




        
        // Hämtar statistik över antalet korrekta svar från databasen
        private int StatistikRatt()
        {
            int antalRatt = 0;

            DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda > 3 OR Spelare2 = " + UserName.userID + " AND Runda > 3");

            while (DBconnect.DataReader.Read())
            {
                antalRatt += Convert.ToInt16((DBconnect.DataReader["ResultatSpelare1"]));
            }
            DBconnect.DataReader.Close();
            DBconnect.Connection.Close();

            return antalRatt;
        }

     

        // Hämtar personlig rating
        private void DinRating()
        {
            DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = '" + UserName.userID + "'");
            DBconnect.DataReader.Read();

            lbl3_statistik.Content = Convert.ToString((DBconnect.DataReader["Rating"]));

            DBconnect.DataReader.Close();
            DBconnect.Connection.Close();
        }

        // Listar topp-10 användare baserat på högst rating
        private void ToppLista()
        {
            DBconnect.openDB("SELECT * FROM Medlemmar WHERE Rating IS NOT NULL ORDER BY Rating DESC LIMIT 10");
            int i = 1;
            while (DBconnect.DataReader.Read())
            {
                
                string anv = Convert.ToString(DBconnect.DataReader["Anvandarnamn"]);
                string rat = Convert.ToString(DBconnect.DataReader["Rating"]);
                List_anv.Items.Add(i + ". " +anv);
                i++;
                List_rating.Items.Add(rat);
            }

            DBconnect.Connection.Close();
            DBconnect.DataReader.Close();

        }

        int countmatch;




        // Hämtar antal spelade matcher.
        private void SpeladeMatcher()
        {
            try
            {
                // Hämtar alla matcher där du är med och har spelat klart
                DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = " + UserName.userID);
                DBconnect.DataReader.Read();

                lbl2_statistik.Content = Convert.ToString(DBconnect.DataReader["SpeladeMatcher"]);

                DBconnect.DataReader.Close();
                DBconnect.Connection.Close();
            }
            catch
            {
                lbl2_statistik.Content = "0";
            }

        }




        // Räknar ut medelvärdet på hur många poäng du får per avslutad match
        private void MedelVarde()
        {
            try
            {
                // Hämtar alla matcher där du är med och har spelat klart
                DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda > 3 OR Spelare2 = " + UserName.userID + " AND Runda > 3");
                while (DBconnect.DataReader.Read())
                {
                    countmatch++;
                }

                int antalRatt = StatistikRatt();

                lbl_mdlvarde.Content = Convert.ToString(antalRatt / countmatch) + "p";

                DBconnect.DataReader.Close();
                DBconnect.Connection.Close();
            }
            catch
            {
                lbl_mdlvarde.Content = "0";
            }
            
        }


        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Meny());
        }


    }
}
