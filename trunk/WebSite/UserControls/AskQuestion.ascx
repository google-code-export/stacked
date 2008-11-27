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
    style="width:600px;position:absolute;top:100px;left:100px;z-index:5000;"
    CssClass="window">
    <div class="askQuestion">
        <ext:Timer 
            runat="server" 
            ID="timerUpdatePreview" 
            Enabled="true" 
            Duration="5000" 
            OnTick="timerUpdatePreview_Tick" />
        <div class="askQuestionLeft">
            <div style="margin:15px;">
                Append; 
                <ra:LinkButton 
                    runat="server" 
                    ID="appendLink" 
                    CssClass="appendButton" 
                    OnClick="Link"
                    Text="Link" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendBold" 
                    CssClass="appendButton"
                    OnClick="Bold"
                    Text="Bold" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendItalic" 
                    CssClass="appendButton"
                    OnClick="Italic"
                    Text="Italic" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendList" 
                    CssClass="appendButton"
                    OnClick="List"
                    Text="List" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendCode" 
                    CssClass="appendButton"
                    OnClick="Code"
                    Text="Code" />
            </div>
            <ra:TextBox 
                runat="server" 
                OnEscPressed="EscPressed" 
                OnEnterPressed="EnterPressed"
                ID="header" />

            <ra:TextArea 
                runat="server" 
                OnEscPressed="EscPressed"
                ID="body" />

            <ra:Button 
                runat="server" 
                ID="btnSubmit" 
                CssClass="questionSubmit"
                OnClick="btnSubmit_Click"
                Text="Ask" />
        </div>
        <div class="askQuestionRight">
<pre style="font-size:0.9em;">
Link;
[http://x.com text]

*bold*

_italic_

* Listitem 1
* Listitem 2
* Listitem 3
</pre>
<p>
2 x Carriage Return is opening a new paragraph
</p>
<p>
1 x Carriage Return is break; &lt;br /&gt;
</p>
<pre>
[code]
if( YouDoThis )
  YouCanWriteCode();
[/code]
</pre>
All HTML will be escaped
        </div>
        <div style="clear:both;margin-left:0;border:none;width:80%;overflow:auto;" class="quizContent">
            <ra:Label 
                runat="server" 
                ID="preview" 
                Tag="div" />
        </div>
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurerAsk" />
</ext:Window>






