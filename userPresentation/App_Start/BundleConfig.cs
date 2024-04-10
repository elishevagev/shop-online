﻿using System.Web;
using System.Web.Optimization;

namespace userPresentation
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new Bundle("~/bundles/complements").Include(
            //           "~/Scripts/DataTables/jquery.dataTables.js",
            //           "~/Scripts/DataTables/dataTables.responsive.js",
            //           "~/Scripts/jquery.validate.js",
            //           "~/Scripts/scripts.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js",
                      "~/Scripts/fontawesome/all.min/js",
                      "~/Scripts/loadingoverlay.min.js",
                      "~/Scripts/sweetalert.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css",

                "~/Content/sweetalert.css",
                "~/Content/DataTables/css/jquery.dataTables.css",
                "~/Content/DataTables/css/responsive.dataTables.css"
                ));
        }
    }
}
