<%@ Page 
    ValidateRequest="false"
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true"
    CodeFile="Item.aspx.cs" 
    Inherits="Item" 
    Title="Untitled Page" %>

<asp:Content 
    ID="cnt" 
    ContentPlaceHolderID="body" 
    Runat="Server">

    <ra:Window 
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
    </ra:Window>


    <div class="answerDiv">
        <ra:Label 
            runat="server" 
            ID="header" 
            CssClass="itemHeader"
            Tag="h1" />
        <div class="tagsHeader">
            <asp:Repeater 
                runat="server" 
                ID="repTags" 
                EnableViewState="false">
                <ItemTemplate>
                    <a class="linkButton tagButton" href='<%#Eval("Name") + ".tag" %>'><%#Eval("Name") %> (<%#Eval("NumberOfOccurencies")%>)</a>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <ra:Label 
        runat="server" 
        ID="errorLabel" 
        Visible="false"
        CssClass="errPanel" />
    <ra:Timer 
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
        <div style="width:100%;">
            <ra:LinkButton 
                runat="server" 
                ID="quoteQuestion"
                OnClick="quoteQuestion_Click"
                CssClass="quoteQuestionBtn"
                Visible="false" 
                Text="Quote" />
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
                Text="Delete" />
            <ra:Label 
                runat="server" 
                CssClass="whenPosted"
                ID="whenPosted" />
            <a runat="server" id="askedBy" class="linkButton askedBy">
                <img id="imgGravatar" alt="Pic" runat="server" style="float:left;margin-right:5px;" width="32" height="32" />
                <ra:Label runat="server" id="askedByLabel" style="float:left;margin-right:5px;" Tag="div" />
            </a>
        </div>
        <ra:Label 
            runat="server" 
            ID="body" 
            CssClass="questionContent"
            Tag="div" />
        
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
            <div class="editQuestionDiv">
                <ra:InPlaceEdit 
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
                <ra:Button 
                    runat="server" 
                    ID="cancelEdit" 
                    CssClass="cancelEditedQuestion"
                    OnClick="cancelEdit_Click"
                    Text="Cancel" />
            </div>
        </ra:Panel>
    </div>
    <br style="clear:both;" />
    <ra:Panel 
        runat="server" 
        CssClass="answersWrapper"
        ID="answersWrapper">

        <asp:Repeater 
            runat="server" 
            ID="answers">
            <ItemTemplate>
                <div class="answer">
                    <ra:HiddenField 
                        runat="server" 
                        Value='<%#Eval("Id") %>' />
                    <div style="width:100%;">
                        <a class="linkAnswer" href='<%# "#" + Eval("Id") %>' name='<%#Eval("Id") %>'>Link</a>
                        <ra:LinkButton 
                            runat="server" 
                            CssClass="editAnswerBtn" 
                            Visible='<%# (bool)(Entities.Operator.Current != null && (Entities.Operator.Current.ID == (int)(Eval("CreatedBy.ID")) || Entities.Operator.Current.CanEditAnswer)) %>'
                            OnClick="EditAnswerBtnClick"
                            Text="Edit" />
                        <ra:LinkButton 
                            runat="server" 
                            OnClick="DeleteQuestionBtnClick"
                            CssClass="deleteAnswerBtn"
                            Visible='<%# (bool)(Entities.Operator.Current != null && (Entities.Operator.Current.ID == (int)(Eval("CreatedBy.ID")) || Entities.Operator.Current.CanEditAnswer)) %>'
                            Text="Delete" />
                        <div class="answerDate">
                            <%#GetTime((DateTime)Eval("Created")) %>
                        </div>
                        <a class="linkButton answeredBy" href='<%# Eval("CreatedBy.Username") + ".user" %>'>
                            <img id="Img1" alt="Pic" runat="server" style="float:left;margin-right:5px;" width="32" height="32" src='<%# Eval("CreatedBy.Gravatar") %>' />
                            <div style="float:left;margin-right:5px;">
                                <%#Eval("CreatedBy.FriendlyName")%> <br />
                                <%#Eval("CreatedBy.CalculateCreds")%> creds
                            </div>
                        </a>
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
                    <ra:Panel 
                        runat="server" 
                        Visible="false" 
                        CssClass="editAnswer">
                        <div class="editAnswerDiv">
                            <ra:TextArea 
                                CssClass="editAnswerTxt"
                                runat="server" />
                            <ra:Button 
                                runat="server" 
                                OnClick="SaveAnswer"
                                CssClass="saveEditedAnswer"
                                Text="Save">
                                <ra:BehaviorUpdater runat="server" Delay="200" />
                            </ra:Button>
                            <ra:Button 
                                ID="cancelEditAnswer" 
                                runat="server" 
                                OnClick="cancelEditAnswer_Click"
                                CssClass="cancelEditedAnswer"
                                Text="Cancel" />
                        </div>
                    </ra:Panel>
                    <ra:LinkButton ID="LinkButton1" 
                        runat="server" 
                        CssClass="comments" 
                        OnClick="ViewComments"
                        Text='<%# "Comments [" + Eval("ChildrenCount") + "]"%>'>
                        <ra:BehaviorUpdater 
                            Delay="400" 
                            runat="server" 
                            ID="obscureChangeOrder" />
                    </ra:LinkButton>
                    <ra:Panel 
                        runat="server" 
                        Visible="false"
                        CssClass="viewComments">
                        <asp:Repeater 
                            runat="server">
                            <ItemTemplate>
                                <div class="oneComment">
                                    <%# Eval("BodyFormated") %> -- 
                                    <a href='<%# Eval("CreatedBy.Username") + ".user" %>'>
                                        <span><%# Eval("CreatedBy.FriendlyName") %></span>
                                    </a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="commentSubmit">
                            <ra:TextArea 
                                runat="server" 
                                CssClass="commentTxt" />
                            <ra:Button 
                                runat="server" 
                                Text="Submit Comment" 
                                CssClass="submitComment"
                                OnClick="SaveComment">
                                <ra:BehaviorUpdater runat="server" Delay="200" />
                            </ra:Button>
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
            Text="Submit Answer">
            <ra:BehaviorUpdater 
                runat="server" 
                ID="obscurerAnswerQuestion" 
                Delay="200" />
        </ra:Button>
    </ra:Panel>

</asp:Content>

