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
    /// Interaction logic for kategori.xaml
    /// </summary>
    public partial class kategori : Window
    {
        public int i = 0;
        public int id1 = -1;
        public int id2 = -1;
        public int id3;

        public class kategoriID
        {
            public static int id = 0;
        }

        public kategori()
        {
            InitializeComponent();

            string[] kat = new string[10] {"Teknologi", "Sport och fritid", "Mat och dryck",
                "Naturvetenskap", "Data- och TV-spel", "Film", "2000-talet", "Musik", "Jorden runt", "Historia"};


            var rand = new Random();


                id1 = rand.Next(0, 10);
                while (btn_Kat1.Content == null || btn_Kat2.Content == null || btn_Kat3.Content == null)
                {
                    if (i == 0)
                    {
                        btn_Kat1.Content = kat[id1];
                        i++;
                    }

                    id2 = rand.Next(0, 10);

                    if (i == 1 && id1 != id2)
                    {
                        btn_Kat2.Content = kat[id2];
                        i++;
                    }

                    id3 = rand.Next(0, 10);

                    if (i == 2 && id1 != id3 && id2 != id3)
                    {
                        btn_Kat3.Content = kat[id3];
                        i++;
                    }
                }
        }

        private void btn_Kat1_Click(object sender, RoutedEventArgs e)
        {
            kategoriID.id = id1 + 1;

            match match1 = new match();
            match1.Show();
            this.Close();
        }

        private void btn_Kat2_Click(object sender, RoutedEventArgs e)
        {
            kategoriID.id = id2 + 1;

            match match1 = new match();
            match1.Show();
            this.Close();
        }

        private void btn_Kat3_Click(object sender, RoutedEventArgs e)
        {
            kategoriID.id = id3 + 1;

            match match1 = new match();
            match1.Show();
            this.Close();
        }



    }
}

//1	Teknologi
//2	Sport och fritid
//3	Mat och dryck
//4	Naturvetenskap
//5	Data och TV-spel
//6	Film
//7	2000-talet
//8	Musik
//9	Jorden runt
//10 Historia