using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;
using Castle.ActiveRecord.Queries;
using System;
using System.Collections;

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
        public int Id
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
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [HasAndBelongsToMany(typeof(QuizItem), 
            Table="QuizItemTag", ColumnKey="TagId", ColumnRef="QuizItemId", Inverse=true)]
        public IList Item
        {
            get { return _quizItem; }
            set { _quizItem = value; }
        }

        public override void Save()
        {
            if (_id == 0)
            {
                Created = DateTime.Now;
            }
            base.Save();
        }
    }
}
