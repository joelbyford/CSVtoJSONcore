[![Test on Pull Request to Main](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-pr.yml/badge.svg)](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-pr.yml)

[![Deploy to Azure](https://aka.ms/deploytoazurebutton)](https://portal.azure.com/#create/Microsoft.Template/uri/https%3A%2F%2Fraw.githubusercontent.com%2Fjoelbyford%2FCSVtoJSONcore%2Fmain%2FDeployTemplates%2FAzureLinuxWebAppArm.json)

# CSVtoJSONcore
**IMPORTANT: For a .NET Framework version of this repo, please see [joelbyford/CSVtoJSON](https://github.com/joelbyford/CSVtoJSON) instead (which was forked from [jeffhollan/CSVtoJSON](https://github.com/jeffhollan/CSVtoJSON)).**

Simple REST API which converts a delimited payload (via HTTP POST) to a JSON object.  



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

## Basic Authentication (Added on 1/19/2021)
Added the ability to use Basic Authentication with the API.  In order to leverage this functionality, please use the `appsettings.json` file to enable basic authentication, provide your "realm" (typically your API's url), and point to the json file where your users are listed (defaults to the provided `authorizedUsers.json`):

```
"AppSettings" : {
    "BasicAuth" : {
      "Enabled" : false,                     # change this to true
      "Realm" : "example-realm.com",         # change this to your API's Domain
      "UsersJson" : "authorizedUsers.json"   # change this (if necessary) to the json file with authorized users
    }
  }

```

The Authorized Users are simply stored in a json file in the following format:

```
{    
    "testUser" : "testPassword",
    "devUser" : "devPassword"
}
```

## Lineage and Credit
This is a psudo-fork dotnet core implementation of the previous .NET Framework [CSVtoJSON](https://github.com/jeffhollan/CSVtoJSON) repo provided by Jeff Hollan. 

Additionally, many thanks go to Josh Close for his [CsvHelper](https://github.com/JoshClose/CsvHelper) package used in this project.
