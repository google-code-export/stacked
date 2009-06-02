/*
 * Ra-Ajax, Copyright 2008 - 2009 - Thomas Hansen polterguy@gmail.com
 * Ra is licensed under an MITish license. The licenses should 
 * exist on disc where you extracted Ra and will be named license.txt
 * 
 */

using System;
using Ra.Widgets;
using Entities;
using Ra.Extensions;
using System.Web;
using Utilities;
using Ra;
using Castle.ActiveRecord;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Samples
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Request.Browser.Browser == "IE")
            {
                ieWarning.Visible = true;
            }
        }

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
            profileWindow.Visible = true;
            changeFriendlyName.Text = Operator.Current.FriendlyName;
            changeEmail.Text = Operator.Current.Email;
            imgGravatar.ImageUrl = Operator.Current.Gravatar.Replace("s=32", "s=64");
            changeFriendlyName.Select();
            changeFriendlyName.Focus();
        }

        protected void changeEmail_KeyUp(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(changeEmail.Text))
                return;

            StringBuilder emailHash = new StringBuilder();
            MD5 md5 = MD5.Create();
            byte[] emailBuffer = Encoding.ASCII.GetBytes(changeEmail.Text);
            byte[] hash = md5.ComputeHash(emailBuffer);

            foreach (byte hashByte in hash)
                emailHash.Append(hashByte.ToString("x2"));

            imgGravatar.ImageUrl = string.Format("http://www.gravatar.com/avatar/{0}?s=64&d=identicon", emailHash.ToString());
        }

        protected void saveProfile_Click(object sender, EventArgs e)
        {
            Operator.Current.FriendlyName = changeFriendlyName.Text;
            Operator.Current.Email = changeEmail.Text;
            goToProfile.Text = Operator.Current.FriendlyName;
            Operator.Current.Save();
            profileWindow.Visible = false;
        }

        protected void changeProfile_EscPressed(object sender, EventArgs e)
        {
            profileWindow.Visible = false;
        }

        protected void auto_RetrieveAutoCompleterItems(object sender, AutoCompleter.RetrieveAutoCompleterItemsEventArgs e)
        {
            if (e.Query.Trim() == string.Empty)
                return;
            foreach (QuizItem idx in QuizItem.Search(e.Query))
            {
                AutoCompleterItem a = new AutoCompleterItem();
                System.Web.UI.WebControls.Literal lit = new System.Web.UI.WebControls.Literal();
                string tmpHeader = idx.Header;
                foreach (string idxStr in e.Query.Split(' '))
                {
                    int index = tmpHeader.IndexOf(idxStr, StringComparison.InvariantCultureIgnoreCase);
                    if (index != -1)
                    {
                        tmpHeader = tmpHeader.Insert(index + idxStr.Length, "</span>");
                        tmpHeader = tmpHeader.Insert(index, "<span class=\"found\">");
                    }
                }
                lit.Text = string.Format("<a href=\"{0}\">{1}</a>", idx.Url, tmpHeader);
                a.Controls.Add(lit);
                e.Controls.Add(a);
            }
        }

        protected void register_UserRegistered(object sender, EventArgs e)
        {
            infoLabel.Text = "Successfully registered";
            new EffectFadeIn(infoLabel, 1000)
                .ChainThese(new EffectHighlight(infoLabel, 500))
                .Render();
            infoTimer.Enabled = true;
            Login();
        }

        protected void infoTimer_Tick(object sender, EventArgs e)
        {
            new EffectHighlight(infoLabel, 500)
                .ChainThese(new EffectFadeOut(infoLabel, 1000))
                .Render();
            infoTimer.Enabled = false;
        }

        protected void askQuestion_Click(object sender, EventArgs e)
        {
            ask.ShowAskQuestion();
        }

        protected void ask_QuestionAsked(object sender, UserControls_AskQuestion.QuestionAskedEventArgs e)
        {
            AjaxManager.Instance.Redirect("~/" + e.Url);
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            login.ShowLogin();
        }

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            register.ShowRegisterWindow();
        }

        protected void login_LoggedIn(object sender, EventArgs e)
        {
            Login();
            new EffectHighlight(loginPnl, 500).Render();
        }

        private void Login()
        {
            loginBtn.Visible = false;
            registerBtn.Visible = false;
            logoutBtn.Visible = true;
            askQuestion.Visible = true;
            goToProfile.Visible = true;
            goToProfile.Text = Operator.Current.FriendlyName;
            logoutBtn.Text = "Logout";
        }

        protected void logoutBtn_Click(object sender, EventArgs e)
        {
            Operator.Logout();
            loginBtn.Visible = true;
            registerBtn.Visible = true;
            goToProfile.Visible = false;
            logoutBtn.Visible = false;
            askQuestion.Visible = false;
            new EffectHighlight(loginPnl, 500).Render();
        }
    }
}
