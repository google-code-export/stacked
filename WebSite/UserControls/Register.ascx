<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="Register.ascx.cs" 
    Inherits="UserControls_Register" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

<ext:Window 
    runat="server" 
    ID="register" 
    CssClass="window registerWindow" 
    DefaultWidget="registerBtn"
    Visible="false" 
    Caption="Register">
    <div class="registerDiv">
        <table class="registerTable">
            <tr>
                <td>Username</td>
                <td>
                    <ra:TextBox 
                        runat="server" 
                        OnEscPressed="CloseWindow"
                        id="username" />
                </td>
            </tr>
            <tr>
                <td>Password</td>
                <td>
                    <ra:TextBox 
                        runat="server" 
                        OnEscPressed="CloseWindow"
                        TextMode="Password"
                        id="password" />
                </td>
            </tr>
            <tr>
                <td>Repeat password</td>
                <td>
                    <ra:TextBox 
                        runat="server" 
                        OnEscPressed="CloseWindow"
                        TextMode="Password"
                        id="repeatPassword" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <ra:Label 
                        runat="server" 
                        CssClass="errLbl"
                        ID="lblErr" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="infoOpenId">You can also use OpenID from Login button!</td>
            </tr>
        </table>
        <ra:Button 
            runat="server" 
            ID="registerBtn" 
            CssClass="loginBtn"
            OnClick="registerBtn_Click"
            Text="Register" />
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurer" />
</ext:Window>







