using System;
using System.Collections.Generic;

namespace CsvToJsonCore
{
   public class JsonResult
    {
        public JsonResult()
        {
            rows = new List<object>();
        }
        public List<object> rows { get; set; }
    }
}
