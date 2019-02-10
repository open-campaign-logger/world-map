using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace CampaignKit.WorldMap.Tests.Infrastructure
{

	public class TestAuthHandler : AuthenticationHandler<TestAuthenticationOptions>
	{
		public TestAuthHandler(IOptionsMonitor<TestAuthenticationOptions> options, ILoggerFactory logger,
			UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
		{
		}

		protected override Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			var authenticationTicket = new AuthenticationTicket(
				new ClaimsPrincipal(Options.Identity),
				new AuthenticationProperties(),
				"Test Scheme");

			return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
		}
	}

	public static class TestAuthenticationExtensions
	{
		public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
		{
			return builder.AddScheme<TestAuthenticationOptions, TestAuthHandler>("Test Scheme", "Test Auth", configureOptions);
		}
	}

	public class TestAuthenticationOptions : AuthenticationSchemeOptions
	{
		public static string TEST_ID = "817be42b-3c0c-4389-841e-97e53ef51337";

		public virtual ClaimsIdentity Identity { get; } = new ClaimsIdentity(new Claim[]
		{
			new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", TEST_ID),
		}, "test");

	}
}
