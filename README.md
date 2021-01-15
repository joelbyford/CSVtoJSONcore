# CSVtoJSONcore
**IMPORTANT: For a .NET Framework version of this repo, please see [joelbyford/CSVtoJSON](https://github.com/joelbyford/CSVtoJSON) instead (which was forked from [jeffhollan/CSVtoJSON](https://github.com/jeffhollan/CSVtoJSON)).**

Simple REST API which converts a delimited payload (via HTTP POST) to a JSON object.  

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjoelbyford%2FCSVtoJSONcore%2Fmain%2FDeployTemplates%2FAzureLinuxWebAppArm.json)

## Examples and Usage
Optional querystring parameter to allow consumers to specify the delimiter being used in the text file and allows for other delimiters (such as | or ;).  Usage is:

mydomain.com/myservice/?delimiter=|

If no delimiter parameter is provided, comma is assumed.

Example test through Postman or REST plugin in VSCode:

```
POST https://SOME-WEBSITE-URL/csvtojson
Content-Type: text/csv

this,is,a,test
1,2,3,4
a,b,c,d
```

Results should appear similar to the following: 

```
{
  "rows": [
    {
      "this": "1",
      "is": "2",
      "a": "3",
      "test": "4"
    },
    {
      "this": "a",
      "is": "b",
      "a": "c",
      "test": "d"
    }
  ]
}

```
Additional examples can be found in the `test` folder.

## Lineage and Credit
This is a psudo-fork dotnet core implementation of the previous .NET Framework [CSVtoJSON](https://github.com/jeffhollan/CSVtoJSON) repo provided by Jeff Hollan. 

Additionally, many thanks go to Josh Close for his [CsvHelper](https://github.com/JoshClose/CsvHelper) package used in this project.
