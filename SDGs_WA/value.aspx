<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="value.aspx.cs" Inherits="index" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content" class="row">
        <div class="large-12 columns">
            <div><a href="./index.aspx">Goals</a> &gt; <a href="./target.aspx?goalId=<asp:Literal ID='goalId' runat='server' />">Targets &amp; Indicators</a> &gt; <a href="./indicator.aspx?indId=<asp:Literal ID='indId' runat='server' />&targId=<asp:Literal ID='targIdBack' runat='server' />">Subindicators</a> &gt; Insert values</div>
            <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                <b>Indicator from Target
                    <asp:Literal runat="server" ID="title" /></b>
                <h3>Indicator
                    <asp:Literal runat="server" ID="indicatorNL" />
                    -
                    <asp:Literal runat="server" ID="subTitle" /></h3>
                <h3>Subindicator
                    <asp:Literal runat="server" ID="subindicatorCode" /></h3>
                <asp:Literal runat="server" ID="indCode" Visible="false" />
            </div>
        </div>

        <div class="large-12 columns">
            <div class="loading"></div>
        <asp:Literal ID="saveMessage" runat="server" />
            <table id="valueTable" class="display compact nowrap insert-value" style="width: 100%">
                <thead>
                    <tr>
                        <asp:Literal ID="headerKey" runat="server" />
                        <th class="no-sort">OBS_VALUE</th>
                        <th class="no-sort">TIME_DETAIL</th>
                        <th class="no-sort">BASE_PER</th>
                        <th class="comment no-sort">COMMENT_OBS</th>
                        <th class="comment no-sort">SOURCE_DETAIL</th>
                        <th class="comment no-sort">SOURCE_DETAIL_AR</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="ValueDetail" runat="server">
                        <ItemTemplate>
                            <tr>
                                <%#Eval("Key") %>
                                <td>
                                    <asp:TextBox ID="OBS_VALUE" Text='<%#Eval("OBS_VALUE") %>' runat="server" />
                                    <asp:HiddenField ID="HKey" Value='<%#Eval("HiddenKey") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="TIME_DETAIL" Text='<%#Eval("TIME_DETAIL") %>' runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="BASE_PER" Text='<%#Eval("BASE_PER") %>' runat="server" />
                                </td>
                                <td class="comment">
                                    <asp:TextBox ID="COMMENT_OBS" Text='<%#Eval("COMMENT_OBS") %>' TextMode="MultiLine" runat="server" />
                                </td>
                                <td class="comment">
                                    <asp:TextBox ID="SOURCE_DETAIL" Text='<%#Eval("SOURCE_DETAIL") %>' TextMode="MultiLine" runat="server" />
                                </td>
                                <td class="comment">
                                    <asp:TextBox ID="SOURCE_DETAIL_AR" Text='<%#Eval("SOURCE_DETAIL_AR") %>' TextMode="MultiLine" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Button ID="save" Text="Save" OnClick="Save" runat="server" />
        </div>
    </main>
    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="//cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript">
        $(window).on('load', function () {
            $(".loading").fadeOut("slow");
        });
    </script>
    <script>
        $(document).ready(function () {
            $('#valueTable').DataTable({
                responsive: true,
                searching: false,
                paging: false,
                scrollX: true,
                "columnDefs": [{
                    "targets": 'no-sort',
                    "orderable": false,
                }]
            });
        });
    </script>
</asp:Content>
