<%@ Page Title="Classification" Language="C#" MasterPageFile="~/MasterPage.master"  AutoEventWireup="true" CodeFile="AddEditCls.aspx.cs" Inherits="Cls_AddEditCls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                 <div><a href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a   title="Classifications"   href="/Management/cls/ClassificationsList.aspx" runat="server" >Classifications List</a> &gt; <span  ID="pageTitle" runat="server"></span></div>
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
                                <td class="text-right" width="20%">Name*
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="name" type="text" runat="server" maxlength="10" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="name"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vName" /></td>

                            </tr>
                             <tr>
                                <td class="text-right" width="20%">CODE
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="code" type="text" runat="server" maxlength="10" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="code"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vCode" /></td>

                            </tr>
                                 <tr>
                                <td class="text-right" width="20%">SEQUENCE
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="sequence" type="number" runat="server"  ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="sequence"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vsequence" /></td>

                            </tr>
                               <tr>
                                <td class="text-right" width="20%">Label ENG
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="labelENG" type="text" runat="server" maxlength="10" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="labelENG"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="RequiredFieldValidator1" /></td>

                            </tr>
                             <tr>
                                <td class="text-right" width="20%">TABLE NAME
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="TABLE_NAME" type="text" runat="server" maxlength="100" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="TABLE_NAME"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vTABLE_NAME" /></td>

                            </tr>
                            <tr>
                                <td class="text-right" width="20%">Description EN*
                                    <asp:LinkButton runat="server" ToolTip="Description in english language"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="Target_DescEn" type="text" runat="server"   ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="Target_DescEn"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vTarget_DescEn" /></td>

                            </tr>
                                 <td class="text-right" width="20%">Label SL
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="labelSL" type="text" runat="server" maxlength="10" ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="labelSL"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vlabelSL" /></td>

                            </tr>
                            <tr>
                                <td class="text-right">Description SL
                                    <asp:LinkButton runat="server" ToolTip="Description in secondary language" 
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px">
                                    <input id="Target_DescAr" type="text" runat="server"   ></td>
                                <td class="text-left"></td>
                            </tr>
                           <tr>
                                <td class="text-right" width="20%">IS_TIME
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="IS_TIME" type="number" runat="server"  ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="IS_TIME"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vIS_TIME" /></td>

                            </tr> <tr>
                                <td class="text-right" width="20%">INT_CODE
                                    <asp:LinkButton runat="server" ToolTip="Target code (max 10)"
                                        CssClass="icon-help-circled">
                                    </asp:LinkButton>:</td>
                                <td style="padding: 5px" class="text-left">
                                    <input id="INT_CODE" type="number" runat="server"  ></td>
                                <td class="text-left" width="1%">
                                    <asp:RequiredFieldValidator ControlToValidate="INT_CODE"
                                        Display="Dynamic" ErrorMessage="*" runat="server"
                                        ID="vINT_CODE" /></td>

                            </tr>
                            <tr>
                                <td colspan="2" class="text-right" style="padding: 3px;">
                                    <p class="small-text-right">*Required fields</p>
                                    <asp:Button ID="btnClear" Text="Reset" CausesValidation="false" OnClick="ClearData" runat="server" />
                                    <asp:Button Text="Save" runat="server" ID="cmdLogin" OnClick="saveCmd" />
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

