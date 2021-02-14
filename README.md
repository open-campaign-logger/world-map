# world-map

Service for managing world maps and associate them with Campaign Logger log entries.

# Development Environment

## .Net SDK
- [.Net Core 3.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1)

## Visual Studio
- [VS2019 Community](https://visualstudio.microsoft.com/downloads/)
- Workloads
  - ASP.NET and web development workload
  - .Net Core cross-platform development

## Visual Studio Extensions
- [StyleCop](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)
- [Markdown Editor](https://marketplace.visualstudio.com/items?itemName=ChrisDahlberg.StyleCop)
- [GhostDoc](https://marketplace.visualstudio.com/items?itemName=sergeb.GhostDoc)
- [GitFlow](https://marketplace.visualstudio.com/items?itemName=vs-publisher-57624.GitFlowforVisualStudio2019)
- [GitHub](https://marketplace.visualstudio.com/items?itemName=GitHub.GitHubExtensionforVisualStudio)

## npm/gulp Reference Material

- Using gulp and npm in VisualStudio: https://blog.bitscry.com/2018/03/13/using-npm-and-gulp-in-visual-studio-2017/
	- note: install Node.js which will install npm 
	- Working with npm behind a proxy: https://superuser.com/questions/347476/how-to-install-npm-behind-authentication-proxy-on-windows

## OpenID Connect Reference Material

- OpenID Connect JavaScript Client library: https://github.com/IdentityModel/oidc-client-js
- Using JWT and Asp.Net Core Cookies: https://amanagrawal.blog/2017/09/18/jwt-token-authentication-with-cookies-in-asp-net-core/
- IdentityServer4 Examples: https://github.com/IdentityServer/IdentityServer4.Samples
- IdentityServer4 QuickStart - Adding User Authentication with OpenID Connect: http://docs.identityserver.io/en/latest/quickstarts/3_interactive_login.html
- IdentityServer4 JavaScript Client Quickstart - http://docs.identityserver.io/en/latest/quickstarts/6_javascript_client.html
- Configuring App to Recognize JWT authorization tokens - https://developer.okta.com/blog/2018/03/23/token-authentication-aspnetcore-complete-guide

## Testing Reference Material

- Testing: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-2.2
- Integration Testing: https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2
- Integration Testing: https://fullstackmark.com/post/20/painless-integration-testing-with-aspnet-core-web-api
- Integration Testing with OpenID Connect: https://github.com/stottle-uk/IntegrationTestingWithIdentityServer
- Mocking authentication in integration tests: https://github.com/jackowild/aspnetcore-bypassing-authentication/tree/master/MockingAuthApi
- Sharing test context between tests: https://xunit.github.io/docs/shared-context
- Supporting AntiForgeryTokens: https://www.matheus.ro/2018/09/03/integration-tests-in-asp-net-core-controllers/

## Security Material

- Running .Net Core 2.1 With Self-Signed Cert: https://www.hanselman.com/blog/DevelopingLocallyWithASPNETCoreUnderHTTPSSSLAndSelfSignedCerts.aspx
- Configure HTTPS in ASP.Net Core 2.1: https://asp.net-hacker.rocks/2018/07/09/aspnetcore-ssl.html