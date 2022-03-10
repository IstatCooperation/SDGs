<%@ Page Title="Add Indicators Type" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="addType.aspx.cs" Inherits="AddType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a href="typeList.aspx">Type List</a> &gt; <b><span  ID="pageTitle" runat="server"></span></b></div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <div class="row">

                    <asp:Literal ID="saveMessage" runat="server" />
                    <div class="large-1  columns"></div>
                    <div class="large-11 columns">

                        <table style="width: 99%;">
          
                            <tr>
                                <td class="text-right">Descr En*
                                    <asp:LinkButton runat="server" ToolTip="Long title in english language, displayed as title in home page"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="Descr_En" type="text" runat="server" class="text-left"></td>
                                <td class="text-left">

                                    <asp:RequiredFieldValidator ControlToValidate="Descr_En"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vDescr_En" /></td>
                            </tr>
                            <tr>
                                <td class="text-right">Descr SL
                                    <asp:LinkButton runat="server" ToolTip="Long title in secondary language, displayed as title in home page"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Descr_Ar" type="text" runat="server"></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td class="text-right">Descr Short*
                                    <asp:LinkButton runat="server" ToolTip="Short title, displayed as menu link in home page (max 20)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Descr_Short" type="text" runat="server"  maxlength="20"></td>
                                <td class="text-left">
                                    <asp:RequiredFieldValidator ControlToValidate="Descr_Short"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vDescr_Short" /></td>
                            </tr>
                             <tr>
                                <td class="text-right" width="20%">Label En*
                                    <asp:LinkButton runat="server" ToolTip="Label's goal in english language (max 50, es. Goal, Subject...)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Label_En" type="text" runat="server" maxlength="50" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="Label_En"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vLabel_En" /></td>

                            </tr>
                            <tr>
                                <td class="text-right">Label SL
                                    <asp:LinkButton runat="server" ToolTip="Label's goal in secondary language (max 50, es. Goal, Subject...)" 
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="Label_Ar" type="text" runat="server"  maxlength="50"></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td class="text-right">Subindicator Separator
                                    <asp:LinkButton runat="server" ToolTip="Character to separate the subindicator code, es _ (max 1)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="Subindicator_Separator" type="text" runat="server"  maxlength="1"></td>
                                <td class="text-left"></td>
                            </tr>

                            <tr>
                                <td class="text-right">Path img
                                    <asp:LinkButton runat="server" ToolTip="Path or url for the image displayed in home page"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="url_img" type="text" runat="server"></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td class="text-right">Order code
                                    <asp:LinkButton runat="server" ToolTip="Order to displayed in home page menu"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="order_code" type="number" runat="server" ></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td colspan="2" class="text-right" style="padding: 3px;">
                                    <p class="small-text-right">*Required fields</p>
                                    <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                    <asp:Button Text="Save" runat="server" ID="cmdLogin" OnClick="saveType" />
                                </td>
                                <td></td>
                            </tr>
                        </table>

                    </div>

                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
</asp:Content>

