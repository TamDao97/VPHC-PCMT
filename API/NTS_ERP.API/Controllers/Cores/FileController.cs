using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NTS.Common.Files;
using NTS_ERP.Api.Attributes;
using NTS_ERP.Models.Cores.Common;

namespace NTS_ERP.Api.Controllers.Cores
{
    [ApiController]
    [Route("api/file")]
    [ValidateModel]
    [ApiHandleExceptionSystem]
    [Authorize]
    public class FileController : BaseApiController
    {
        private readonly IUploadFileService uploadFileService;
        public FileController(IUploadFileService uploadFileService)
        {
            this.uploadFileService = uploadFileService;
        }

        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        [Route("upload-file")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel<UploadResultModel>>> UploadFile([FromForm] IFormFile? file, [FromForm] string? folderName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await uploadFileService.UploadFile(file, folderName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [Route("upload-files")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel<UploadResultModel>>> UploadFile([FromForm] List<IFormFile>? files, [FromForm] string? folderName)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            apiResultModel.Data = await uploadFileService.UploadFiles(files, folderName);
            apiResultModel.IsStatus = true;
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("download-files")]
        public async Task<IActionResult> DownloadMedia(DownloadFileModel downloadFileModel)
        {
            try
            {
                var file = await uploadFileService.DownloadFiles(downloadFileModel);

                return File(file.FileStream, file.ContentType, file.FileName);

            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Route("download-file")]
        public async Task<IActionResult> DownloadFiles(FileModel fileModel)
        {
            try
            {
                var file = await uploadFileService.DownloadFile(fileModel);

                return File(file.FileStream, file.ContentType, file.FileName);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}