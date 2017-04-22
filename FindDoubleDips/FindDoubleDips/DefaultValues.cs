using System;
using System.Runtime.InteropServices;
using System.Globalization;

namespace DoenaSoft.DVDProfiler.FindDoubleDips
{
    [ComVisible(false)]
    public class DefaultValues
    {
        public Boolean CheckOnlyTitles = false;

        public Boolean IgnoreProductionYear = false;

        public Boolean IgnoreWishListTitles = false;

        public Boolean IgnoreTelevisonTitles = false;

        public Boolean IgnoreSameDatePurchases = true;

        public Boolean IgnoreBoxSetContents = true;

        public Int32 UiCulture = CultureInfo.CurrentUICulture.LCID;
    }
}