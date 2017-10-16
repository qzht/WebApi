using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using WebApi.App_Start;

namespace WebApi.Filter
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(ITraceWriter), new AppLog());
            var trace = GlobalConfiguration.Configuration.Services.GetTraceWriter();
            trace.Info(actionContext.Request, "Controller : " + actionContext.ControllerContext.ControllerDescriptor.ControllerType.FullName + Environment.NewLine + "Action : " + actionContext.ActionDescriptor.ActionName, "JSON", actionContext.ActionArguments);
            //base.OnActionExecuting(actionContext);
          //  base.OnActionExecuting(actionContext);
        }
    }
}