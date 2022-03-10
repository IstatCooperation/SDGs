<%@ Page Title="Indicators List" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="IndicatorList.aspx.cs" Inherits="Management_indicators_IndicatorList" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <main id="content" class="row">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <contenttemplate>
            <div class="row">
                <div class="large-12 columns">
                    <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a id="backLinkGT"  title="Types"   runat="server" ></a> &gt; <a  title="Goals"  id="backLinkG" runat="server" ></a> &gt; <a  title="Targets" id="backLinkT" runat="server" ></a> &gt; <b>Indicators List</b></div>
                </div>
                 
            </div>
            <div class="row">
                <div class="large-4 columns">
                  </div>
                <div class="large-4 columns">
                    <asp:Literal ID="saveMessage" runat="server" />
                </div>
                <div class="large-4 columns"></div>
            </div>
            <div class="row">

                <div class="large-12 columns">
                    <div class="text-right">
                        <a id="linkAddE" href='AddEditIndicator.aspx?ed=f&ex=t' runat="server" class="icon-plus">Add Existing Indicator</a>
                        <a id="linkAddN" href='AddEditIndicator.aspx?ed=f&&ex=t'  runat="server"  class="icon-plus">Add New Indicator</a>
                    </div>
                    <asp:GridView ID="tableGrid" runat="server" AutoGenerateColumns="false" OnRowDataBound="OnRowDataBound" RowStyle-CssClass="gridview-row" CssClass="user-list"
                        OnPageIndexChanging="OnPageIndexChanging" PageSize="20" EmptyDataText="No Records Found!" AllowPaging="true"
                        AllowSorting="true">

                        <Columns>
                                      <asp:TemplateField HeaderText="#"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowNumber" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:BoundField DataField="Indicator_Code_Value" HeaderText="Code"  ItemStyle-HorizontalAlign="Center" />
                              <asp:BoundField DataField="Indicator_Code" HeaderText="Internal Code"  ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="Indicator_NL" HeaderText="Code in Target"  ItemStyle-HorizontalAlign="Center" />
                             <asp:BoundField DataField="Indicator_descEn" HeaderText="Descr En" />
                               <asp:BoundField DataField="Indicator_descAr" HeaderText="Descr SL" />
  <asp:TemplateField  HeaderText="Related">
                <ItemTemplate>
                    <asp:Literal  ID="relatedCodes" runat="server" ></asp:Literal>

                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:TemplateField HeaderText="SubIndicators"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="nSubIndicators" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit"  ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="linkEdit"
                                        CommandArgument='<%# Eval("Indicator_Code").ToString().Trim() %>'
                                        CssClass="icon-indent-right" ToolTip="Edit">
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                           <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" ID="statusButton"
                                        CommandArgument='<%# Eval("Indicator_Code")+";"+ Eval("IS_ACTIVE") %>'
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


