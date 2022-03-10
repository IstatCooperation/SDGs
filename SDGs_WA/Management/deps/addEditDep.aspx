<%@ Page Title="Add Indicators Type" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="addEditDep.aspx.cs" Inherits="AdEditDep" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a href="DepList.aspx">Departments List</a> &gt; <b><span id="pageTitle" runat="server"></span></b></div>
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
                            <% if (this.isEditAction())
                                {%>

                            <tr>
                                <td class="text-right">ID*
                                    <asp:LinkButton runat="server" ToolTip="Departement ID (Integer)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">

                                    <b>
                                        <asp:Label ID="Label_ID" runat="server" /></b>
                                </td>
                                <td></td>


                            </tr>
                            <%} %>
                            <tr>
                                <td class="text-right">Description*
                                    <asp:LinkButton runat="server" ToolTip="Departement description (max 100)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="dep_description" type="text" runat="server" maxlength="100"></td>
                                <td class="text-left">
                                    <asp:RequiredFieldValidator ControlToValidate="dep_description"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vDescription" /></td>
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

