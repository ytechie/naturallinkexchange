using System;
using YTech.General.DataMapping;

namespace Nle.Components
{
	/// <summary>
	///		Represents a subscription to a <see cref="LinkPackage"/>
	/// </summary>
	public class Subscription
	{
		private int _id;
		private int _siteId;
		private int _planId;
		private DateTime _startTime;
		private DateTime _endTime;
		private string _payPal_SubscriptionId;

		private bool _idSet;

		#region Public Properties

		/// <summary>
		///		The unique identifier
		/// </summary>
		[FieldMapping("Id")]
		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				_idSet = true;
			}
		}

		/// <summary>
		///		The ID of the <see cref="Site"/> that this
		///		subscription is for.
		/// </summary>
		[FieldMapping("SiteId")]
		public int SiteId
		{
			get { return _siteId; }
			set { _siteId = value; }
		}

		/// <summary>
		///		The ID of the <see cref="LinkPackage"/> that
		///		this subscription is for.
		/// </summary>
		[FieldMapping("PlanId")]
		public int PlanId
		{
			get { return _planId; }
			set { _planId = value; }
		}

		/// <summary>
		///		The <see cref="DateTime"/> that this subscription
		///		starts on.
		/// </summary>
		[FieldMapping("StartTime")]
		public DateTime StartTime
		{
			get { return _startTime; }
			set { _startTime = value; }
		}

		/// <summary>
		///		The <see cref="DateTime"/> that this subscription ends on.
		/// </summary>
		[FieldMapping("EndTime")]
		public DateTime EndTime
		{
			get { return _endTime; }
			set { _endTime = value; }
		}

		/// <summary>
		///		The subscription ID that was assigned from the PayPal system.
		/// </summary>
		[FieldMapping("PayPal_SubscriptionId")]
		public string PayPal_SubscriptionId
		{
			get { return _payPal_SubscriptionId; }
			set { _payPal_SubscriptionId = value; }
		}

		/// <summary>
		///		Keeps track of whether or not the ID has been set.
		/// </summary>
		public bool IdSet
		{
			get { return _idSet; }
		}

		#endregion

		/// <summary>
		///		Creates a new instance of the <see cref="Subscription"/> class.
		/// </summary>
		public Subscription()
		{
			_idSet = false;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="Subscription"/> class.
		/// </summary>
		/// <param name="id"></param>
		public Subscription(int id)
		{
			_id = id;
			_idSet = true;
		}
	}
}
