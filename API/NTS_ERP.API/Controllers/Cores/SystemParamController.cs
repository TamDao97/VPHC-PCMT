using NTS_ERP.Api.Attributes;
using NTS_ERP.Services.Cores.SystemParams;
using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;
using NTS.Common.Resource;
using System.Threading.Tasks;
using NTS_ERP.Models.Cores.SystemParams;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/system-param")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Logging]
    public class SystemParamController : BaseApiController
    {
        private readonly ISystemParamService _systemParamService;
        public SystemParamController(ISystemParamService systemParamService)
        {
            _systemParamService = systemParamService;
        }

        /// <summary>
        /// Lấy chi tiết param
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-info/{paramName}")]
        [ActionName(TextResourceKey.Action_Get)]
        public async Task<ActionResult<SystemParamInfoModel>> GetById([FromRoute] string paramName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();
            apiResultModel.Data = await _systemParamService.GetSystemParamInfoAsync(paramName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }
    }
}
