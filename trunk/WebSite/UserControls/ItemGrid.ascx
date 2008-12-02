<%@ Control 
    Language="C#" 
    AutoEventWireup="true" 
    CodeFile="ItemGrid.ascx.cs" 
    Inherits="UserControls_ItemGrid" %>

<%@ Register 
    Assembly="Ra" 
    Namespace="Ra.Widgets" 
    TagPrefix="ra" %>

<%@ Register 
    Assembly="Extensions" 
    Namespace="Ra.Extensions" 
    TagPrefix="ext" %>

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
<ra:Panel runat="server" ID="wrap">
    <asp:Repeater runat="server" ID="rep">
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
                        <%# ((Entities.Operator)Eval("CreatedBy")).FriendlyName%>
                    </a>
                </div>
                <div class="header">
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
