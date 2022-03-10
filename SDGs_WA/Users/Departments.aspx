<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Departments.aspx.cs" Inherits="Users_Departments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <main id="content" class="row">
        <div><a id="backLink"  href='<%= ResolveUrl("~/") %>'>Home</a> &gt; <a href="UsersList.aspx">Users List</a> &gt; User  &amp; Indicators</div>

        <div class="row">
            <div class="large-5 columns">
                <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
                    <b>Enable access to indicators  for  <i>
                        <asp:Literal runat="server" ID="Username" /></i></b>
                    in     
                                            <asp:DropDownList ID="ddlGoalTypes" runat="server" AutoPostBack="true"   OnSelectedIndexChanged="ddlGoalTypes_SelectedIndexChanged">
                                            </asp:DropDownList>
                                
                </div>
            </div>
            <div class="large-3 columns">
                <asp:Literal ID="saveMessage" runat="server" />
            </div>
            <div class="large-2 columns align-right">
                <asp:Button Text="Save" runat="server" ID="cmdSave" OnClick="saveIndicators" />
            </div>
        </div>
        <div class="row">
            <div class="large-12 columns">
                <a href="javascript: void(0);" id="open-all" onclick="openDetails();" class="icon-plus">Open All </a>
                &nbsp;&nbsp;&nbsp;<a href="javascript: void(0);"><label><input type="checkbox" onclick="checkAll();" class='indicator-checkbox' name="select-all" value="" id="select-all"><span id="label-select-all">Select All</span></label></a>
                <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
            </div>
        </div>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.0/jquery.min.js"></script>
        <script src="../jquery.details.js"></script>
        <script>
            var openall = false;
            var checkall = false;
            
            function openDetails() {
                openall = !openall;
                $('details').prop('open', openall);
                if (openall) {
                    $('#open-all').addClass("icon-minus");
                    $('#open-all').removeClass("icon-plus");
                    $('#open-all').text("Close All");
                }
                else {
                    $('#open-all').addClass("icon-plus");
                    $('#open-all').removeClass("icon-minus");
                    $('#open-all').text("Open All");
                }

            }
            function checkAll() {
                checkall = !checkall;
                $('input:checkbox').prop('checked', checkall);
                  
            }
            
            function selectDep(dip) {
                 var checked = $('#checkbox-dep-' + dip).prop("checked");
              //   $('#checkbox-dep-' + dip).prop("checked", checked);
                $('.indicator-dep-'+dip).prop('checked', checked);
                 
            }
            $(function () {
                // Add conditional classname based on support
                $('html').addClass($.fn.details.support ? 'details' : 'no-details');
                // Emulate <details> where necessary and enable open/close event handlers
                $('details').details();

                $('input:checkbox').change(function () {
                    if( $(this).val()!='')
                    $(":checkbox[value=" + $(this).val() + "]").prop("checked", $(this).prop('checked'));
                });
            });


        </script>
        
    </main>
</asp:Content>

