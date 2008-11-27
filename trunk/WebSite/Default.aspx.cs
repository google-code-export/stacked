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
            Title = "Profile of " + _questionsForOperator.Username;
            topQuestions.Visible = false;
            unanswered.Visible = false;
            tabFavored.Visible = true;
            tabFavored.Caption += _questionsForOperator.Username;
            newQuiz.Caption = "Questions asked by; " + _questionsForOperator.Username;
        }
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindNewQuestions();
            lblCount.Text += Operator.Count();
        }
    }

    protected string GetCssClass(int count)
    {
        if (count < -10)
            return "really-bad";
        if (count < 0)
            return "bad";
        if (count > 9)
            return "great";
        if (count > 0)
            return "good";

        // "0"
        return "neutral";
    }

    private void DataBindNewQuestions()
    {
        newRep.DataSource = QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.New);
        newRep.DataBind();
    }

    protected void tabContent_ActiveTabViewChanged(object sender, EventArgs e)
    {
        if (tabContent.ActiveTabViewIndex == 0)
        {
            newQuiz.Style["display"] = "none";
            new EffectFadeIn(newQuiz, 500).Render();
        }
        if (tabContent.ActiveTabViewIndex == 1)
        {
            if (repTopQuestions.DataSource == null)
            {
                repTopQuestions.DataSource = QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.Top);
                repTopQuestions.DataBind();
                topQuestionsPanel.ReRender();
            }
            topQuestionsPanel.Style["display"] = "none";
            new EffectFadeIn(topQuestionsPanel, 500).Render();
        }
        else if (tabContent.ActiveTabViewIndex == 2)
        {
            if (repUnansweredQuestions.DataSource == null)
            {
                repUnansweredQuestions.DataSource = QuizItem.GetQuestions(_questionsForOperator, QuizItem.OrderBy.Unanswered);
                repUnansweredQuestions.DataBind();
                unansweredQuestionsPanel.ReRender();
            }
            unansweredQuestionsPanel.Style["display"] = "none";
            new EffectFadeIn(unansweredQuestionsPanel, 500).Render();
        }
        else if (tabContent.ActiveTabViewIndex == 3)
        {
            if (repFavoredBy.DataSource == null)
            {
                repFavoredBy.DataSource = QuizItem.GetFavoredQuestions(_questionsForOperator);
                repFavoredBy.DataBind();
                panelFavoredBy.ReRender();
            }
            panelFavoredBy.Style["display"] = "none";
            new EffectFadeIn(panelFavoredBy, 500).Render();
        }
    }

    protected string GetTime(DateTime time)
    {
        return TimeFormatter.Format(time);
    }

    public void QuestionsUpdated()
    {
        DataBindNewQuestions();
        newQuiz.ReRender();
    }
}
