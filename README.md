# world-map

Service for managing world maps and associate them with Campaign Logger log entries.

THIS REPO IS NO LONGER ACTIVELY MAINTAINED AND THEREFORE ARCHIVED.

# Development Environment

## SDKs

- [.Net 5.0 SDK](https://dotnet.microsoft.com/download/visual-studio-sdks?utm_source=getdotnetsdk&utm_medium=referral)

## Azure Tools
- [Microsoft Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/)
- [Microsoft Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)

## Visual Studio
- [VS2019 Community](https://visualstudio.microsoft.com/downloads/)
- Workloads
  - ASP.NET and web development
  - .Net Core cross-platform development
  - Node.js development

## Visual Studio Extensions
- [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)
- [GhostDoc](https://marketplace.visualstudio.com/items?itemName=sergeb.GhostDoc)
- [GitFlow](https://marketplace.visualstudio.com/items?itemName=vs-publisher-57624.GitFlowforVisualStudio2019)
- [GitHub](https://marketplace.visualstudio.com/items?itemName=GitHub.GitHubExtensionforVisualStudio)
- [Web Compiler](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.WebCompiler)

## Docker
- [Docker](https://docs.docker.com/docker-for-windows/install/)
 
## Azure Storage Emulator
The Azure Storage Emulator is used by the application to simulate reading/writing from Azure storage in the local environment.  See: [Running the Azure Storage Emulator](https://medium.com/oneforall-undergrad-software-engineering/setting-up-the-azure-storage-emulator-environment-on-windows-5f20d07d3a04)

Ensure that the following Azure Storage items have been created:
- **world-map** blob container (ensure that you setup public access to the blobs)
- **worldmapmaps** table

# Running and Debugging the Application Natively

## Docker Desktop
1. Start Docker Desktop
2. Click "Start" to begin debugging.  This will build and run the following Docker image:
  - CampaignKit.WorldMap.UI
  - CampaignKit.WorldMap.Function

![Docker Running World-Map Projects](./DockerRunningWorldMapProjects.png)

## User Secrets
Both the **CampaignKit.WorldMap.Function** and **CampaignKit.WorldMap.UI** projects make use of ASP.Net Core's user secrets functionality to override values in **local.settings.json** and **appsettings.json** respectively.  Visual Studio has built in support for user secrets that you can use to provide configuration overrides during development.

![Visual Studio Support for User Secrets](./UserSecrets.png)

Once you open your user secrets file you can provide local configuration overrides that don't get stored in the code repository.

```
{
  "ConnectionStrings:AzureStorage": "TableEndpoint=https://clst03.table.core.windows.net/;SharedAccessSignature=...",
  "AzureBlobBaseURL": "https://clst03.blob.core.windows.net/world-map"
}
```

## Simulating EventGrid Events Locally

EventGrid triggers works as follows:
1. When a map is created a master file is added to blob storage.
2. When the master file is added to blob storage a blob creation event is sent to the ImageTrigger function.
3. The ImageTrigger executes and creates zoom level images for the master file.  These zoom level images are added to blob storage.
4. When zoom level images are added to blob storage blob creation events are sent to the ImageTrigger function.
6. The ImageTrigger executes are creates tile images for each zoom level.

[Postman](https://www.postman.com/) can be used to simulate steps 2 and 4 above.

To do this you need to:
1. Create a POST request to **http://localhost:3001/runtime/webhooks/EventGrid?functionName=ImageTrigger**
2. Add a custom **aeg-event-type** header to the request with value **Notification**
3. Set the JSON body of the call to the text below making the following substitutions to **data.url**:
   - **FOLDERNAME** the folder created in blob storage to hold the map files.
   - **FILENAME** either **master-file.png** for the initial EventGrid call or **0_zoom-level.png**, **1_zoom-level.png**, **2_zoom-level.png**, etc... for each zoom level EventGrid call.

```
[{
  "topic": "/subscriptions/{subscription-id}/resourceGroups/Storage/providers/Microsoft.Storage/storageAccounts/my-storage-account",
  "subject": "/blobServices/default/containers/test-container/blobs/new-file.txt",
  "eventType": "Microsoft.Storage.BlobCreated",
  "eventTime": "2017-06-26T18:41:00.9584103Z",
  "id": "831e1650-001e-001b-66ab-eeb76e069631",
  "data": {
    "api": "PutBlockList",
    "clientRequestId": "6d79dbfb-0e37-4fc4-981f-442c9ca65760",
    "requestId": "831e1650-001e-001b-66ab-eeb76e000000",
    "eTag": "\"0x8D4BCC2E4835CD0\"",
    "contentType": "text/plain",
    "contentLength": 524288,
    "blobType": "BlockBlob",
    "url": "http://127.0.0.1:10000/devstoreaccount1/world-map/FOLDERNAME/FILENAME",
    "sequencer": "00000000000004420000000000028963",
    "storageDiagnostics": {
      "batchId": "b68529f3-68cd-4744-baa4-3c0498ec19f0"
    }
  },
  "dataVersion": "",
  "metadataVersion": "1"
}]
```

# Reference Material

## OpenID Connect

- [OpenID Connect JavaScript Client library](https://github.com/IdentityModel/oidc-client-js)
- [Using JWT and Asp.Net Core Cookies](https://amanagrawal.blog/2017/09/18/jwt-token-authentication-with-cookies-in-asp-net-core/)
- [IdentityServer4 Examples](https://github.com/IdentityServer/IdentityServer4.Samples)
- [IdentityServer4 QuickStart - Adding User Authentication with OpenID Connect](http://docs.identityserver.io/en/latest/quickstarts/3_interactive_login.html)
- [IdentityServer4 JavaScript Client Quickstart](http://docs.identityserver.io/en/latest/quickstarts/6_javascript_client.html)
- [Configuring App to Recognize JWT authorization tokens](https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide)

## Testing
- [Unit Testing Controllers in ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-5.0)
- [Integration Testing in ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0)
- [Integration Testing with OpenID Connect](https://github.com/stottle-uk/IntegrationTestingWithIdentityServer)
- [Mocking authentication in integration tests](https://github.com/jackowild/aspnetcore-bypassing-authentication/tree/master/MockingAuthApi)
- [Supporting AntiForgeryTokens](https://www.matheus.ro/2018/09/03/integration-tests-in-asp-net-core-controllers/)

## Azure

- [Azure Storage Blobs - Quickstart](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet)
- [Cosmos DB - Quickstart](https://docs.microsoft.com/en-us/azure/cosmos-db/create-table-dotnet)
- [Cosmos DB - Query Tables](https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-query-table)
- [Cosmos DB - Query Examples](https://docs.microsoft.com/en-us/azure/cosmos-db/sql-api-dotnet-v3sdk-samples#query-examples)
- [Azure Connection Strings](https://docs.microsoft.com/en-us/azure/storage/common/storage-configure-connection-string)
- [Configuring Logging in Azure App Service](https://ardalis.com/configuring-logging-in-azure-app-services/)
- [Develop, test, and deploy an Azure Function with Visual Studio](https://docs.microsoft.com/en-us/learn/modules/develop-test-deploy-azure-functions-with-visual-studio/)
- [Use dependency injections in .Net Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection)
- [Azure Functions Dependency Injection](https://blog.rasmustc.com/azure-functions-dependency-injection/)
- [Using JSON and User Secrets configuration with Azure Functions](https://dev.to/cesarcodes/using-json-and-user-secrets-configuration-with-azure-functions-3f7g)

## ASP.Net Core
- [ASP.Net User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows)
- [Visual Studio Support for User Secrets](https://www.mssqltips.com/sqlservertip/6348/securely-manage-database-credentials-using-visual-studio-manage-user-secrets/)
