using System.Web;
using System.Web.Optimization;

namespace LALoDep
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            BundleTable.EnableOptimizations = true;
            bundles.Add(new StyleBundle("~/assets/css/styles").Include(
                /* Basic Styles */
                    "~/assets/css/bootstrap.css",
                    "~/assets/css/font-awesome.css",
                    "~/assets/css/weather-icons.css",
                    "~/assets/css/demo.css",
                    "~/assets/css/typicons.css",
                    "~/assets/css/mentalhealth.css",
                    "~/assets/css/animate.css",
                    "~/assets/css/beyond.css",
                    "~/assets/other/DataTable-1.10.10/css/dataTables.bootstrap.css"
    ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
               "~/assets/css/bootstrap-table.css",
               "~/assets/css/custom.css",
               "~/assets/css/zoom.css"
            ));
            bundles.Add(new StyleBundle("~/Content/css/mobile").Include(
               "~/assets/css/custom-mobile.css"
             ));
            bundles.Add(new ScriptBundle("~/assets/js/jquery").Include(

         
           "~/assets/js/jquery-2.0.3.min.js" 
        
       ));
            bundles.Add(new ScriptBundle("~/assets/js/scripts").Include(

                "~/assets/js/skins.js",
             "~/assets/js/jquery-2.0.3.min.js" ,
                "~/assets/js/bootstrap.min.js",
                "~/assets/js/datetime/bootstrap-datepicker.js",
                "~/assets/js/datetime/bootstrap-timepicker.js",
                "~/assets/js/datetime/moment.js",
                "~/assets/js/datetime/daterangepicker.js",
                "~/assets/js/toastr/toastr.js",
                "~/assets/js/fuelux/treeview/tree-custom.min.js",
                "~/assets/js/slimscroll/jquery.slimscroll.min.js",
                "~/assets/js/beyond.js",
                "~/assets/js/bootbox/bootbox.js",
                  "~/assets/js/editors/summernote/summernote.js",
              "~/assets/js/textarea/jquery.autosize.js"
            ));
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                "~/assets/other/jquery.maskedinput-1.3.1.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/ApplicationScripts/DataEntry/Mental.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/assets/other/DataTable-1.10.10/js/jquery.dataTables.js",
                "~/assets/other/DataTable-1.10.10/js/dataTables.tableTools.js",
                "~/assets/other/DataTable-1.10.10/js/dataTables.bootstrap.js",
                 "~/assets/other/DataTable-1.10.10/js/dataTables.scroller.min.js",
                "~/assets/other/mousetrap.js",
                "~/assets/other/bootstrap-table.js",
                "~/assets/other/simpleStorage.js",
                "~/assets/other/jquery.mask.js",
                  "~/assets/pages/Encryption/aes.js",
                "~/assets/other/custom.js",
                 "~/assets/other/device.min.js"

            ));


            bundles.Add(new ScriptBundle("~/FileUpload/js").Include(
              "~/assets/js/jfu/js/vendor/jquery.ui.widget.js",
             "~/assets/js/jfu/js/tmpl.min.js",
             "~/assets/js/jfu/js/jquery.iframe-transport.js",
              "~/assets/js/jfu/js/jquery.fileupload.js",
              "~/assets/js/jfu/js/jquery.fileupload-process.js",
              "~/assets/js/jfu/js/jquery.fileupload-validate.js",
              "~/assets/js/jfu/js/jquery.fileupload-ui.js"

          ));
            bundles.Add(new StyleBundle("~/FileUpload/css").Include(
             "~/assets/js/jfu/css/jquery.fileupload-ui.css",
             "~/assets/js/jfu/css/jquery.fileupload.css",
             "~/assets/js/zoom.css"
          ));

            bundles.Add(new ScriptBundle("~/Autocomplete/js").Include("~/assets/js/jquery.autocomplete.js"));
            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862

            bundles.Add(new ScriptBundle("~/DeviceDetector/js").Include("~/Scripts/device.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/tutorial").Include("~/Scripts/Application-Script/tutorial/jtutorial.js"));
            bundles.Add(new StyleBundle("~/Scripts/tutorial/css").Include("~/Scripts/Application-Script/tutorial/jtutorial.css"));
        }
    }
}
