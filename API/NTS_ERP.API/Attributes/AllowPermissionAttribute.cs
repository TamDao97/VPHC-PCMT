using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NTS.Common.Resource;

namespace NTS_ERP.Api.Attributes
{
    public class AllowPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimPermissions = context.HttpContext.User.Claims
                                          .FirstOrDefault(c => c.Type.Equals("Permissions"));
            if (!string.IsNullOrEmpty(claimPermissions?.Value))
            {
                if (!string.IsNullOrEmpty(this.Permissions))
                {
                    var hasPermission = claimPermissions.Value
                                                        .Split(",")
                                                        .Intersect(this.Permissions.Split(";").Select(p => p.Trim()).ToArray());
                    if (hasPermission.Any())
                    {
                        return;
                    }
                    else
                    {
                        context.Result = new JsonResult(new { message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0003) }) { StatusCode = StatusCodes.Status405MethodNotAllowed };
                    }
                }
            }
            else
            {
                context.Result = new JsonResult(new { message = ResourceUtil.GetResourcesNoLag(MessageResourceKey.MSG0022) }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
