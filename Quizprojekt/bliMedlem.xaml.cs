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
using MySql.Data.MySqlClient;

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
            // Öppnar connection med databasen.
            MySqlConnection connection = new MySqlConnection(@"Server=83.168.226.169;Database=db1131745_BrainiestDB;Uid=u1131745_admin;Pwd=kidco[0lao;Port=3306;");
            MySqlCommand com = new MySqlCommand("INSERT INTO Medlemmar (Anvandarnamn, Losenord) VALUES (@Anv, @Los)", connection);


            com.Parameters.Add("@Anv", MySqlDbType.VarChar);
            com.Parameters.Add("@Los", MySqlDbType.VarChar);
            com.Parameters["@Anv"].Value = txtbox_Anv.Text;
            com.Parameters["@Los"].Value = md5Hash(txtbox_Password.Password);
            
            
            connection.Open();
            com.ExecuteNonQuery();
            connection.Close();



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

            DBconnect.openDB("SELECT * FROM Medlemmar");

            anvFinns = false;

            // Läser alla rader i databasen med kommandot givet ovan.
            while (DBconnect.DataReader.Read() && anvFinns == false)
            {
                // Kollar oberoende på om du har stora eller små bokstäver om användarnamnet finns redan eller inte
                //txtbox_Anv.Text.IndexOf(Convert.ToString(myOleDbDataReader["Anvandarnamn"]), StringComparison.CurrentCultureIgnoreCase) > -1
                if (String.Compare(txtbox_Anv.Text, Convert.ToString(DBconnect.DataReader["Anvandarnamn"]), true) == 0)
                {
                    anvFinns = true;
                    if (lbl_AnvFinns.Visibility != Visibility.Visible)
                    {
                        lbl_AnvFinns.Visibility = Visibility.Visible;
                        Storyboard lblAni_kommaIn = (Storyboard)gridBliMedlem.Resources["lblAni_kommaIn"];
                        lblAni_kommaIn.Begin(lbl_AnvFinns);
                    }
                    else
                    {
                    }
                    btn_BliMedlem.Visibility = Visibility.Hidden;
                    kolla();
                }
                else
                {
                    anvFinns = false;

                    Storyboard lblAni_akaUt = (Storyboard)gridBliMedlem.Resources["lblAni_akaUt"];
                    lblAni_akaUt.Begin(lbl_AnvFinns);

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

                    Storyboard lblAni_skaka = (Storyboard)gridBliMedlem.Resources["lblAni_skaka"];
                    lblAni_skaka.Begin(lbl_MatcharEj);
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



        private void lblAni_akaUt_Completed(object sender, EventArgs e)
        {
            lbl_AnvFinns.Visibility = Visibility.Hidden;
        }

        private void lblAni_kommaIn_Completed(object sender, EventArgs e)
        {
            lbl_AnvFinns.Visibility = Visibility.Visible;
        }    
    }
}
