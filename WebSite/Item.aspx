<%@ Page 
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Item.aspx.cs" 
    Inherits="Item" 
    Title="Untitled Page" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

<asp:Content 
    ID="cnt" 
    ContentPlaceHolderID="body" 
    Runat="Server">

    <ext:Window 
        runat="server" 
        ID="wndDelete" 
        Caption="You SURE? There's no rollback for this..."
        Visible="false" 
        style="width:300px;position:absolute;top:100px;left:100px;z-index:5000;"
        CssClass="window">
        <div style="padding:70px;">
            <ra:Button 
                runat="server" 
                ID="deleteBtn" 
                OnClick="deleteBtn_Click"
                Text="Yes I am sure!" />
        </div>
        <ra:BehaviorObscurable runat="server" ID="deleteObscurer" />
    </ext:Window>


    <ra:Label 
        runat="server" 
        ID="header" 
        Tag="h2" />
    <ra:Label 
        runat="server" 
        ID="errorLabel" 
        Visible="false"
        CssClass="error" />
    <ext:Timer 
        runat="server" 
        ID="timerRemoveError" 
        Enabled="false" 
        OnTick="timerRemoveError_Tick"
        Duration="5000" />
    <hr />
    <div class="star">
        <ra:LinkButton 
            runat="server" 
            OnClick="star_Click"
            ID="star" />
    </div>
    <div class="vote">
        <ra:LinkButton 
            runat="server" 
            ID="up" 
            Text="&nbsp;" 
            OnClick="up_Click" />
        <ra:Label 
            runat="server" 
            Text=""
            CssClass="votes"
            ID="count" />
        <ra:LinkButton 
            runat="server" 
            ID="down" 
            Text="&nbsp;"
            OnClick="down_Click" />
        <ra:LinkButton 
            runat="server" 
            ID="deleteQuestion"
            OnClick="deleteQuestion_Click"
            CssClass="deleteQuestion"
            Text="X" />
    </div>
    <div class="quizContent">
        <ra:Label 
            runat="server" 
            ID="body" 
            Tag="div" />
        <a 
            runat="server" 
            id="askedBy" 
            class="operator" />
    </div>
    <br style="clear:both;" />
    <ra:Panel 
        runat="server" 
        ID="answersWrapper">

        <asp:Repeater runat="server" ID="answers">
            <ItemTemplate>
                <div class="answer">
                    <a class="operator" href='<%#Eval("CreatedBy.Username") + ".user" %>'>
                        <%#Eval("CreatedBy.Username")%> - <%#((Entities.Operator)Eval("CreatedBy")).GetCreds()%> creds
                    </a>
                    <div class="vote">
                        <ra:HiddenField 
                            runat="server" 
                            Value='<%#Eval("Id") %>' />
                        <ra:LinkButton 
                            runat="server" 
                            Text="&nbsp;" 
                            OnClick="VoteAnswerUp"
                            CssClass='<%#GetUpCssClassIfVoted((int)Eval("Id")) %>' />
                        <ra:Label 
                            runat="server" 
                            Text='<%#Eval("Score") %>'
                            CssClass="votes" />
                        <ra:LinkButton 
                            runat="server" 
                            Text="&nbsp;"
                            OnClick="VoteAnswerDown"
                            CssClass='<%#GetDownCssClassIfVoted((int)Eval("Id")) %>' />
                    </div>
                    <div class="answerContent">
                        <p>
                            <%#Eval("BodyFormated")%>
                        </p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>


    </ra:Panel>

    <div class="answer">
        <ra:TextArea 
            runat="server" 
            ID="answerBody" />

        <ra:Button 
            runat="server" 
            ID="btnSubmit" 
            OnClick="btnSubmit_Click"
            Text="Answer" />
    </div>

</asp:Content>

