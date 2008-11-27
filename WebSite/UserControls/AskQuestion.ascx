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
        <div class="askQuestionLeft">
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
    </div>
    <ra:BehaviorObscurable runat="server" ID="obscurerAsk" />
</ext:Window>






