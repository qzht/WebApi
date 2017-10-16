using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected PetaPoco.Database db = new PetaPoco.Database("LocalSqlServer");
        protected HttpResponseMessage AjaxContentResult(int success, string content, string msg, bool isObject = false)
        {
            string formatString = string.Format("{0}\"success\":\"" + success + "\",\"msg\":\"" + msg + "\",\"content\":{2}{3}{4}{5}", "{",
                success, isObject ? "" : "\"", content, isObject ? "" : "\"", "}");
            return new HttpResponseMessage { Content = new StringContent(formatString, System.Text.Encoding.UTF8, "application/json") }; ;
        }

    }
}
