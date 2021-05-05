using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.Configuration;
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
            String value;
            
            string[] headers = new string[1024]; //max of 1024 columns for now

            using (TextReader sr = new StringReader(body))
            {
                //set the delimiter type
                var config = new CsvConfiguration(System.Globalization.CultureInfo.CurrentCulture) { Delimiter = delimiter.ToString() };
                using var csv = new CsvReader(sr, config);

                //read header - not necessary to leverage header record functionality currently
                //csv.Configuration.HasHeaderRecord = false;
                if (csv.Read())
                {
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        headers[i] = value;
                    }
                }

                //read the rest of the file
                while (csv.Read())
                {
                    //initialize a new row object
                    var rowObject = new Dictionary<string, string>();

                    //loop through each element in the row
                    for (int i = 0; csv.TryGetField<string>(i, out value); i++)
                    {
                        //Add the row value with the matching header
                        rowObject.Add(headers[i], value);
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
