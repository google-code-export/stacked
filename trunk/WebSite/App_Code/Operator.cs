using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Web;
using System.Web.Caching;
using Utilities;

namespace Entities
{
    [ActiveRecord(Table = "Operators")]
    public class Operator : ActiveRecordBase<Operator>
    {
        private int _id;
        private DateTime _created;
        private string _username;
        private string _friendlyName;
        private string _password;
        private bool _isAdmin;

        [PrimaryKey]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; }
        }

        [Property]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        [Property]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        [Property]
        public string FriendlyName
        {
            get { return _friendlyName; }
            set { _friendlyName = value; }
        }

        [Property]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool CanDelete
        {
            get
            {
                return IsAdmin || CalculateCreds >= Settings.CredsNeededToDelete;
            }
        }

        public static Operator Current
        {
            get { return HttpContext.Current.Session["__CurrentOperator"] as Operator; }
        }

        public static void Logout()
        {
            HttpContext.Current.Session["__CurrentOperator"] = null;
            HttpCookie c = new HttpCookie("username", "mumboJumbo|zxzxzx");
            HttpContext.Current.Response.Cookies.Add(c);
        }

        public static void LoginOpenID(string username, string friendlyName, bool publicTerminal)
        {
            // Escaping username since often it contains things which we cannot legally use in Stacked like
            // e.g. "http://" etc...
            username = username.Replace("http", "").Replace("https", "");
            int index = 0;
            while (index < username.Length)
            {
                if (("abcdefghijklmnopqrstuvwxyz0123456789.-_").IndexOf(username[index]) == -1)
                {
                    username = username.Substring(0, index) + username.Substring(index + 1);
                }
                else
                    index += 1;
            }

            Operator oper = Operator.FindOne(
                Expression.Eq("Username", username));
            if (oper == null)
            {
                oper = new Operator();
                oper.Username = username;
                oper.IsAdmin = false;
                oper.FriendlyName = friendlyName;
                oper.Password = Guid.NewGuid().ToString();
                oper.Save();
            }
            StoreLoggedInOperatorToSessionAndCookie(!publicTerminal, oper);
        }

        public static bool Login(string username, string password, bool persist)
        {
            Operator oper = Operator.FindOne(
                Expression.Eq("Username", username),
                Expression.Eq("Password", password));
            StoreLoggedInOperatorToSessionAndCookie(persist, oper);
            return oper != null;
        }

        private static void StoreLoggedInOperatorToSessionAndCookie(bool persist, Operator oper)
        {
            HttpContext.Current.Session["__CurrentOperator"] = oper;

            if (persist && oper != null)
            {
                // Creating persistant cookie to avoid having to log in again...
                HttpCookie cookie = new HttpCookie("username", oper.Username + "|" + oper.Password.GetHashCode().ToString());
                cookie.Expires = DateTime.Now.AddMonths(3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                // We must *destroy* the old cookie here...!
                if (HttpContext.Current.Response.Cookies["username"] != null)
                {
                    HttpCookie c = new HttpCookie("username", "mumboJumbo|zxzxzx");
                    HttpContext.Current.Response.Cookies.Add(c);
                }
            }
        }

        public static int GetCount()
        {
            return Count();
        }

        public override void Save()
        {
            if (_id == 0)
                Created = DateTime.Now;
            base.Save();
        }

        public static bool TryLoginFromCookie()
        {
            if (Current != null)
                return true;
            if (HttpContext.Current.Request.Cookies["username"] != null)
            {
                HttpCookie creds = HttpContext.Current.Request.Cookies["username"];
                string username = creds.Value.Split('|')[0];
                string hashedPwd = creds.Value.Split('|')[1];
                Operator oper = Operator.FindOne(
                    Expression.Eq("Username", username));
                if (oper != null && oper.Password.GetHashCode().ToString() == hashedPwd)
                {
                    HttpContext.Current.Session["__CurrentOperator"] = oper;
                    return true;
                }
            }
            return false;
        }

        public int CalculateCreds
        {
            get { return GetCreds(); }
        }

        public int GetCreds()
        {
            // Checking cache first
            if (HttpContext.Current.Cache["operatorCreds" + this.ID] != null)
                return (int)HttpContext.Current.Cache["operatorCreds" + this.ID];

            // Figuring out how many points the user have
            int questionsAsked = QuizItem.Count(
                Expression.Eq("CreatedBy", this),
                Expression.IsNull("Parent"));
            int upVotesGivenForQuestions = 0;
            QuizItem[] items = QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNull("Parent"));
            if (items != null)
            {
                foreach (QuizItem idx in items)
                {
                    upVotesGivenForQuestions += idx.Score;
                }
            }
            int upVotesGivenForAnswers = 0;
            items = QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNotNull("Parent"));
            if (items != null)
            {
                foreach (QuizItem idx in items)
                {
                    upVotesGivenForAnswers += idx.Score;
                }
            }
            int numberOfFavoritesForQuestions = 0;
            items = QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNull("Parent"));
            if (items != null)
            {
                foreach (QuizItem idx in items)
                {
                    numberOfFavoritesForQuestions += idx.CountFavorites(this);
                }
            }

            int creds = 0;
            creds += questionsAsked;
            creds += (upVotesGivenForQuestions * 5);
            creds += (upVotesGivenForAnswers * 10);
            creds += (numberOfFavoritesForQuestions * 20);

            // Adding to cache
            HttpContext.Current.Cache.Insert("operatorCreds" + this.ID, creds, null, DateTime.Now.AddMinutes(5), Cache.NoSlidingExpiration);
            return creds;
        }

        public static int Count()
        {
            return ActiveRecordBase<Operator>.Count();
        }
    }
}