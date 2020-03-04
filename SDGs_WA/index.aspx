<%@ Page Title="GOAL List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <main id="content" class="row">
        <div class="large-12 columns">
            <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                <b>Sustainable Development Goals</b>
            </div>
        </div>
        <div class="large-12 columns">
            <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
        </div>
    </main>
</asp:Content>

