using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace DocumentApp.API
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Assets/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                                         "~/Assets/Scripts/jquery.unobtrusive*",
                                         "~/Assets/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                                         "~/Assets/Scripts/knockout-{version}.js",
                                         "~/Assets/Scripts/knockout.validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                                         "~/Assets/Scripts/sammy-{version}.js",
                                         "~/Assets/Scripts/app/common.js",
                                         "~/Assets/Scripts/app/app.datamodel.js",
                                         "~/Assets/Scripts/app/app.viewmodel.js",
                                         "~/Assets/Scripts/app/home.viewmodel.js",
                                         "~/Assets/Scripts/app/_run.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                                         "~/Assets/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                                         "~/Assets/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Assets/Styles/css").Include(
                                        "~/Assets/Styles/bootstrap.css",
                                        "~/Assets/Styles/Site.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
