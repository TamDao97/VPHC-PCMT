using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using NTS.Common;
using NTS.Common.Resource;
using NTS_ERP.Models.Cores.UserHistory;
using NTS_ERP.Services.Cores.Log;
using System.Security.Claims;

namespace NTS_ERP.Api.Attributes
{
    public class BaseActionFilterAttribute : ActionFilterAttribute
    {
        #region Log

        /// <summary>
        /// Ghi log khi có request
        /// </summary>
        /// <param name="actionContext"></param>
        //public void LogRequest(ActionExecutingContext actionContext)
        //{
        //    var activityService = actionContext.HttpContext.RequestServices.GetRequiredService<ILogEventService>();
        //    var descriptor = actionContext.ActionDescriptor as ControllerActionDescriptor;
        //    if (!descriptor.ActionName.StartsWith(NTSConstants.NoLogEvent))
        //    {
        //        var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
        //        //actionContext.HttpContext.Request.Headers.TryGetValue("windowcode", out var windowCode);

        //        //if (string.IsNullOrEmpty(windowCode))
        //        //{
        //        //    windowCode = string.Empty;
        //        //}

        //        if (!string.IsNullOrEmpty(actionName.Trim()))
        //        {
        //            var content = JsonConvert.SerializeObject(actionContext.ActionArguments);
        //            if (!content.Equals("{}"))//loại bỏ nhưng request chứa content null
        //            {
        //                UserHistoryModel activityModel = new UserHistoryModel()
        //                {
        //                    Content = content,
        //                    Name = actionName.Trim(),
        //                };

        //                activityService.LogEventAsync(actionContext.HttpContext.Request, activityModel, GetUserIdByRequest(actionContext));
        //            }

        //        }
        //    }
        //}

        /// <summary>
        /// Ghi log trả ra response
        /// </summary>
        /// <param name="resultContext"></param>
        public void LogResponse(ResultExecutedContext resultContext)
        {
            string userId = GetUserIdByRequest(resultContext);
            //Không log lịch sử tài khoản root
            if (string.IsNullOrEmpty(userId) || userId.Equals(NTSConstants.IdUserRootFix))
                return;

            var activityService = resultContext.HttpContext.RequestServices.GetRequiredService<ILogEventService>();
            var descriptor = resultContext.ActionDescriptor as ControllerActionDescriptor;
            if (!descriptor.ActionName.StartsWith(NTSConstants.NoLogEvent))
            {
                var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
                //actionContext.HttpContext.Request.Headers.TryGetValue("windowcode", out var windowCode);

                //if (string.IsNullOrEmpty(windowCode))
                //{
                //    windowCode = string.Empty;
                //}

                if (!string.IsNullOrEmpty(actionName.Trim()))
                {
                    UserHistoryModel activityModel = new UserHistoryModel()
                    {
                        Content = JsonConvert.SerializeObject(resultContext.Result),
                        Name = actionName.Trim(),
                        Type = NTSConstants.UserHistory_Type_Data
                    };

                    activityService.LogEventAsync(resultContext.HttpContext.Request, activityModel, userId);
                }
            }
        }

        ///// <summary>
        ///// Ghi log validation
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="errors"></param>
        //public void LogResponseValidate(ActionExecutingContext context, List<ErrorValidateModel> errors)
        //{
        //    var activityService = context.HttpContext.RequestServices.GetService<IActivityService>();
        //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //    var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
        //    context.HttpContext.Request.Headers.TryGetValue("windowCode", out var windowCode);

        //    if (string.IsNullOrEmpty(windowCode))
        //    {
        //        windowCode = string.Empty;
        //    }

        //    ActivityCreateModel activityModel = new ActivityCreateModel()
        //    {
        //        AD_UserId = GetUserIdByRequest(context),
        //        Content = JsonConvert.SerializeObject(errors),
        //        ActionName = actionName,
        //        FunctionName = descriptor.DisplayName,
        //        Type = NTSConstants.Response,
        //        WindowCode = windowCode,
        //        CreateDate = DateTime.Now,
        //        IP = GetIPAddress(context),
        //        ResultStatus = false
        //    };

        //    activityService.CreateActivity(activityModel);
        //}

        //public void LogResponseException(ExceptionContext context, ExceptionInfoModel exceptionModel, bool isException)
        //{
        //    var activityService = context.HttpContext.RequestServices.GetService<IActivityService>();
        //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //    var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
        //    context.HttpContext.Request.Headers.TryGetValue("windowCode", out var windowCode);

        //    if (string.IsNullOrEmpty(windowCode))
        //    {
        //        windowCode = string.Empty;
        //    }

        //    ActivityCreateModel activityModel = new ActivityCreateModel()
        //    {
        //        AD_UserId = GetUserIdByRequest(context),
        //        ActionName = actionName,
        //        FunctionName = descriptor.DisplayName,
        //        Type = NTSConstants.Response,
        //        WindowCode = windowCode,
        //        CreateDate = DateTime.Now,
        //        IP = GetIPAddress(context),
        //        ResultStatus = false,
        //        IsException = isException,
        //        Parameters = JsonConvert.SerializeObject(exceptionModel.Data),
        //        Content = exceptionModel.Message,
        //    };

        //    activityService.CreateActivity(activityModel);
        //}

        //public void LogException(ExceptionContext context, bool isException)
        //{
        //    var activityService = context.HttpContext.RequestServices.GetService<IActivityService>();
        //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //    var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
        //    context.HttpContext.Request.Headers.TryGetValue("windowCode", out var windowCode);

        //    if (string.IsNullOrEmpty(windowCode))
        //    {
        //        windowCode = string.Empty;
        //    }

        //    var exceptionModel = context.Exception.GetExceptionInfo();

        //    ActivityCreateModel activityModel = new ActivityCreateModel()
        //    {
        //        AD_UserId = GetUserIdByRequest(context),
        //        ActionName = actionName,
        //        FunctionName = descriptor.DisplayName,
        //        Type = NTSConstants.Exception,
        //        WindowCode = windowCode,
        //        CreateDate = DateTime.Now,
        //        IP = GetIPAddress(context),
        //        ResultStatus = false,
        //        IsException = isException,
        //        Parameters = JsonConvert.SerializeObject(exceptionModel.Data),
        //        LineNumber = exceptionModel.LineNumber,
        //        FileName = exceptionModel.FileName,
        //        ExceptionContent = exceptionModel.Message,
        //    };

        //    activityService.CreateActivity(activityModel);
        //}

        //public void LogNTSException(ExceptionContext context, ExceptionInfoModel exceptionModel, bool isException)
        //{
        //    var activityService = context.HttpContext.RequestServices.GetService<IActivityService>();
        //    var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
        //    var actionName = ResourceUtil.GetTextResource(descriptor.ActionName) + " " + ResourceUtil.GetTextResource(descriptor.ControllerName);
        //    context.HttpContext.Request.Headers.TryGetValue("windowCode", out var windowCode);

        //    if (string.IsNullOrEmpty(windowCode))
        //    {
        //        windowCode = string.Empty;
        //    }

        //    ActivityCreateModel activityModel = new ActivityCreateModel()
        //    {
        //        AD_UserId = GetUserIdByRequest(context),
        //        ActionName = actionName,
        //        FunctionName = exceptionModel.MethodName,
        //        FileName = exceptionModel.FileName,
        //        LineNumber = exceptionModel.LineNumber,
        //        Type = NTSConstants.Exception,
        //        WindowCode = windowCode,
        //        CreateDate = DateTime.Now,
        //        IP = GetIPAddress(context),
        //        ResultStatus = false,
        //        IsException = isException,
        //        Parameters = JsonConvert.SerializeObject(exceptionModel.Data),
        //        ExceptionContent = exceptionModel.Message,
        //    };

        //    activityService.CreateActivity(activityModel);
        //}
        #endregion

        #region Protected
        protected string GetUserIdByRequest(ResultExecutedContext resultContext)
        {
            var identity = (ClaimsIdentity)resultContext.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value;

            return signedInUserId;
        }

        protected string GetUserIdByRequest(ActionExecutingContext actionContext)
        {
            var identity = (ClaimsIdentity)actionContext.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value;

            return signedInUserId;
        }

        protected string GetUserIdByRequest(ExceptionContext actionContext)
        {
            var identity = (ClaimsIdentity)actionContext.HttpContext.User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value;

            return signedInUserId;
        }
        #endregion

        #region Private
        //private string GetIPAddress(ActionExecutingContext actionContext)
        //{
        //    var ip = actionContext.HttpContext.Connection.RemoteIpAddress;
        //    return ip.ToString();
        //}

        //private string GetIPAddress(ResultExecutedContext resultContext)
        //{
        //    var ip = resultContext.HttpContext.Connection.RemoteIpAddress;
        //    return ip.ToString();
        //}

        //private string GetIPAddress(ExceptionContext resultContext)
        //{
        //    var ip = resultContext.HttpContext.Connection.RemoteIpAddress;
        //    return ip.ToString();
        //}
        #endregion
    }
}
