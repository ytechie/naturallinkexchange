using System;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.UI;
using log4net;
using Nle.Components;
using Nle.Db.SqlServer;
using YTech.General.Web;
using YTech.General.Web.PayPal;

namespace Nle.Website.ServerServices.PayPalIpn
{
	/// <summary>
	///		PayPal IPN page.
	/// </summary>
	public partial class PayPalIpnPage : Page
	{
		/// <summary>
		///		The path of this page relative to the root with a trailing slash.
		/// </summary>
		public const string MY_PATH = "ServerServices/PayPalIpn/";
		/// <summary>
		///		The file name of this file.
		/// </summary>
		public const string MY_FILE_NAME = "";

		/// <summary>The URL to use for sandbox testing</summary>
		public const string SANDBOX_POST_URL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
		/// <summary>The URL to use for production use</summary>
		public const string PRODUCTION_POST_URL = "https://www.paypal.com/cgi-bin/webscr";

		/// <summary>
		///		The name of the session variable that stores the site id to make the payment for
		/// </summary>
		public const string SESSSION_SITE_ID = "Pay_SiteId";
		/// <summary>
		///		The name of the session variable that stores the id of the plan for make the payment for
		/// </summary>
		public const string SESSSION_PLAN_ID = "Pay_PlanId";
		/// <summary>
		///		The name of the session variable that stores a <see cref="bool"/>
		///		indicating if the payment is for a yearly pre-payment
		/// </summary>
		public const string SESSION_PLAN_YEARLY = "Pay_PlanYearly";

		private bool _sandbox = false;
		private Database _db;

		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		protected void Page_Load(object sender, EventArgs e)
		{
			_db = Global.GetDbConnection();

			_log.Info("PayPal instant payment notification (IPN) page called");

            if (Request.Form.AllKeys.Length == 0)
            {
                _log.Debug("No IPN values received, showing a message to the requestor");
                Response.Write("This page is not meant to be viewed");
                return;
            }
            else
            {
                //Log all received values for debugging/troubleshooting
                _log.Debug(Request.Form.ToString());
                _log.Info("PayPal IPN values logged, check debug log for complete listing");
            }

			try
			{
				validateFields();
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message) ;	
			}
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

		/// <summary>
		///		Verify with PayPal that the information we received is correct.
		/// </summary>
		private void validateFields()
		{
			string postString;
			string postUrl;

			if (_sandbox)
				postUrl = SANDBOX_POST_URL;
			else
				postUrl = PRODUCTION_POST_URL;

			_log.DebugFormat("PayPal IPN is using a post url of '{0}'", postUrl);

			//POST the data back to PayPal.
			postString = Request.Form.ToString() + "&cmd=_notify-validate";
			WebClient client = new WebClient();
			client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
			byte[] postByteArray = Encoding.ASCII.GetBytes(postString);
			byte[] responseArray = client.UploadData(postUrl, "POST", postByteArray);
			string response = Encoding.ASCII.GetString(responseArray);

			_log.DebugFormat("PayPal gave verification result of '{0}'", response);

			if (response == "VERIFIED")
				transactionVerified();
			else
				transactionInvalid();
		}

		/// <summary>
		///		Handles a transaction that has been successfully verified
		/// </summary>
		private void transactionVerified()
		{
			NameValueCollection formData;
			string transactionString;

			_log.Debug("Transaction was verified successfully");

			formData = Request.Form;
			transactionString = formData["txn_type"];

            _log.DebugFormat("Transaction string is '{0}'", transactionString);

			if (transactionString == "subscr_signup")
				processSignup(formData);
			else if (transactionString == "subscr_modify")
				processModification(formData);
			else if(transactionString == "subscr_payment")
				processPayment(formData);
			else if(transactionString == "subscr_cancel")
				processCancellation(formData);
			else
				throw new NotImplementedException("Unsupported Transaction Type: '" + transactionString + "'");
		}

		/// <summary>
		///		Processes the PayPal IPN post data.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="postData"></param>
		private void processSignup(NameValueCollection postData)
		{
			SubscriptionTransaction st;
			string transactionId;

			_log.Debug("Recieved subscription sign-up IPN post");

			//Get the transaction ID from the payment
			transactionId = postData["invoice"];
			_log.DebugFormat("Payment IPN was called for signup transaction '{0}", transactionId);
			
			//Load the transaction for the signup
			st = _db.GetSubscriptionTransaction(transactionId);
			if (st == null)
				throw new ApplicationException("Could not find a transaction matching the specified ID");

			//Make sure there hasn't been any PayPal tampering
			verifyTransactionData(st, postData);

			//Mark the subscription as processed
			st.Processed = true;

			_db.SaveSubscriptionTransaction(st);
			_log.DebugFormat("Saved subscription transaction {0} back to database", st.GuidId);
		}

		private void processPayment(NameValueCollection postData)
		{
			string transactionId;
			SubscriptionTransaction st;
			Payment p;
			Subscription sub;
            DateTime serverTime;

            //Retrieve the server time
            serverTime = _db.GetServerTime();

			//Get the transaction ID from the payment
			transactionId = postData["invoice"];
			_log.DebugFormat("Payment IPN was called for transaction '{0}", transactionId);

			if (transactionId == null || transactionId == "")
				throw new ApplicationException("A valid transaction ID could not be found in the IPN data");

			//Load the transaction for the payment
			st = _db.GetSubscriptionTransaction(transactionId);
			if (st == null)
				throw new ApplicationException("Could not find a transaction matching the specified ID");

			_log.Debug("Loaded transaction");
            
			p = new Payment();
			p.PostData = postData.ToString();
			p.Amount = st.PaymentAmount;
			p.PayPal_PayerEmail = postData["payer_email"];
			p.PayPal_SubscriptionId = postData["subscr_id"];
			p.PayPal_VerifySign = postData["verify_sign"];
			p.PayPal_PayerId = postData["payer_id"];
			p.PayPal_TransactionId = postData["txn_id"];
			p.PayPal_Fee = double.Parse(postData["payment_fee"]);
			p.Applied = false;
			p.SubscriptionTransactionId = st.GuidId;
			_db.SavePayment(p);
			_log.DebugFormat("Saved payment {0} before applying it", p.Id);
			
			sub = _db.GetSiteSubscription(st.SiteId);

            //If they don't have a subscription, assume free for right now
            if (sub == null)
            {
                _log.Debug("The customer is paying, but they never enabled their account, enabling now");
                sub = new Subscription();
                sub.StartTime = serverTime;
                sub.SiteId = st.SiteId;
                sub.PlanId = 1; //default to free
            }
			
			renewAccount(st, sub);

			//Check if their plan is changing
			if(st.PlanId != sub.PlanId)
			{
				_log.DebugFormat("The last payment changed site {0} from plan {1} to plan {2}", sub.SiteId, sub.PlanId, st.PlanId);
				sub.PlanId = st.PlanId;
			}

			_db.SaveSubscription(sub);
			_log.Debug("Subscription sucessfully saved back to the database");

			_log.Debug("Updating the saved payment to mark the payment as applied.") ;
			p.Applied = true;
			_db.SavePayment(p);
			_log.Debug("Payment sucessfully applied and marked as applied, cha-ching!");		
		}

		/// <summary>
		///		Verifies that tampering has not occurred by comparing the posted data
		///		with the data we have saved for the transaction.
		/// </summary>
		/// <param name="st"></param>
		/// <param name="postData"></param>
		private void verifyTransactionData(SubscriptionTransaction st, NameValueCollection postData)
		{
			double subAmount;
			string subInterval;

			subAmount = double.Parse(postData["amount3"]);
			subInterval = postData["period3"];

			//Verify that the PayPal subscription amount matches what we think it should be
			if(subAmount < st.PaymentAmount)
				throw new ApplicationException("User's subscription amount did not match what they are supposed to pay, possible fraud");

            //If they are paying more, I guess we'll let it go through!  That would be interesting.
            if (subAmount > st.PaymentAmount)
                _log.WarnFormat("PayPal subscription amount of ${0} exceeds the required ${1}, letting it go through", subAmount, st.PaymentAmount);

			if(subInterval != st.PaymentInterval)
				throw new ApplicationException("User's subscription interval did not match what it should be, possible fraud");
		}
		
		private void renewAccount(SubscriptionTransaction st, Subscription sub)
		{
			DateTime dbNow;
			PayPalInterval ppi;

			_log.DebugFormat("Renewing account for transaction '{0}'", st.GuidId.ToString());

			dbNow = _db.GetServerTime();

			ppi = new PayPalInterval(st.PaymentInterval);
            			
			//Add the interval to their subscription
			if(ppi.Units == IntervalUnits.Months)
				sub.EndTime = dbNow.AddMonths(ppi.Quantity);
			else if(ppi.Units == IntervalUnits.Years)
				sub.EndTime = dbNow.AddYears(ppi.Quantity);
			else
				throw new Exception("Cannot handle payment interval string: '" + st.PaymentInterval + "'");

			_db.SaveSubscription(sub);
		}

		private void processCancellation(NameValueCollection postData)
		{
			string transactionId;
			DateTime dbNow;
			SubscriptionTransaction st;
			Subscription sub;

			//Get the transaction ID from the payment
			transactionId = postData["invoice"];
			_log.DebugFormat("Payment IPN cancellation was called for transaction '{0}", transactionId);

			dbNow = _db.GetServerTime();

			//Load the transaction for the signup
			st = _db.GetSubscriptionTransaction(transactionId);
			if (st == null)
				throw new ApplicationException("Could not find a transaction matching the specified ID");

			sub = _db.GetSiteSubscription(st.SiteId);
			sub.EndTime = dbNow;

			_log.Debug("Account was cancelled, so the account expiration was set to now");
		}

		private void processModification(NameValueCollection postData)
		{
			SubscriptionTransaction st;
			string transactionId;

			_log.Debug("Recieved subscription modification IPN post");

			//Get the transaction ID from the payment
			transactionId = postData["invoice"];
			_log.DebugFormat("Payment IPN was called for transaction '{0}", transactionId);

			//Load the transaction
			st = _db.GetSubscriptionTransaction(transactionId);
			if (st == null)
				throw new ApplicationException("Could not find a transaction matching the specified ID");

			//check for tampering
			verifyTransactionData(st, postData);

			//Note: The modification will not take effect until a payment is posted
		}

		private void transactionInvalid()
		{
			_log.Warn("Transaction was invalid!  Tampering possible!");
		}

		/// <summary>
		///		Gets the URL that can be used to load this page.
		/// </summary>
		/// <returns></returns>
		public static string GetLoadUrl()
		{
			UrlBuilder url;
			
			url = new UrlBuilder(Global.VirtualDirectory + MY_PATH + MY_FILE_NAME);

			return url.ToString();
		}

	}
}