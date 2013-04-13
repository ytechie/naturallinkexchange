<%@ Page language="c#" Inherits="Nle.Website.Members.Rank_Graphing.RankGraphingDefault"
    CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" %>

<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Search Engine Ranking Charting</h1>
	
	<div id="chartOptions">
		<div class="divPanel phraseSelection">
			<h1>Url's & Phrases</h1>
			Site Url: <asp:DropDownList Runat="server" ID="ddlSiteRankUrls" AutoPostBack="True" /><br />
			Search Phrase: <asp:DropDownList Runat="server" ID="ddlSearchPhrases" />
		</div>
		
		<div class="divPanel">
			<h1>Search Engines</h1>
				<asp:CheckBox Runat="server" ID="chkPlotGoogle" Checked="True" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/GoogleTiny.gif" alt="Google" /><br />
				<asp:CheckBox Runat="server" ID="chkPlotYahoo" Checked="True" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/YahooTiny.gif" alt="Yahoo" /><br />
				<asp:CheckBox Runat="server" ID="chkPlotMSN" Checked="True" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/MSNTiny.gif" alt="MSN Search" /><br />
				<asp:CheckBox Runat="server" ID="chkPlotAskJeeves" Checked="False" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/JeevesTiny.gif" alt="Ask Jeeves" /><br />
				<asp:CheckBox Runat="server" ID="chkPlotAll" Text="All (Averaged)" Enabled="False" />
		</div>
		
		<div class="divPanel">
			<h1>Trending</h1>
				<asp:CheckBox Runat="server" ID="chkTrendGoogle" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/GoogleTiny.gif" alt="Google" /><br />
				<asp:CheckBox Runat="server" ID="chkTrendYahoo" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/YahooTiny.gif" alt="Yahoo" /><br />
				<asp:CheckBox Runat="server" ID="chkTrendMSN" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/MSNTiny.gif" alt="MSN Search" /><br />
				<asp:CheckBox Runat="server" ID="chkTrendAskJeeves" />
				<img src="<%= Nle.Website.Global.VirtualDirectory %>Images/JeevesTiny.gif" alt="Ask Jeeves" />
		</div>
				
		<div class="divPanel">
			<h1>Time Range</h1>
			<asp:RadioButtonList runat="server" ID="rdoTimeRange">
				<asp:ListItem Selected="True" Value="3">Last 3 Months</asp:ListItem>
				<asp:ListItem Value="6">Last 6 Months</asp:ListItem>
				<asp:ListItem Value="12">Last Year</asp:ListItem>
				<asp:ListItem Value="-1">All Time</asp:ListItem>
			</asp:RadioButtonList>
		</div>
		
		<div class="divPanel">
			<h1>Chart Size</h1>
			<asp:DropDownList Runat="server" ID="ddlChartSize">
				<asp:ListItem Value="200x300">200x300 (Small)</asp:ListItem>
				<asp:ListItem Value="300x500" Selected="True">300x500 (Medium)</asp:ListItem>
				<asp:ListItem Value="500x800">500x800 (Large)</asp:ListItem>
			</asp:DropDownList>
		</div>
	</div>

	<div class="chartSection">
		<asp:Button Runat="server" ID="cmdRefresh" Text="Refresh" /><br />
		<asp:PlaceHolder Runat="server" ID="ControlPlaceHolder" />
	</div>
</asp:Content>