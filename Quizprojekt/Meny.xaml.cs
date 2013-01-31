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
    public partial class Meny : Window
    {
        public Meny()
        {
            InitializeComponent();
        }

        private void btn_LoggaUt_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_Spela1_Click(object sender, RoutedEventArgs e)
        {
            kategori kategori = new kategori();
            kategori.Show();
        }
    

   
    }
}
