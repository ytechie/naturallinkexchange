using System;
using System.Collections.Generic;
using System.Text;
using YTech.General.DataMapping;

namespace Nle.Components.ABTesting
{
    /// <summary>
    ///     Represents a rotator key/content key pair.  In other words,
    ///     an instance of this object can tell us the content that was
    ///     displayed to the user by a particular
    ///     <see cref="YTech.General.Web.Controls.ContentRotation.ServerContentRotator"/>.
    /// </summary>
    public class ABContent
    {
        private int _id;
        private int _userId;
        private string _rotatorKey;
        private string _contentKey;
        private DateTime _timestamp;
        private Actions _action;

        private bool _hasId;

        /// <summary>
        ///     Creates a new instance of the <see cref="ABContentShown"/> class.
        /// </summary>
        public ABContent()
        {
            _hasId = false;
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="ABContentShown"/> class.
        /// </summary>
        /// <param name="id">
        ///     The unique ID of this object in the database.
        /// </param>
        public ABContent(int id)
        {
            _id = id;
            _hasId = true;
        }

        /// <summary>
        ///     Gets a boolean indicating if the ID of this object
        ///     has been set yet.
        /// </summary>
        public bool HasId
        {
            get { return _hasId; }
        }

        #region Databound Properties

        /// <summary>
        ///     The unique ID of this object in the database.
        /// </summary>
        [FieldMapping("Id")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        ///     The ID of the user that saw the content and then performed some action.
        /// </summary>
        [FieldMapping("UserId")]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        /// <summary>
        ///     The key to uniqely identify a specific rotator.
        /// </summary>
        [FieldMapping("RotatorKey")]
        public string RotatorKey
        {
            get { return _rotatorKey; }
            set { _rotatorKey = value; }
        }

        /// <summary>
        ///     The key used to identify a content panel that was displayed.
        /// </summary>
        [FieldMapping("ContentKey")]
        public string ContentKey
        {
            get { return _contentKey; }
            set { _contentKey = value; }
        }

        /// <summary>
        ///     The time that the user performed the action.
        /// </summary>
        [FieldMapping("Timestamp")]
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        
        /// <summary>
        ///     The ID of the action that the user performed that caused
        ///     the content keys to be written to the database. The meaning
        ///     of the ID's is currently only stored in code, not the database.
        /// </summary>
        [FieldMapping("ActionId")]
        public Actions Action
        {
            get { return _action; }
            set { _action = value; }
        }

        #endregion
    }
}
