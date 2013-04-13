using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Data;

namespace Nle.Components.Reporting
{
    /// <summary>
    /// An interface that provides functionality for Natural Link Exchange reporting.
    /// </summary>
    public interface INleReport
    {
        /// <summary>
        /// Gets and Sets the title of the report.
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// Gets and Sets the description of the report.
        /// </summary>
        string Description { get; set; }
        /// <summary>
        /// Passes the data needed to display the report to the report control.
        /// </summary>
        /// <param name="data">The data to be displayed in the report.</param>
        /// <returns>True if the data is valid, otherwise false.</returns>
        bool SetData(DataSet data);
        /// <summary>
        /// Gets the control that represents the report.
        /// </summary>
        /// <returns>The control that displays the data for the report.</returns>
        Control GetControl();
    }
}
