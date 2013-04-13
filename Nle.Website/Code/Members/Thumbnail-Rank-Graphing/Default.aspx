<%@ Page language="c#" Inherits="Nle.Website.Members.Thumbnail_Rank_Graphing.ThumbnailRankGraphingDefault"
    enableViewState="False" CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Page="~/members/rank-graphing/default.aspx" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
    <script language="JavaScript" type="text/javascript" src="<%= Nle.Website.Global.VirtualDirectory %>Scripts/Global.js"></script>
    
	<div class="divPanel">
		<h1>Overview</h1>
		<p>
			This page is an easy way to see how your sites are performing in the search engines.  With
			a simple glance, you can see if your site has been moving up, moving down, or has been staying
			the same.
		</p>
		<p>
			Each chart is scaled from 1 to 50.  The top of the chart represents being at the
			to in the search engine for that key phrse.  If you would like to view more specific
			data about a particular search term, simply click on that chart.  To view the search results
			for yourself, click on the corresponding search engine logo.
		</p>
	</div>
	
	<asp:PlaceHolder Runat="server" ID="ControlPlaceholder" />
</asp:Content>