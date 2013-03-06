using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quizprojekt
{
    class UserName
    {
        private static string m_userName = "";

        public static string userName
        {
            get { return m_userName; }
            set { m_userName = value; }
        }

        private static string m_userID = "";


        public static string userID
        {
            get { return m_userID; }
            set { m_userID = value; }
        }

        private static string m_matchID = "";

        public static string MatchID
        {
            get { return m_matchID; }
            set { m_matchID = value; }
        }


        private static string m_spelarNummer = "";

        public static string spelarNummer
        {
            get { return m_spelarNummer; }
            set { m_spelarNummer = value; }
        }

        private static int m_kategoriID;

        public static int kategoriID
        {
            get { return m_kategoriID; }
            set { m_kategoriID = value; }
        }
      

    }




}
