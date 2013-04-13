using System;
using System.Collections.Generic;
using System.Text;

namespace Nle.LinkPage
{
    /// <summary>
    ///     The different types of ways that link pages can be put
    ///     on to a a customers site.
    /// </summary>
    public enum GenerateTypes
    {
        FtpUpload = 1,
        PhpScript = 2,
        NetScript = 3,
        NetControlScript = 4
    }
}
