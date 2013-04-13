<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.cs" Inherits="Members_Reporting_Default" Title="Natural Link Exchange - Reporting" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<%@ Reference Control="~/Common_Controls/StatusHeader.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContent" Runat="Server">
    <table>
        <tr valign="top">
            <td style="padding-right: 20px;">
                <asp:PlaceHolder ID="ReportLinks" runat="server" />
            </td>
            <td><Nle:NleReportDisplay runat="server" id="report" /></td>
        </tr>
    </table>
</asp:Content>

