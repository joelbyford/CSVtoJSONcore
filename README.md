[![Test on Pull Request to Main](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-pr.yml/badge.svg)](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-pr.yml) [![Deploy on Push to Main](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-push.yml/badge.svg)](https://github.com/joelbyford/CSVtoJSONcore/actions/workflows/main-push.yml)

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

## Azure Deployment Instructions
If you are deploying to Azure, the following are steps you can use to re-use/leverage the existing CI/CD pipeline defined in the GitHub Actions YML files which already exist in this repo:

1. Create your [App Service](https://learn.microsoft.com/en-us/azure/app-service/) in an Azure Subscription
2. Create 2 additional [Deployment Slots](https://learn.microsoft.com/en-us/azure/app-service/deploy-staging-slots?tabs=portal) (one for testing and one for staging)
3. Define an [OpenID Connect](https://learn.microsoft.com/en-us/azure/developer/github/connect-from-azure-openid-connect) credential for GitHub to leverage.
4. Fork the Repo (this will probably trigger a GitHub Action which will fail . . .don't worry. . .it's not configured yet).
5. Add the following Secrets in your forked repo:

<u>Used for OpenID Connect Authentication:</u>
- **AZURE_CLIENTID** (See OpenID Connect Instructions)
- **AZURE_TENANTID** (See OpenID Connect Instructions)
- **AZURE_SUBSCRIPTIONID** (See OpenID Connect Instructions)

<u>General Parameters kept secret so not giving away too many details about your deployment in a public repo:</u>
- **APP_NAME** - the name of your AppService in Azure
- **APP_RG** - the resource group name where your AppService lives

<u>Automated Smoke Testing (currently uses PyTest) parameters:</u>
- **TEST_URL** - the URL for accessing your AppService test slot in Azure (e.g. "https://{mysite}-test.azurewebsites.net/" if you named your slot "test")
- **STAGING_URL** - the URL for accessing your AppService staging slot in Azure (e.g. "https://{mysite}-staging.azurewebsites.net/" if you named your slot "staging").


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

