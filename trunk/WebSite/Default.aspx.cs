using System;
using Entities;
using NHibernate.Expression;
using Ra.Widgets;

public partial class _Default : System.Web.UI.Page, IDefault
{
    private Operator _questionsForOperator;

    protected override void OnInit(EventArgs e)
    {
        string id = Request["operatorProfile"];
        if (id != null)
        {
            _questionsForOperator = Operator.FindOne(Expression.Eq("Username", id));
            Title = "Profile of " + _questionsForOperator.FriendlyName;
            tabMost.Visible = false;
            tabUn.Visible = false;
            tabFav.Visible = true;
            tabFav.Caption += _questionsForOperator.FriendlyName;
            tabNew.Caption = "Questions asked by; " + _questionsForOperator.FriendlyName;
        }
        if (!IsPostBack)
        {
            DataBindNewQuestions();
            lblCount.Text += Operator.Count();
        }
        base.OnInit(e);
    }

    private void DataBindNewQuestions()
    {
        gridNew.DataBindGrid(QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.New));
    }

    protected void tabContent_ActiveTabViewChanged(object sender, EventArgs e)
    {
        if (tab.ActiveTabViewIndex == 0)
        {
            tabNew.Style["display"] = "none";
            new EffectFadeIn(tabNew, 500).Render();
        }
        else if (tab.ActiveTabViewIndex == 1)
        {
            if (!gridMost.IsDataBound)
            {
                gridMost.DataBindGrid(QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.Top));
            }
            new EffectFadeIn(tabMost, 500).Render();
        }
        else if (tab.ActiveTabViewIndex == 2)
        {
            if (!gridUn.IsDataBound)
            {
                gridUn.DataBindGrid(QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.Unanswered));
            }
            new EffectFadeIn(tabUn, 500).Render();
        }
        else if (tab.ActiveTabViewIndex == 3)
        {
            if (!gridFav.IsDataBound)
            {
                gridFav.DataBindGrid(QuizItem.GetFavoredQuestions(_questionsForOperator));
            }
            new EffectFadeIn(tabFav, 500).Render();
        }
    }

    public void QuestionsUpdated()
    {
        DataBindNewQuestions();
    }
}
