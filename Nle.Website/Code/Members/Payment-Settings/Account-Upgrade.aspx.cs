using System;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;
using Nle.Website.Common_Controls;
using YTech.General.Web;
using YTech.General.Web.PayPal;

namespace Nle.Website.Members.Payment_Settings
{
	/// <summary>
	///		This is the page that allows the user to change their subscription
	///		levels, etc.
	/// </summary>
	public partial class Account_Upgrade : Page
	{
		protected Button cmdCancel;

        private Database _db;
        private MainMaster _master;
        private StatusHeader header;
        private int _upgradeFlag;
        private int _userId;

        /// <summary>
        ///     The parameter used to signal this page that the user
        ///     got here because this is they choose to upgrade when
        ///     they signed up.
        /// </summary>
        public const string PARAM_UPGRADE_FLAG = "UpgradeFlag";

#if (SANDBOX)
		private const string PAYPAL_BASE_URL = "https://www.sandbox.paypal.com/us/cgi-bin/webscr?";
		private const string PAYPAL_EMAIL = "BusTest@NaturalLinkExchange.com";
#else
		private const string PAYPAL_BASE_URL = "https://www.paypal.com/cgi-bin/webscr?";
		private const string PAYPAL_EMAIL = "Jason@NaturalLinkExchange.com";
#endif

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


		protected void Page_Load(object sender, EventArgs e)
		{
			_log.Debug("Loading account upgrade page");

            _master = (MainMaster)Master;
            header = _master.MasterStatusHeader;

            _userId = Global.GetCurrentUserId();

            getParameters();
            
			_db = Global.GetDbConnection();

            //Check if they want to upgrade right away
            showUpgradeFlagInfo();

			cmdPayNow.Click += new EventHandler(cmdPayNow_Click);
		}

        private void showUpgradeFlagInfo()
        {
            if (_upgradeFlag > 0)
            {
                pnlUpgradeFlag.Visible = true;
                lnkCancelUpgrade.Click += new EventHandler(lnkCancelUpgrade_Click);
            }
        }

        void lnkCancelUpgrade_Click(object sender, EventArgs e)
        {
            int siteId;
            Site s;

            siteId = header.GetSelectedSiteId();
            
            s = new Site(siteId);
            _db.PopulateSite(s);

            //Reset the upgrade flag
            s.UpgradeFlag = 0;
            _db.SaveSite(s);

            //Send them to the control panel
            Response.Redirect(ResolveUrl("~/Members/Control-Panel/"));
        }

		/// <summary>
		///		Creates a URL that can be used to initiate a payment through the PayPal
		///		system.
		/// </summary>
		/// <remarks>
		///		Before using the generated URL, you will need to save the corresponding
		///		subscription transaction, so that it can be referenced when the payment
		///		is received over the PayPal IPN system.
		/// </remarks>
		/// <param name="trans">
		///		The transaction that this payment is associated with.  This is what
		///		is used to look up the details of the transaction after the PayPal
		///		IPN is received.
		/// </param>
		/// <param name="monthly">
		///		If true, the subscription will be paid monthly, otherwise it will be
		///		paid yearly.
		/// </param>
		/// <param name="subscriptionPackage">
		///		The subscription package that the user is interested in modifying or creating.
		/// </param>
		/// <returns></returns>
		private static string buildPayPalUrl(SubscriptionTransaction trans, LinkPackage subscriptionPackage, bool modify)
		{
			UrlBuilder url;
			PayPalInterval ppi;

			url = new UrlBuilder(PAYPAL_BASE_URL);

			url.Parameters.AddParameter("cmd", "_xclick-subscriptions");

			url.Parameters.AddParameter("business", PAYPAL_EMAIL);
				
			//Add the payment number
			url.Parameters.AddParameter("invoice", trans.GuidId.ToString());

			//Add the subscription description
			url.Parameters.AddParameter("item_name", subscriptionPackage.FriendlyName);

			//Set the return URL
			url.Parameters.AddParameter("return", Nle.Website.ServerServices.PayPalIpn.PayPalIpnPage.GetLoadUrl());

			ppi = new PayPalInterval(trans.PaymentInterval);

			if(ppi.Quantity != 1)
				throw new ApplicationException("Cannot handle intervals other than 1");

			//Set the subscription price
			if(ppi.Units == IntervalUnits.Months)
			{
				url.Parameters.AddParameter("a3", trans.PaymentAmount);
				url.Parameters.AddParameter("p3", 1);
				url.Parameters.AddParameter("t3", "M");
			}
			else if(ppi.Units == IntervalUnits.Years)
			{
				url.Parameters.AddParameter("a3", trans.PaymentAmount);
				url.Parameters.AddParameter("p3", 1);
				url.Parameters.AddParameter("t3", "Y");
			}
			else
			{
				throw new ApplicationException("Unable to handle interval unit");
			}

			//Set the return page
			url.Parameters.AddParameter("return", PayPal_Return.GetLoadUrl(true));

			//Set the subscription to recurring
			url.Parameters.AddParameter("src", 1);

			//Check if we should only allow modifying
			if(modify)
				url.Parameters.AddParameter("modify", 2);
			
			//Don't allow notes (they're not available anyway)
			url.Parameters.AddParameter("no_note", 1);
			
			return url.ToString();
		}

		#region Web Form Designer generated code

		protected override void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}


		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}


		#endregion

		private void cmdPayNow_Click(object sender, EventArgs e)
		{
			string payPalUrl;
			SubscriptionTransaction trans;
			int selectedSiteId;
			LinkPackage selectedPackage;
			Subscription currentSubscription;
			bool modify;
			PayPalInterval ppi;
            bool discountEligible;

		    _log.Debug("User clicked the 'Pay Now' button");

			//Lookup the site and the current subscription
			selectedSiteId = header.GetSelectedSiteId();
			_log.DebugFormat("User has initiated a payment for site ID {0}.", selectedSiteId);
			currentSubscription = _db.GetSiteSubscription(selectedSiteId);
            
			if(currentSubscription == null || currentSubscription.PlanId == 1)
			{
				_log.DebugFormat("Site {0} is currently not on a paying plan", selectedSiteId);
				modify = false;
			}
			else
			{
				_log.DebugFormat("Site {0} is currently on plan {1}", selectedSiteId, currentSubscription.PlanId);
				modify = true; //we are modifying an existing subscription
			}

			_log.Debug("Generating a new subscription transaction");
			trans = new SubscriptionTransaction();
			trans.SiteId = selectedSiteId;

			//Determine the plan that is selected
			selectedPackage = _db.GetLinkPackage(getSelectedPlanId());
			trans.PlanId = selectedPackage.Id;

            //Determine if they are eligible for a discount
            discountEligible = _db.EligibleForMultiSiteDiscount(selectedSiteId, selectedPackage.Id);
		
			//Determine if they will pay monthly or yearly
			if(rdoMonthly.Checked)
			{
                if (discountEligible)
                    trans.PaymentAmount = selectedPackage.MonthlyPriceMultiple;
                else
				    trans.PaymentAmount = selectedPackage.MonthlyPrice;

				ppi = new PayPalInterval(1, IntervalUnits.Months);
			}
			else if(rdoYearly.Checked)
			{
                if (discountEligible)
                    trans.PaymentAmount = selectedPackage.YearlyPriceMultiple;
                else
				    trans.PaymentAmount = selectedPackage.YearlyPrice;

				ppi = new PayPalInterval(1, IntervalUnits.Years);
			}
			else
			{
				throw new ApplicationException("Cannot determine payment interval"); 
			}

			//Set the payment interval
			trans.PaymentInterval = ppi.ToString();
			_log.DebugFormat("Set payment interval to '{0}'", trans.PaymentInterval);

			_log.Debug("Saving the subscription transaction to the database");
			_db.SaveSubscriptionTransaction(trans);
				
			payPalUrl = buildPayPalUrl(trans, selectedPackage, modify);
			_log.DebugFormat("Generated PayPal URL '{0}', and now the user is being redirected", payPalUrl);

			//Redirect to the PayPal url where the user will complete the payment transaction
			//that has been opened in the database.
			Response.Redirect(payPalUrl);
		}

		private int getSelectedPlanId()
		{
			if(rdoSilver.Checked)
				return 2;
			else if(rdoGold.Checked)
				return 3;
			else
				throw new InvalidOperationException("Silver and Gold radio buttons were unchecked, that shouldn't be possible.");
		}

        private void getParameters()
        {
            string rawParam;
            bool wasParsed;

            rawParam = Request.QueryString[PARAM_UPGRADE_FLAG];
            wasParsed = int.TryParse(rawParam, out _upgradeFlag);
            if (!wasParsed)
                _upgradeFlag = 0;
        }
	}
}