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
        CssClass="window wndDelete">
        <div class="divDelete">
            <ra:Button 
                runat="server" 
                ID="deleteBtn" 
                OnClick="deleteBtn_Click"
                Text="Yes I am sure!" />
        </div>
        <ra:BehaviorObscurable 
            runat="server" 
            ID="deleteObscurer" />
    </ext:Window>


    <div class="answerDiv">
        <ra:Label 
            runat="server" 
            ID="header" 
            Tag="h1" />
        <div class="tagsHeader">
            <asp:Repeater runat="server" ID="repTags">
                <ItemTemplate>
                    <a class="linkButton" href='<%#Eval("Name") + ".tag" %>'><%#Eval("Name") %> (<%#Eval("Item.Count") %>)</a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <ra:Label 
        runat="server" 
        ID="errorLabel" 
        Visible="false"
        CssClass="errPanel" />
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
    <div class="questionStart">
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
        <ra:Label 
            runat="server" 
            CssClass="whenPosted"
            ID="whenPosted" />
        <ra:Label 
            runat="server" 
            ID="body" 
            CssClass="questionContent"
            Tag="div" />
        <a 
            runat="server" 
            id="askedBy" 
            class="linkButton operatorAsked" />
        <ra:LinkButton 
            runat="server" 
            ID="changeOrdering"
            Text="Change ordering [current - determine]"
            OnClick="changeOrdering_Click"
            CssClass="changeOrdering">
            <ra:BehaviorUpdater 
                Delay="200" 
                runat="server" 
                ID="obscureChangeOrder" />
        </ra:LinkButton>
        <ra:Panel 
            runat="server" 
            Visible="false" 
            CssClass="editQuestion"
            ID="editQuestion">
            <ext:InPlaceEdit 
                runat="server" 
                CssClass="editHeader"
                ID="editHeader" />
            <ra:TextArea 
                runat="server" 
                CssClass="editQuestionText"
                ID="editTxt" />
            <ra:Button 
                runat="server" 
                ID="saveEdit" 
                CssClass="saveEditedQuestion"
                OnClick="saveEdit_Click"
                Text="Save" />
        </ra:Panel>
    </div>
    <br style="clear:both;" />
    <ra:Panel 
        runat="server" 
        CssClass="answersWrapper"
        ID="answersWrapper">

        <asp:Repeater runat="server" ID="answers">
            <ItemTemplate>
                <div class="answer">
                    <ra:HiddenField ID="HiddenField1" 
                        runat="server" 
                        Value='<%#Eval("Id") %>' />
                    <a class="linkAnswer" href='<%# "#" + Eval("ID") %>' name='<%#Eval("ID") %>'>
                        link
                    </a>
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
                    <a class="linkButton answeredBy" href='<%#Eval("CreatedBy.Username") + ".user" %>'>
                        <%#Eval("CreatedBy.FriendlyName")%> - <%#Eval("CreatedBy.CalculateCreds")%> creds
                    </a>
                    <div class="answerDate">
                        <%#GetTime((DateTime)Eval("Created")) %>
                    </div>
                    <ra:Panel runat="server" CssClass="vote voteAnswer">
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
                    <ra:LinkButton 
                        runat="server" 
                        CssClass="comments" 
                        OnClick="ViewComments"
                        Text='<%# "Comments [" + Eval("Children.Count") + "]"%>'>
                        <ra:BehaviorUpdater 
                            Delay="200" 
                            runat="server" 
                            ID="obscureChangeOrder" />
                    </ra:LinkButton>
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
                    <ra:Panel 
                        runat="server" 
                        Visible="false"
                        CssClass="viewComments">
                        <asp:Repeater runat="server">
                            <ItemTemplate>
                                <div class="oneComment">
                                    <%#Eval("BodyFormated") %>
                                    <div class="commentedByWho">
                                        <a
                                            href='<%#Eval("CreatedBy.Username") + ".user" %>'>
                                            <%#Eval("CreatedBy.FriendlyName") %>
                                        </a>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="commentSubmit">
                            <ra:TextArea 
                                runat="server" 
                                CssClass="commentTxt" />
                            <ra:Button 
                                runat="server" 
                                Text="Save" 
                                CssClass="submitComment"
                                OnClick="SaveComment" />
                        </div>
                    </ra:Panel>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </ra:Panel>

    <ra:Panel 
        runat="server" 
        ID="answerQuestion" 
        CssClass="answerQuestion">
        <ra:TextArea 
            runat="server" 
            CssClass="answerBody"
            ID="answerBody" />

        <ra:Button 
            runat="server" 
            ID="btnSubmit" 
            OnClick="btnSubmit_Click"
            CssClass="answerQuestionBtn"
            Text="Answer" />
    </ra:Panel>

</asp:Content>

