using System;
using Entities;
using Ra.Extensions;
using NHibernate.Expression;
using System.Collections.Generic;

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
        if (body.Text.Trim() == "")
            wndAsk.Visible = false;
    }

    protected void tags_RetrieveAutoCompleterItems(object sender, AutoCompleter.RetrieveAutoCompleterItemsEventArgs e)
    {
        if (e.Query.Trim().Length != 0)
        {
            string[] ents = e.Query.Split(' ');
            if (ents != null && ents.Length > 0 && ents[ents.Length - 1].Trim().Length > 0)
            {
                timerUpdatePreview.Enabled = false;
                preview.Visible = false;
                foreach (Tag idx in Tag.FindAll(Expression.Like("Name", "%" + ents[ents.Length - 1] + "%")))
                {
                    if (idx.Name == ents[ents.Length - 1])
                        continue;
                    AutoCompleterItem i = new AutoCompleterItem();
                    i.CssClass = tags.CssClass + "-item";
                    i.Text = idx.Name;
                    i.ID = "id" + idx.Id;
                    e.Controls.Add(i);
                }
            }
        }
        if (e.Controls.Count > 0)
        {
            preview.Visible = false;
            timerUpdatePreview.Enabled = false;
        }
        else
        {
            preview.Visible = true;
            timerUpdatePreview.Enabled = true;
        }
    }

    protected void tags_AutoCompleterItemSelected(object sender, EventArgs e)
    {
        preview.Visible = true;
        timerUpdatePreview.Enabled = true;
        int id = int.Parse(tags.SelectedItem.Substring(2));
        Tag t = Tag.Find(id);
        string[] ents = tags.Text.Split(' ');
        string tmp = "";
        int idxNo = 0;
        foreach (string idx in ents)
        {
            if (idxNo++ == ents.Length - 1)
                break;
            tmp += idx + " ";
        }
        tags.Text = tmp + t.Name + " ";
    }

    protected void Link(object sender, EventArgs e)
    {
        body.Text += "[http://x.com some anchor text]";
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
        string[] ents = tags.Text.Split(' ');
        if (ents != null && ents.Length > 0 && ents[ents.Length - 1].Trim().Length > 0)
        {
            q.Tags = new List<Tag>();
            foreach (string idx in ents)
            {
                if (idx.Trim().Length > 0)
                {
                    string idx2 = idx.ToLower();
                    Tag t = Tag.FindOne(Expression.Eq("Name", idx2));
                    if (t == null)
                    {
                        t = new Tag();
                        t.Name = idx2;
                        t.Save();
                    }
                    q.Tags.Add(t);
                }
            }
        }
        q.Save();
        if (QuestionAsked != null)
            QuestionAsked(this, new EventArgs());
    }
}
