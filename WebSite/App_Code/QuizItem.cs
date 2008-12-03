using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using Castle.ActiveRecord.Queries;
using System.Collections;
using Utilities;

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
        private IList _tags;
        private IList _children;

        public enum OrderBy { New, Unanswered, Top };
        public enum OrderAnswersBy { Newest, Oldest, MostVotes, Determine };

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

        [HasMany(typeof(QuizItem))]
        public IList Children
        {
            get { return _children; }
            set { _children = value; }
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

        [HasAndBelongsToMany(typeof(Tag),
            Table = "QuizItemTag", ColumnRef = "TagId", ColumnKey = "QuizItemId")]
        public IList Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }

        public string TagsFormated
        {
            get
            {
                string retVal = "";
                foreach (Tag idx in Tags)
                {
                    if (retVal.Length > 50)
                    {
                        retVal = retVal.Trim().Replace(" ", " | ");
                        retVal += " (" + Tags.Count.ToString() + " in total)";
                        return retVal;
                    }
                    retVal += idx.Name + " ";
                }
                return retVal.Trim().Replace(" ", " | ");
            }
        }

        public string BodyQuote
        {
            get
            {
                string tmp = ">" + Body;
                tmp = tmp.Replace("\n", "\n>");
                return tmp;
            }
        }

        public string BodyFormated
        {
            get
            {
                string tmp = Body.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
                tmp = FormatWiki(tmp);
                return "<p>" + tmp + "</p>";
            }
        }

        public string BodySummary
        {
            get
            {
                string retVal = Body;
                if (retVal.Length > 100)
                    return retVal.Substring(0, 100);
                return retVal;
            }
        }

        public static string FormatWiki(string tmp)
        {
            string nofollow = ConfigurationManager.AppSettings["nofollow"] == "true" ? " rel=\"nofollow\"" : "";

            // Sanitizing carriage returns...
            tmp = tmp.Replace("\\r\\n", "\\n");

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

            // Quoting
            tmp = Regex.Replace(tmp,
                "(?<content>^&gt;.+$)",
                "<blockquote>${content}</blockquote>",
                RegexOptions.Compiled | RegexOptions.Multiline).Replace("</blockquote>\n<blockquote>", "\n");

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

        public static IEnumerable<QuizItem> GetQuestions(Operator oper, Tag tag, OrderBy order)
        {
            if (oper == null)
                return GetQuestions(order, tag);
            else
                return GetQuestionsForUser(oper, tag, order, false);
        }

        public static IEnumerable<QuizItem> GetFavoredQuestions(Operator oper)
        {
            return GetQuestionsForUser(oper, null, OrderBy.New, true);
        }

        private static IEnumerable<QuizItem> GetQuestions(OrderBy order, Tag tag)
        {
            if (tag == null)
                return GetQuestionsNoTag(order);
            else
                return GetQuestionsWithTag(order, tag);
        }

        private static IEnumerable<QuizItem> GetQuestionsNoTag(OrderBy order)
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

        private static IEnumerable<QuizItem> GetQuestionsWithTag(OrderBy order, Tag tag)
        {
            switch (order)
            {
                case OrderBy.New:
                    return QuizItem.SlicedFindAll(0, 20,
                        new Order[] { Order.Desc("Created") },
                        Expression.IsNull("Parent"),
                        Expression.Sql("exists(select * from QuizItemTag qt where qt.QuizItemId = this_.Id and exists(select * from Tags t where t.Id = qt.TagId and t.Name='" + tag.Name + "'))"));
                case OrderBy.Top:
                    SimpleQuery<QuizItem> retVal = new SimpleQuery<QuizItem>(QueryLanguage.Sql,
                        @"select this_.* from QuizItems this_, QuizItems c2 
where this_.ID = c2.FK_Parent 
and exists(select * from QuizItemTag qt where qt.QuizItemId = this_.Id and exists(select * from Tags t where t.Id = qt.TagId and t.Name='" + tag.Name + @"'))
group by c2.FK_Parent order by count(c2.FK_Parent) desc, this_.Created desc");
                    retVal.SetQueryRange(20);
                    retVal.AddSqlReturnDefinition(typeof(QuizItem), "this_");
                    return retVal.Execute();
                case OrderBy.Unanswered:
                    return QuizItem.SlicedFindAll(0, 20,
                        new Order[] { Order.Desc("Created") },
                        Expression.IsNull("Parent"),
                        Expression.Sql("not exists(select * from QuizItems where FK_Parent=this_.ID)"),
                        Expression.Sql("exists(select * from QuizItemTag qt where qt.QuizItemId = this_.Id and exists(select * from Tags t where t.Id = qt.TagId and t.Name='" + tag.Name + "'))"));
                default:
                    throw new ApplicationException("Not implemented OrderBy");
            }
        }

        private static IEnumerable<QuizItem> GetQuestionsForUser(Operator oper, Tag tag, OrderBy order, bool onlyFavored)
        {
            if (tag == null)
                return GetQuestionsForUseNoTag(oper, onlyFavored);
            else
                return GetQuestionsForUseWithTag(oper, tag, onlyFavored);
        }

        private static IEnumerable<QuizItem> GetQuestionsForUseNoTag(Operator oper, bool onlyFavored)
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

        private static IEnumerable<QuizItem> GetQuestionsForUseWithTag(Operator oper, Tag tag, bool onlyFavored)
        {
            if (onlyFavored)
            {
                return QuizItem.FindAll(
                    new Order[] { Order.Desc("Created") },
                    Expression.IsNull("Parent"),
                    Expression.Sql(
                    string.Format("exists(select * from Favorites f where f.FK_FavoredBy={0} and this_.ID = f.FK_Question)", oper.ID)),
                    Expression.Sql("exists(select * from QuizItemTag qt where qt.QuizItemId = this_.Id and exists(select * from Tags t where t.Id = qt.TagId and t.Name='" + tag.Name + "'))"));
            }
            else
            {
                return QuizItem.FindAll(
                    new Order[] { Order.Desc("Created") },
                    Expression.Eq("CreatedBy", oper),
                    Expression.IsNull("Parent"),
                    Expression.Sql("exists(select * from QuizItemTag qt where qt.QuizItemId = this_.Id and exists(select * from Tags t where t.Id = qt.TagId and t.Name='" + tag.Name + "'))"));
            }
        }

        public override void Delete()
        {
            foreach (Favorite idxFav in Favorite.FindAll(
                Expression.Eq("Question", this)))
            {
                idxFav.Delete();
            }
            foreach (Vote idxV in Vote.FindAll(
                Expression.Eq("QuizItem", this)))
            {
                idxV.Delete();
            }
            foreach (QuizItem idxQ in QuizItem.FindAll(
                Expression.Eq("Parent", this)))
            {
                idxQ.Delete();
            }
            base.Delete();
        }

        public override void Save()
        {
            // Checking to see if this is FIRST saving and if it is create a new friendly URL...
            if (!string.IsNullOrEmpty(this.Header))
                this.Header = this.Header.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            if (_id == 0)
            {
                Created = DateTime.Now;

                if (this.Parent != null)
                {
                    // Must have random Url...
                    this.Url = Guid.NewGuid().ToString();
                }
                else
                {
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
                    {
                        while (true)
                        {
                            if (QuizItem.Count(Expression.Like("Url", (Url + countOfOldWithSameURL) + "%.quiz")) > 0)
                            {
                                countOfOldWithSameURL += 1;
                            }
                            else 
                                break;
                        }
                        Url += (countOfOldWithSameURL).ToString();
                    }
                    Url += ".quiz";
                }
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

        public IEnumerable<QuizItem> GetAnswers(OrderAnswersBy order)
        {
            List<QuizItem> retVal = new List<QuizItem>(QuizItem.FindAll(Expression.Eq("Parent", this)));

            if (order == OrderAnswersBy.MostVotes)
            {
                OrderByVotes(retVal);
            }
            else if (order == OrderAnswersBy.Newest)
            {
                retVal.Sort(
                    delegate(QuizItem left, QuizItem right)
                    {
                        return right.Created.CompareTo(left.Created);
                    });
            }
            else if (order == OrderAnswersBy.Oldest)
            {
                OrderByOldest(retVal);
            }
            else if (order == OrderAnswersBy.Determine)
            {
                // Here we are supposed to determine according to settings in Web.Config how
                // to order the answers...
                if (this.Created + Settings.SpanBeforeOrderingAnswersByVotes < DateTime.Now)
                {
                    OrderByVotes(retVal);
                }
                else
                {
                    OrderByOldest(retVal);
                }
            }
            else
                throw new ApplicationException("Unknown ordering of votes");
            return retVal;
        }

        private static void OrderByOldest(List<QuizItem> retVal)
        {
            retVal.Sort(
                delegate(QuizItem left, QuizItem right)
                {
                    return left.Created.CompareTo(right.Created);
                });
        }

        private static void OrderByVotes(List<QuizItem> retVal)
        {
            retVal.Sort(
                delegate(QuizItem left, QuizItem right)
                {
                    int scoreCompare = right.Score.CompareTo(left.Score);
                    if (scoreCompare != 0)
                        return scoreCompare;
                    else
                    {
                        return left.Created.CompareTo(right.Created);
                    }
                });
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

        public static IEnumerable<QuizItem> Search(string query)
        {
            string[] words = query.Split(' ');
            List<ICriterion> exp = new List<ICriterion>();
            foreach (string idx in words)
            {
                exp.Add(Expression.Like("Header", "%" + idx + "%"));
            }
            exp.Add(Expression.IsNull("Parent"));
            List<QuizItem> retVal = new List<QuizItem>(FindAll(exp.ToArray()));
            retVal.Sort(
                delegate(QuizItem left, QuizItem right)
                {
                    return right.Score.CompareTo(left.Score);
                });
            if (retVal.Count > 10)
                return retVal.GetRange(0, 10);
            return retVal;
        }
    }
}