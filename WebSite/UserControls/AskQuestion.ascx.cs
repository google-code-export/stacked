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

    protected void Link(object sender, EventArgs e)
    {
        body.Text += "[http://x.com some acnchor text]";
    }

    protected void Bold(object sender, EventArgs e)
    {
        body.Text += "*some bold text*";
    }

    protected void Italic(object sender, EventArgs e)
    {
        body.Text += "_some italic text_";
    }

    protected void List(object sender, EventArgs e)
    {
        body.Text += "\r\n* list item 1\r\n* list item 2\r\n* list item 3";
    }

    protected void Code(object sender, EventArgs e)
    {
        body.Text += "\r\n[code]\r\n    int x = foo();\r\n    bar(54);\r\n[/code]";
    }

    protected void EnterPressed(object sender, EventArgs e)
    {
        body.Select();
        body.Focus();
    }

    protected void timerUpdatePreview_Tick(object sender, EventArgs e)
    {
        preview.Text = QuizItem.FormatWiki(body.Text);
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
