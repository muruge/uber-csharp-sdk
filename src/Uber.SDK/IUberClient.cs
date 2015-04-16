using System.Collections.Generic;
using System.Threading.Tasks;
using Uber.SDK.Models;

namespace Uber.SDK
{
    public interface IUberClient
    {
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
        string GetAuthorizeUrl(List<string> scopes = null, string state = null, string redirectUrl = null);

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
        /// Returns an <see cref="AccessToken"/>.
        /// </returns>
        Task<AccessToken> GetAccessToken(string authorizationCode, string redirectUri);

        /// <summary>
        /// Exchange this authorization code for an AccessToken, allowing requests to be mande on behalf of a user.
        /// </summary>
        /// <param name="refreshToken">
        /// The refresh token.
        /// </param>
        /// <param name="redirectUri">
        /// The URL the user should be redrected back to 
        /// </param>
        /// <returns>
        /// Returns an <see cref="AccessToken"/>.
        /// </returns>
        Task<AccessToken> RefreshAccessToken(string refreshToken, string redirectUri);

        /// <summary>
        /// Revoke a user's access to the Uber API via the application.
        /// </summary>
        /// <param name="accessToken">
        /// The access token being revoked.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        Task<bool> RevokeAccessTokenAsync(string accessToken);

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
        Task<UberResponse<ProductCollection>> GetProductsAsync(float latitude, float longitude);

        /// <summary>
        /// Gets an estimated price range for each product available at a given location.
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
        /// Returns a <see cref="PriceEstimateCollection"/>.
        /// </returns>
        Task<UberResponse<PriceEstimateCollection>> GetPriceEstimateAsync(float startLatitude, float startLongitude, float endLatitude, float endLongitude);

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
        Task<UberResponse<TimeEstimateCollection>> GetTimeEstimateAsync(float startLatitude, float startLongitude, string customerId, string productId);
         
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
        Task<UberResponse<Promotion>> GetPromotionAsync(float startLatitude, float startLongitude, float endLatitude, float endLongitude);

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
        Task<UberResponse<UserActivity>> GetUserActivityAsync(int offset, int limit);

        /// <summary>
        /// Gets the user's Uber profile.
        /// </summary>
        /// <returns>
        /// Returns a <see cref="UserProfile"/>.
        /// </returns>
        Task<UberResponse<UserProfile>> GetUserProfileAsync();

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
        Task<UberResponse<Request>> RequestAsync(string productId, float startLatitude, float startLongitude, float endLatitude, float endLongitude, string surgeConfirmationId = null);

        /// <summary>
        /// Gets a request details.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestDetails"/>.
        /// </returns>
        Task<UberResponse<RequestDetails>> GetRequestDetailsAsync(string requestId);

        /// <summary>
        /// Gets the map for a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a <see cref="RequestMap"/>.
        /// </returns>
        Task<UberResponse<RequestMap>> GetRequestMap(string requestId);

        /// <summary>
        /// Cancels a given request.
        /// </summary>
        /// <param name="requestId">
        /// The request ID.
        /// </param>
        /// <returns>
        /// Returns a boolean indicating if the Uber API returned a successful HTTP status.
        /// </returns>
        Task<UberResponse<bool>> CancelRequestAsync(string requestId);
    }
}