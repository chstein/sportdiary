using Sporty.DataModel;
using System;

namespace Sporty.Business.Helper
{
    public static class StringHelper
    {
        public static string GetShortname(Zone item)
        {
            if (item == null)
            {
                return String.Empty;
            }
            else if (item.Name != null && item.Name.Length > 15)
            {
                return item.Name.Substring(0, 15) + "..";
            }
            else
            {
                return item.Name;
            }
        }
    }
}