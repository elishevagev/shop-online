using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace userPresentation.Filter
{
    public class ValidarSessionAttribure :ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (HttpContext.Current.Session["Customer"] == null)
            {
                filterContext.Result = new RedirectResult("~/Access/Index");
                return;
            }
            base.OnActionExecuted(filterContext);
        }
    }
}