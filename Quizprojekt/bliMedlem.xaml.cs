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
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Media.Animation;
using System.Security.Cryptography;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for bliMedlem.xaml
    /// </summary>
    public partial class bliMedlem : UserControl, ISwitchable
    {
        public bliMedlem()
        {
            InitializeComponent();

        }

        // Krypterar lösenordet till md5.
        public String md5Hash(String input)
        {
            String result = "";

            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                result = BitConverter.ToString(md5.ComputeHash(ASCIIEncoding.Default.GetBytes(input)));
            }

            return result;
        }


        private void btn_Avbryt_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new MainWindow());
        }

        private void btn_BliMedlem_Click(object sender, RoutedEventArgs e)
        {
            OleDbConnection thisConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb");

            thisConnection.Open();

            //string Cmd3 = "INSERT INTO Medlemmar(Anvandarnamn, Losenord) VALUES (" + txtbox_Anv.Text + ", " + txtbox_Password.Password + ")";
            string Cmd3 = "INSERT INTO Medlemmar(Anvandarnamn, Losenord) VALUES (?, ?)";

            OleDbCommand myCommand2 = new OleDbCommand(Cmd3, thisConnection);
            OleDbParameter para1 = myCommand2.Parameters.Add("@InputParm", OleDbType.VarChar, 255);
            para1.Value = txtbox_Anv.Text;
            OleDbParameter para2 = myCommand2.Parameters.Add("@InputParm", OleDbType.VarChar, 255);
            para2.Value = md5Hash(txtbox_Password.Password);

            myCommand2.ExecuteNonQuery();

            thisConnection.Close();
        }



        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        #endregion

        private void txtbox_Password2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtbox_Password.Password == txtbox_Password2.Password)
            {
                btn_BliMedlem.Visibility = Visibility.Visible;
                lbl_MatcharEj.Visibility = Visibility.Hidden;
       
            }

            else
            {
                btn_BliMedlem.Visibility = Visibility.Hidden;
                lbl_MatcharEj.Visibility = Visibility.Visible;
            }


        }
    }
}
