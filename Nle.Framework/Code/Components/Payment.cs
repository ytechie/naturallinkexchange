using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a payment by a user, or a placeholder
	///		for a payment that will be made shortly.
	/// </summary>
	public class Payment
	{
		private bool _hasId;

		private int _id;
		private double _amount;
		private bool _applied;
		private string _postData;
		private Guid _subscriptionTransactionId;
		private string _payPal_SubscriptionId;
		private string _payPal_VerifySign;
		private string _payPal_TransactionId;
		private string _payPal_PayerId;
		private double _payPal_Fee;
		private string _payPal_PayerEmail;
		
		#region Public Properties

		/// <summary>
		///		If true, this <see cref="Payment"/> is saved to the
		///		database and can be retrieved with the <see cref="Id"/>.
		/// </summary>
		public bool HasId
		{
			get { return _hasId; }
			set { _hasId = value; }
		}

		/// <summary>
		///		The unique identifier of this particular
		///		payment in the database.
		/// </summary>
		public int Id
		{
			get
			{
				if(!_hasId)
					throw new InvalidOperationException("Cannot read Id when there is none.");

				return _id;
			}
			set
			{
				_id = value;
				_hasId = true;
			}
		}

		/// <summary>
		///		The amount of money they payment is for.
		/// </summary>
		[FieldMapping("Amount")]
		public double Amount
		{
			get { return _amount; }
			set { _amount = value; }
		}

		/// <summary>
		///		If true, the payment has been processed and applied
		///		to the action that it was supposed to be for.
		/// </summary>
		[FieldMapping("Applied")]
		public bool Applied
		{
			get	{	return _applied;	}
			set	{ _applied = value;	}
		}

		/// <summary>
		///		The name value pairs that were recieved from
		///		the payment system post.  Right now, this would
		///		just be the PayPal IPN post.
		/// </summary>
		public string PostData
		{
			get	{	return _postData;	}
			set	{	_postData = value;	}
		}

		/// <summary>
		///		Gets or sets the <see cref="Guid"/> of the 
		///		<see cref="SubscriptionTransaction"/> that this payment is associated with, if any.
		/// </summary>
		/// <remarks>
		///		Right now, this will always be set because payments are only made for
		///		subscriptions using subscription transactions.
		/// </remarks>
		public Guid SubscriptionTransactionId
		{
			get	{	return _subscriptionTransactionId;	}
			set	{	_subscriptionTransactionId = value;	}
		}

		/// <summary>
		///		The subscription Id from the paypal transaction.
		/// </summary>
		[FieldMapping("PayPal_SubscriptionId")]
		public string PayPal_SubscriptionId
		{
			get { return _payPal_SubscriptionId; }
			set { _payPal_SubscriptionId = value; }
		}

		/// <summary>
		///		The PayPal verification signature of the payment.
		/// </summary>
		[FieldMapping("PayPal_VerifySign")]
		public string PayPal_VerifySign
		{
			get { return _payPal_VerifySign; }
			set { _payPal_VerifySign = value; }
		}

		/// <summary>
		///		The PayPal transaction Id of the payment.
		/// </summary>
		[FieldMapping("PayPal_TransactionId")]
		public string PayPal_TransactionId
		{
			get { return _payPal_TransactionId; }
			set { _payPal_TransactionId = value; }
		}

		/// <summary>
		///		The PayPal payer Id of the person sending the payment.
		/// </summary>
		[FieldMapping("PayPal_PayerId")]
		public string PayPal_PayerId
		{
			get { return _payPal_PayerId; }
			set { _payPal_PayerId = value; }
		}

		/// <summary>
		///		The fee that PayPal is charging for this transaction.
		/// </summary>
		[FieldMapping("PayPal_Fee")]
		public double PayPal_Fee
		{
			get { return _payPal_Fee; }
			set { _payPal_Fee = value; }
		}

		/// <summary>
		///		The email of the person making the payment.
		/// </summary>
		[FieldMapping("PayPal_PayerEmail")]
		public string PayPal_PayerEmail
		{
			get { return _payPal_PayerEmail; }
			set { _payPal_PayerEmail = value; }
		}

		#endregion
    
		/// <summary>
		///		Creates a new instance of the <see cref="Payment"/> class.
		/// </summary>
		public Payment()
		{
			_hasId = false;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="Payment"/> class.
		/// </summary>
		public Payment(int paymentId)
		{
			_hasId = true;
		}
	}
}
