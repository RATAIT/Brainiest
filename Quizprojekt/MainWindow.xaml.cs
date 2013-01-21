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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Standardvärden för textboxarna
        string anvStand = "Användarnamn";
        string passStand = "Password";

        // Sätter värdet i textboxen till empty
        private void txtbox_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            txtbox_Password.Password = String.Empty;
            txtbox_Password.GotFocus -= txtbox_Password_LostFocus;
        }


        // Lite annan skit!!
        private void txtbox_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbox_Password.Password != "")
                passStand = txtbox_Password.Password;

            if (txtbox_Password.Password == "")
                txtbox_Password.Password = passStand;
        }

        private void txtbox_Anv_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbox_Anv.Text == "Användarnamn")
            txtbox_Anv.Text = String.Empty;

            txtbox_Anv.GotFocus -= txtbox_Anv_LostFocus;
        }

        private void txtbox_Anv_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbox_Anv.Text != "")
                anvStand = txtbox_Anv.Text;

            if (txtbox_Anv.Text == "")
                txtbox_Anv.Text = anvStand;
        }

        private void btn_LoggaIn_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void btn_LoggaIn_Click(object sender, RoutedEventArgs e)
        {
            Meny menyWin = new Meny();
            menyWin.Show();
            this.Close();
        }


    }
}
