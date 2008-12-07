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
    CssClass="window askQuestionWindow">
    <div class="askQuestion">
        <ext:Timer 
            runat="server" 
            ID="timerUpdatePreview" 
            Enabled="true" 
            Duration="5000" 
            OnTick="timerUpdatePreview_Tick" />
        <div class="askQuestionLeft">
            <div class="actionButtons">
                Append; 
                <ra:LinkButton 
                    runat="server" 
                    ID="appendLink" 
                    CssClass="linkButton actionButton" 
                    OnClick="Link"
                    Text="Link" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendBold" 
                    CssClass="linkButton actionButton"
                    OnClick="Bold"
                    Text="Bold" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendItalic" 
                    CssClass="linkButton actionButton"
                    OnClick="Italic"
                    Text="Italic" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendList" 
                    CssClass="linkButton actionButton"
                    OnClick="List"
                    Text="List" />
                <ra:LinkButton 
                    runat="server" 
                    ID="appendCode" 
                    CssClass="linkButton actionButton"
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
            <ext:AutoCompleter 
                runat="server" 
                ID="tags" 
                CssClass="auto tagsAuto" 
                OnAutoCompleterItemSelected="tags_AutoCompleterItemSelected"
                OnRetrieveAutoCompleterItems="tags_RetrieveAutoCompleterItems" />

            <ra:Button 
                runat="server" 
                ID="btnSubmit" 
                CssClass="questionSubmit"
                OnClick="btnSubmit_Click"
                Text="Ask" />
        </div>
        <div class="askQuestionRight">
<pre class="legend">
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
&gt; Quote
<br />
All HTML will be escaped
        </div>
        <ra:Label 
            runat="server" 
            ID="preview" 
            CssClass="quizContent"
            Tag="div" />
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurerAsk" />
</ext:Window>






