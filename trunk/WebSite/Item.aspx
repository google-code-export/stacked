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

    <ra:Label runat="server" ID="header" Tag="h2" />
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
    </div>
    <div class="quizContent">
        <ra:Label 
            runat="server" 
            ID="body" 
            Tag="p" />
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
                        <%#Eval("CreatedBy.Username")%>
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
                        <h3><%#Eval("Header") %></h3>
                        <p>
                            <%#Eval("Body") %>
                        </p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>


    </ra:Panel>

    <div class="answer">
        <ra:TextBox 
            runat="server" 
            ID="answerHeader" />

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

