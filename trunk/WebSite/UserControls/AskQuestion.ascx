<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="AskQuestion.ascx.cs" 
    Inherits="UserControls_AskQuestion" %>

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
    ID="wndAsk" 
    Caption="Ask a question..."
    Visible="false"
    style="width:500px;position:absolute;top:100px;left:100px;z-index:5000;"
    CssClass="window">
    <div class="askQuestion">
        <ra:TextBox 
            runat="server" 
            ID="header" />

        <ra:TextArea 
            runat="server" 
            ID="body" />

        <ra:Button 
            runat="server" 
            ID="btnSubmit" 
            OnClick="btnSubmit_Click"
            Text="Ask" />
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurerAsk" />
</ext:Window>






