<%@ Page 
    ValidateRequest="false"
    Language="C#" 
    MasterPageFile="~/MasterPage.master" 
    AutoEventWireup="true" 
    CodeFile="Default.aspx.cs" 
    Inherits="_Default" 
    Title="Stacked - Questions and Answer done right!" %>

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

    <ext:TabControl 
        runat="server" 
        ID="tabContent" 
        OnActiveTabViewChanged="tabContent_ActiveTabViewChanged"
        CssClass="tab">

        <ext:TabView 
            Caption="Fresh" 
            runat="server" 
            ID="newQuiz" 
            CssClass="content">

            <ra:Panel runat="server" ID="newQuestionsPanel">
                <div class="question">
                    <div class="count top">
                        Score
                    </div>
                    <div class="answers-count top">
                        Answers
                    </div>
                    <div class="date top">
                        Date
                    </div>
                    <div class="operatorAsked top">
                        User
                    </div>
                    <div class="header top">
                        Question
                    </div>
                </div>
                <asp:Repeater runat="server" ID="newRep">
                    <ItemTemplate>
                        <div class="question">
                            <div class='<%# "count " + GetCssClass((int)Eval("Score")) %>'>
                                <%# Eval("Score") %>
                            </div>
                            <div class='<%# "answers-count " + GetCssClass((int)Eval("AnswersCount")) %>'>
                                <%# Eval("AnswersCount") %>
                            </div>
                            <div class="date">
                                <%# GetTime((DateTime)Eval("Created")) %>
                            </div>
                            <div class="operatorAsked">
                                <a href='<%# ((Entities.Operator)Eval("CreatedBy")).Username + ".user" %>'>
                                    <%# ((Entities.Operator)Eval("CreatedBy")).Username%>
                                </a>
                            </div>
                            <div class="header">
                                <span class="viewCount"><%# Eval("Views") %> views</span>
                                <a href='<%# Eval("Url") %>'>
                                    <%# Eval("Header") %>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ra:Panel>

        </ext:TabView>

        <ext:TabView 
            Caption="Most answers" 
            runat="server" 
            ID="topQuestions" 
            CssClass="content">
            
            <ra:Panel runat="server" ID="topQuestionsPanel">
                <div class="question">
                    <div class="count top">
                        Score
                    </div>
                    <div class="answers-count top">
                        Answers
                    </div>
                    <div class="date top">
                        Date
                    </div>
                    <div class="operatorAsked top">
                        User
                    </div>
                    <div class="header top">
                        Question
                    </div>
                </div>
                <asp:Repeater runat="server" ID="repTopQuestions">
                    <ItemTemplate>
                        <div class="question">
                            <div class='<%# "count " + GetCssClass((int)Eval("Score")) %>'>
                                <%# Eval("Score") %>
                            </div>
                            <div class='<%# "answers-count " + GetCssClass((int)Eval("AnswersCount")) %>'>
                                <%# Eval("AnswersCount") %>
                            </div>
                            <div class="date">
                                <%# GetTime((DateTime)Eval("Created")) %>
                            </div>
                            <div class="operatorAsked">
                                <a href='<%# ((Entities.Operator)Eval("CreatedBy")).Username + ".user" %>'>
                                    <%# ((Entities.Operator)Eval("CreatedBy")).Username%>
                                </a>
                            </div>
                            <div class="header">
                                <span class="viewCount"><%# Eval("Views") %> views</span>
                                <a href='<%# Eval("Url") %>'>
                                    <%# Eval("Header") %>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ra:Panel>

        </ext:TabView>

        <ext:TabView 
            Caption="Unanswered" 
            runat="server" 
            ID="unanswered" 
            CssClass="content">


            <ra:Panel runat="server" ID="unansweredQuestionsPanel">
                <div class="question">
                    <div class="count top">
                        Score
                    </div>
                    <div class="answers-count top">
                        Answers
                    </div>
                    <div class="date top">
                        Date
                    </div>
                    <div class="operatorAsked top">
                        User
                    </div>
                    <div class="header top">
                        Question
                    </div>
                </div>
                <asp:Repeater runat="server" ID="repUnansweredQuestions">
                    <ItemTemplate>
                        <div class="question">
                            <div class='<%# "count " + GetCssClass((int)Eval("Score")) %>'>
                                <%# Eval("Score") %>
                            </div>
                            <div class='<%# "answers-count " + GetCssClass((int)Eval("AnswersCount")) %>'>
                                <%# Eval("AnswersCount") %>
                            </div>
                            <div class="date">
                                <%# GetTime((DateTime)Eval("Created")) %>
                            </div>
                            <div class="operatorAsked">
                                <a href='<%# ((Entities.Operator)Eval("CreatedBy")).Username + ".user" %>'>
                                    <%# ((Entities.Operator)Eval("CreatedBy")).Username%>
                                </a>
                            </div>
                            <div class="header">
                                <span class="viewCount"><%# Eval("Views") %> views</span>
                                <a href='<%# Eval("Url") %>'>
                                    <%# Eval("Header") %>
                                </a>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ra:Panel>

        </ext:TabView>

    </ext:TabControl>
    <asp:Label 
        runat="server" 
        ID="lblCount" 
        style="margin-left:25px;font-size:1.1em;font-weight:bold;margin-top:25px;margin-bottom:25px;display:block;"
        Text="Number of users in Stacked; " />

</asp:Content>

