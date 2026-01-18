using Microsoft.AspNetCore.Mvc;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Services.Cores.ViewFileWeb;

namespace NTS_ERP.Api.Controllers.Cores
{
    [Route("api/view-file")]
    [ApiController]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    public class ViewFileController : BaseApiController
    {
        private readonly IViewFileWebService _fileViewService;
        public ViewFileController(IViewFileWebService fileViewService)
        {
            this._fileViewService = fileViewService;
        }

        [HttpGet]
        [Route("get-file-view")]
        public async Task<IActionResult> GetFileView(string path)
        {
            try
            {
                var file = await _fileViewService.GetFileViewAsync(path);

                return File(file.FileStream, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
