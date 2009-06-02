using System;
using Entities;
using Ra.Widgets;
using Ra.Effects;
using Utilities;

public partial class UserControls_Login : System.Web.UI.UserControl
{
    public event EventHandler LoggedIn;

    private bool _wasVisibleInLoad;

    public void ShowLogin()
    {
        login.Visible = true;
        lblErr.Text = "";
        InitializeVisibilityOfLoginControls();
    }

    private void InitializeVisibilityOfLoginControls()
    {
        if (Settings.AllowOpenID)
        {
            // OpenID allowed
            openIdWrapper.Visible = true;
        }
        else
        {
            // No OpenID allowed...
            openIdWrapper.Visible = false;
            useOpenID.Visible = false;
        }
        if (Settings.AllowNativeRegistering)
        {
            nativeWrapper.Visible = true;
        }
        else
        {
            // No "native" registering allowed
            nativeWrapper.Visible = false;
            btnNoOpenId.Visible = false;
        }
        if (Settings.DefaultRegistering == RegisteringType.Native)
        {
            // Native registering is default
            if (!Settings.AllowNativeRegistering)
                throw new ApplicationException("Native registering was set as default, but native registering is also not ALLOWED - modify your web.config please to allow native registering");

            // Making sure we're resetting any "effect" styles...
            openIdWrapper.Style["display"] = "none";
            nativeWrapper.Style["display"] = "";
            nativeWrapper.Style["opacity"] = "";
        }
        else
        {
            // OpenID registering is default
            if (!Settings.AllowOpenID)
                throw new ApplicationException("OpenID login was set as default, but OpenID login is also not ALLOWED - modify your web.config please to allow OpenID");

            // Making sure we're resetting any "effect" styles...
            nativeWrapper.Style["display"] = "none";
            openIdWrapper.Style["display"] = "";
            openIdWrapper.Style["opacity"] = "";
        }
    }

    protected void CloseLogin(object sender, EventArgs e)
    {
        login.Visible = false;
    }

    protected override void OnLoad(EventArgs e)
    {
        _wasVisibleInLoad = login.Visible;
        lblErr.Text = "";
        base.OnLoad(e);
    }

    protected void whatsOpenID_Click(object sender, EventArgs e)
    {
        wndWhatIsOpenID.Visible = true;
    }

    protected void login_Click(object sender, EventArgs e)
    {
        Session["publicTerminal"] = publicTerminalOpenID.Checked;
        openIdTxt.LogOn();
    }

    protected void btnNoOpenId_Click(object sender, EventArgs e)
    {
        username.Text = "username";
        new EffectFadeOut(openIdWrapper, 300)
            .ChainThese(new EffectFadeIn(nativeWrapper, 300)
                .ChainThese(new EffectFocusAndSelect(username)))
        .Render();
    }

    protected void useOpenID_Click(object sender, EventArgs e)
    {
        new EffectFadeOut(nativeWrapper, 300)
            .ChainThese(new EffectFadeIn(openIdWrapper, 300)
                .ChainThese(new EffectFocusAndSelect(openIdTxt.FindControl("wrappedTextBox"))))
        .Render();
    }

    protected void openIdTxt_LoggedIn(object sender, DotNetOpenId.RelyingParty.OpenIdEventArgs e)
    {
        if (e.Response.ClaimedIdentifier != null && 
            e.Response.Status == DotNetOpenId.RelyingParty.AuthenticationStatus.Authenticated)
        {
            string userNameTxt = e.Response.ClaimedIdentifier;
            bool publicTerminal = true;
            if (Session["publicTerminal"] != null)
                publicTerminal = (bool)Session["publicTerminal"];
            Operator.LoginOpenID(userNameTxt, e.Response.FriendlyIdentifierForDisplay, publicTerminal);

            login.Visible = false;
            if (LoggedIn != null)
                LoggedIn(this, new EventArgs());
        }
    }

    protected void openIdTxt_Failed(object sender, EventArgs e)
    {
        errLblOpenId.Text = "Couldn't log you in";
        new EffectHighlight(errLblOpenId, 500).Render();
        new EffectFocusAndSelect(openIdTxt.FindControl("wrappedTextBox")).Render();
    }

    protected override void OnPreRender(EventArgs e)
    {
        if (login.Visible && !_wasVisibleInLoad)
        {
            if (nativeWrapper.Style["display"] != "none")
            {
                username.Text = "username";
                password.Text = "";
                username.Select();
                username.Focus();
            }
            else
            {
                openIdTxt.Text = "user.someOpenId.com";
                new EffectFocusAndSelect(openIdTxt.FindControl("wrappedTextBox")).Render();
            }
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
