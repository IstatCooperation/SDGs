<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="changePassword.aspx.cs" Inherits="Users_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href="/index.aspx">Goals</a> <span id="linkUsers" runat="server">&gt; <a href="UsersList.aspx">Users List</a></span> &gt; Change Password</div>
            </div>
          
                <div class="row">
                <div class="large-12 columns">
                 
                    <div style="text-align: center;">

                        <div style="width: 45%; margin: 0 auto; text-align: left;">
                            <table style="width: 99%;">

                                <tr>
                                    <td colspan="2">
                                       <asp:Literal ID="saveMessage" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Username:</td>
                                    <td style="padding: 5px"><i>
                                       <asp:Label ID="txtUser" runat="server"   ></asp:Label>
                                        <asp:HiddenField ID="txtUserId"  runat="server"></asp:HiddenField>
                                     </i>
                                    <td > </td>
                                </tr>
                                <tr>
                                    <td>Password:</td>
                                    <td style="padding: 5px">
                                        <input id="txtUserPass" type="password" runat="server"></td>
                                    <td class="text-left">
                                          <asp:RequiredFieldValidator ControlToValidate="txtUserPass"
                                            Display="Static" ErrorMessage="*" runat="server"
                                            ID="vUserPass" />

                                        <asp:CompareValidator  ControlToValidate="txtUserPass" ControlToCompare="txtUserPass1"
                                            Display="Static" Text="Password Mismatch" runat="server"
                                            ID="Comp1" />

                                      
                                    </td>
                                </tr>
                                <tr>
                                    <td>Repeat Password:</td>
                                    <td style="padding: 5px">
                                        <input id="txtUserPass1" type="password" runat="server"></td>
                                    <td class="text-left">
                                       
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td colspan="2" class="text-right" style="padding:3px;">
                                        <p></p>
                                           <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                          <asp:Button  Text="Save" runat="server" id="cmdLogin"  OnClick="ChangePassword"  />
                                    </td>
                                    <td></td>
                                    
                                </tr>

                            </table>

                        </div>
                    </div>

                </div>
            </div>

        </div>
    </main>
</asp:Content>

