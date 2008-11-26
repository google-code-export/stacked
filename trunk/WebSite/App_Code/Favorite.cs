using System;
using Castle.ActiveRecord;
using NHibernate.Expression;

namespace Entities
{
    [ActiveRecord(Table = "Favorites")]
    public class Favorite : ActiveRecordBase<Favorite>
    {
        private int _id;
        private DateTime _created;
        private Operator _favoredBy;
        private QuizItem _question;

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

        [BelongsTo("FK_FavoredBy")]
        public Operator FavoredBy
        {
            get { return _favoredBy; }
            set { _favoredBy = value; }
        }

        [BelongsTo("FK_Question")]
        public QuizItem Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public override void Save()
        {
            if (Question == null)
                throw new ApplicationException("Serious error, tried to favor a 'null' question");
            if (FavoredBy == null)
                throw new Exception("You must log in or create an account to add favorites");
            if (_id == 0)
            {
                Created = DateTime.Now;
            }
            base.Save();
        }
    }
}
