using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.Common;

namespace NTS_ERP.Api.Attributes
{
    public class ApiHandleExceptionSystemAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var errorModel = new ApiResultModel();

            if (context.Exception is NTSException)
            {
                var customError = context.Exception as NTSException;
                errorModel.MessageCode = customError.ErrorCode;
            }
            else
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Exception system");
                if (context.Exception.Message.Contains(MessageResourceKey.ERR0002))
                {
                    logger.LogError(context.Exception.InnerException.Message);
                    logger.LogError(context.Exception.StackTrace);
                    errorModel.MessageCode = MessageResourceKey.ERR0002;
                }
                else
                {
                    // Log exception to file                   
                    logger.LogError(context.Exception.Message);
                    logger.LogError(context.Exception.StackTrace);
                    errorModel.MessageCode = MessageResourceKey.ERR0001;
                }

            }


            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var contentResult = JsonConvert.SerializeObject(errorModel, serializerSettings);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.HttpContext.Response.WriteAsync(contentResult);
            context.ExceptionHandled = true;
        }

    }
}
