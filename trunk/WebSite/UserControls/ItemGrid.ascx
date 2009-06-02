<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="ItemGrid.ascx.cs" 
    Inherits="UserControls_ItemGrid" %>

<div class="list-item list-header">
    <div class="score header-item">
        Score
    </div>
    <div class="answers-count header-item">
        Answers
    </div>
    <div class="date header-item">
        Date
    </div>
    <div class="operator header-item">
        User
    </div>
    <div class="question header-item">
        Question
    </div>
</div>
<ra:Panel 
    runat="server" 
    ID="wrap" 
    EnableViewState="false">

    <asp:Repeater runat="server" ID="rep">
        <ItemTemplate>

            <div class="list-item item">

                <div class='<%# "score content-item " + GetCssClass((int)Eval("Score")) %>'>
                    <%# Eval("Score") %>
                </div>

                <div class='<%# "answers-count content-item " + GetCssClass((int)Eval("AnswersCount")) %>'>
                    <%# Eval("AnswersCount") %>
                </div>

                <div class="date content-item">
                    <%# GetTime((DateTime)Eval("Created")) %>
                </div>

                <div class="operator content-item">
                    <a href='<%# ((Entities.Operator)Eval("CreatedBy")).Username + ".user" %>'>
                        <%# ((Entities.Operator)Eval("CreatedBy")).FriendlyName%>
                    </a>
                </div>

                <div class="question content-item">
                    <span class="viewCount"><%# Eval("Views") %> views</span>
                    <span class="tags"><%# Eval("TagsFormated") %></span>
                    <a class="headerLink" href='<%# Eval("Url") %>' title='<%# Eval("BodySummary") %>'>
                        <%# Eval("Header") %>
                    </a>
                </div>

            </div>

        </ItemTemplate>
    </asp:Repeater>

</ra:Panel>
