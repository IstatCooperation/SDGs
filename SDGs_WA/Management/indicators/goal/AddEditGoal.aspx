<%@ Page Title="Goal" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="AddEditGoal.aspx.cs" Inherits="Management_indicators_goal_AddEditGoal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
      <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a id="backLinkGT" title="Types"   runat="server" ></a> &gt; <a title="Goals"  id="backLinkG" runat="server" ></a>  &gt;<b> <span  ID="pageTitle" runat="server"></span></b></div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="updatePanel" runat="server">
            <ContentTemplate>
                <div class="row">

                    <asp:Literal ID="saveMessage" runat="server" />
                    <div class="large-1  columns"></div>
                    <div class="large-11 columns">

                        <table style="width: 99%;">
 
                             <tr>
                                <td class="text-right" width="20%">Goal Code*
                                    <asp:LinkButton runat="server" ToolTip="Goal Code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Goal_Code" type="text" runat="server" maxlength="10" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="Goal_Code"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vGoal_Code" /></td>

                            </tr>
                            <tr>
                                <td class="text-right" width="20%">Description EN*
                                    <asp:LinkButton runat="server" ToolTip="Description in english language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Goal_DescEn" type="text" runat="server" maxlength="50" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="Goal_DescEn"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vGoal_DescEn" /></td>

                            </tr>
                            <tr>
                                <td class="text-right">Description SL
                                    <asp:LinkButton runat="server" ToolTip="Description in secondary language" 
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="Goal_DescAr" type="text" runat="server"  maxlength="50"></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td class="text-right">Url Image EN
                                    <asp:LinkButton runat="server" ToolTip="Path or url for the english goal's image"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="GoalImageEn" type="text" runat="server" class="text-left"></td>
                                <td class="text-left"></td>
                            </tr>
                            <tr>
                                <td class="text-right">Url Image SL
                                    <asp:LinkButton runat="server" ToolTip="Path or url for the secondary language goal's image"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="GoalImageAr" type="text" runat="server"></td>
                                <td class="text-left"></td>
                            </tr>
                         
                            <tr>
                                <td colspan="2" class="text-right" style="padding: 3px;">
                                    <p class="small-text-right">*Required fields</p>
                                    <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                    <asp:Button Text="Save" runat="server" ID="cmdLogin" OnClick="saveGoal" />
                                </td>
                                <td></td>
                            </tr>
                        </table>

                    </div>

                </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

    </main>
</asp:Content>

