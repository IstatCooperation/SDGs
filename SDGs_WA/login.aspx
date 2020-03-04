<%@ Page Title="Login" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-4 columns">&nbsp;</div>
            <div class="large-4 columns">
                <div style="text-align: center;">
                    <table style="width: 99%;">
                        <tr>
                            <td colspan="2">
                                <h3>Login Page
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="ErrorLabel" runat="Server" ForeColor="Red"
                                    Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>Username:</td>
                            <td style="padding: 5px">
                                <input id="txtUserName" type="text" runat="server"></td>
                            <td>
                                <asp:RequiredFieldValidator ControlToValidate="txtUserName"
                                    Display="Static" ErrorMessage="*" runat="server"
                                    ID="vUserName" /></td>
                        </tr>
                        <tr>
                            <td>Password:</td>
                            <td style="padding: 5px">
                                <input id="txtUserPass" type="password" runat="server"></td>
                            <td>
                                <asp:RequiredFieldValidator ControlToValidate="txtUserPass"
                                    Display="Static" ErrorMessage="*" runat="server"
                                    ID="vUserPass" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="text-right">
                                <p></p>
                                <input type="submit" value="Logon" runat="server" id="cmdLogin">
                            </td>
                        </tr>

                    </table>
                </div>

            </div>
            <div class="large-4 columns">&nbsp;</div>
        </div>
    </main>
</asp:Content>
