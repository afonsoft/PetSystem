using System;
using System.Threading;
using System.Web.UI;

namespace Afonsoft.Petz
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //http://www.hanselman.com/blog/CDNsFailButYourScriptsDontHaveToFallbackFromCDNToLocalJQuery.aspx
            //http://eddmann.com/posts/providing-local-js-and-css-resources-for-cdn-fallbacks/

            ScriptManager.ScriptResourceMapping.AddDefinition("jQuery", new ScriptResourceDefinition
            {
                Path = "~/Scripts/jquery-3.1.1.min.js",
                DebugPath = "~/Scripts/jquery-3.1.1.js",
                CdnPath = "https://code.jquery.com/jquery-3.1.1.min.js",
                CdnDebugPath = "https://code.jquery.com/jquery-3.1.1.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.jQuery"

            });

            ScriptManager.ScriptResourceMapping.AddDefinition("bootstrap", new ScriptResourceDefinition
            {
                Path = "~/Scripts/bootstrap.min.js",
                DebugPath = "~/Scripts/bootstrap.js",
                CdnPath = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js",
                CdnDebugPath = "https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "jQuery.fn.modal"
            });

            ScriptManager.ScriptResourceMapping.AddDefinition("Moment", new ScriptResourceDefinition
            {
                Path = "~/Scripts/moment-with-locales.min.js",
                DebugPath = "~/Scripts/moment-with-locales.js",
                CdnPath = "https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.0/moment-with-locales.min.js",
                CdnDebugPath = "https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.17.0/moment-with-locales.js",
                CdnSupportsSecureConnection = true,
                LoadSuccessExpression = "window.Moment"
            });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.RequestContext.HttpContext.Request.ContentType.Equals("text/xml"))
            {
                Request.RequestContext.HttpContext.Request.ContentType = "text/xml; charset=UTF-8";
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //// Get the exception object.
            //Exception ex = Server.GetLastError();
            //if (ex is ThreadAbortException)
            //    return;
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}