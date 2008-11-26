using System;
using Entities;
using NHibernate.Expression;

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
            newQuiz.Caption = "Questions asked by; " + _questionsForOperator.Username;
        }
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBindNewQuestions();
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
        newRep.DataSource = QuizItem.GetQuestions(_questionsForOperator);
        newRep.DataBind();
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
