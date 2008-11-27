using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using Castle.ActiveRecord.Queries;

namespace Entities
{
    [ActiveRecord(Table = "QuizItems")]
    public class QuizItem : ActiveRecordBase<QuizItem>
    {
        private int _id;
        private int _views;
        private DateTime _created;
        private string _header;
        private string _body;
        private Operator _createdBy;
        private QuizItem _parent;
        private string _url;

        [PrimaryKey]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public int Views
        {
            get { return _views; }
            set { _views = value; }
        }

        [Property(Unique = true)]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        [BelongsTo("FK_CreatedBy")]
        public Operator CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }

        [BelongsTo("FK_Parent")]
        public QuizItem Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        [Property]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        [Property(Length=150)]
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        [Property(ColumnType = "StringClob", SqlType = "TEXT")]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public string BodyFormated
        {
            get
            {
                string tmp = Body.Replace("<", "&lt;").Replace(">", "&gt;");
                tmp = FormatWiki(tmp);
                return "<p>" + tmp + "</p>";
            }
        }

        public static string FormatWiki(string tmp)
        {
            string nofollow = ConfigurationManager.AppSettings["nofollow"] == "true" ? " rel=\"nofollow\"" : "";

            // Replacing dummy links...
            tmp = Regex.Replace(
                " " + tmp,
                "(?<spaceChar>\\s+)(?<linkType>http://|https://)(?<link>\\S+)",
                "${spaceChar}<a href=\"${linkType}${link}\"" + nofollow + ">${link}</a>",
                RegexOptions.Compiled).Trim();

            // Replacing wiki links
            tmp = Regex.Replace(tmp,
                "(?<begin>\\[{1})(?<linkType>http://|https://)(?<link>\\S+)\\s+(?<content>[^\\]]+)(?<end>[\\]]{1})",
                "<a href=\"${linkType}${link}\"" + nofollow + ">${content}</a>",
                RegexOptions.Compiled);

            // Replacing bolds
            tmp = Regex.Replace(tmp,
                "(?<begin>\\*{1})(?<content>.+?)(?<end>\\*{1})",
                "<strong>${content}</strong>",
                RegexOptions.Compiled);

            // Replacing italics
            tmp = Regex.Replace(tmp,
                "(?<begin>_{1})(?<content>.+?)(?<end>_{1})",
                "<em>${content}</em>",
                RegexOptions.Compiled);

            // Replacing lists
            tmp = Regex.Replace(tmp,
                "(?<begin>\\*{1}[ ]{1})(?<content>.+)(?<end>[^*])",
                "<li>${content}</li>",
                RegexOptions.Compiled);
            tmp = Regex.Replace(tmp,
                "(?<content>\\<li\\>{1}.+\\<\\/li\\>)",
                "<ul>${content}</ul>",
                RegexOptions.Compiled);

            // Paragraphs
            tmp = Regex.Replace(tmp,
                "(?<content>)\\n{2}",
                "${content}</p><p>",
                RegexOptions.Compiled);

            // Breaks
            tmp = Regex.Replace(tmp,
                "(?<content>)\\n{1}",
                "${content}<br />",
                RegexOptions.Compiled);

            // Code
            tmp = Regex.Replace(tmp,
                "(?<begin>\\[code\\])(?<content>[^$]+)(?<end>\\[/code\\])",
                "<pre class=\"code\">${content}</pre>",
                RegexOptions.Compiled);
            return tmp;
        }

        public int Score
        {
            get { return GetScore(); }
        }

        public int AnswersCount
        {
            get
            {
                return QuizItem.Count(Expression.Eq("Parent", this));
            }
        }

        public enum OrderBy { New, Unanswered, Top };

        public static IEnumerable<QuizItem> GetQuestions(Operator oper, OrderBy order)
        {
            if (oper == null)
                return GetQuestions(order);
            else
                return GetQuestionsForUser(oper, order, false);
        }

        public static IEnumerable<QuizItem> GetFavoredQuestions(Operator oper)
        {
            return GetQuestionsForUser(oper, OrderBy.New, true);
        }

        private static IEnumerable<QuizItem> GetQuestions(OrderBy order)
        {
            switch (order)
            {
                case OrderBy.New:
                    return QuizItem.SlicedFindAll(0, 20,
                        new Order[] { Order.Desc("Created") },
                        Expression.IsNull("Parent"));
                case OrderBy.Top:
                    SimpleQuery<QuizItem> retVal = new SimpleQuery<QuizItem>(QueryLanguage.Sql,
                        "select this_.* from QuizItems this_, QuizItems c2 where this_.ID = c2.FK_Parent group by c2.FK_Parent order by count(c2.FK_Parent) desc, this_.Created desc");
                    retVal.SetQueryRange(20);
                    retVal.AddSqlReturnDefinition(typeof(QuizItem), "this_");
                    return retVal.Execute();
                case OrderBy.Unanswered:
                    return QuizItem.SlicedFindAll(0, 20,
                        new Order[] { Order.Desc("Created") },
                        Expression.IsNull("Parent"),
                        Expression.Sql("not exists(select * from QuizItems where FK_Parent=this_.ID)"));
                default:
                    throw new ApplicationException("Not implemented OrderBy");
            }
        }

        private static IEnumerable<QuizItem> GetQuestionsForUser(Operator oper, OrderBy order, bool onlyFavored)
        {
            if (onlyFavored)
            {
                return QuizItem.FindAll(
                    new Order[] { Order.Desc("Created") },
                    Expression.IsNull("Parent"),
                    Expression.Sql(
                    string.Format("exists(select * from Favorites f where f.FK_FavoredBy={0} and this_.ID = f.FK_Question)", oper.ID)));
            }
            else
            {
                return QuizItem.FindAll(
                    new Order[] { Order.Desc("Created") },
                    Expression.Eq("CreatedBy", oper),
                    Expression.IsNull("Parent"));
            }
        }

        public override void Save()
        {
            // Checking to see if this is FIRST saving and if it is create a new friendly URL...
            if (_id == 0)
            {
                Created = DateTime.Now;
                
                // Building UNIQUE friendly URL
                Url = Header.ToLower();
                if (Url.Length > 100)
                    Url = Url.Substring(0, 100);
                int index = 0;
                while (index < Url.Length)
                {
                    if (("abcdefghijklmnopqrstuvwxyz0123456789").IndexOf(Url[index]) == -1)
                    {
                        Url = Url.Substring(0, index) + "-" + Url.Substring(index + 1);
                    }
                    index += 1;
                }
                Url = Url.Trim('-');
                bool found = true;
                while (found)
                {
                    found = false;
                    if (Url.IndexOf("--") != -1)
                    {
                        Url = Url.Replace("--", "-");
                        found = true;
                    }
                }
                int countOfOldWithSameURL = QuizItem.Count(Expression.Like("Url", Url + "%.quiz"));
                if (countOfOldWithSameURL > 0)
                    Url += (countOfOldWithSameURL + 1).ToString();
                Url += ".quiz";
                base.Save();
            }
            base.Save();
        }

        public int GetScore()
        {
            int plus = Vote.Count(Expression.Eq("QuizItem", this), Expression.Eq("Score", 1));
            int minus = Vote.Count(Expression.Eq("QuizItem", this), Expression.Eq("Score", -1));
            return plus - minus;
        }

        public IEnumerable<QuizItem> GetAnswers()
        {
            return QuizItem.FindAll(Expression.Eq("Parent", this));
        }

        public int CountFavorites(Operator exclude)
        {
            if (exclude == null)
            {
                return Favorite.Count(Expression.Eq("Question", this));
            }
            else
            {
                return Favorite.Count(Expression.Eq("Question", this), Expression.Not(Expression.Eq("FavoredBy", exclude)));
            }
        }

        public void IncreaseViewCount()
        {
            _views += 1;
            this.Save();
        }
    }
}