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
   

        public Meny()
        {

            InitializeComponent();
             
          
            anvNamn = UserName.userName;
            txtbox_Spelare1.Text = UserName.userID;

            btn_LoggaUt.Content = "Logga ut " + anvNamn;
            btn_LoggaUt.Width = ("Logga ut " + anvNamn).Length * 8;

        }

        private void btn_LoggaUt_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainWindow());
        }

        private void btn_Spela1_Click(object sender, RoutedEventArgs e)
        {

            Switcher.Switch(new kategori());
     
        }

        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void btn_NyttSpel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new NyttSpel());
        }

        private void btn_HurSpelarMan_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new Info());
     
        }

   
    }
}
