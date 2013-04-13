Exec Verify_GlobalSettingsRow 2, 'The default page template to use for link pages', Null, '<html>
  <head>
    <title>{title}</title>
    <meta name="keywords" content="{metaKeywords}" />
    <meta name="description" content="{metaDescription}" />
    <link rel="stylesheet" type="text/css" href="http://www.NaturalLinkExchange.com/ServerServices/LinksHtml/StyleSheets/Default.css" />
  </head>
  <body>
    <h1 id="titleHeader">
      {title}</h1>
    <div id="relatedCategoriesContainer">
      <h2 id="relatedCategoriesHeader">
        Related Categories</h2>
      <div id="relatedCategories">
        {relatedCategories}
      </div>
    </div>
    <div id="latestNewsContainer">
      <h2 id="latestNewsHeader">
        Latest News</h2>
      <div id="latestNews">
        {rssFeeds}
      </div>
    </div>
    <div id="relatedArticlesContainer">
      <h2 id="relatedArticlesHeader">
        Articles</h2>
      <div id="articles">
        {articles}
      </div>
    </div>
  </body>
</html>'
Exec Verify_GlobalSettingsRow 5, 'The "from" email address used to send emails from the system', Null, 'Support@NaturalLinkExchange.com'
Exec Verify_GlobalSettingsRow 6, 'The maximum number of times to try sending mail in the email queue before giving up', 5, Null
Exec Verify_GlobalSettingsRow 7, 'The minimum number of minutes to wait before trying to resend an email in the email queue', 60, Null
Exec Verify_GlobalSettingsRow 8, 'The SMTP mail server to use for sending emails', Null, 'mail.NaturalLinkExchange.com'
Exec Verify_GlobalSettingsRow 9, 'The Terms Of Service', Null, '<h3>Agreement</h3>
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
					removed from the system.
				</p>
				<h3>Notification</h3>
				<p>
					You agree to receive occasional service announcements via email from Natural Link Exchange. These
					announcements will be used to provide information about new services or updates to existing features
					and functionality. Also changes in the search engines.
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
				</p>'
Exec Verify_GlobalSettingsRow 10, 'The initial email that is sent when a user signs up', Null, '<h3>
    Welcome to Natural Link Exchange!</h3>
  <p>
    Start setting up your sites today. Here''s how to get started:
  </p>
  <p>
    Step 1. Click this link: <a href="http://www.NaturalLinkExchange.com/Members/">http://www.NaturalLinkExchange.com/Members/</a><br />
    Step 2. Sign in using your email address and this temporary password: {UsersPassword}<br />
    Step 3. Click the Change Password link under the Account header<br />
    Step 4. Now you are ready to set up your sites. Just follow the directions under
    the Site Configuration heading.
  </p>
  <p>
    Accounts will be available instantly but links will not be published until your
    site is approved by our editors. As long as our review of your site meets the criteria
    outlined in our Guidelines a <a href="http://www.NaturalLinkExchange.com/Guidelines/">
      http://www.NaturalLinkExchange.com/Guidelines/</a> . The approval process will
    take less than 24 hours and if during business hours may happen much sooner. You
    will receive an email indicating your approval status.
  </p>
  <p>
    After your site has been approved and once your site is configured to receive links
    the software will take over and you will begin to host and receive links.
  </p>
  <h4>
    Need some help?
  </h4>
  <p>
    If the links in this email don''t work, just copy the link text into your browser.
    If you need additional assistance please don''t hesitate to open a support case with
    us by visiting <a href="http://www.naturallinkexchange.com/Support/">http://www.naturallinkexchange.com/Support/</a>.
    If you have any other questions or comments, please feel free to contact us at <a
      href="http://www.naturallinkexchange.com/Contact/">http://www.naturallinkexchange.com/Contact/</a>.
  </p>
  <h4>
    Wrong email address?
  </h4>
  <p>
    If you received this email in error, we do applogize. Just ignore this email and
    you will never hear from Natural Link Exchange again.
  </p>
  <h4>
    Spread the Word - Earn Some Cash</h4>
  <p>
    If you want to spread the word about Natural Link Exchange and earn some money,
    join our affiliate program by visiting <a href="http://www.naturallinkexchange.com/Members/Affiliate-Program/"
      rel="nofollow">http://www.naturallinkexchange.com/Members/Affiliate-Program/</a>
  </p>
  <h4>
    See you at the top!</h4>'

Exec Verify_GlobalSettingsRow 11, 'The forgot email password', Null,
	'<h3>Natural Link Exchange Password</h3>
	<p>
		Your password: {UsersPassword}
	</p>
	<p>
		If you still have problems logging in, please contact support.
	</p>
  <p>
    If you received this email in error, we do applogize. Please let our support
	team know at http://www.NaturalLinkExchange.com/Support/
  </p>'

/* 
select * from globalsettings

*/