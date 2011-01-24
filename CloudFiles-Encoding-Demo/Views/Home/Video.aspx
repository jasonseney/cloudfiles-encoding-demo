<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<string>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">Video</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Viewing: <%= Model.Substring(Model.LastIndexOf('/') + 1) %></h2>
    
    <video controls="controls">
        <source src="<%= Model %>" />
    </video>
    
</asp:Content>