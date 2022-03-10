<%@ Page Title="Departements and Indicators" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DepIndicators.aspx.cs" Inherits="DepIndicators" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a href="DepList.aspx">Departments List</a> &gt;Indicator enabled for  <b> <asp:Literal runat="server" ID="Departement" /></b> department</div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
          
                  <div class="row">

                    <details <%= openDetails %> >
                        <summary class="text-right"><a><b>Add Indicators</b></a></summary>

                        <table style="width: 99%; padding: 3px">
                            <tr>
                                <td style="padding: 3px" width="20%">
                                    <label for="ddlGoalType">Goal types</label>
                                    <asp:DropDownList ID="ddlGoalType" runat="server" OnSelectedIndexChanged="ddlGoalType_Selection_Change" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>

                                <td style="padding: 3px" width="40%">
                                    <label for="ddlGoal">Goals</label>
                                    <asp:DropDownList ID="ddlGoal" runat="server" OnSelectedIndexChanged="ddlGoal_Selection_Change" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>

                                <td style="padding: 3px" width="40%">
                                    <label for="ddlTarget">Targets</label>
                                    <asp:DropDownList ID="ddlTarget" runat="server" OnSelectedIndexChanged="ddlTarget_Selection_Change" AutoPostBack="True">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 3px" colspan="3">

                                   <div  style="width:90%"> <label for="lbxIndicator">Indicators</label>

                                    <div class="select-indicators">
                                        <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
                                    </div>
                                       </div>
                                     <div class="right" >
                                      <asp:Button Text="Add" Visible="false" runat="server" ID="cmdSave" OnClick="saveIndicators" />
                                     </div>
                                </td>

                            </tr>

                        </table>

                    </details>
                </div>
                <div class="row">
                    <div style="padding:1px">       <asp:Literal ID="saveMessage" runat="server" />      </div>
                   
                </div>

                <div class="row">
                    <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="False" OnRowDataBound="OnRowDataBound"   CssClass="user-list"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="20" EmptyDataText="No Records Found!" AllowPaging="true" OnDataBound="OnDataBound"
                        AllowSorting="true">

                        <Columns>
                            <asp:TemplateField HeaderText="#"   ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="IND_CODE" HeaderText="Internal Code"   ItemStyle-HorizontalAlign="Center" />

                            <asp:TemplateField HeaderText="Values Code" >
                                <ItemTemplate>
                                    <asp:Literal ID="relatedCodes" runat="server"></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Indicator_descEn" HeaderText="Description" />

                            <asp:TemplateField HeaderText="Remove"   ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="deleteButton"
                                        CommandArgument='<%# Eval("IND_CODE").ToString().Trim() %>'
                                        OnClick="removeIndicator"
                                        OnClientClick="if (!RemoveConfirmation()) return false;"
                                        CssClass="icon-trash" ToolTip="Remove">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblTotal" runat="server" />
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
        <script>
            function RemoveConfirmation() {
                return confirm("Are you sure to remove this indicator?");
            }
        </script>
</asp:Content>

