using System;
using Entities;

public partial class UserControls_AskQuestion : System.Web.UI.UserControl
{
    public event EventHandler QuestionAsked;

    public void ShowAskQuestion()
    {
        wndAsk.Visible = true;
        header.Text = "Header of question";
        header.Select();
        header.Focus();
    }

    protected void EscPressed(object sender, EventArgs e)
    {
        wndAsk.Visible = false;
    }

    protected void EnterPressed(object sender, EventArgs e)
    {
        body.Select();
        body.Focus();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        wndAsk.Visible = false;
        QuizItem q = new QuizItem();
        q.CreatedBy = Operator.Current;
        q.Header = header.Text;
        q.Body = body.Text;
        q.Save();
        if (QuestionAsked != null)
            QuestionAsked(this, new EventArgs());
    }
}
