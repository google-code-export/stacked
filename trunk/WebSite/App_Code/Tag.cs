using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Entities
{
    [ActiveRecord(Table = "Tags")]
    public class Tag : ActiveRecordBase<Tag>
    {
        private string _name;
        private IList _quizItem;
        private int _id;
        private DateTime _created;

        [PrimaryKey]
        public virtual int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public virtual DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        [Property]
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [HasAndBelongsToMany(typeof(QuizItem), 
            Table="QuizItemTag", ColumnKey="TagId", ColumnRef="QuizItemId", Inverse=true, Lazy=true)]
        public virtual IList Item
        {
            get { return _quizItem; }
            set { _quizItem = value; }
        }

        public override void Save()
        {
            this.Name = this.Name.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
            if (_id == 0)
            {
                Created = DateTime.Now;
            }
            base.Save();
        }

        public int NumberOfOccurencies
        {
            get
            {
                // Checking cache...
                if (HttpContext.Current.Cache["tagCount_" + this.Name] != null)
                    return (int)HttpContext.Current.Cache["tagCount_" + this.Name];

                // Cache miss
                int retVal = Item.Count;
                HttpContext.Current.Cache.Insert(
                    "tagCount_" + this.Name, 
                    retVal, 
                    null, 
                    DateTime.Now.AddMinutes(5), 
                    Cache.NoSlidingExpiration);
                return retVal;
            }
        }
    }
}
