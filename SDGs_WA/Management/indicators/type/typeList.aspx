<%@ Page Title="Type List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="typeList.aspx.cs" Inherits="Management_typeList" %>

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
                            <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <b>Type List</b></div>
                        </div>
                        <div class="large-4 columns">
                            <asp:Literal ID="saveMessage" runat="server" />
                        </div>
                        <div class="large-4 columns">&nbsp;</div>
                    </div>
                    </div>
                    <div class="row">

                        <div class="large-12 columns">
                            <div class="text-right">
                                <a href='AddType.aspx?ed=f' class="icon-plus">Add Indicators Type</a>
                            </div>
                            <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" RowStyle-CssClass="gridview-row" CssClass="user-list" AllowPaging="true"
                                OnPageIndexChanging="OnPageIndexChanging" PageSize="10">
                                <Columns>
                                    <asp:TemplateField HeaderText="#"  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowNumber" runat="server"  />
                                               </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:BoundField DataField="Descr_Short" HeaderText="Short Descr" />
    
                                    <asp:BoundField DataField="Descr_En" HeaderText="Descr EN" />
                                    <asp:BoundField DataField="Descr_Ar" HeaderText="Descr AR" />
                                   <asp:BoundField DataField="Label_En" HeaderText="Label's Goal EN" />
                                    <asp:BoundField DataField="Label_Ar" HeaderText="Label's Goal SL" />
                                    <asp:BoundField DataField="Subindicator_Separator" HeaderText="Separator"   ItemStyle-HorizontalAlign="Center" />

                                    <asp:TemplateField HeaderText="URL IMG">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ToolTip='<%# Eval("url_img") %>' ID="urlImg" Text='<%# Utility.StringCrop(Eval("url_img").ToString(), 20) %>'   ></asp:Label>
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:BoundField DataField="order_code" HeaderText="Order" ItemStyle-HorizontalAlign="Center" />
                                    <asp:TemplateField HeaderText="Goals" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="nGoals" runat="server" />
                                        </ItemTemplate>

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Targets" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="nTargets" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Indicators" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="nIndicators" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubIndicators" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="nSubIndicators" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Edit"  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="linkFamilly"
                                                CommandArgument='<%# Eval("Type_ID") %>'
                                                CssClass="icon-indent-right" ToolTip="Edit">
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status"  ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="statusButton"
                                                CommandArgument='<%# Eval("Type_ID")+";"+ Eval("IS_ACTIVE") %>'
                                                OnClick="setActive"
                                            OnClientClick='<%#Eval("IS_ACTIVE","return ChangeStatusConfirmation(\"{0}\"); ")%>'

                                              >
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </div>

                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </main>
    <script>
        function ChangeStatusConfirmation(status) {
             
            var status_text="DISABLE";
            if (status.toLowerCase()=='false') status_text="ACTIVATE"
            return confirm("Are you sure to " + status_text+" this records?");
        }
    </script>








</asp:Content>

