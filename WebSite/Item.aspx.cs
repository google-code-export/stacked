using System;
using Entities;
using NHibernate.Expression;
using Ra.Widgets;
using Ra;

public partial class Item : System.Web.UI.Page
{
    private QuizItem _question;

    protected override void OnInit(EventArgs e)
    {
        GetQuestion();
        FillOutContent();
        SetCssClassIfCurrentOperatorHasVoted();
        SetCssClassForFavorite();
        base.OnInit(e);
    }

    protected void star_Click(object sender, EventArgs e)
    {
        try
        {
            // Clicking the star TOGGLES favorites, meaning if it's there it'll be deleted and vice versa...
            Favorite favorite = Favorite.FindFirst(
                Expression.Eq("FavoredBy", Operator.Current),
                Expression.Eq("Question", _question));
            if (favorite != null)
                favorite.Delete();
            else
            {
                Favorite f = new Favorite();
                f.FavoredBy = Operator.Current;
                f.Question = _question;
                f.Save();
            }
            Highlight(star);
            SetCssClassForFavorite();
        }
        catch (Exception err)
        {
            ShowError(err.Message);
        }
    }

    private void SetCssClassForFavorite()
    {
        if (Favorite.FindFirst(
            Expression.Eq("FavoredBy", Operator.Current),
            Expression.Eq("Question", _question)) != null)
        {
            star.CssClass = "starFavored";
            star.Tooltip = "Click to REMOVE from favorites";
        }
        else
        {
            star.CssClass = "starNone";
            star.Tooltip = "Click to add to favorites";
        }
    }

    private void FillOutContent()
    {
        header.Text = _question.Header;
        body.Text = _question.BodyFormated;
        count.Text = _question.GetScore().ToString();
        askedBy.InnerHtml = _question.CreatedBy.Username + " - " + _question.CreatedBy.GetCreds();
        askedBy.HRef = _question.CreatedBy.Username + ".user";
        Title = _question.Header;

        deleteQuestion.Visible = Operator.Current != null && Operator.Current.IsAdmin;
    }

    protected void deleteQuestion_Click(object sender, EventArgs e)
    {
        wndDelete.Visible = true;
        deleteBtn.Focus();
    }

    protected void deleteBtn_Click(object sender, EventArgs e)
    {
        _question.Delete();
        AjaxManager.Instance.Redirect("~/");
    }

    private void SetCssClassIfCurrentOperatorHasVoted()
    {
        // Setting CSS classes of voters for QUESTION
        up.CssClass = "up";
        down.CssClass = "down";
        if (Operator.Current != null)
        {
            Vote hasVoted = Vote.FindOne(
                Expression.Eq("VotedBy", Operator.Current),
                Expression.Eq("QuizItem", _question));
            if (hasVoted != null)
            {
                if (hasVoted.Score > 0)
                    up.CssClass = "upVoted";
                else
                    down.CssClass = "downVoted";
            }
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!IsPostBack)
        {
            answers.DataSource = _question.GetAnswers();
            answers.DataBind();
        }
        base.OnLoad(e);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        // Saving question...
        QuizItem q = new QuizItem();
        q.Body = answerBody.Text;
        q.CreatedBy = Operator.Current;
        q.Header = answerHeader.Text;
        q.Parent = _question;
        q.Save();

        // Binding grid again
        answers.DataSource = _question.GetAnswers();
        answers.DataBind();
        answersWrapper.ReRender();

        new EffectHighlight(answersWrapper, 500).Render();
        answerHeader.Text = "";
        answerBody.Text = "";
    }

    private int GetIdOfAnswer(System.Web.UI.Control ctrl)
    {
        foreach (System.Web.UI.Control idx in ctrl.Parent.Controls)
        {
            if (idx is HiddenField)
            {
                return int.Parse((idx as HiddenField).Value);
            }
        }
        return -1;
    }

    private Label FindLabelForAnswer(System.Web.UI.Control ctrl)
    {
        foreach (System.Web.UI.Control idx in ctrl.Parent.Controls)
        {
            if (idx is Label)
            {
                return idx as Label;
            }
        }
        return null;
    }

    private LinkButton FindUpLinkButtonForAnswer(System.Web.UI.Control ctrl)
    {
        foreach (System.Web.UI.Control idx in ctrl.Parent.Controls)
        {
            if (idx is LinkButton)
            {
                return idx as LinkButton;
            }
        }
        return null;
    }

    private LinkButton FindDownLinkButtonForAnswer(System.Web.UI.Control ctrl)
    {
        bool first = true;
        foreach (System.Web.UI.Control idx in ctrl.Parent.Controls)
        {
            if (idx is LinkButton)
            {
                if (first)
                {
                    first = false;
                    continue;
                }
                return idx as LinkButton;
            }
        }
        return null;
    }

    protected void VoteAnswerUp(object sender, EventArgs e)
    {
        int idOfQuizItem = GetIdOfAnswer(sender as System.Web.UI.Control);

        // Saving will throw if you vote for your own question/answer...
        try
        {
            Vote v = new Vote();
            v.QuizItem = QuizItem.Find(idOfQuizItem);
            v.Score = 1;
            v.VotedBy = Operator.Current;
            v.Save();
            FindLabelForAnswer(sender as System.Web.UI.Control).Text = QuizItem.Find(idOfQuizItem).Score.ToString();
            FindUpLinkButtonForAnswer(sender as System.Web.UI.Control).CssClass = "upVoted";
            FindDownLinkButtonForAnswer(sender as System.Web.UI.Control).CssClass = "down";
        }
        catch (Exception err)
        {
            ShowError(err.Message);
        }
    }

    private void ShowError(string error)
    {
        errorLabel.Text = error;
        errorLabel.Visible = true;
        errorLabel.Style["display"] = "none";
        new EffectFadeIn(errorLabel, 1000).Render();
        timerRemoveError.Enabled = true;
    }

    protected void timerRemoveError_Tick(object sender, EventArgs e)
    {
        new EffectFadeOut(errorLabel, 1000).Render();
        timerRemoveError.Enabled = false;
    }

    protected void VoteAnswerDown(object sender, EventArgs e)
    {
        try
        {
            int idOfQuizItem = GetIdOfAnswer(sender as System.Web.UI.Control);
            Vote v = new Vote();
            v.QuizItem = QuizItem.Find(idOfQuizItem);
            v.Score = -1;
            v.VotedBy = Operator.Current;
            v.Save();
            FindLabelForAnswer(sender as System.Web.UI.Control).Text = QuizItem.Find(idOfQuizItem).Score.ToString();
            FindUpLinkButtonForAnswer(sender as System.Web.UI.Control).CssClass = "up";
            FindDownLinkButtonForAnswer(sender as System.Web.UI.Control).CssClass = "downVoted";
        }
        catch (Exception err)
        {
            ShowError(err.Message);
        }
    }

    protected string GetUpCssClassIfVoted(int id)
    {
        QuizItem quiz = QuizItem.Find(id);
        Vote hasVoted = Vote.FindOne(
            Expression.Eq("VotedBy", Operator.Current),
            Expression.Eq("QuizItem", quiz),
            Expression.Eq("Score", 1));
        if (hasVoted != null)
            return "upVoted";
        return "up";
    }

    protected string GetDownCssClassIfVoted(int id)
    {
        QuizItem quiz = QuizItem.Find(id);
        Vote hasVoted = Vote.FindOne(
            Expression.Eq("VotedBy", Operator.Current),
            Expression.Eq("QuizItem", quiz),
            Expression.Eq("Score", -1));
        if (hasVoted != null)
            return "downVoted";
        return "down";
    }

    protected void up_Click(object sender, EventArgs e)
    {
        try
        {
            Vote o = Vote.FindOne(
                Expression.Eq("VotedBy", Operator.Current),
                Expression.Eq("QuizItem", _question),
                Expression.Eq("Score", 1));
            if (o != null)
            {
                // User has voted this BEFORE. Therefore removing old vote...
                o.Delete();
                count.Text = _question.GetScore().ToString();
                down.CssClass = "down";
                up.CssClass = "up";
                Highlight(up);
            }
            else
            {
                CreateVoteForCurrent(true);
            }
        }
        catch (Exception err)
        {
            ShowError(err.Message);
        }
    }

    protected void down_Click(object sender, EventArgs e)
    {
        try
        {
            Vote o = Vote.FindOne(
                Expression.Eq("VotedBy", Operator.Current),
                Expression.Eq("QuizItem", _question),
                Expression.Eq("Score", -1));
            if (o != null)
            {
                // User has voted this BEFORE. Therefore removing old vote...
                o.Delete();
                count.Text = _question.GetScore().ToString();
                down.CssClass = "down";
                up.CssClass = "up";
                Highlight(down);
            }
            else
            {
                CreateVoteForCurrent(false);
            }
        }
        catch (Exception err)
        {
            ShowError(err.Message);
        }
    }

    private void CreateVoteForCurrent(bool isUp)
    {
        Vote v = new Vote();
        v.Score = isUp ? 1 : -1;
        v.VotedBy = Operator.Current;
        v.QuizItem = _question;
        v.Save();
        count.Text = _question.GetScore().ToString();
        down.CssClass = isUp ? "down" : "downVoted";
        up.CssClass = isUp ? "upVoted" : "up";
        Highlight(isUp ? up : down);
    }

    private void Highlight(LinkButton ctrl)
    {
        new EffectHighlight(ctrl, 500).Render();
    }

    private void GetQuestion()
    {
        string id = Request["id"];
        if (string.IsNullOrEmpty(id))
            Response.Redirect("~/", true);
        _question = QuizItem.FindOne(Expression.Eq("Url", id + ".quiz"));
        if (_question == null)
            Response.Redirect("~/", true);

        // Checking to see if we should increase the "view count" of this question
        if (!IsPostBack)
        {
            _question.IncreaseViewCount();
        }
    }
}
