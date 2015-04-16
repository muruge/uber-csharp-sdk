using System.Configuration;
using Shouldly;
using Uber.SDK.Models;
using Xunit;

namespace Uber.SDK.Tests
{
    public class UberAuthenticationClientTests
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public UberAuthenticationClientTests()
        {
            _clientId = ConfigurationManager.AppSettings["UBER.ClientId"];
            _clientSecret = ConfigurationManager.AppSettings["UBER.ClientSecret"];
        }

        [Fact]
        public void Authorize_ReturnsRedirectUrl()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = uberClient.GetAuthorizeUrl();

            response.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void GetAccessToken_ForValidCode_ReturnsValidAccessToken()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.GetAccessTokenAsync("TODO", "https://sandbox-api.uber.com/");

            response.ShouldNotBe(null);
            response.ShouldBeOfType<AccessToken>();
            response.Value.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void GetAccessToken_ForInvalidCode_ReturnsNull()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.GetAccessTokenAsync("INVALID", "https://sandbox-api.uber.com/");

            response.ShouldBe(null);
        }

        [Fact(Skip = "OAuth")]
        public async void RefreshAccessToken_ForValidRefreshToken_ReturnsValidAccessToken()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.RefreshAccessTokenAsync("TODO", "https://sandbox-api.uber.com/");

            response.ShouldNotBe(null);
            response.ShouldBeOfType<AccessToken>();
            response.Value.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void RefreshAccessToken_ForInvalidefreshToken_ReturnsNull()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.RefreshAccessTokenAsync("INVALID", "https://sandbox-api.uber.com/");

            response.ShouldBe(null);
        }

        [Fact(Skip = "OAuth")]
        public async void RevokeAccessToken_ForValidCode_ReturnsTrue()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.RevokeAccessTokenAsync("...");

            response.ShouldBe(true);
        }

        [Fact(Skip = "OAuth")]
        public async void RevokeAccessToken_ForInvalidCode_ReturnsFalse()
        {
            var uberClient = new UberAuthenticationClient(_clientId, _clientSecret);

            var response = await uberClient.RevokeAccessTokenAsync("...");

            response.ShouldBe(false);
        }
    }
}
