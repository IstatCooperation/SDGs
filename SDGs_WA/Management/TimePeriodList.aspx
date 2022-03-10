<%@ Page Title="Time Period" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TimePeriodList.aspx.cs" Inherits="TimePeriodList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div>
                    <a href='<%= ResolveUrl("~/") %>'>Home</a> &gt;  Time Period List
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

                <div class="row">

                    <details>
                        <summary class="text-right"><a><b>Add Time Period</b></a></summary>

                        <table>
                            <tr>
                                <td style="padding: 3px; vertical-align: bottom;">New Time Period*
                                    <asp:LinkButton runat="server" ToolTip="Insert new Time Period (max 4)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:
                                </td>
                                <td style="padding: 3px; vertical-align: bottom;">
                                    <asp:RequiredFieldValidator ControlToValidate="time_period"
                                        Display="Dynamic" ErrorMessage="Required" runat="server"
                                        ID="vtime_period" />
                                    <asp:RangeValidator ID="RangeValidatorYear" runat="server"
                                        ControlToValidate="time_period" ErrorMessage="RangeValidator"
                                        Type="Integer" />
                                    <input id="time_period" type="number" runat="server" maxlength="4">
                                </td>

                                <td style="padding: 3px; vertical-align: bottom;">
                                    <asp:Button Text="Add" runat="server" ID="cmdSave" OnClick="addTimePeriod" />
                                </td>
                            </tr>
                        </table>
                    </details>
                </div>
                <div class="row">
                    <div style="padding: 1px">
                        <asp:Literal ID="saveMessage" runat="server" />
                    </div>

                </div>

                <div class="row">
                    <div class="large-4  columns">&nbsp;</div>
                    <div class="large-4  columns">
                        <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound" CssClass="user-list"
                            OnPageIndexChanging="OnPageIndexChanging" PageSize="40" EmptyDataText="No Records Found!" AllowPaging="true" OnDataBound="OnDataBound"
                            AllowSorting="true" Width="50%">

                            <Columns>
                                <asp:TemplateField HeaderText="#" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRowNumber" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="TIME_PERIOD" HeaderText="Time Period" ItemStyle-HorizontalAlign="Center" />


                                <asp:TemplateField HeaderText="Remove" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="deleteButton"
                                            CommandArgument='<%# Eval("TIME_PERIOD").ToString().Trim() %>'
                                            OnClick="removeTimePeriod"
                                            OnClientClick='<%#Eval("TIME_PERIOD","RemoveConfirmation({0})")%>'
                                            CssClass="icon-trash" ToolTip="Remove" >
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Label ID="lblTotal" runat="server" />
                    </div>
                    <div class="large-1  columns">&nbsp;</div>
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

