<%@ Page Title="Add User" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="addUser.aspx.cs" Inherits="Users_AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href="/index.aspx">Goals</a> &gt; <a href="UsersList.aspx">Users List</a> &gt; Add User</div>
            </div>
        </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="large-2 columns">&nbsp;</div>
                        <div class="large-8 columns">
                            <div style="text-align: center;">
                                <table style="width: 99%;">

                                    <tr>
                                        <td colspan="2">
                                            <asp:Literal ID="saveMessage" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Username:</td>
                                        <td style="padding: 5px">
                                            <input id="txtUser" type="text" runat="server"></td>
                                        <td class="text-left">
                                            <asp:RequiredFieldValidator ControlToValidate="txtUser"
                                                Display="Dynamic" ErrorMessage="*" runat="server"
                                                ID="vUserName" /></td>
                                    </tr>
                                    <tr>
                                        <td>Password:</td>
                                        <td style="padding: 5px">
                                            <input id="txtUserPass" type="password" runat="server"></td>
                                        <td class="text-left">
                                            <asp:RequiredFieldValidator ControlToValidate="txtUserPass"
                                                Display="Static" ErrorMessage="*" runat="server"
                                                ID="vUserPass" />

                                            <asp:CompareValidator ControlToValidate="txtUserPass" ControlToCompare="txtUserPass1"
                                                Display="Static" Text="Password Mismatch" runat="server"
                                                ID="Comp1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Repeat Password:</td>
                                        <td style="padding: 5px">
                                            <input id="txtUserPass1" type="password" runat="server"></td>
                                        <td class="text-left"></td>
                                    </tr>
                                    <tr>
                                        <td>Role:</td>
                                        <td style="padding: 5px">
                                            <!--
                                            asp:dropdown issue: if you don't set AutoPostBack="true" you
                                            can't get the real selected value in the code behind, but this
                                            property lets the page to reload itself everytime the dropdown 
                                            selection is changed; unfortunately this reloading delete the
                                            values of the password type fields. A workaround is to use the 
                                            simple html select tag.
                                        -->
                                            <select id="ddlRoles" runat="server">
                                            </select>
                                        </td>
                                        <td class="text-left"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" class="text-right" style="padding: 3px;">
                                            <p></p>
                                            <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                            <asp:Button Text="Save" runat="server" ID="cmdLogin" OnClick="AddUser" />
                                        </td>
                                        <td></td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <div class="large-2 columns">&nbsp;</div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </main>
</asp:Content>

