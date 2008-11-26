using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Web;

namespace Entities
{
    [ActiveRecord(Table = "Operators")]
    public class Operator : ActiveRecordBase<Operator>
    {
        private int _id;
        private DateTime _created;
        private string _username;
        private string _password;

        [PrimaryKey]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
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
        public string Password
        {
            get { return _password; }
            set { _password = value; }
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

        public static bool Login(string username, string password, bool persist)
        {
            Operator oper = Operator.FindOne(
                Expression.Eq("Username", username),
                Expression.Eq("Password", password));
            HttpContext.Current.Session["__CurrentOperator"] = oper;

            if (persist)
            {
                // Creating persistant cookie to avoid having to log in again...
                HttpCookie cookie = new HttpCookie("username", username + "|" + password.GetHashCode().ToString());
                cookie.Expires = DateTime.Now.AddMonths(3);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            else
            {
                if (HttpContext.Current.Response.Cookies["username"] != null)
                {
                    HttpCookie c = new HttpCookie("username", "mumboJumbo|zxzxzx");
                    HttpContext.Current.Response.Cookies.Add(c);
                }
            }
            return oper != null;
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

        public int GetCreds()
        {
            // Figuring out how many points the user have
            int questionsAsked = QuizItem.Count(
                Expression.Eq("CreatedBy", this),
                Expression.IsNull("Parent"));
            int upVotesGivenForQuestions = 0;
            foreach (QuizItem idx in QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNull("Parent")))
            {
                upVotesGivenForQuestions += idx.Score;
            }
            int upVotesGivenForAnswers = 0;
            foreach (QuizItem idx in QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNotNull("Parent")))
            {
                upVotesGivenForAnswers += idx.Score;
            }
            int numberOfFavoritesForQuestions = 0;
            foreach (QuizItem idx in QuizItem.FindAll(Expression.Eq("CreatedBy", this), Expression.IsNull("Parent")))
            {
                numberOfFavoritesForQuestions += idx.CountFavorites(this);
            }

            int creds = 0;
            creds += questionsAsked;
            creds += (upVotesGivenForQuestions * 5);
            creds += (upVotesGivenForAnswers * 10);
            creds += (numberOfFavoritesForQuestions * 20);
            return creds;
        }
    }
}