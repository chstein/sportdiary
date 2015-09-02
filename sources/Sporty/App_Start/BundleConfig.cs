using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Sporty.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/leaflet", "http://cdn.leafletjs.com/leaflet-0.6.4/leaflet.js")
                .Include("~/Scripts/leaflet-0.6.4.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/leafAndGmaps").Include("~/Scripts/Google.js"));

            bundles.Add(new ScriptBundle("~/bundles/main").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/jquery-ui-{version}.js",
                         "~/Scripts/jquery.ui.datepicker-de.js",
                         "~/Scripts/jquery.unobtrusive-ajax.js",
                //"~/Scripts/jquery.tmpl.js",
                            "~/Scripts/globalize.js",
                              "~/Scripts/globalize.culture*",
                        "~/Scripts/bootstrap*",
                        "~/Scripts/bootbox.js",
                          "~/Scripts/jquery.validate.js",
                          "~/Scripts/jquery.ddslick.js",
                          "~/Scripts/global.js",
                          "~/Scripts/jquery-ui-timepicker-addon.js",

                           "~/Scripts/highcharts.js",
                           "~/Scripts/jquery.ssp-oneclick.js",
                           "~/Scripts/leaflet-0.6.4.js",
                           //"http://cdn.leafletjs.com/leaflet-0.6.4/leaflet.js",
                           //"http://maps.google.com/maps/api/js?v=3&sensor=false",
                           //"~/Scripts/Google.js",
                            "~/Scripts/knockout-{version}.js",
                            "~/Scripts/knockout-sortable.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryvalidate").Include(
                        "~/Scripts/jquery.validate.js"));


            bundles.Add(new Bundle("~/bundles/model").Include(
                 "~/Scripts/app/calendar.bindings.js",
                 "~/Scripts/app/calendar.datacontext.js",
                 "~/Scripts/app/calendar.model.js",
               "~/Scripts/app/calendar.viewmodel.js"));

            bundles.Add(new ScriptBundle("~/bundles/upload").Include(
                "~/Scripts/fileuploader.js"));
            bundles.Add(new StyleBundle("~/content/upload").Include(
                "~/Content/fileuploader.css"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/style.css"));

            bundles.Add(new StyleBundle("~/content/maincss").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/style.css",
                "~/Content/jquery-ui-1.8.5.custom.css",
                "~/Content/leaflet.css",
                "~/Content/ssp-oneclick.css"));
            //"~/Content/font-awesome.css"));

            //bundles.Add(new StyleBundle("~/content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}