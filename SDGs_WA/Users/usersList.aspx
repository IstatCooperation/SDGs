<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="UsersList.aspx.cs" Inherits="userList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <main id="content" class="row">
        <asp:Panel ID="pnlAssignRoles" runat="server" Visible="False">
            <!-- the ScriptManager and UpdatePanel tags are mandatory to avoid page reload on every checkbox click -->
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div class="row">
                        <div class="large-4 columns">
                            <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; Users List</div>
                        </div>
                        <div class="large-4 columns">
                            <asp:Literal ID="saveMessage" runat="server" />
                        </div>
                        <div class="large-4 columns">&nbsp;</div>
                    </div>
                    </div>
                    <div class="row">
                        <div class="large-2 columns">&nbsp;</div>
                        <div class="large-8 columns">
                            <div>
                                <a href='AddUser.aspx' class="icon-user-add">Add user</a>
                            </div>
                            <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" RowStyle-CssClass="gridview-row" CssClass="user-list">
                                <Columns>
                                    <asp:BoundField DataField="Username" HeaderText="Username" />
                                    
                                    <asp:TemplateField HeaderText="Role">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlRoles" runat="server" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnUpdate" Text="Update Role" runat="server" CommandArgument='<%# Eval("User_ID") %>'
                                                OnClick="UpdateRole" CssClass="icon-floppy"
                                                OnClientClick="window.scrollTo(0,0);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="deleteUserButton"
                                                CommandArgument='<%# Eval("User_ID") %>'
                                                OnClick="deleteUser_ServerClick"
                                                OnClientClick="if (!UserDeleteConfirmation()) return false;"
                                                CssClass="icon-trash" Text="Delete User">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <a class="icon-vcard" href="changePassword.aspx?uID=<%# Eval("User_ID") %>" title="Change Password">&nbsp;Change Password</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="linkIndicators"
                                                  CommandArgument='<%# Eval("User_ID") %>'
                                                CssClass="icon-indent-right" Text="Edit Indicators Access">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="large-2 columns"></div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </main>
    <script>
        function UserDeleteConfirmation() {
            return confirm("Are you sure you want to delete this user?");
        }
    </script>
</asp:Content>

