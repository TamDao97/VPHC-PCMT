using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS_ERP.Api.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var buffer = new MemoryStream();
             context.HttpContext.Request.Body.CopyToAsync(buffer);
            context.HttpContext.Request.Body = buffer;
            buffer.Position = 0;

            var encoding = Encoding.UTF8;

            var requestContent =  new StreamReader(buffer, encoding).ReadToEndAsync();
            context.HttpContext.Request.Body.Position = 0;

            if (!context.ModelState.IsValid)
            {
                string message = string.Join('\n', context.ModelState.ToList().SelectMany(s => s.Value.Errors).Select(s=>s.ErrorMessage));
                //throw new Exception(message);
                throw new Exception(MessageResourceKey.ERR0002, new Exception(message));
            }
        }

        private string ReadBodyAsString(HttpRequest request)
        {
            var initialBody = request.Body; // Workaround

            try
            {
                request.EnableBuffering();

                using (StreamReader reader = new StreamReader(request.Body))
                {
                    string text = reader.ReadToEnd();
                    return text;
                }
            }
            finally
            {
                // Workaround so MVC action will be able to read body as well
                request.Body = initialBody;
            }

            return string.Empty;
        }
    }
}
