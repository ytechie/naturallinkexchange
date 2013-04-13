<%@ Page Language="c#" Inherits="Nle.Website.Members.Administration.Send_Email.SendEmailDefault"
    CodeFile="Default.aspx.cs" ValidateRequest="false" MasterPageFile="~/MasterPage.master"
    Title="Natural Link Exchange - Send Email To All Users" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="mainContent">
    <asp:PlaceHolder runat="server" ID="controlPlaceholder" />
    
    <p>To create the message body of the email, do the following:
        <ol>
            <li>Create the email message in your favorite editor (Word works because it has a spell checker).</li>
            <li>Copy the text and paste it into notepad to remove the editor's formatting.</li>
            <li>Copy the text from notepad and paste it into Visual Studio 2005.</li>
            <li>Apply HTML formatting to make it look good.  You can even use the designer to assist in this.</li>
        </ol>
    </p>
    <hr />
    <div id="emailHeader">
        <div class="standardFieldLabel">
            To:</div>
        <div class="To">
            <asp:DropDownList ID="ddlFilters" runat="server" /></div>
        <div class="standardFieldLabel">
            Subject:</div>
        <asp:TextBox runat="server" ID="txtSubject" CssClass="headerInput" />
        <asp:RequiredFieldValidator runat="server" ID="txtSubjectValidator" ControlToValidate="txtSubject"
            Display="Dynamic" ErrorMessage="Subject is required" />
        <br />
    </div>
    <asp:TextBox runat="server" ID="txtBody" TextMode="MultiLine" Rows="10" CssClass="Body" /><br />
    <asp:Button runat="server" ID="cmdPreview" Text="Preview" />
    <asp:Button runat="server" ID="cmdSendPreview" Text="Send Preview" />
    <asp:Button runat="server" ID="cmdSend" Text="Send" />
    <asp:Button runat="server" ID="cmdCancel" Text="Cancel" />
    <br />
    <br />
    <div class="Replacements">
        <asp:Table ID="tblReplacements" runat="server" />
    </div>
</asp:Content>
