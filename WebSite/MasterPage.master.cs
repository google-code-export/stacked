using System;
using Ra.Widgets;
using Entities;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected override void OnInit(EventArgs e)
    {
        if (Operator.TryLoginFromCookie())
        {
            // Successfully logged in from cookie...
            Login();
        }
        base.OnInit(e);
    }

    protected void goToProfile_Click(object sender, EventArgs e)
    {
    }

    protected void askQuestion_Click(object sender, EventArgs e)
    {
        ask.ShowAskQuestion();
    }

    protected void ask_QuestionAsked(object sender, EventArgs e)
    {
        IDefault def = (Page as IDefault);
        if (def != null)
            def.QuestionsUpdated();
    }

    protected void loginBtn_Click(object sender, EventArgs e)
    {
        login.ShowLogin();
    }

    protected void login_LoggedIn(object sender, EventArgs e)
    {
        Login();
        new EffectHighlight(loginPnl, 500).Render();
    }

    private void Login()
    {
        loginBtn.Visible = false;
        logouBtn.Visible = true;
        askQuestion.Visible = true;
        goToProfile.Visible = true;
        goToProfile.Text = 
            string.Format("Profile - {1} creds", 
                Operator.Current.Username, 
                Operator.Current.GetCreds());
        logouBtn.Text = "Logout " + Operator.Current.Username;
    }

    protected void logouBtn_Click(object sender, EventArgs e)
    {
        Operator.Logout();
        loginBtn.Visible = true;
        logouBtn.Visible = false;
        askQuestion.Visible = false;
        new EffectHighlight(loginPnl, 500).Render();
    }
}
