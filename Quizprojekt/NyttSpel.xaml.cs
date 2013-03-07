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
        int spelare1;
        int spelare2;

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

        // Slumpa fram motståndare
        private void btn_Slumpa_Click(object sender, RoutedEventArgs e)
        {
            DBconnect.openDB("SELECT * FROM Medlemmar ORDER BY rand() LIMIT 1");
            DBconnect.DataReader.Read();

            
            spelare2 = Convert.ToInt16(DBconnect.DataReader["MedlemmarID"]);
            DBconnect.Connection.Close();


            // Spelare1 tilldelas de medlemmarID som man loggat in med.
            spelare1 = Convert.ToInt16(UserName.userID);

            if (spelare1 != spelare2)
            {
                // Öppnar connection med databasen för att lägga in matchen.
                MySqlConnection connection = new MySqlConnection(@"Server=83.168.226.169;Database=db1131745_BrainiestDB;Uid=u1131745_admin;Pwd=kidco[0lao;Port=3306;");

                MySqlCommand com = new MySqlCommand("INSERT INTO `Match` (Runda, Spelare1, Spelare2, Resultatspelare1, Resultatspelare2) VALUES (@Runda, @Spel1, @Spel2, @Res1, @Res2)", connection);

                com.Parameters.Add("@Runda", MySqlDbType.Int16);
                com.Parameters.Add("@Spel1", MySqlDbType.Int16);
                com.Parameters.Add("@Spel2", MySqlDbType.Int16);
                com.Parameters.Add("@Res1", MySqlDbType.Int16);
                com.Parameters.Add("@Res2", MySqlDbType.Int16);
                com.Parameters["@Runda"].Value = 0;
                com.Parameters["@Spel1"].Value = spelare1;
                com.Parameters["@Spel2"].Value = spelare2;
                com.Parameters["@Res1"].Value = 0;
                com.Parameters["@Res2"].Value = 0;


                connection.Open();
                com.ExecuteNonQuery();
                connection.Close();

                Switcher.Switch(new Meny());
            }
            else
                MessageBox.Show("Du kan inte möta dig själv, " + UserName.userName + "!");

        }

        // Sök på användare
        private void btn_Soek_Click(object sender, RoutedEventArgs e)
        {
            lstBox_sok.Items.Clear();

            try
            {
                string soekAnv = txtbox_Sok.Text;

                DBconnect.openDB("SELECT * FROM Medlemmar WHERE Anvandarnamn LIKE  '%" + soekAnv + "%'");
                while (DBconnect.DataReader.Read())
                {
                   
                   lstBox_sok.Items.Add(Convert.ToString(DBconnect.DataReader["Anvandarnamn"]));
                }

                DBconnect.Connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        // Väljer en motståndare
        private void btn_Valj_Click(object sender, RoutedEventArgs e)
        {
            // Väljer det selectade i listboxen.
            string selectedItem = Convert.ToString(lstBox_sok.SelectedItem);

            if (selectedItem != "")
            {
                // Tar medlemmarID ifrån de användarnamnet som valts i listboxen.
                DBconnect.openDB("SELECT * FROM Medlemmar WHERE Anvandarnamn = '" + selectedItem + "'");
                DBconnect.DataReader.Read();

                // Tilldelar variablen spelare2 de medlemmarID som tidigare valts.
                spelare2 = Convert.ToInt16(DBconnect.DataReader["MedlemmarID"]);
                DBconnect.DataReader.Close();
                DBconnect.Connection.Close();

                // Spelare1 tilldelas de medlemmarID som man loggat in med.
                spelare1 = Convert.ToInt16(UserName.userID);

                if (spelare1 != spelare2)
                {
                    // Öppnar connection med databasen.
                    MySqlConnection connection = new MySqlConnection(@"Server=83.168.226.169;Database=db1131745_BrainiestDB;Uid=u1131745_admin;Pwd=kidco[0lao;Port=3306;");

                    MySqlCommand com = new MySqlCommand("INSERT INTO `Match` (Runda, Spelare1, Spelare2, Resultatspelare1, Resultatspelare2) VALUES (@Runda, @Spel1, @Spel2, @Res1, @Res2)", connection);

                    com.Parameters.Add("@Runda", MySqlDbType.Int16);
                    com.Parameters.Add("@Spel1", MySqlDbType.Int16);
                    com.Parameters.Add("@Spel2", MySqlDbType.Int16);
                    com.Parameters.Add("@Res1", MySqlDbType.Int16);
                    com.Parameters.Add("@Res2", MySqlDbType.Int16);
                    com.Parameters["@Runda"].Value = 0;
                    com.Parameters["@Spel1"].Value = spelare1;
                    com.Parameters["@Spel2"].Value = spelare2;
                    com.Parameters["@Res1"].Value = 0;
                    com.Parameters["@Res2"].Value = 0;


                    connection.Open();
                    com.ExecuteNonQuery();
                    connection.Close();


                    Switcher.Switch(new Meny());
                }
                else
                    MessageBox.Show("Du kan inte möta dig själv, " + UserName.userName + "!");
            }
            else
                MessageBox.Show("Välj en motståndare dåå, " + UserName.userName + "!");

        }



    }
}
