using System;
using System.Web.UI.WebControls;

namespace Sporty.Helper
{
    public class UrlTableHelper
    {
        private readonly Uri uri;
        public int? Page;
        public int? SortColumn;
        public SortDirection SortDirection;

        /// <summary>
        /// Parse complete Url with params like page and sorting
        /// </summary>
        /// <param name="uri"></param>
        public UrlTableHelper(Uri uri)
        {
            this.uri = uri;
            Initialize();
        }

        private void Initialize()
        {
            if (uri.Query.Length < 1) return;
            string[] queryParts = uri.Query.ToLower().Substring(1).Split("&".ToCharArray(),
                                                                         StringSplitOptions.RemoveEmptyEntries);
            foreach (string queryPart in queryParts)
            {
                string[] parts = queryPart.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (parts[0] == "page")
                {
                    int temp;
                    if (Int32.TryParse(parts[1], out temp))
                        Page = temp;
                }
                else if (parts[0] == "sort")
                {
                    int temp;
                    if (Int32.TryParse(parts[1], out temp))
                        SortColumn = temp;
                }
                else if (parts[0] == "dir")
                {
                    SortDirection = parts[1] == "desc" ? SortDirection.Descending : SortDirection.Ascending;
                }
            }
        }
    }
}