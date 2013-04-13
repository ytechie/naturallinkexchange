<%@ Control Language="c#" AutoEventWireup="false" Codebehind="PollSelectCategories.ascx.cs" Inherits="PeterBlum.Poll_WebAdmin.PollSelectCategories" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>


<asp:table id=CategoriesTable Width="100%" Runat="server">
<asp:TableRow ID=CategoriesRow Runat=server EnableViewState="False">
<asp:TableCell Width=20%>
Select by categories<br>
<asp:RadioButtonList ID=CategoryConditionID Runat=server RepeatDirection=Horizontal >
<asp:ListItem Selected=True>Any</asp:ListItem>
<asp:ListItem>All</asp:ListItem>
</asp:RadioButtonList>
</asp:TableCell>
<asp:TableCell >
<asp:DropDownList ID=Category1DDL Runat=server >
<asp:ListItem Value="0">Exact</asp:ListItem>
<asp:ListItem Value="1">Contains</asp:ListItem>
<asp:ListItem Value="2">Starts with</asp:ListItem>
<asp:ListItem Value="3">Ends with</asp:ListItem>
</asp:DropDownList>
&nbsp;
<asp:TextBox ID=Category1TextBox Runat=server Width="150px" />
&nbsp;
<asp:DropDownList ID="Category2DDL" Runat=server>
<asp:ListItem Value="0">Exact</asp:ListItem>
<asp:ListItem Value="1">Contains</asp:ListItem>
<asp:ListItem Value="2">Starts with</asp:ListItem>
<asp:ListItem Value="3">Ends with</asp:ListItem>
</asp:DropDownList>
&nbsp;
<asp:TextBox ID="Category2TextBox" Runat=server Width="150px" />
&nbsp;
<br>
<asp:DropDownList ID="Category3DDL" Runat=server>
<asp:ListItem Value="0">Exact</asp:ListItem>
<asp:ListItem Value="1">Contains</asp:ListItem>
<asp:ListItem Value="2">Starts with</asp:ListItem>
<asp:ListItem Value="3">Ends with</asp:ListItem>
</asp:DropDownList>
&nbsp;
<asp:TextBox ID="Category3TextBox" Runat=server Width="150px" />
&nbsp;
<asp:DropDownList ID="Category4DDL" Runat=server>
<asp:ListItem Value="0">Exact</asp:ListItem>
<asp:ListItem Value="1">Contains</asp:ListItem>
<asp:ListItem Value="2">Starts with</asp:ListItem>
<asp:ListItem Value="3">Ends with</asp:ListItem>
</asp:DropDownList>
&nbsp;
<asp:TextBox ID="Category4TextBox" Runat=server Width="150px" />
&nbsp;

</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
Category commands
</asp:TableCell>
<asp:TableCell CssClass=SelectionCriteriaQuickLookupBox >
Quick Lookup&nbsp;
<asp:DropDownList ID="SavedCatDDL" Runat=server>
</asp:DropDownList>
&nbsp;
<asp:Button ID=GetButton Runat=server CommandName="Get" Text="Get" />
&nbsp;
<asp:Button ID="SetButton" Runat=server CommandName="Set" Text="Save" />
&nbsp;
<asp:Button id="ClearButton" Runat="server" Text="Clear Categories"></asp:Button>
</asp:TableCell>
</asp:TableRow>
</asp:table>
