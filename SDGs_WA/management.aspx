<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="management.aspx.cs" Inherits="Management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <main id="content">
        <div class="row">
            <div class="large-12 columns">
                <div><a href='<%= ResolveUrl("~/") %>'>Home</a>  &gt; System Management</div>
            </div>
  </div>
             <div class="row">
                <div class="large-12 columns">
                    &nbsp;
                      </div>
                </div>
            <div class="row">
     <div class="large-2 columns">&nbsp;     </div>
                <div class="large-10 columns">
                    <ul>
                        <li>
                                <a href="Users/usersList.aspx"><i class='icon-users' title='Users Management'></i>Users</a>
                        </li>
                           <li>
                                  <a href="Management/indicators/type/typeList.aspx"><i class='icon-users' title='Indicator Element'></i>Indicator's Elements</a>
                        </li>
                           <li>

                                   <a href="Management/deps/DepList.aspx"><i class='icon-users' title='Departments List'></i>Departments</a>
                        </li>
                       <!-- li> <a href="Management/cls/ClassificationsList.aspx"><i class='icon-users' title='Classification'></i>Classification</a></!-->
                                   <li>  <a href="Management/TimePeriodList.aspx"><i class='icon-users' title='Time periodo'></i>Time Period</a></li>
                                   

                    </ul>
                    
             </div>
                 </div>
                 
      
 
    </main>
</asp:Content>

