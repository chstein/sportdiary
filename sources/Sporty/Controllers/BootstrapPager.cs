using MvcContrib.Pagination;
using MvcContrib.UI.Pager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sporty.Controllers
{
    public class BootstrapPager : Pager
    {
        private readonly IPagination _pagination;
        private readonly ViewContext _viewContext;
        private string _paginationFormat = "Zeige {0} - {1} von {2} ";
        private string _paginationSingleFormat = "Zeige {0} von {1} ";
        private string _paginationFirst = "Beginn";
        private string _paginationPrev = "Zurück";
        private string _paginationNext = "Vor";
        private string _paginationLast = "Letzte";
        private string _pageQueryName = "page";
        private Func<int, string> _urlBuilder;


        public BootstrapPager(IPagination pagination, ViewContext context) : base(pagination, context   )
        {
            base.Format("Zeige {0} - {1} von {2}");
            base.SingleFormat("Zeige {0} von {1}");
            _pagination = pagination;
            _viewContext = context;

            _urlBuilder = CreateDefaultUrl;
        }

        protected override void RenderRightSideOfPager(System.Text.StringBuilder builder)
        {
            builder.Append("<ul>");
            //If we're on page 1 then there's no need to render a link to the first page. 
            if (_pagination.PageNumber == 1)
            {
                builder.Append(CreatePageLink(1, _paginationFirst, "disabled"));
            }
            else
            {
                builder.Append(CreatePageLink(1, _paginationFirst));
            }

            
            //If we're on page 2 or later, then render a link to the previous page. 
            //If we're on the first page, then there is no need to render a link to the previous page. 
            if (_pagination.HasPreviousPage)
            {
                builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev));
            }
            else
            {
                builder.Append(CreatePageLink(_pagination.PageNumber - 1, _paginationPrev, "disabled"));
            }

            
            //Only render a link to the next page if there is another page after the current page.
            if (_pagination.HasNextPage)
            {
                builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext));
            }
            else
            {
                builder.Append(CreatePageLink(_pagination.PageNumber + 1, _paginationNext, "disabled"));
            }

           
            int lastPage = _pagination.TotalPages;

            //Only render a link to the last page if we're not on the last page already.
            if (_pagination.PageNumber < lastPage)
            {
                builder.Append(CreatePageLink(lastPage, _paginationLast));
            }
            else
            {
                builder.Append(CreatePageLink(lastPage, _paginationLast, "disabled"));
            }
            builder.Append("</ul>");
        }

        private string CreatePageLink(int pageNumber, string text, string cssClass="")
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            builder.MergeAttribute("href", _urlBuilder(pageNumber));
            var li = new TagBuilder("li");
            li.AddCssClass(cssClass);
            li.InnerHtml = builder.ToString(TagRenderMode.Normal);
            
            return li.ToString(TagRenderMode.Normal);
        }

        private string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            foreach (var key in _viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
            {
                routeValues[key] = _viewContext.RequestContext.HttpContext.Request.QueryString[key];
            }

            routeValues[_pageQueryName] = pageNumber;

            var url = UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes, _viewContext.RequestContext, true);
            return url;
        }
    }
}