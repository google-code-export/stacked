using System;
using Entities;

public partial class UserControls_Login : System.Web.UI.UserControl
{
    public event EventHandler LoggedIn;

    private bool _wasVisibleInLoad;

    public void ShowLogin()
    {
        login.Visible = true;
        lblErr.Text = "";
    }

    protected void CloseLogin(object sender, EventArgs e)
    {
        login.Visible = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        _wasVisibleInLoad = login.Visible;
        base.OnLoad(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (login.Visible && !_wasVisibleInLoad)
        {
            username.Text = "username";
            password.Text = "";
            username.Select();
            username.Focus();
        }
        base.OnPreRender(e);
    }

    protected void loginBtn_Click(object sender, EventArgs e)
    {
        if (Operator.Login(username.Text, password.Text, !publicTerminale.Checked))
        {
            login.Visible = false;
            if (LoggedIn != null)
                LoggedIn(this, new EventArgs());
        }
        else
        {
            lblErr.Text = "Username/password combination was wrong...";
            password.Text = "";
            username.Select();
            username.Focus();
        }
    }
}
