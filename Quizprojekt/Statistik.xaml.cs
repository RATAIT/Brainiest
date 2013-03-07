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
            StatistikFel();
            DinRating();
            ToppLista();
            MedelVarde();
        }




        
        // Hämtar statistik över antaler korrekta svar från databasen
       
        private void StatistikRatt()
        {
            DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = '" + UserName.userID + "'");
            DBconnect.DataReader.Read();


            lbl_statistik.Content = Convert.ToString((DBconnect.DataReader["AntalRatt"]));
            DBconnect.Connection.Close();
        }

        // Hämtar statistik över antalet felaktiga svar från databasen

        private void StatistikFel()
        {
            DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = '" + UserName.userID + "'");
            DBconnect.DataReader.Read();

            lbl2_statistik.Content = Convert.ToString((DBconnect.DataReader["AntalFel"]));
            DBconnect.DataReader.Close();
        }

        // Hämtar personlig rating

        private void DinRating()
        {
            DBconnect.openDB("SELECT * FROM `Medlemmar` WHERE MedlemmarID = '" + UserName.userID + "'");
            DBconnect.DataReader.Read();

            lbl3_statistik.Content = Convert.ToString((DBconnect.DataReader["Rating"]));
            DBconnect.DataReader.Close();
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


        }
        int countrundor;
        private void MedelVarde()
        {

            DBconnect.openDB("SELECT * FROM `Match` WHERE Spelare1 = " + UserName.userID + " AND Runda > 3 OR Spelare2 = " + UserName.userID + " AND Runda > 3");
            while (DBconnect.DataReader.Read())
            {
                countrundor++;
            }
            lbl_mdlvarde.Content = Convert.ToString(countrundor);
            DBconnect.DataReader.Close();

            
            

            
            
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
