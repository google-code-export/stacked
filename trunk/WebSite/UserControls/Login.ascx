<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="Login.ascx.cs" 
    Inherits="UserControls_Login" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

<%@ Register 
    Assembly="DotNetOpenId" 
    Namespace="DotNetOpenId.RelyingParty" 
    TagPrefix="openId" %>

<ext:Window 
    runat="server" 
    ID="login" 
    CssClass="window loginWindow" 
    Visible="false" 
    Caption="Please login">

    <div class="loginDiv">
        <ra:Panel 
            runat="server" 
            DefaultWidget="Button1"
            CssClass="openIdWrapper"
            ID="openIdWrapper">
            <openId:OpenIdTextBox 
                runat="server" 
                OnLoggedIn="openIdTxt_LoggedIn" 
                OnFailed="openIdTxt_Failed" 
                Text="user.someOpenId.com"
                ID="openIdTxt" />
            <asp:Button 
                runat="server" 
                ID="Button1" 
                Text="Login with OpenID" 
                OnClick="login_Click" />
            <ra:LinkButton 
                runat="server" 
                ID="btnNoOpenId" 
                CssClass="linkButton openIdBtn"
                OnClick="btnNoOpenId_Click" 
                AccessKey="I" 
                Tooltip="Keyboard shortcut SHIFT+ALT+I"
                Text="I don't use OpenID" />
            <ra:LinkButton 
                runat="server" 
                ID="whatsOpenID" 
                CssClass="linkButton whatsOpenId"
                OnClick="whatsOpenID_Click"
                Text="What is 'OpenID'...?" />
            <br />
            <ra:CheckBox 
                runat="server" 
                Text="Public terminal"
                CssClass="publicTerminalOpenID" 
                Tooltip="Mark this checkbox if you're NOT on a computer you own yourself!"
                ID="publicTerminalOpenID" />
            <ra:Label 
                runat="server" 
                CssClass="errLblOpenId"
                ID="errLblOpenId" />
        </ra:Panel>
        <ra:Panel 
            runat="server" 
            DefaultWidget="loginBtn"
            ID="nativeWrapper">
            <table class="loginTable">
                <tr>
                    <td>Username: </td>
                    <td>
                        <ra:TextBox 
                            runat="server" 
                            OnEscPressed="CloseLogin"
                            ID="username" />
                    </td>
                </tr>
                <tr>
                    <td>Password: </td>
                    <td>
                        <ra:TextBox 
                            runat="server" 
                            ID="password" 
                            OnEscPressed="CloseLogin"
                            TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <ra:CheckBox 
                            runat="server" 
                            Text="Public terminal?"
                            Tooltip="Mark this checkbox if you're NOT on a computer you own yourself!"
                            ID="publicTerminale" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <ra:Label 
                            runat="server" 
                            style="color:Red;"
                            ID="lblErr" />
                    </td>
                </tr>
            </table>
            <ra:LinkButton 
                runat="server" 
                ID="useOpenID" 
                CssClass="linkButton openIdBtn"
                OnClick="useOpenID_Click" 
                AccessKey="D"
                Tooltip="Keyboard shortcut SHIFT+ALT+D"
                Text="I want to use OpenID" />
            <ra:Button 
                runat="server" 
                ID="loginBtn" 
                CssClass="loginNativeBtn"
                OnClick="loginBtn_Click"
                Text="Login" />
        </ra:Panel>
    </div>

    <ra:BehaviorObscurable runat="server" ID="obscurer" />
</ext:Window>


<ext:Window 
    runat="server" 
    ID="wndWhatIsOpenID" 
    CssClass="window" 
    Visible="false" 
    style="width:800px;top:50px;right:50px;position:absolute;z-index:5005;"
    Caption="What is OpenID...?">
    <div style="padding:15px;height:400px;overflow:auto;background:Transparent url(media/openidnet_logo.gif) no-repeat;">
        <h1>What is OpenID...?</h1>
        <p>
            Snippet from the <a target="_blank" href="http://openid.net/what/">OpenID.net</a> website...
        </p>
        <p>
            OpenID eliminates the need for multiple usernames across different websites, simplifying your 
            online experience.
        </p>
        <p>
            You get to choose the OpenID Provider that best meets your needs and most importantly that you 
            trust. At the same time, your OpenID can stay with you, no matter which Provider you move to. 
            And best of all, the OpenID technology is not proprietary and is completely free.
        </p>
        <p>
            For businesses, this means a lower cost of password and account management, while drawing new 
            web traffic. OpenID lowers user frustration by letting users have control of their login.
        </p>
        <p>
            For geeks, OpenID is an open, decentralized, free framework for user-centric digital identity. 
            OpenID takes advantage of already existing internet technology (URI, HTTP, SSL, Diffie-Hellman) 
            and realizes that people are already creating identities for themselves whether it be at their 
            blog, photostream, profile page, etc. With OpenID you can easily transform one of these existing 
            URIs into an account which can be used at sites which support OpenID logins.
        </p>
        <p>
            OpenID is still in the adoption phase and is becoming more and more popular, as large organizations 
            like AOL, Microsoft, Sun, Novell, etc. begin to accept and provide OpenIDs. Today it is estimated 
            that there are over 160-million OpenID enabled URIs with nearly ten-thousand sites supporting 
            OpenID logins.
        </p>
        <h2>How to use OpenID</h2>
        <p>
            First of all you probably already *have* an Open ID. If you're a registered user at Google, Yahoo
            or any other large website then you can use your existing OpenID from these providers. The way to
            do this depends upon your OpenID provider, but for the most common OpenID providers we have listed
            the URLs below;
        </p>
        <ul>
            <li><strong>AOL</strong> - openid.aol.com/<strong>screenname</strong></li>
            <li><strong>Blogger</strong> - <strong>blogname</strong>.blogspot.com</li>
            <li><strong>Flickr</strong> - www.flickr.com/photos/<strong>username</strong></li>
            <li><strong>Technorati</strong> - technorati.com/people/technorati/<strong>username</strong></li>
            <li>Plus many more...</li>
        </ul>
        <p style="padding-top:25px;">
            Check out a more extensive list of OpenID providers at <a target="_blank" href="http://openid.net/get/">the OpenID.net website</a>...
        </p>
        <p style="padding-bottom:50px;">
            <em>Read more about OpenID at the <a target="_blank" href="http://openid.net/what/">OpenID.net website</a>...</em>
        </p>
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurerWhat" />
</ext:Window>


