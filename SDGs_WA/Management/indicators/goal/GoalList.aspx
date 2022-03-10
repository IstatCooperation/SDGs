<%@ Page Title="Type List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GoalList.aspx.cs" Inherits="Management_indicators_goal_GoalList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <main id="content" class="row">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <contenttemplate>
            <div class="row">
                <div class="large-4 columns">
                    <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a title="Types" id="backLinkGT" runat="server"  ></a>&gt;<b> Goal List</b></div>
                </div>
                <div class="large-4 columns">
                    <asp:Literal ID="saveMessage" runat="server" />
                </div>
                <div class="large-4 columns">&nbsp;</div>
            </div>

            <div class="row">

                <div class="large-12 columns">
                    <div class="text-right">
                        <a id="linkAddEdit" runat="server" class="icon-plus" title="Add new Goal">Add Goal</a>
                    </div>
                    <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" RowStyle-CssClass="gridview-row" CssClass="user-list"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="20" EmptyDataText="No Records Found!" AllowPaging="true"
                        AllowSorting="true">

                        <Columns>
                            <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Goal_Code" HeaderText="Code" ItemStyle-HorizontalAlign="Center" />

                            <asp:BoundField DataField="Goal_DescEn" HeaderText="Descr En" />
                            <asp:BoundField DataField="Goal_DescAr" HeaderText="Descr SL" />
                            <asp:BoundField DataField="GoalImageEn" HeaderText="Path Image En" />
                            <asp:BoundField DataField="GoalImageAr" HeaderText="Path Imag SL" />

                            <asp:TemplateField HeaderText="Targets" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HyperLink ID="nTargets" runat="server" />
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
                            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="linkEdit"
                                        CommandArgument='<%# Eval("Goal_ID") %>'
                                        CssClass="icon-indent-right" ToolTip="Edit">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="statusButton"
                                        CommandArgument='<%# Eval("Goal_ID")+";"+ Eval("IS_ACTIVE") %>'
                                        OnClick="setActive"
                                        OnClientClick='<%#Eval("IS_ACTIVE","return ChangeStatusConfirmation(\"{0}\"); ")%>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>

            </div>

        </contenttemplate>


    </main>
    <script>
        function ChangeStatusConfirmation(status) {

            var status_text = "DISABLE";
            if (status.toLowerCase() == 'false') status_text = "ACTIVATE"
            return confirm("Are you sure to " + status_text + " this record?");
        }
    </script>









</asp:Content>


