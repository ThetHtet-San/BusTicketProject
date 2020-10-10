using Com.MrIT.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Com.MrIT.App.Filters
{
    public class MrITActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            string actionName = filterContext.ActionDescriptor.RouteValues["action"];
            string controller_action = controllerName + "_" + actionName;

            string scheme = filterContext.HttpContext.Request.Scheme;
            string host = filterContext.HttpContext.Request.Host.Value;
            string path = filterContext.HttpContext.Request.Path.Value;
            string querystring = filterContext.HttpContext.Request.QueryString.Value;

            string currenturl = scheme + "://" + host + path + querystring;
            currenturl = currenturl.Replace("&", "_AND_");
            var vrSession = filterContext.HttpContext.Session;



            byte[] b_FullName;
            vrSession.TryGetValue("FullName", out b_FullName);
            if (actionName.ToLower() == "signin")
            {
                if (b_FullName != null)
                {
                    // Redirect to login page action
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"controller", "Home"},
                        {"action", "Index"}
                    });
                }
            }
            else
            {
                if (b_FullName == null)
                {
                    // Redirect to login page action
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"controller", "Users"},
                        {"action", "SignIn"},
                        {"currenturl", currenturl}
                    });
                }
                else
                {

                }
            }
        }
    }
}
