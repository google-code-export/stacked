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
    CssClass="window" 
    Visible="false" 
    DefaultWidget="loginBtn"
    style="width:400px;top:10px;right:10px;position:absolute;z-index:5000;"
    Caption="Please login">
    <div style="position:relative;height:140px;">
        <ra:Panel 
            runat="server" 
            style="padding:25px;padding-top:40px;" 
            DefaultWidget="Button1"
            ID="openIdWrapper">
            <openId:OpenIdTextBox 
                runat="server" 
                OnLoggedIn="openIdTxt_LoggedIn"
                ID="openIdTxt" />
            <asp:Button 
                runat="server" 
                ID="Button1" 
                Text="Login with OpenID" 
                OnClick="login_Click" />
            <ra:LinkButton 
                runat="server" 
                ID="btnNoOpenId" 
                CssClass="loginBtnOpenID"
                OnClick="btnNoOpenId_Click"
                Text="I don't use OpenID" />
        </ra:Panel>
        <ra:Panel 
            runat="server" 
            Visible="false"
            style="display:none;"
            ID="nativeWrapper">
            <table class="login">
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
                    <td>Public terminal</td>
                    <td>
                        <ra:CheckBox 
                            runat="server" 
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
                CssClass="loginBtnOpenID"
                OnClick="useOpenID_Click"
                Text="I want to use OpenID" />
            <ra:Button 
                runat="server" 
                ID="loginBtn" 
                CssClass="loginBtn"
                OnClick="loginBtn_Click"
                Text="Login" />
        </ra:Panel>
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurer" />
</ext:Window>




