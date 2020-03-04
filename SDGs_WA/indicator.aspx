<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="indicator.aspx.cs" Inherits="index" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content" class="row">
        <div class="large-12 columns">
            <div><a href="./index.aspx">Goals</a> &gt; <a id="backLink" runat="server" href="">Targets &amp; Indicators</a> &gt; Subindicators</div>
            <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                <b>Indicator from Target
                    <asp:Literal runat="server" ID="title" /></b>
                <h3>Indicator
                    <asp:Literal runat="server" ID="indicatorNL" />
                    -
                    <asp:Literal runat="server" ID="subTitle" /></h3>
                <asp:Literal runat="server" ID="indCode" Visible="false" />
                <asp:Literal runat="server" ID="hiddenTargId" Visible="false" />
            </div>
        </div>

        <div class="large-12 columns">
            <asp:Literal ID="saveMessage" runat="server" />
            <asp:Repeater ID="SubDetail" runat="server">
                <ItemTemplate>
                    <div class="item" id="id<%#Eval("Subindicator_Code") %>">
                        <div class="row">
                            <div class="small-9 columns">
                                <h3>Subindicator Code: <%#Eval("Subindicator_Code") %>
                                    /
                                    Series:
                                    <asp:TextBox ID="SERIES" Text='<%#Eval("SERIES") %>' Width="200px" runat="server" />

                                </h3>
                            </div>
                            <div class="small-3 columns" style="text-align: right">
                                <table>
                                    <tr>
                                        <td>
                                            <img src="images/excel_icon.png" alt="Excel template" title="Excel template" style="width: 50px;">
                                        </td>
                                        <td>
                                            <asp:LinkButton runat="server" ID="template"
                                                Text="Template"
                                                CommandArgument='<%# Eval("Subindicator_Code") %>'
                                                OnClick="template_ServerClick">
                                                    Download template
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <asp:HiddenField ID="goalId" Value='<%#Eval("Goal_ID") %>' runat="server" />
                        <asp:HiddenField ID="indicatorCode" Value='<%#Eval("Indicator_Code") %>' runat="server" />
                        <asp:HiddenField ID="subindicatorCode" Value='<%#Eval("Subindicator_Code") %>' runat="server" />

                        <table class="propertiesTable">
                            <tr>
                                <td>Unit measure:</td>
                                <td>
                                    <asp:DropDownList ID="unitMeasure"
                                        DataTextField="DESC_EN"
                                        DataValueField="UNIT_MEASURE"
                                        runat="server" />
                                </td>
                                <td>Dimensions:</td>
                                <td class="disaggregation"><%#getDimensionsDescription(Eval("Dimensions")) %></td>
                            </tr>
                            <tr>
                                <td>Unit multiplier:</td>
                                <td>
                                    <asp:DropDownList ID="unitMultiplier"
                                        DataTextField="DESC_EN"
                                        DataValueField="UNIT_MULT"
                                        runat="server" />
                                </td>
                                <td>Observation status:</td>
                                <td>
                                    <asp:DropDownList ID="obsStatus"
                                        DataTextField="DESC_EN"
                                        DataValueField="OBS_STATUS"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>

                        Description EN:<br>
                        <asp:TextBox ID="descEn" TextMode="MultiLine" Text='<%#Eval("Subindicator_DescEn") %>' runat="server" /><br>
                        Description AR:<br>
                        <asp:TextBox ID="descAr" TextMode="MultiLine" Text='<%#Eval("Subindicator_DescAr") %>' CssClass="arDirection" runat="server" /><br>
                        <asp:Button ID="save" Text="Save" OnClick="Save" runat="server" />
                        <a class="value" href="value.aspx?targId=<%#Eval("Target_Id").ToString().Trim()%>&subId=<%#Eval("Subindicator_Code") %>">Insert values</a>
                        <asp:Button ID="delete" Text="Delete" OnClick="Delete" OnClientClick="if ( ! UserDeleteConfirmation()) return false;" runat="server" />
                        <br />
                        <div style="text-align: right;">
                            <asp:Literal ID="uploadResult" runat="server" />
                            <asp:FileUpload ID="excelFileUpload" CssClass="inputfile" runat="server" onchange="changeFileName(this)" />
                            <asp:Label ID="excelFileLabel" AssociatedControlID="excelFileUpload" CssClass="inputfile" runat="server" Text="Select file..." />
                            <asp:Button ID="btnUpload" Text="Load Data" OnClick="Upload" runat="server" />
                            <div class="light">
                                <div class='red red<%#Eval("Is_Uploaded") %>'>&nbsp;</div>
                                <div class='green green<%#Eval("Is_Uploaded") %>'>&nbsp;</div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div id="popup1" class="overlay">
                <div class="popup">
                    <h2>Add new subindicator</h2>
                    <a class="close" href="#">&times;</a>
                    <div class="content">
                        <table class="propertiesTable">
                            <tr>
                                <td>Series:</td>
                                <td>
                                    <input name="newSubSeries" id="newSubSeries" runat="server" />
                                </td>
                                <td>Unit measure:</td>
                                <td>
                                    <asp:DropDownList ID="newUnitMeasure"
                                        DataTextField="DESC_EN"
                                        DataValueField="UNIT_MEASURE"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Unit multiplier:</td>
                                <td>
                                    <asp:DropDownList ID="newUnitMultiplier"
                                        DataTextField="DESC_EN"
                                        DataValueField="UNIT_MULT"
                                        runat="server" />
                                </td>
                                <td>Observation status:</td>
                                <td>
                                   <asp:DropDownList ID="newObsStatus"
                                        DataTextField="DESC_EN"
                                        DataValueField="OBS_STATUS"
                                        runat="server" />
                                </td>
                            </tr>
                        </table>
                        Description EN:<br>
                        <textarea name="newSubDescEN" rows="2" cols="20" id="newSubDescEN" runat="server"></textarea>
                        Description AR:<br>
                        <textarea name="newSubDescAR" rows="2" cols="20" id="newSubDescAR" class="arDirection" runat="server"></textarea>
                        <div>
                            <asp:CheckBoxList ID="newDimensions" runat="server"
                                RepeatColumns="3"
                                RepeatDirection="Vertical"
                                RepeatLayout="Table"
                                CssClass="MenuItem"
                                Width="100%" />
                        </div>
                        <asp:Button ID="btnAddNewSubInd" Text="Add" OnClick="AddNewSubIndicator" OnClientClick="if(!checkInsert()) return false;" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="large-12 columns"><a href="#popup1">+ Add new subindicator</a></div>
    </main>
    <script src="//code.jquery.com/jquery-3.3.1.min.js"></script>
    <script>
        function changeFileName(element) {
            id = element.id.substring(0, element.id.lastIndexOf("_") + 1) + "excelFileLabel";
            a = element.value.split('\\'); // split is necessary because lastIndexOf can't find "\" 
            document.getElementById(id).innerText = a[a.length - 1];
        }
        function UserDeleteConfirmation() {
            return confirm("Are you sure you want to delete this subindicator?");
        }
        function checkInsert() {
            if ($(".popup input[type=checkbox]").is(':checked')) return true;
            alert("Error! At least a dimension is mandatory.");
            return false;
        }
    </script>
</asp:Content>
