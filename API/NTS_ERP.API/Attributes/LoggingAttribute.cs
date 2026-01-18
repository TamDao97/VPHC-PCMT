
using Microsoft.AspNetCore.Mvc.Filters;
using NTS_ERP.Api.Attributes;

namespace NTS_ERP.Api.Attributes
{
    public class LoggingAttribute : BaseActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    LogRequest(context);
        //    base.OnActionExecuting(context);
        //}

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            LogResponse(context);
            base.OnResultExecuted(context);
        }
    }
}
