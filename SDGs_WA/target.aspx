<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="target.aspx.cs" Inherits="index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<main id="content" class="row">
    <div><a id="backLink" runat="server" href="./index.aspx">Goals</a> &gt; Targets &amp; Indicators</div>
    <div class="large-12 columns">
        <div class="www" style="font-size: 16pt; margin-bottom: 20px;">
            <b>Targets from Goal <asp:Literal runat="server" ID="title" EnableViewState="false" /></b>
        </div>
    </div>
    <div class="large-12 columns">
        <asp:Literal runat="server" ID="htmlStr" EnableViewState="false" />
    </div>
</main>
<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.0/jquery.min.js"></script>
<script src="jquery.details.js"></script>
<script>
	$(function() {
		// Add conditional classname based on support
		$('html').addClass($.fn.details.support ? 'details' : 'no-details');
		// Emulate <details> where necessary and enable open/close event handlers
		$('details').details();
	});
</script>
</asp:Content>