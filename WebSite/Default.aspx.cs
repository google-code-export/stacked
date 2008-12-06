using System;
using Entities;
using NHibernate.Expression;
using Ra.Widgets;
using Ra.Extensions;
using System.Drawing;

public partial class _Default : System.Web.UI.Page
{
    private Operator _questionsForOperator;
    private Tag _questionsForTag;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = Request["operatorProfile"];
        string tag = Request["tags"];
        if (id != null)
        {
            _questionsForOperator = Operator.FindOne(Expression.Eq("Username", id));
            Title = "Profile of " + _questionsForOperator.FriendlyName;
        }
        else if (tag != null)
        {
            _questionsForTag = Tag.FindOne(Expression.Eq("Name", tag));
            Title = "Posts tagged with " + _questionsForTag.Name;
        }
        if (!IsPostBack)
        {
            DataBindGrid();
            lblCount.Text += Operator.Count();
        }
        CreateBehaviorsForTabViews();
    }

    protected void tabContent_ActiveTabViewChanged(object sender, EventArgs e)
    {
        DataBindGrid();

        // We must RE-add the bahviors to the TbView's "Button" property here since
        // when changing TabView the Button is "re-created"...
        CreateBehaviorsForTabViews();
    }

    private void CreateBehaviorsForTabViews()
    {
        tabLateAct.Button.Controls.Add(new BehaviorUpdater(200, 0.3M));
        tabMost.Button.Controls.Add(new BehaviorUpdater(200, 0.3M));
        tabNew.Button.Controls.Add(new BehaviorUpdater(200, 0.3M));
        tabUn.Button.Controls.Add(new BehaviorUpdater(200, 0.3M));
    }

    private void DataBindGrid()
    {
        if (_questionsForOperator != null && _questionsForTag != null)
        {
            // Cannot have BOTH filter by tags and by operator
            throw new ApplicationException("Bugin application, cannot have both filter operations on Tags and User");
        }
        TabView tabViewToUpdate = null;
        UserControls_ItemGrid gridToUpate = null;
        QuizItem.OrderBy order;
        if (tab.ActiveTabView == tabLateAct)
        {
            tabViewToUpdate = tabLateAct;
            gridToUpate = gridLateAct;
            order = QuizItem.OrderBy.LatestActivity;
        }
        else if (tab.ActiveTabView == tabNew)
        {
            tabViewToUpdate = tabNew;
            gridToUpate = gridNew;
            order = QuizItem.OrderBy.New;
        }
        else if (tab.ActiveTabView == tabMost)
        {
            tabViewToUpdate = tabMost;
            gridToUpate = gridMost;
            order = QuizItem.OrderBy.Top;
        }
        else if (tab.ActiveTabView == tabUn)
        {
            tabViewToUpdate = tabUn;
            gridToUpate = gridUn;
            order = QuizItem.OrderBy.Unanswered;
        }
        else
            throw new ApplicationException("Added grid without adding databinding logic to it!");

        if (_questionsForTag != null)
            gridToUpate.DataBindGrid(QuizItem.GetTaggedQuestions(order, _questionsForTag));
        else if (_questionsForOperator != null)
            gridToUpate.DataBindGrid(QuizItem.GetQuestionsFromOperator(order, _questionsForOperator));
        else
            gridToUpate.DataBindGrid(QuizItem.GetQuestions(order));
    }
}
