using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Uber.SDK.Models;

namespace Uber.SDK
{
    public class UberClient : IUberClient
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        protected readonly HttpClient _httpClient;

        /// <summary>
        /// Initialises a new <see cref="UberClient"/> with the required configurations
        /// </summary>
        /// <param name="tokenType">
        /// The token type - server or client
        /// </param>
        /// <param name="token">
        /// The token
        /// </param>
        /// <param name="clientId">
        /// The client ID, this can be found at https://developer.uber.com/apps/
        /// </param>
        /// <param name="clientSecret">
        /// The client secret, this can be found at https://developer.uber.com/apps/
        /// </param>
        /// <param name="baseUri">
        /// The base URI, production should use https://api.uber.com, sandbox should use https://sandbox-api.uber.com
        /// </param>
        public UberClient(AccessTokenType tokenType, string token, string clientId, string clientSecret, string baseUri)
        {
            if (string.IsNullOrWhiteSpace(token)) throw new ArgumentException("Parameter is required", "token");
            if (string.IsNullOrWhiteSpace(clientId)) throw new ArgumentException("Parameter is required", "clientId");
            if (string.IsNullOrWhiteSpace(clientSecret)) throw new ArgumentException("Parameter is required", "clientSecret");
            if (string.IsNullOrWhiteSpace(baseUri)) throw new ArgumentException("Parameter is required", "baseUri");

            _clientId = clientId;
            _clientSecret = clientSecret;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUri)
            };

            // Default auth to server, if 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType == AccessTokenType.Server ? "Token" : "Bearer", token);

            // Set accept headers to JSON only
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Generates an Uber OAuth authorization URL based on scopes, state and redirect.
        /// </summary>
        /// <param name="scopes">
        /// The permission scope/s being requested.
        /// </param>
        /// <param name="state">
        /// State which will be passed back to you to prevent tampering.
        /// </param>
        /// <param name="redirectUrl">
        /// The URI we will redirect back to after an authorization by the resource owner.
        /// </param>
        /// <returns>
        /// Returns the OAuth authorization URL.
        /// </returns>
        public string GetAuthorizeUrl(List<string> scopes = null, string state = null, string redirectUrl = null)
        {
            var authorizeUrl = string.Concat("https://login.uber.com/oauth/authorize?response_type=code&client_id=", _clientId);

            if (scopes != null && scopes.Any())
            {
                authorizeUrl += string.Concat("&scope=", string.Join(" ", scopes));
            }

            if (!string.IsNullOrWhiteSpace(state))
            {
                authorizeUrl += string.Concat("&state=", state);
            }

            if (!string.IsNullOrWhiteSpace(redirectUrl))
            {
                authorizeUrl += string.Concat("&redirectUrl=", HttpUtility.UrlEncode(redirectUrl));
            }

            return authorizeUrl;
        }

        /// <summary>
        /// Exchange this authorization code for an AccessToken, allowing requests to be mande on behalf of a user.
        /// </summary>
        /// <param name="authorizationCode">
        /// The authorization code.
        /// </param>
        /// <param name="redirectUri">
        /// The URL the user should be redrected back to 
        /// </param>
        /// <returns>
        /// Returns the <see cref="AccessToken"/>.
        /// </returns>
        public async Task<AccessToken> GetAccessToken(string authorizationCode, string redirectUri)
        {
            var data = new Dictionary<string, string>
            {
                { "client_id", _clientId },
                { "client_secret", _clientSecret },
                { "grant_type", "authorization_code" },
                { "code", authorizationCode },
                { "redirect_uri", redirectUri }
            };

            return await AuthorizeAsync(data);
        }

        public async Task<AccessToken> RefreshAccessToken(string refreshToken, string redirectUri)
		{
            var data = new Dictionary<string, string>
			{
				{ "client_id", _clientId },
				{ "client_secret", _clientSecret },
				{ "grant_type", "refresh_token" },
				{ "refresh_token", refreshToken },
				{ "redirect_uri", redirectUri }
			};

            return await AuthorizeAsync(data);
		}

        /// <summary>
        /// Authorizes a client with Uber OAuth
        /// </summary>
        /// <param name="content">
        /// The HTTP request content
        /// </param>
        /// <returns>
        /// Returns the <see cref="AccessToken"/> if authorization is successful
        /// Returns null if authorization is not successful
        /// </returns>
        private async Task<AccessToken> AuthorizeAsync(Dictionary<string, string> content)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient
                    .PostAsync("https://login.uber.com/oauth/token", new FormUrlEncodedContent(content))
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<AccessToken>(responseContent);
            }
        }

        /// <summary>
        /// Revoke a user's access to the Uber API via the application.
        /// </summary>
        /// <param name="accessToken">
        /// The access token being revoked.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        public async Task<bool> RevokeAccessTokenAsync(string accessToken)
        {
            var formData = new Dictionary<string, string>
			{
				{ "client_id", _clientId },
				{ "client_secret", _clientSecret },
				{ "token", accessToken }
			};

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient
                    .PostAsync("https://login.uber.com/oauth/revoke", new FormUrlEncodedContent(formData))
                    .ConfigureAwait(false);

                return response.IsSuccessStatusCode;
            }
        }

        /// <summary>
        /// Gets the products available at a given location.
        /// </summary>
        /// <param name="latitude">
        /// The location latitude.
        /// </param>
        /// <param name="longitude">
        /// The location longitude.
        /// </param>
        /// <returns>
        /// Returns a <see cref="ProductCollection"/>.
        /// </returns>
        public async Task<UberResponse<ProductCollection>> GetProductsAsync(float latitude, float longitude)
        {
            var url = string.Format(
                "/v1/products?latitude={0}&longitude={1}", 
                latitude, longitude);

            return await GetAsync<ProductCollection>(url);
        }

        public async Task<UberResponse<PriceEstimateCollection>> GetPriceEstimateAsync(float startLatitude, float startLongitude, float endLatitude, float endLongitude)
        {
            var url = string.Format(
                "/v1/estimates/price?start_latitude={0}&start_longitude={1}&end_latitude={2}&end_longitude={3}",
                startLatitude, startLongitude, endLatitude, endLongitude);

            return await GetAsync<PriceEstimateCollection>(url);
        }

        /// <summary>
        /// Gets an ETA for each product available at a given location.
        /// </summary>
        /// <param name="startLatitude">
        /// The start location latitude.
        /// </param>
        /// <param name="startLongitude">
        /// The start location longitude.
        /// </param>
        /// <param name="customerId">
        /// Optional customer ID. Uber documentation describes as a "Unique customer identifier to be used for experience customization."
        /// </param>
        /// <param name="productId">
        /// Optional product ID to filter results.
        /// </param>
        /// <returns>
        /// Returns a <see cref="TimeEstimateCollection"/>.
        /// </returns>
        public async Task<UberResponse<TimeEstimateCollection>> GetTimeEstimateAsync(float startLatitude, float startLongitude, string customerId = null, string productId = null)
        {
            var url = string.Format(
                "/v1/estimates/time?start_latitude={0}&start_longitude={1}",
                startLatitude, startLongitude);

            if (!string.IsNullOrWhiteSpace(customerId))
            {
                url += string.Format("&customer_uuid={0}", customerId);
            }

            if (!string.IsNullOrWhiteSpace(productId))
            {
                url += string.Format("&product_id={0}", productId);
            }

            return await GetAsync<TimeEstimateCollection>(url);
        }

        /// <summary>
        /// Gets a promotion available to new users based on location.
        /// </summary>
        /// <param name="startLatitude">
        /// The start location latitude.
        /// </param>
        /// <param name="startLongitude">
        /// The start location longitude.
        /// </param>
        /// <param name="endLatitude">
        /// The end location latitude.
        /// </param>
        /// <param name="endLongitude">
        /// The end location longitude.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Promotion"/>.
        /// </returns>
        public async Task<UberResponse<Promotion>> GetPromotionAsync(float startLatitude, float startLongitude, float endLatitude, float endLongitude)
        {
            var url = string.Format(
                "/v1/promotions?start_latitude={0}&start_longitude={1}&end_latitude={2}&end_longitude={3}",
                startLatitude, startLongitude, endLatitude, endLongitude);

            return await GetAsync<Promotion>(url);
        }

        /// <summary>
        /// Gets a list of the user's Uber activity.
        /// </summary>
        /// <param name="offset">
        /// The results offset.
        /// </param>
        /// <param name="limit">
        /// The results limit.
        /// </param>
        /// <returns>
        /// Returns a <see cref="UserActivity"/>.
        /// </returns>
        public async Task<UberResponse<UserActivity>> GetUserActivityAsync(int offset, int limit)
        {
            var url = string.Format(
                "/v1.1/history?offset={0}&limit={1}",
                offset, limit);

            return await GetAsync<UserActivity>(url);
        }

        /// <summary>
        /// Gets the user's Uber profile.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="UserProfile"/>.
        /// </returns>
        public async Task<UberResponse<UserProfile>> GetUserProfileAsync()
        {
            var url = string.Format("/v1/me");

            return await GetAsync<UserProfile>(url);
        }

        /// <summary>
        /// Makes a pickup request.
        /// </summary>
        /// <param name="productId">
        /// The product ID.
        /// </param>
        /// <param name="startLatitude">
        /// The start location latitude.
        /// </param>
        /// <param name="startLongitude">
        /// The start location longitude.
        /// </param>
        /// <param name="endLatitude">
        /// The end location latitude.
        /// </param>
        /// <param name="endLongitude">
        /// The end location longitude.
        /// </param>
        /// <param name="surgeConfirmationId">
        /// The surge pricing confirmation ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="Request"/>.
        /// </returns>
        public async Task<UberResponse<Request>> RequestAsync(string productId, float startLatitude, float startLongitude, float endLatitude, float endLongitude, string surgeConfirmationId = null)
        {
            var url = "/v1/requests";

            var postData = new Dictionary<string, string>
            {
                { "product_id", productId },
                { "start_latitude", startLatitude.ToString("0.00000") },
                { "start_longitude", startLongitude.ToString("0.00000") },
                { "end_latitude", endLatitude.ToString("0.00000") },
                { "end_longitude", endLongitude.ToString("0.00000") },
            };

            if (!string.IsNullOrWhiteSpace(surgeConfirmationId))
            {
                postData.Add("surge_confirmation_id", surgeConfirmationId);
            }

            var content = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

            return await PostAsync<Request>(url, content);
        }

        /// <summary>
        /// Gets a request details.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestDetails"/>.
        /// </returns>
        public async Task<UberResponse<RequestDetails>> GetRequestDetailsAsync(string requestId)
        {
            var url = string.Format("/v1/requests/{0}", requestId);

            return await GetAsync<RequestDetails>(url);
        }

        /// <summary>
        /// Gets the map for a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestMap"/>.
        /// </returns>
        public async Task<UberResponse<RequestMap>> GetRequestMap(string requestId)
        {
            var url = string.Format("/v1/requests/{0}/map", requestId);

            return await GetAsync<RequestMap>(url);
        }

        /// <summary>
        /// Cancels a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        public async Task<UberResponse<bool>> CancelRequestAsync(string requestId)
        {
            var url = string.Format("/v1/requests/{0}", requestId);

            return await DeleteAsync(url);
        }

        /// <summary>
        /// Makes a GET request.
        /// </summary>
        /// <typeparam name="T">
        /// The response data type.
        /// </typeparam>
        /// <param name="url">
        /// The URL being requested.
        /// </param>
        /// <returns>
        /// Returns a <see cref="T"/>.
        /// </returns>
        private async Task<UberResponse<T>> GetAsync<T>(string url)
        {
            var uberResponse = new UberResponse<T>();

            var response = await _httpClient
                .GetAsync(url)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                uberResponse.Data = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                uberResponse.Error = JsonConvert.DeserializeObject<UberError>(responseContent);
            }

            return uberResponse;
        }

        /// <summary>
        /// Makes a POST request.
        /// </summary>
        /// <typeparam name="T">
        /// The response data type.
        /// </typeparam>
        /// <param name="url">
        /// The URL being requested.
        /// </param>
        /// <param name="content">
        /// The content being POST-ed.
        /// </param>
        /// <returns>
        /// Returns a <see cref="T"/>.
        /// </returns>
        private async Task<UberResponse<T>> PostAsync<T>(string url, HttpContent content)
        {
            var uberResponse = new UberResponse<T>();

            var response = await _httpClient
                .PostAsync(url, content)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                uberResponse.Data = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                uberResponse.Error = JsonConvert.DeserializeObject<UberError>(responseContent);
            }

            return uberResponse;
        }

        /// <summary>
        /// Makes a PUT request.
        /// </summary>
        /// <typeparam name="T">
        /// The response data type.
        /// </typeparam>
        /// <param name="url">
        /// The URL being requested.
        /// </param>
        /// <param name="content">
        /// The content being PUT-ed.
        /// </param>
        /// <returns>
        /// Returns a <see cref="T"/>.
        /// </returns>
        private async Task<UberResponse<T>> PutAsync<T>(string url, HttpContent content)
        {
            var uberResponse = new UberResponse<T>();

            var response = await _httpClient
                .PostAsync(url, content)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                uberResponse.Data = JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                uberResponse.Error = JsonConvert.DeserializeObject<UberError>(responseContent);
            }

            return uberResponse;
        }

        /// <summary>
        /// Makes a DELETE request.
        /// </summary>
        /// <param name="url">
        /// The URL being requested.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        private async Task<UberResponse<bool>> DeleteAsync(string url)
        {
            var uberResponse = new UberResponse<bool>();

            var response = await _httpClient
                .DeleteAsync(url)
                .ConfigureAwait(false);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                uberResponse.Data = true;
            }
            else
            {
                uberResponse.Error = JsonConvert.DeserializeObject<UberError>(responseContent);
            }

            return uberResponse;
        }
    }
}
