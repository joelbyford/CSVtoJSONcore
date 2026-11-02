using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CsvHelper;
using System.IO;


namespace CsvToJsonCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CsvToJsonController : ControllerBase
    {

        private readonly ILogger<CsvToJsonController> _logger;

        public CsvToJsonController(ILogger<CsvToJsonController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult<JsonResult> Post([FromBody] string body, [FromQuery] char delimiter = ',')
        {
            JsonResult resultSet = new JsonResult();
            
            using (TextReader sr = new StringReader(body))
            {
                var options = new CsvReaderOptions { Separator = delimiter };
                using var csv = CsvReader.Create(sr, options);
                var headers = csv.Headers;

                //read the rest of the file
                while (csv.Read())
                {
                    //initialize a new row object
                    var rowObject = new Dictionary<string, string>();

                    //loop through each element in the row
                    for (int i = 0; i < csv.FieldCount; i++)
                    {
                        //Add the row value with the matching header
                        rowObject.Add(headers[i], csv.GetString(i));
                        //Log the results for debugging
                        //_logger.LogWarning(value);
                    }
                    //Add the populated row object to the row array
                    resultSet.rows.Add(rowObject);
                    
                }
            }
            return resultSet;
        }
        
    }
}
