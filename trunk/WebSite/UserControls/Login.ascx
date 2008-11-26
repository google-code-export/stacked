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

<ext:Window 
    runat="server" 
    ID="login" 
    CssClass="window" 
    Visible="false" 
    DefaultWidget="loginBtn"
    style="width:350px;top:10px;right:10px;position:absolute;z-index:5000;"
    Caption="Please login">
    <div>
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
                <ra:Button 
                    runat="server" 
                    ID="loginBtn" 
                    CssClass="btn"
                    OnClick="loginBtn_Click"
                    Text="Login" />
            </tr>
        </table>
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurer" />
</ext:Window>




