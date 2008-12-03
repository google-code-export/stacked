<%@ Page 
    ValidateRequest="false"
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


    <div style="position:relative;">
        <ra:Label 
            runat="server" 
            ID="header" 
            Tag="h2" />
        <ra:Label 
            runat="server" 
            CssClass="whenPosted"
            ID="whenPosted" />
        <div class="tagsHeader">
            <asp:Repeater runat="server" ID="repTags">
                <ItemTemplate>
                    <a href='<%#Eval("Name") + ".tag" %>'><%#Eval("Name") %> (<%#Eval("Item.Count") %>)</a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
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
        <ra:Label 
            runat="server" 
            CssClass="numberOfFavorites"
            ID="numberOfFavorites" />
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
    </div>
    <div class="quizContent">
        <ra:Label 
            runat="server" 
            ID="body" 
            Tag="div" />
        <a 
            runat="server" 
            id="askedBy" 
            class="operator operatorAskedQuestion" />
        <ra:LinkButton 
            runat="server" 
            ID="quoteQuestion"
            OnClick="quoteQuestion_Click"
            CssClass="quoteQuestionBtn"
            Visible="false" 
            Text="Quote..." />
        <ra:LinkButton 
            runat="server" 
            ID="editQuestionBtn" 
            CssClass="editQuestionBtn" 
            Visible="false" 
            OnClick="editQuestionBtn_Click"
            Text="Edit" />
        <ra:LinkButton 
            runat="server" 
            ID="deleteQuestion"
            OnClick="deleteQuestion_Click"
            CssClass="deleteQuestionBtn"
            Visible="false" 
            Text="Delete..." />
        <ra:LinkButton 
            runat="server" 
            ID="changeOrdering"
            Text="Change ordering [current - determine]"
            OnClick="changeOrdering_Click"
            CssClass="changeOrdering" />
        <ra:Panel 
            runat="server" 
            Visible="false" 
            CssClass="editQuestion"
            ID="editQuestion">
            <ext:InPlaceEdit 
                runat="server" 
                ID="editHeader" />
            <ra:TextArea 
                runat="server" 
                ID="editTxt" />
            <ra:Button 
                runat="server" 
                ID="saveEdit" 
                OnClick="saveEdit_Click"
                Text="Save" />
        </ra:Panel>
    </div>
    <br style="clear:both;" />
    <ra:Panel 
        runat="server" 
        ID="answersWrapper">

        <asp:Repeater runat="server" ID="answers">
            <ItemTemplate>
                <div class="answer">
                    <ra:LinkButton 
                        runat="server" 
                        CssClass="editAnswerBtn" 
                        Visible='<%# (bool)(Entities.Operator.Current != null && Entities.Operator.Current.CanEditAnswer) %>'
                        OnClick="EditQuestionBtnClick"
                        Text="Edit" />
                    <ra:LinkButton 
                        runat="server" 
                        OnClick="DeleteQuestionBtnClick"
                        CssClass="deleteAnswerBtn"
                        Visible='<%# (bool)(Entities.Operator.Current != null && Entities.Operator.Current.CanDeleteAnswer) %>'
                        Text="Delete..." />
                    <a class="operator" href='<%#Eval("CreatedBy.Username") + ".user" %>'>
                        <%#Eval("CreatedBy.FriendlyName")%> - <%#Eval("CreatedBy.CalculateCreds")%> creds
                    </a>
                    <a class="linkAnswer" href='<%# "#" + Eval("ID") %>' name='<%#Eval("ID") %>'>
                        link
                    </a>
                    <div class="answerDate">
                        <%#GetTime((DateTime)Eval("Created")) %>
                    </div>
                    <ra:Panel runat="server" CssClass="vote">
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
                    </ra:Panel>
                    <div class="answerContent">
                        <%#Eval("BodyFormated")%>
                    </div>
                    <ra:Panel 
                        runat="server" 
                        Visible="false" 
                        CssClass="editAnswer">
                        <ra:TextArea 
                            runat="server" />
                        <ra:Button 
                            runat="server" 
                            OnClick="SaveAnswer"
                            Text="Save" />
                    </ra:Panel>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </ra:Panel>

    <ra:Panel 
        runat="server" 
        ID="answerQuestion" 
        CssClass="answer">
        <ra:TextArea 
            runat="server" 
            ID="answerBody" />

        <ra:Button 
            runat="server" 
            ID="btnSubmit" 
            OnClick="btnSubmit_Click"
            Text="Answer" />
    </ra:Panel>

</asp:Content>

