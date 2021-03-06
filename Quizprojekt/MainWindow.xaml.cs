﻿using System;
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
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Windows.Media.Animation;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace Quizprojekt
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl, ISwitchable
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Standardvärden för textboxarna
        string anvStand = "Användarnamn";
        string passStand = "Password";
    
        // Om txtbox_Anv får focus
        private void txtbox_Anv_GotFocus(object sender, RoutedEventArgs e)
        {
            // Sätter värdet i Användarnamn-textbox till empty om texten är "Användarnamn"
            if (txtbox_Anv.Text == "Användarnamn")
            txtbox_Anv.Text = String.Empty;

            // När man släpper focus hoppar man till metoden txtbox_Anv_LostFocus
            txtbox_Anv.GotFocus -= txtbox_Anv_LostFocus;
        }

        // Om txtbox_Anv tappar focus
        private void txtbox_Anv_LostFocus(object sender, RoutedEventArgs e)
        {
            // Om Användarnamnboxen INTE är tom sätts anvStand till det som står där i
            if (txtbox_Anv.Text != "")
                anvStand = txtbox_Anv.Text;

            // Om Passwordboxen är tom sätts texten i boxen till stringen anvStand
            if (txtbox_Anv.Text == "")
                txtbox_Anv.Text = anvStand;
        }
        
        // Om txtbox_Password får focus
        private void txtbox_Password_GotFocus(object sender, RoutedEventArgs e)
        {
            // Sätter värdet i Password-textbox till empty
            txtbox_Password.Password = String.Empty;
            // När man släpper focus hoppar man till metoden txtbox_Password_LostFocus
            txtbox_Password.GotFocus -= txtbox_Password_LostFocus;
        }

        // Om txtbox_Password tappar focus
        private void txtbox_Password_LostFocus(object sender, RoutedEventArgs e)
        {
            // Om Passwordboxen INTE är tom sätts passStand till det som står där i
            if (txtbox_Password.Password != "")
                passStand = txtbox_Password.Password;
            // Om Passwordboxen är tom sätts texten i boxen till stringen passStand
            else if (txtbox_Password.Password == "")
                txtbox_Password.Password = passStand;
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

        // Klick på Logga in
        private void btn_LoggaIn_Click(object sender, RoutedEventArgs e)
        {
            string anvNamn = txtbox_Anv.Text;
            string losenord = md5Hash(txtbox_Password.Password);

            string connectionString = @"Server=83.168.226.169;Database=db1131745_BrainiestDB;Uid=u1131745_admin;Pwd=kidco[0lao;Port=3306;";

            MySqlConnection Connection = new MySqlConnection(connectionString);
            MySqlCommand Command = Connection.CreateCommand();

            Command.CommandText = "SELECT * FROM Medlemmar WHERE Anvandarnamn =@username and Losenord =@password";
            Command.Parameters.AddWithValue("@username", anvNamn);
            Command.Parameters.AddWithValue("@password", losenord);
            Connection.Open();

            MySqlDataReader DataReader = Command.ExecuteReader();

            DataReader.Read();

            // Om lösenord är rätt kommer man in
            try
            {
                if (losenord == Convert.ToString(DataReader["Losenord"]))
                {
                    UserName.userName = Convert.ToString(DataReader["Anvandarnamn"]);
                    UserName.userID = Convert.ToString(DataReader["MedlemmarID"]);

                    DataReader.Close();
                    Connection.Close();

                    Meny meny = new Meny();

                    Switcher.Switch(meny);
                }
            }
                // Om lösenord är fel får du reda på detta med animerad label
            catch
            {
                // Om labeln inte är synlig kommer den fram
                if (lbl_felAnvLos.Visibility != Visibility.Visible)
                {
                    lbl_felAnvLos.Visibility = Visibility.Visible;
                    Storyboard lbl_felAnvLos_ani = (Storyboard)gridMainWindow.Resources["lbl_felAnvLos_ani"];
                    lbl_felAnvLos_ani.Begin(lbl_felAnvLos);

                    DataReader.Close();
                    Connection.Close();
                }
                // Om labeln är synlig skakar den några gånger
                else
                {
                    Storyboard lbl_felAnvLos_ani2 = (Storyboard)gridMainWindow.Resources["lbl_felAnvLos_ani2"];
                    lbl_felAnvLos_ani2.Begin(lbl_felAnvLos);

                    DataReader.Close();
                    Connection.Close();
                }
            }
        }

        private void btn_BliMedlem_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(new bliMedlem());
        }

        #region ISwitchable Members

        public void UtilizeState(object state)
        {
            throw new NotImplementedException();
        }

        #endregion

        // Vid klick på enter i passwordboxen loggar man in
        private void txtbox_Password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btn_LoggaIn_Click(null, null);
            }
        }

        

     
    }
}
