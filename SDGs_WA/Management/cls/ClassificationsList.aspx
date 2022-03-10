<%@ Page Title="Classifications List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ClassificationsList.aspx.cs" Inherits="ClassificationsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div>
                    <a href='<%= ResolveUrl("~/") %>'>Home</a> &gt;  Classifications List
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

      
                <div class="row">
                    <div style="padding: 1px">
                        <asp:Literal ID="saveMessage" runat="server" />
                    </div>

                </div>
                    <div class="row">
                     <div class="text-right">
                              <a href='AddEditCls.aspx?ed=f' class="icon-plus">Add Classifications</a>
                          </div>
                         </div>
                <div class="row">
        
                    <div class="large-12 columns">
               
                        <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="True" OnRowDataBound="OnRowDataBound" CssClass="user-list"
                            OnPageIndexChanging="OnPageIndexChanging" PageSize="40" EmptyDataText="No Records Found!" AllowPaging="true" OnDataBound="OnDataBound"
                            AllowSorting="true" Width="99%">

                            <Columns>
                                <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Edit"   ItemStyle-HorizontalAlign="Center" >
                                      <ItemTemplate>
                                          <asp:LinkButton runat="server" ID="linkEdit"
                                              CommandArgument='<%# Eval("NAME") %>'
                                              CssClass="icon-indent-right" ToolTip="Edit">
                                          </asp:LinkButton>
                                      </ItemTemplate>
                                  </asp:TemplateField>
                                   <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="deleteButton"
                                            CommandArgument='<%# Eval("NAME").ToString().Trim() %>'
                                            OnClick="removeCls"
                                            OnClientClick='<%#Eval("NAME","RemoveConfirmation({0})")%>'
                                            CssClass="icon-trash" ToolTip="Remove" >
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblTotal" runat="server" />
                 
                </div>
  </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
    <script>
        function RemoveConfirmation(timep) {
            return confirm("Are you sure to remove the Time Period: " + timep + " ?");
        }
    </script>
</asp:Content>

