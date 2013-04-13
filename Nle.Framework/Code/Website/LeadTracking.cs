using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Nle.Website
{
    /// <summary>
    ///     This class is used to encapsulate the functionality needed
    ///     to track ad leads.
    /// </summary>
    public class LeadTracking
    {
        /// <summary>
        ///     The name of the cookie used to store the source of the lead.
        /// </summary>
        public const string COOKIE_LEAD_KEY = "LeadSourceKey";

        /// <summary>
        ///     Saves the lead key to the specified cookie collection that
        ///     will be sent to the user.
        /// </summary>
        /// <param name="responseCookies">
        ///     The <see cref="HttpCookieCollection"/> that the cookied will
        ///     be stored in.  This usually comes from the <see cref="HttpResponse"/> object.
        /// </param>
        /// <param name="leadKey">
        ///     The unique key that identifies a lead (aka ad) source.
        /// </param>
        public static void SaveLead(HttpCookieCollection responseCookies, int leadKey)
        {
            HttpCookie leadCookie;

            leadCookie = new HttpCookie(COOKIE_LEAD_KEY);
            leadCookie.Value = leadKey.ToString();

            responseCookies.Add(leadCookie);
        }

        /// <summary>
        ///     Retrieves the lead key stored by the <see cref="SaveLead"/> method.
        /// </summary>
        /// <param name="requestCookies">
        ///     The <see cref="HttpCookieCollection"/> from the current <see cref="HttpRequest"/>
        ///     object.  This is where the cookie value will be read from.
        /// </param>
        /// <returns>
        ///     The lead key stored by the <see cref="SaveLead"/>, unless a cookie was not
        ///     saved.  In that case, null is returned.
        /// </returns>
        public static int GetLead(HttpCookieCollection requestCookies)
        {
            HttpCookie leadCookie;
            string cookieVal;
            int leadKey;

            leadCookie = requestCookies[COOKIE_LEAD_KEY];

            if (leadCookie == null)
                return 0;

            cookieVal = leadCookie.Value;
            if (string.IsNullOrEmpty(cookieVal))
                return 0;

            if (int.TryParse(cookieVal, out leadKey))
                return leadKey;
            else
                return 0;
        }
    }
}
