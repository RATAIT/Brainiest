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

        public Meny()
        {
            InitializeComponent();
             
            // Användarnamn hämtas från det du loggat in med
            anvNamn = UserName.userName;
            userID = UserName.userID;
            txtbox_Spelare1.Text = UserName.userID;

            // Lägger till ditt användarnamn efter "Logga ut"
            btn_LoggaUt.Content = "Logga ut " + anvNamn;
            btn_LoggaUt.Width = ("Logga ut " + anvNamn).Length * 8;

            //Loaded += new RoutedEventHandler(getMatch);
        }

        // Framtida metod för att få fram matcherna
        private void getMatch(object sender, RoutedEventArgs e)
        {
            DBconnect.openDB("SELECT * FROM Match WHERE MatchID = 1");
            DBconnect.DataReader.Read();

            txtbox_Resultat1.Text = Convert.ToString(DBconnect.DataReader["ResultatSpelare1"]);

            DBconnect.Connection.Close();
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

    }
}
