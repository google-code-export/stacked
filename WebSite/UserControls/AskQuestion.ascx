<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="AskQuestion.ascx.cs" 
    Inherits="UserControls_AskQuestion" %>

<ra:Window 
    runat="server" 
    ID="wndAsk" 
    Caption="Ask A Question"
    Visible="false" 
    CssClass="window askQuestionWindow">
    <div class="askQuestion">
        <ra:Timer 
            runat="server" 
            ID="timerUpdatePreview" 
            Enabled="true" 
            Duration="5000" 
            OnTick="timerUpdatePreview_Tick" />
        <div class="askQuestionLeft">
            <br />Question Title:
            <ra:TextBox 
                runat="server" 
                OnEscPressed="EscPressed" 
                OnEnterPressed="EnterPressed"
                ID="header" 
                Style="border: 1px solid #CCD1FF;" />
            <br />Question:
            <div class="actionButtons">
                <ra:LinkButton 
                    runat="server" 
                    ID="appendLink" 
                    CssClass="linkButton actionButton" 
                    OnClick="Link"
                    Tooltip="Link"><img src="media/icons/link_add.png" alt="Link" /></ra:LinkButton>
                <ra:LinkButton 
                    runat="server" 
                    ID="appendBold" 
                    CssClass="linkButton actionButton"
                    OnClick="Bold"
                    Tooltip="Bold"><img src="media/icons/text_bold.png" alt="Bold" /></ra:LinkButton>
                <ra:LinkButton 
                    runat="server" 
                    ID="appendItalic" 
                    CssClass="linkButton actionButton"
                    OnClick="Italic"
                    Tooltip="Italic"><img src="media/icons/text_italic.png" alt="Italic" /></ra:LinkButton>
                <ra:LinkButton 
                    runat="server" 
                    ID="appendList" 
                    CssClass="linkButton actionButton"
                    OnClick="List"
                    Tooltip="List"><img src="media/icons/text_list_bullets.png" alt="List" /></ra:LinkButton>
                <ra:LinkButton 
                    runat="server" 
                    ID="appendCode" 
                    CssClass="linkButton actionButton"
                    OnClick="Code"
                    Tooltip="Code"><img src="media/icons/script_code.png" alt="Code" /></ra:LinkButton>
            </div>
            <ra:TextArea 
                runat="server" 
                OnEscPressed="EscPressed"
                ID="body" />
            <br />Tags (space sperated):
            <ra:AutoCompleter 
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
        <div>
<pre class="legend">

Link:
[http://website.com text]

*bold*

_italic_

 * Listitem 1
 * Listitem 2
 * Listitem 3

[youtube xxx]
YouTube video xxx = ID

[gmaps x y]
GMaps x = longitude
      y = latitude
</pre>
<p>
2 x Carriage Return = new paragraph
</p>
<p>
1 x Carriage Return = line break &lt;br /&gt;
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
</ra:Window>






