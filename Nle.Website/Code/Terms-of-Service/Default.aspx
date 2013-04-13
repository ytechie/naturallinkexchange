<%@ Page language="c#" Inherits="Nle.Website.TermsOfServiceDefault" CodeFile="Default.aspx.cs"
    MasterPageFile="~/MasterPage.master" %>

<%@ Register TagPrefix="Nle" TagName="SignUpNowButton" Src="~/Common_Controls/SignUpNowButton.ascx" %>

 <asp:Content runat="server" ContentPlaceHolderID="mainContent">
	<h1>Terms of Service</h1>

	<h3>Agreement</h3>
	<p>
		By registering for a Natural Link Exchange account, you agree to abide by all rules and regulations
		regarding our service. Your use of the service constitutes your agreement to the terms, conditions,
		policies and notices set forth below. In addition, our service shall only be used in accordance with
		any and all applicable laws and regulations.
	</p>
	<h3>Maintaining of links</h3>
	<p>
		To be part of this link network, you must link to your link pages or link script from the front page
		of your site.  If you remove the link, you must notify Natural Link Exchange, and your links will be
		removed from the system. You agree to allow links to other sites to be placed on your website through
		our automated process. 
	</p>
	<h3>Notification</h3>
	<p>
		You agree to receive occasional service announcements via email from Natural Link Exchange. These
		announcements will be used to provide information about new services or updates to existing features
		and functionality as well as changes in the search engines.
	</p>
	<h3>Web Site Content</h3>
	<p>
		Natural Link Exchange does not accept pharmacy sites, doorway sites, pornography, or anything illegal.
		Any site may be turned down or cancelled based on content that is deemed inappropriate.				
	</p>
	<p>
	
	</p>
	<h3>Guarantee</h3>
	<p>
		Natural Link Exchange does not guarantee any specific ranking within the search engines the service
		is to provide inbound links and reporting facilities. No one can guarantee ranking in the search engines
		apart from the search engines themselves.
	</p>
	<h3>Termination of Service</h3>
	<p>
		Natural Link Exchange reserves the right to terminate service for any customer at any time, at our sole discretion.
		You also have the right to terminate the service at any time you see fit.  Service is prepaid, and at if an account
		is cancelled, no refunds will be given.
	</p>
	<h3></h3>
	<p>
		THIS SERVICE IS PROVIDED "AS IS" AND ANY EXPRESSED OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
		THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT
		SHALL WE BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
		(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS;
		OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
		OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SERVICE, EVEN IF
		ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
	</p>
	
	<Nle:SignUpNowButton runat="server" />
</asp:Content>