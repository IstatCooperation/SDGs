<%@ Page Title="Indicator" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AddEditIndicator.aspx.cs" Inherits="Management_indicators_AddEditIndicator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a id="backLinkGT" title="Types"  runat="server"></a>&gt; <a title="Goals" id="backLinkG" runat="server"></a>&gt; <a title="Targets" id="backLinkT" runat="server"></a>&gt; <a title="Targets" id="backLinkI" runat="server"></a>&gt; <b><span id="pageTitle" runat="server"></span></b></div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>

                <div class="row">

                    <asp:Literal ID="saveMessage" runat="server" />

                    <div class=" columns">
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">Target</legend>
                            <table style="width: 99%;">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="targetLabel" runat="server" /></td>

                                </tr>
                                <tr>
                                    <td class="text-right align-top" style="padding: 5px; vertical-align: top;" width="20%">Indicators Code:</td>
                                    <td class="text-left">
                                        <asp:Literal ID="indicatorsTarget" runat="server" />
                                    </td>
                                    <td class="text-left" width="1%"></td>

                                </tr>

                            </table>
                        </fieldset>


                        <%  if (this.isAddExisting())
                            { %>
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">Existing Indicator</legend>
                            <table style="width: 99%; padding: 3px">
                                <tr>
                                    <td style="padding: 3px" width="50%">
                                        <label for="ddlGoalType">Goal types</label>
                                        <asp:DropDownList ID="ddlGoalType" runat="server" OnSelectedIndexChanged="ddlGoalType_Selection_Change" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>

                                    <td style="padding: 3px" width="50%">
                                        <label for="ddlGoal">Goals</label>
                                        <asp:DropDownList ID="ddlGoal" runat="server" OnSelectedIndexChanged="ddlGoal_Selection_Change" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding: 3px" width="50%">
                                        <label for="ddlTarget">Targets</label>
                                        <asp:DropDownList ID="ddlTarget" runat="server" OnSelectedIndexChanged="ddlTarget_Selection_Change" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding: 3px" width="50%">
                                        <label for="ddlIndicator">Indicators</label>
                                        <asp:DropDownList ID="ddlIndicator" runat="server" OnSelectedIndexChanged="ddlIndicator_Selection_Change" AutoPostBack="True">
                                        </asp:DropDownList>
                                    </td>

                                </tr>

                            </table>
                        </fieldset>
                        <fieldset class="scheduler-border">
                            <legend class="scheduler-border">New Indicator Link</legend>
                            <table style="width: 99%;">
                                <tr>
                                    <tr>
                                    <td class="text-right" width="20%"><b>Internal Code ID</b>*:
                                    <asp:LinkButton runat="server" ToolTip="Internal Indicator Code ID"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <b>
                                            <asp:Label ID="Indicator_Code_ID_Ex" runat="server" /></b>
                                    </td>
                                    <td class="text-left" width="1%">  <asp:RequiredFieldValidator ControlToValidate="ddlIndicator"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="vIndicator_Code_ID_Ex" /></td>
                                </tr>
                                <tr>
                                    <td class="text-right" width="20%"><b>Code</b>*:
                                    <asp:LinkButton runat="server" ToolTip="Indicator Code  (max 20)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <b>
                                        <input id="Indicator_Code_Value_Label_Ex" type="text" runat="server" maxlength="20" size="20"></b>
                                    </td>
                                    <td class="text-left" width="1%">  <asp:RequiredFieldValidator ControlToValidate="Indicator_Code_Value_Label_Ex"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="vIndicator_Code_Value_Label_Ex" /></td>
                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Code in Target*
                                    <asp:LinkButton runat="server" ToolTip="Code displayed in Target page  (max 10, field INDICATOR_NL)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <input id="Indicator_NL_EX" type="text" runat="server" maxlength="10" size="10"></td>
                                    <td class="text-left" width="1%">
                                        <asp:RequiredFieldValidator ControlToValidate="Indicator_NL_EX"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="Indicator_NL2" /></td>

                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Description EN*:
                                    <asp:LinkButton runat="server" ToolTip="Description in english language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <asp:Label ID="Indicator_descEn_Ex" runat="server" /></b></td>
                                    <td class="text-left" width="1%"></td>

                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Description SL:
                                    <asp:LinkButton runat="server" ToolTip="Description in secondary language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <asp:Label ID="Indicator_descAr_Ex" runat="server" /></b></td>
                                    <td class="text-left" width="1%"></td>

                                </tr>

                            </table>
                            </table>
                            </fieldset>
                                <% }
                            else
                            {  %>
                          <fieldset class="scheduler-border">
                            <legend class="scheduler-border"> Indicator</legend>
                            <table style="width: 99%;">
                           
                                   
                                <tr>
                                    <td class="text-right" width="20%"><b>Indicator Code</b>*:
                                    <asp:LinkButton runat="server" ToolTip="Displayed Indicator Code (max 20)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                   
                                        <input id="Indicator_Code_Value" type="text" runat="server" maxlength="20">
                                    
                                    </td>
                                    <td class="text-left" width="1%">
                                        <asp:RequiredFieldValidator ControlToValidate="Indicator_Code_Value"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="Indicator_Code_Value1" /></td>

                                </tr>
                                 <tr>
                                    <td class="text-right" width="20%"><b>Internal Code ID</b>*:
                                    <asp:LinkButton runat="server" ToolTip="Internal Indicator Code ID (max 20, can be the same of Indicator Code)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">

                                          <%  if (this.isEditAction())
                                            { %>
                                        <b>
                                            <asp:Label ID="Indicator_Code_ID_label" runat="server" /></b>
                                        <% }
                                            else
                                            {%>
                                        <input id="Indicator_Code_ID" type="text" runat="server" maxlength="20">
                                        <% }  %>

                                    </td>
                                    <td class="text-left" width="1%"> <asp:RequiredFieldValidator ControlToValidate="Indicator_Code_ID"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="vIndicator_Code_ID" /></td>
                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Code in Target*
                                    <asp:LinkButton runat="server" ToolTip="Code displayed in Target page (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">

                                        <input id="Indicator_NL" type="text" runat="server" maxlength="10" size="10"></td>
                                    <td class="text-left" width="1%">
                                        <asp:RequiredFieldValidator ControlToValidate="Indicator_NL"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="vIndicator_NL" /></td>

                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Description EN*:
                                    <asp:LinkButton runat="server" ToolTip="Description in english language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <asp:TextBox ID="Indicator_descEn" TextMode="MultiLine" runat="server" /></td>
                                    <td class="text-left" width="1%">
                                        <asp:RequiredFieldValidator ControlToValidate="Indicator_descEn"
                                            Display="Dynamic" ErrorMessage="*" runat="server"
                                            ID="vIndicator_descEn" /></td>

                                </tr>
                                <tr>
                                    <td class="text-right" width="20%">Description SL:
                                    <asp:LinkButton runat="server" ToolTip="Description in secondary language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                    <td style="padding: 5px" class="text-left">
                                        <asp:TextBox ID="Indicator_descAr" runat="server" TextMode="MultiLine" /></td>
                                    <td class="text-left" width="1%"></td>

                                </tr>

                            </table>
  </fieldset>
                            <% }    %>
                      
                        <table style="width: 99%;">

                            <tr>
                                <td>

                                    <fieldset class="scheduler-border">
                                        <legend class="scheduler-border">Related</legend>


                                        <asp:Literal ID="relatedCodes" runat="server"></asp:Literal>

                                    </fieldset>
                                </td>
                                <td>
                                    <span class="right" style="padding: 1px;">
                                        <p class="small-text-right">*Required fields</p>
                                        <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                        <asp:Button Text="Save" runat="server" ID="saveIndicator" OnClick="saveCmd" />
                                    </span>
                                </td>
                            </tr>
                        </table>
                    </div>

                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
</asp:Content>

