using Nle.Components;
using Nle.Db.SqlServer;

namespace Nle.Website.Members.Payment_Settings
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Displays a table of the different membership levels and
	///		their details.
	/// </summary>
	public partial class LinkPackagesTable : System.Web.UI.UserControl
	{
		protected DataGrid dgPackages;

		private int _highlightPlan = -1;
		private Database _db;

		/// <summary>
		///		Sets the plan that should be highlighted in the table
		/// </summary>
		public int HighlightPlan
		{
			set { _highlightPlan = value; }
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			_db = Global.GetDbConnection();

			this.PreRender += new EventHandler(LinkPackagesTable_PreRender);
		}

		private void showLinkPackages()
		{
			LinkPackage[] linkPackages;
			TableRow currRow;

			linkPackages = _db.GetAllLinkPackages();
			
			currRow = addFeatureRow("");
			currRow.CssClass = "linkPackagesTable_header";
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.FriendlyName, currLP.Id);

			currRow = addFeatureRow("Monthly Price");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.MonthlyPrice.ToString("$0.00"), currLP.Id);

			currRow = addFeatureRow("Annual Price (Save 33%!!) (if purchased yearly)");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.YearlyPrice.ToString("$0.00"), currLP.Id);

            currRow = addFeatureRow("Each Additional Site (Monthly)");
            foreach (LinkPackage currLP in linkPackages)
                addFeatureText(currRow, currLP.MonthlyPriceMultiple.ToString("$0.00"), currLP.Id);

			currRow = addFeatureRow("Link Ratio");
			foreach(LinkPackage currLP in linkPackages)
			{
				string ratioText;

				if(currLP.OutInRatio == 2)
					ratioText = "2 outbound links for each inbound";
				else if(currLP.OutInRatio == 1)
					ratioText = "1 inbound for each outbound";
				else if(currLP.OutInRatio == 0.5)
					ratioText = "2 inbound links for each outbound link";
				else
					ratioText = currLP.OutInRatio.ToString() + ":1";

				addFeatureText(currRow, ratioText, currLP.Id);
			}

			currRow = addFeatureRow("Link Anchor Text Variations");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.AnchorCount.ToString(), currLP.Id);

			currRow = addFeatureRow("Articles text variations");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.ArticlesPerGroupDisplayText.ToString(), currLP.Id);

			currRow = addFeatureRow("Link addition rate");
			foreach(LinkPackage currLP in linkPackages)
			{
				string rateText;

				//Todo: These strings should be stored in the database
				if(currLP.Id == 1)
					rateText = "1 every 10 days";
				else if(currLP.Id == 2)
					rateText = "1-3 every 5 days";
				else if(currLP.Id == 3)
					rateText = "1-6 nearly every day";
				else
					rateText = "Contact Support";

				addFeatureText(currRow, rateText, currLP.Id);
			}

			currRow = addFeatureRow("Number of sites allowed to ban");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.BansDisplayText, currLP.Id);

			currRow = addFeatureRow("Number of RSS feeds per page");
			foreach(LinkPackage currLP in linkPackages)
				addFeatureText(currRow, currLP.MinFeedsPerLinkPage.ToString() + " - " + currLP.MaxFeedsPerLinkPage, currLP.Id);

            currRow = addFeatureRow("");
            addPayPalImageRow(currRow);

            currRow = addFeatureRow("Redirect Linking");
            addDomainLinkingRow(currRow);
		}

		private TableRow addFeatureRow(string rowName)
		{
			TableRow newRow;
			TableCell currCell;

			newRow = new TableRow();
			tblPackages.Rows.Add(newRow);

			currCell = new TableCell();
			currCell.Text = rowName;
			currCell.CssClass = "linkPackagesTable_featureColumn";
			newRow.Cells.Add(currCell);

			return newRow;
		}

		private void addFeatureText(TableRow row, string featureText, int linkPackageId)
		{
			TableCell newCell;

			newCell = new TableCell();
			newCell.Text = featureText;

			if(_highlightPlan != -1 && _highlightPlan == linkPackageId)
				newCell.BackColor = Color.Yellow;

			row.Cells.Add(newCell);
		}

        private void addPayPalImageRow(TableRow row)
        {
            TableCell newCell;
            System.Web.UI.WebControls.Image paypalLogo;

            //Free column
            newCell = new TableCell();
            row.Cells.Add(newCell);

            //Silver column
            newCell = new TableCell();
            row.Cells.Add(newCell);
            paypalLogo = new System.Web.UI.WebControls.Image();
            paypalLogo.ImageUrl = ResolveUrl("~/Images/PayPalPaymentsLogo.gif");
            newCell.Controls.Add(paypalLogo);

            //Gold column
            newCell = new TableCell();
            row.Cells.Add(newCell);
            paypalLogo = new System.Web.UI.WebControls.Image();
            paypalLogo.ImageUrl = ResolveUrl("~/Images/PayPalPaymentsLogo.gif");
            newCell.Controls.Add(paypalLogo);
        }

        private void addDomainLinkingRow(TableRow row)
        {
            TableCell newCell;

            //Blank for free
            newCell = new TableCell();
            row.Cells.Add(newCell);
            newCell.Text = "N/A";

            //Blank for silver
            newCell = new TableCell();
            row.Cells.Add(newCell);
            newCell.Text = "N/A";

            //Describe it in gold
            newCell = new TableCell();
            row.Cells.Add(newCell);
            newCell.Text = "Redirect Linking-Allows 2 URLs, 1 gets the links 1 gives (must be approved-no spam)";
        }


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Load += new EventHandler(Page_Load);
		}
		#endregion

		private void LinkPackagesTable_PreRender(object sender, EventArgs e)
		{
			showLinkPackages();
		}
	}
}
