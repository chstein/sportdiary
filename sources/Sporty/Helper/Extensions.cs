using MvcContrib.Pagination;
using Sporty.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sporty.Helper
{
    public static class Extensions
    {
        public static bool IsRelease(this HtmlHelper helper)
        {
#if DEBUG
            return false;
#else
            return true;
#endif
        }

        /// <summary>
        /// Creates a pager component using the item from the viewdata with the specified key as the datasource.
        /// </summary>
        /// <param name="helper">The HTML Helper</param>
        /// <param name="viewDataKey">The viewdata key</param>
        /// <returns>A Pager component</returns>
        public static BootstrapPager BootstrapPager(this HtmlHelper helper, string viewDataKey)
        {
            var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IPagination;

            if (dataSource == null)
            {
                throw new InvalidOperationException(string.Format("Item in ViewData with key '{0}' is not an IPagination.",
                                                                  viewDataKey));
            }

            return helper.BootstrapPager(dataSource);
        }

        /// <summary>
        /// Creates a pager component using the specified IPagination as the datasource.
        /// </summary>
        /// <param name="helper">The HTML Helper</param>
        /// <param name="pagination">The datasource</param>
        /// <returns>A Pager component</returns>
        public static BootstrapPager BootstrapPager(this HtmlHelper helper, IPagination pagination)
        {
            return new BootstrapPager(pagination, helper.ViewContext);
        }
    }
}