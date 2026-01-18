using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NTS.Common.Resource;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.About;
using NTS_ERP.Models.Cores.Common;
using NTS_ERP.Services.Cores.Abouts;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/about")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Logging]
    [Authorize]
    public class AboutController : BaseApiController
    {
        private readonly IAboutService _aboutService;
        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        /// <summary>
        /// Lấy thông tin giới thiệu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        //[Route("{id}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<AboutModel>> GetById()
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _aboutService.GetAboutAsync();
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Chỉnh sửa nội dung giới thiệu
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName(TextResourceKey.Action_Update)]
        [AllowPermission(Permissions = "F0141")]
        public async Task<ActionResult<AboutModel>> UpdateAsync([FromBody] AboutModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            await _aboutService.CreateAboutAsync(model, CurrentUser.UserId);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
