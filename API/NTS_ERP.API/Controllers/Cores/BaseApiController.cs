using NTS_ERP.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NTS_ERP.Models.Cores.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.Auth;

namespace NTS_ERP.Api.Controllers.Cores
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        /// <summary>
        /// Thông tin người đăng nhập
        /// </summary>
        protected CurrentUserModel CurrentUser
        {
            get
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                return new CurrentUserModel
                {
                    UserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value,
                    UserName = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value,
                    DonViId = claims.FirstOrDefault(cl => cl.Type.Equals("DonVi"))?.Value
                };
            }
        }

        /// <summary>
        /// Đã xác thực người dùng chưa
        /// </summary>
        protected bool IsUserAuthenticated
        {
            get
            {
                return HttpContext.User.Identity.IsAuthenticated;
            }
        }
    }
}
