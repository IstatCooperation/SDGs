<%@ Page Title="Depatement List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DepList.aspx.cs" Inherits="deps_depList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <main id="content" class="row">

        <!-- the ScriptManager and UpdatePanel tags are mandatory to avoid page reload on every checkbox click -->
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="row">
                    <div class="large-4 columns">
                        <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <b>Departments List</b></div>
                    </div>
                    <div class="large-4 columns">
                        <asp:Literal ID="saveMessage" runat="server" />
                    </div>
                    <div class="large-4 columns">&nbsp;</div>
                </div>
                </div>
                  <div class="row">
                       <div class="large-4 columns">&nbsp;</div>
                      <div class="large-8 columns">

                          <div class="text-right">
                              <a href='AddEditDep.aspx?ed=f' class="icon-plus">Add Departement</a>
                          </div>
                          <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="false" EmptyDataText="No Records Found!" OnRowDataBound="OnRowDataBound" CssClass="user-list" AllowPaging="true"
                              OnDataBound="OnDataBound" OnPageIndexChanging="OnPageIndexChanging" PageSize="20">
                              <Columns>
                                  <asp:TemplateField HeaderText="#"   ItemStyle-HorizontalAlign="Center">
                                      <ItemTemplate>
                                          <asp:Label ID="lblRowNumber" runat="server" />
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:BoundField DataField="dep_id" HeaderText="ID"   ItemStyle-HorizontalAlign="Center" />

                                  <asp:BoundField DataField="description" HeaderText="Description" />
                                  <asp:TemplateField HeaderText="Indicators"   ItemStyle-HorizontalAlign="Center">
                                      <ItemTemplate>
                                          <asp:HyperLink ID="nIndicators" runat="server"  ToolTip="Edit enabled indicators "/>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Edit"   ItemStyle-HorizontalAlign="Center" >
                                      <ItemTemplate>
                                          <asp:LinkButton runat="server" ID="linkEdit"
                                              CommandArgument='<%# Eval("dep_id") %>'
                                              CssClass="icon-indent-right" ToolTip="Edit">
                                          </asp:LinkButton>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Delete"   ItemStyle-HorizontalAlign="Center">
                                      <ItemTemplate>
                                          <asp:LinkButton runat="server" ID="deleteUserButton"
                                              CommandArgument='<%# Eval("dep_id") %>'
                                              OnClick="deleteUser_ServerClick"
                                              OnClientClick="if (!UserDeleteConfirmation()) return false;"
                                              CssClass="icon-trash" ToolTip="Delete">
                                          </asp:LinkButton>
                                      </ItemTemplate>
                                  </asp:TemplateField>

                              </Columns>
                          </asp:GridView>
                          <br />
                          <asp:Label ID="lblTotal" runat="server" />
                      </div>
                  </div>
                </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
    <script>
        function UserDeleteConfirmation() {
            return confirm("Are you sure to remove this indicator?");
        }
    </script>








</asp:Content>

