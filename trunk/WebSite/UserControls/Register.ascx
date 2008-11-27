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
    CssClass="window" 
    DefaultWidget="registerBtn"
    Visible="false" 
    style="width:350px;top:10px;right:10px;position:absolute;z-index:5000;"
    Caption="Register new profile">
    <div style="position:relative;height:170px;">
        <table class="login">
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
                <td>Email</td>
                <td>
                    <ra:TextBox 
                        runat="server" 
                        OnEscPressed="CloseWindow"
                        id="email" />
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
        <ra:Button 
            runat="server" 
            ID="registerBtn" 
            CssClass="loginBtn"
            OnClick="registerBtn_Click"
            Text="Register" />
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurer" />
</ext:Window>







