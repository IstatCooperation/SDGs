<%@ Page Title="GOAL List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="header-bottom">
        <div class="row">
            <div class="large-12 columns">
                <nav id="navigation" class="navigation top-bar" data-topbar>
                    <div class="menu-primary-menu-container">
                        <ul id="menu-primary-menu" class="menu">
                             <asp:Literal runat="server" ID="itemsMenu" EnableViewState="false" />
                      
                        </ul>
                    </div>
                </nav>
                <!--/ .navigation-->

            </div>
        </div>
        <!--/ .row-->
    </div>
    <!--/ .header-bottom-->

    <main id="content" class="row">
        <div class="large-12 columns">
            <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                <b> <asp:Literal runat="server" ID="typeDescr" EnableViewState="false" /></b>
            </div>
        </div>
        <div class="large-12 columns">
            <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
        </div>
    </main>
</asp:Content>

