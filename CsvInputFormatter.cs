using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace CsvToJsonCore
{

    // Thanks to https://github.com/RickStrahl/AspNetCoreRawRequestSample for the starting example here.
   public class CsvInputFormatter : InputFormatter
    {
        public CsvInputFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));
        }

        //Accept text/plain and text/csv only
        public override Boolean CanRead(InputFormatterContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var contentType = context.HttpContext.Request.ContentType;

            if (contentType == "text/plain" || contentType == "text/csv")
                return true;
            else
                return false;
        }

        // handle the raw text input
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var contentType = context.HttpContext.Request.ContentType;


            if (contentType == "text/csv" || contentType == "text/plain")
            {
                using (var reader = new StreamReader(request.Body))
                {
                    var content = await reader.ReadToEndAsync();
                    return await InputFormatterResult.SuccessAsync(content);
                }
            }

            return await InputFormatterResult.FailureAsync();
        }        
    }
}