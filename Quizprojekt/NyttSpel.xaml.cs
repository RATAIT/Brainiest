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
using MySql.Data.MySqlClient;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for NyttSpel.xaml
    /// </summary>
    public partial class NyttSpel : UserControl, ISwitchable
    {
        public NyttSpel()
        {
            InitializeComponent();
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

        private void btn_Slumpa_Click(object sender, RoutedEventArgs e)
        {
            DBconnect.openDB("SELECT * FROM Medlemmar ORDER BY rand() LIMIT 1");
            DBconnect.DataReader.Read();
            txtbox_Sok.Text = Convert.ToString(DBconnect.DataReader["MedlemmarID"]);
            DBconnect.Connection.Close();
        }

        private void btn_Soek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string soekAnv = txtbox_Sok.Text;

                DBconnect.openDB("SELECT * FROM Medlemmar WHERE MedlemmarID = " + soekAnv);
                while (DBconnect.DataReader.Read())
                {

                    listBox1.Items.Add(Convert.ToString(DBconnect.DataReader["Anvandarnamn"]));

                }

                DBconnect.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



    }
}
