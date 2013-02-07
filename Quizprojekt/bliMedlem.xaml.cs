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

            // Tar med sig användarnamn och lösenord spelaren valt till loginmenyn
            MainWindow newMain = new MainWindow();
            newMain.txtbox_Anv.Text = txtbox_Anv.Text;
            newMain.txtbox_Password.Password = txtbox_Password.Password;
            Switcher.Switch(newMain);
        }



        #region ISwitchable Members
        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }
        #endregion

        bool losenOk; // Sparar värde om lösenordet är ok eller ej
        bool anvFinns; // Sparar värde om användarnamn finns eller ej

        // Om du ändrar lösenord i första boxen nollställs det i andra
        private void txtbox_Password_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            txtbox_Password2.Password = null;
        }

        
        // Om andra lösenordsboxen ändras kollas detta mot första vid varje knapptryck
        private void txtbox_Password2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            kolla();
        }

        // Om användarnamnsboxen tappar fokus kollas detta mot användardatabasen för att se om användaren redan finns eller ej
        private void txtbox_Anv_LostFocus(object sender, RoutedEventArgs e)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=BrainiestDB.accdb";

            OleDbConnection myOleDbConnection = new OleDbConnection(connectionString);

            OleDbCommand myOleDbCommand = myOleDbConnection.CreateCommand();

            //Query för att hämta alla för en viss kategori
            myOleDbCommand.CommandText = "SELECT * FROM Medlemmar";
            myOleDbConnection.Open();

            OleDbDataReader myOleDbDataReader = myOleDbCommand.ExecuteReader();

            anvFinns = false;
            // Läser alla rader i databasen med kommandot givet ovan.
            while (myOleDbDataReader.Read() && anvFinns == false)
            {
                // Kollar oberoende på om du har stora eller små bokstäver om användarnamnet finns redan eller inte
                //txtbox_Anv.Text.IndexOf(Convert.ToString(myOleDbDataReader["Anvandarnamn"]), StringComparison.CurrentCultureIgnoreCase) > -1
                if (String.Compare(txtbox_Anv.Text, Convert.ToString(myOleDbDataReader["Anvandarnamn"]), true) == 0)
                {
                    anvFinns = true;
                    lbl_AnvFinns.Visibility = Visibility.Visible;
                    btn_BliMedlem.Visibility = Visibility.Hidden;
                    kolla();
                }
                else
                {
                    anvFinns = false;
                    lbl_AnvFinns.Visibility = Visibility.Hidden;
                    btn_BliMedlem.Visibility = Visibility.Visible;
                    kolla();
                }

                if (anvFinns == true || losenOk == false)
                    btn_BliMedlem.Visibility = Visibility.Hidden;
            }
        }      

        private void kolla()
        {
            if (txtbox_Password.Password == txtbox_Password2.Password && txtbox_Password.Password.Length > 0)
            {
                if (txtbox_Anv.Text != "")
                {
                    losenOk = true;
                    btn_BliMedlem.Visibility = Visibility.Visible;
                    lbl_MatcharEj.Visibility = Visibility.Hidden;
                }
                else
                {
                    losenOk = true;
                    btn_BliMedlem.Visibility = Visibility.Hidden;
                    lbl_MatcharEj.Visibility = Visibility.Hidden;
                }
            }
            else if (txtbox_Password.Password != txtbox_Password2.Password)
            {
                if (txtbox_Password.Password != null)
                {
                    losenOk = false;
                    btn_BliMedlem.Visibility = Visibility.Hidden;
                    lbl_MatcharEj.Visibility = Visibility.Visible;
                }
                else
                {
                    losenOk = false;
                    btn_BliMedlem.Visibility = Visibility.Hidden;
                    lbl_MatcharEj.Visibility = Visibility.Hidden;
                }
            }
            if (anvFinns == true || losenOk == false)
                btn_BliMedlem.Visibility = Visibility.Hidden;
        }

       

    }
}
