using System;
using System.Collections.Generic;
using System.Text;
using Nle.LinkPage.Spider;
using YTech.General.DataMapping;

namespace Nle.Components
{
    /// <summary>
    ///     Represents the recorded status of a link page check
    ///     by the <see cref="SiteSpider"/>
    /// </summary>
    public class LinkPageStatus
    {
        private int _id;
        private DateTime _checkedOn;
        private bool _valid;
        private int _siteId;

        private bool _hasId = false;

        /// <summary>
        ///     Creates a new instance of the <see cref="LinkPageStatus"/> class.
        /// </summary>
        public LinkPageStatus()
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="LinkPageStatus"/> class, and
        ///     assigns it a specific ID from the database.
        /// </summary>
        public LinkPageStatus(int id)
        {
            Id = id;
        }

        /// <summary>
        ///     The unique ID of this status.
        /// </summary>
        [FieldMapping("Id")]
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                _hasId = true;
            }
        }

        /// <summary>
        ///     If true, this object has been assigned a unique ID.
        /// </summary>
        [FieldMapping("HasId")]
        public bool HasId
        {
            get { return _hasId; }
        }

        /// <summary>
        ///     The time that the site was spidered, as represented
        ///     by this object instance.
        /// </summary>
        [FieldMapping("CheckedOn")]
        public DateTime CheckedOn
        {
            get { return _checkedOn; }
            set { _checkedOn = value; }
        }

        /// <summary>
        ///     If true, the site had as valid link page at the time
        ///     of checking.
        /// </summary>
        [FieldMapping("Valid")]
        public bool Valid
        {
            get { return _valid; }
            set { _valid = value; }
        }

        /// <summary>
        ///     The unique ID of the site that was checked.
        /// </summary>
        [FieldMapping("SiteId")]
        public int SiteId
        {
            get { return _siteId; }
            set { _siteId = value; }
        }

        public override string ToString()
        {
            return string.Format("LinkPageStatus: Id={0}, CheckedOn={1}, Valid={2}, SiteId={3}", _id, _checkedOn, _valid, _siteId);
        }
    }
}
