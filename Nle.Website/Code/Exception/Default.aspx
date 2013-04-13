<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Exception_Default" 
    MasterPageFile="~/MasterPage.master" %>
<%@ Register TagPrefix="Nle" Namespace="Nle.Website.Common_Controls" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="mainContent">
    <div id="Body">
	    <div id="ErrorMessage">
	        <h1>Unexpected Error</h1>
	        <p>We're sorry, but an error has occured.  An email has been sent to
	        us to notify us of the error, but if you would like to, you may visit
	        <asp:HyperLink runat="server" NavigateUrl="~/Support/">our Support page</asp:HyperLink>
	        to submit a case with us.</p>
    	    
	        <p>If you submit a case to us, please try to include the following information:
	        </p>
            <ol>
                <li>What you were doing at the time the error happened.</li>
                <li>Whether you can reproduce the problem or not.</li>
                <li>If you can reproduce the problem, please provide the steps so that we 
                can reproduce the problem as well. It is easier for us to know that
                we have fixed the problem if we produce the error ourselve.</li>
            </ol>
	    </div>
	</div>
</asp:Content>