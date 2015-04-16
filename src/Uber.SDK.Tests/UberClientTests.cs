using System.Configuration;
using System.Linq;
using Shouldly;
using Uber.SDK.Models;
using Xunit;

namespace Uber.SDK.Tests
{
    public class UberClientTests
    {
        private readonly string _clientToken;
        private readonly string _serverToken;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _baseUri;

        public UberClientTests()
        {
            _clientToken = ConfigurationManager.AppSettings["UBER.ClientToken"];
            _serverToken = ConfigurationManager.AppSettings["UBER.ServerToken"];
            _clientId = ConfigurationManager.AppSettings["UBER.ClientId"];
            _clientSecret = ConfigurationManager.AppSettings["UBER.ClientSecret"];
            _baseUri = ConfigurationManager.AppSettings["UBER.BaseUri"];
        }

        [Fact]
        public void Authorize_ReturnsRedirectUrl()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = uberClient.GetAuthorizeUrl();

            response.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void GetAccessToken_ForValidCode_ReturnsValidAccessToken()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetAccessToken("TODO", "https://sandbox-api.uber.com/");

            response.ShouldNotBe(null);
            response.ShouldBeOfType<AccessToken>();
            response.Value.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void GetAccessToken_ForInvalidCode_ReturnsNull()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetAccessToken("INVALID", "https://sandbox-api.uber.com/");

            response.ShouldBe(null);
        }

        [Fact(Skip = "OAuth")]
        public async void RefreshAccessToken_ForValidRefreshToken_ReturnsValidAccessToken()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RefreshAccessToken("TODO", "https://sandbox-api.uber.com/");

            response.ShouldNotBe(null);
            response.ShouldBeOfType<AccessToken>();
            response.Value.ShouldNotBeNullOrEmpty();
        }

        [Fact(Skip = "OAuth")]
        public async void RefreshAccessToken_ForInvalidefreshToken_ReturnsNull()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RefreshAccessToken("INVALID", "https://sandbox-api.uber.com/");

            response.ShouldBe(null);
        }

        [Fact(Skip = "OAuth")]
        public async void RevokeAccessToken_ForValidCode_ReturnsTrue()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RevokeAccessTokenAsync("...");

            response.ShouldBe(true);
        }

        [Fact(Skip = "OAuth")]
        public async void RevokeAccessToken_ForInvalidCode_ReturnsFalse()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RevokeAccessTokenAsync("...");

            response.ShouldBe(false);
        }

        [Fact]
        public async void GetProducts_ForValidParameters_ReturnsListOfProducts()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetProductsAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<ProductCollection>();
            response.Data.Products.ShouldNotBe(null);
            response.Data.Products.Count.ShouldBeGreaterThan(0);
            response.Data.Products[0].DisplayName.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetProducts_ForInvalidParameters_ReturnsEmptyList()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetProductsAsync(
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<ProductCollection>();
            response.Data.Products.ShouldNotBe(null);
            response.Data.Products.Count.ShouldBe(0);
        }

        [Fact]
        public async void GetPriceEstimate_ForValidParameters_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetPriceEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, 
                Constants.SorrentoLatitude, Constants.SorrentoLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<PriceEstimateCollection>();
            response.Data.PriceEstimates.ShouldNotBe(null);
            response.Data.PriceEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.PriceEstimates[0].DisplayName.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetPriceEstimate_ForInvalidParameters_ReturnsEmptyList()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetPriceEstimateAsync(
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude,
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<PriceEstimateCollection>();
            response.Data.PriceEstimates.ShouldNotBe(null);
            response.Data.PriceEstimates.Count.ShouldBe(0);
        }

        [Fact]
        public async void GetTimeEstimate_ForValidDefaultParameters_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.TimeEstimates[0].ProductId.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetTimeEstimate_ForInvalidDefaultParameters_ReturnsEmptyList()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBe(0);
        }

        [Fact]
        public async void GetTimeEstimate_ForValidCustomerId_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, "TODO");

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.TimeEstimates[0].ProductId.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetTimeEstimate_ForInvalidCustomerId_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, "INVALID");

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.TimeEstimates[0].ProductId.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetTimeEstimate_ForValidProductId_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, "TODO", "TODO");

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.TimeEstimates[0].ProductId.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetTimeEstimate_ForInvalidProductId_ReturnsListOfPriceEstimates()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetTimeEstimateAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, "TODO", "INVALID");

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<TimeEstimateCollection>();
            response.Data.TimeEstimates.ShouldNotBe(null);
            response.Data.TimeEstimates.Count.ShouldBeGreaterThan(0);
            response.Data.TimeEstimates[0].ProductId.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async void GetPromotion_ForValidParameters_ReturnsPromotion()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetPromotionAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude,
                Constants.SorrentoLatitude, Constants.SorrentoLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<Promotion>();
        }

        [Fact]
        public async void GetPromotion_ForInvalidParameters_ReturnsError()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);
            
            var response = await uberClient.GetPromotionAsync(
                Constants.MelbourneLatitude, Constants.MelbourneLongitude, 
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldBe(null);
            response.Error.ShouldNotBe(null);
        }

        [Fact]
        public async void GetUserActivity_ForValidParameters_ReturnsUserUserActivity()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetUserActivityAsync(0, 10);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<Promotion>();
        }

        [Fact]
        public async void GetUserActivity_ForInvalidParameters_ReturnsError()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetUserActivityAsync(0, -1);

            response.ShouldNotBe(null);
            response.Data.ShouldBe(null);
            response.Error.ShouldNotBe(null);
        }

        [Fact]
        public async void GetUserProfile_ForValidToken_ReturnsUserUserActivity()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetUserProfileAsync();

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<Promotion>();
        }

        [Fact]
        public async void GetUserProfile_ForInvalidToken_ReturnsError()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.GetUserProfileAsync();

            response.ShouldNotBe(null);
            response.Data.ShouldBe(null);
            response.Error.ShouldNotBe(null);
        }

        [Fact]
        public async void Request_ForValidParameters_ReturnsRequest()
        {
            var uberClient = new UberClient(AccessTokenType.Server, _serverToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RequestAsync(
                "893b94af-ca9d-4f0f-9201-6d426cedaa5c",
                Constants.MelbourneLatitude, Constants.MelbourneLongitude,
                Constants.SorrentoLatitude, Constants.SorrentoLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<Request>();
        }

        [Fact]
        public async void Request_ForInvalidParameters_ReturnsError()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var response = await uberClient.RequestAsync(
                "INVALID",
                Constants.MelbourneLatitude, Constants.MelbourneLongitude,
                Constants.SouthPoleLatitude, Constants.SouthPoleLongitude);

            response.ShouldNotBe(null);
            response.Data.ShouldBe(null);
            response.Error.ShouldNotBe(null);
        }

        [Fact]
        public async void GetRequestDetails_ForValidParameters_ReturnsRequestDetails()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);

            var allRequests = await uberClient.GetUserActivityAsync(0, 50);
            var response = await uberClient.GetRequestDetailsAsync(allRequests.Data.History.First().Id);

            response.ShouldNotBe(null);
            response.Data.ShouldNotBe(null);
            response.Data.ShouldBeOfType<RequestDetails>();
        }

        [Fact]
        public async void GetRequestDetails_ForValidParameters_ReturnsError()
        {
            var uberClient = new UberClient(AccessTokenType.Client, _clientToken, _clientId, _clientSecret, _baseUri);
            
            var response = await uberClient.GetRequestDetailsAsync("INVALID");

            response.ShouldNotBe(null);
            response.Data.ShouldBe(null);
            response.Error.ShouldNotBe(null);
        }
    }
}
