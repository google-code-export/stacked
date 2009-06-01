using System;
using Entities;
using NHibernate.Expression;
using Ra.Widgets;

public partial class UserControls_Register : System.Web.UI.UserControl
{
    public event EventHandler UserRegistered;

    public void ShowRegisterWindow()
    {
        register.Visible = true;
        username.Text = "username";
        username.Focus();
        username.Select();
    }

    protected void CloseWindow(object sender, EventArgs e)
    {
        register.Visible = false;
    }

    protected void registerBtn_Click(object sender, EventArgs e)
    {
        // Validating data...
        if (Operator.FindFirst(Expression.Eq("Username", username.Text)) != null)
        {
            ShowError("Username NOT available");
        }
        else if (password.Text != repeatPassword.Text)
        {
            ShowError("Passwords do not match");
        }
        else if (password.Text.Trim().Length < 4)
        {
            ShowError("Passwords is too short");
        }
        else
        {
            // VALIDATED...!! :)

            Operator o = new Operator();
            o.IsAdmin = false;
            o.Password = password.Text;
            o.Username = username.Text;
            o.FriendlyName = username.Text;
            o.Save();
            Operator.Login(username.Text, password.Text, false);

            register.Visible = false;
            if (UserRegistered != null)
                UserRegistered(this, new EventArgs());
        }
    }

    private void ShowError(string error)
    {
        lblErr.Text = error;
        new EffectHighlight(lblErr, 1000).Render();
        username.Select();
        username.Focus();
    }
}
