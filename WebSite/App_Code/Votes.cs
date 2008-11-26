using System;
using Castle.ActiveRecord;
using NHibernate.Expression;
using System.Collections.Generic;

namespace Entities
{
    [ActiveRecord(Table = "Votes")]
    public class Vote : ActiveRecordBase<Vote>
    {
        private int _id;
        private DateTime _created;
        private Operator _votedBy;
        private QuizItem _quizItem;
        private int _score = 1;

        [PrimaryKey]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        [Property]
        public int Score
        {
            get { return _score; }
            set
            {
                if (value != 1 && value != -1)
                    throw new ApplicationException("Something went wrong, a vote had a value of not 1 or -1");
                _score = value;
            }
        }

        [Property]
        public DateTime Created
        {
            get { return _created; }
            set { _created = value; }
        }

        [BelongsTo("FK_VotedBy")]
        public Operator VotedBy
        {
            get { return _votedBy; }
            set { _votedBy = value; }
        }

        [BelongsTo("FK_QuizItem")]
        public QuizItem QuizItem
        {
            get { return _quizItem; }
            set { _quizItem = value; }
        }

        public override void Save()
        {
            if (VotedBy == null)
                throw new ApplicationException("You must login or create an account to vote");
            if (VotedBy.ID == QuizItem.CreatedBy.ID)
                throw new Exception("You cannot vote for your own questions/answers");
            if (_id == 0)
            {
                // Making sure user
                Vote[] previous = Vote.FindAll(
                    Expression.Eq("VotedBy", this.VotedBy),
                    Expression.Eq("QuizItem", this.QuizItem));
                foreach (Vote idx in previous)
                {
                    idx.Delete();
                }
                Created = DateTime.Now;
            }
            base.Save();
        }
    }
}