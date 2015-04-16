# Uber C# SDK

## Summary

* Simple wrapper of the [Uber API](http://developer.uber.com)
* You need an app.
* All methods are async.
* All methods return an instance of UberResponse<T> where T is the serialized Uber response data. An UberResponse also contains an Error property which will contain information in the event of an error being returned by the underlying API.
* Contains an UberSandboxClient for submitting requests against the Uber API sandbox.

## Authentication

When making requests on behalf of a user via OAuth, ensure you understand [scopes.](https://developer.uber.com/v1/api-reference/#scopes)

The following methods can use client OR server authentication:

* GetProductsAsync
* GetPriceEstimateAsync
* GetTimeEstimateAsync
* GetPromotionAsync

The following methods require client (OAuth) authentication:

* GetUserActivityAsync
* GetUserProfileAsync
* RequestAsync
* GetRequestDetailsAsync
* GetRequestMapAsync
* CancelRequestAsync

### Server authentication
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken, clientId, clientSecret, baseUri);
```
### Client authentication
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken, clientId, clientSecret, baseUri);
```
The web application example demonstrates how to authenticate users and execute each method.
### OAuth flow
```
// Create authentication client
var authClient = new UberAuthenticationClient(clientId, clientSecret);
// Generate authorize URL - If you don't specify scope, state or redirect URI then app defaults will be used
var authorizeUrl = authClient.GetAuthorizeUrl();
// Once authorized, swap authorization code for an access token
var accessToken = await authClient.GetAccessTokenAsync(code, "http://your-redirect-uri.com");
// You can now make requests on behalf of the user
```
***
### GetAuthorizeUrl
[Uber Docs](https://developer.uber.com/v1/auth/)
```
var authClient = new UberAuthenticationClient(clientId, clientSecret);
var url = uberClient.GetAuthorizeUrl();
```
### GetAccessTokenAsync
[Uber Docs](https://developer.uber.com/v1/auth/)
```
var authClient = new UberAuthenticationClient(clientId, clientSecret);
var accessToken = await uberClient.GetAccessTokenAsync(code, "http://your-redirect-uri.com");
```
### RefreshAccessTokenAsync
[Uber Docs](https://developer.uber.com/v1/auth/)
```
var authClient = new UberAuthenticationClient(clientId, clientSecret);
var accessToken = await uberClient.RefreshAccessTokenAsync(refreshToken, "http://your-redirect-uri.com");
```
### RevokeAccessTokenAsync
[Uber Docs](https://developer.uber.com/v1/auth/)
```
var authClient = new UberAuthenticationClient(clientId, clientSecret);
var revoked = await uberClient.RevokeAccessTokenAsync(accessToken);
```
***
### GetProductsAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#product-types)
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken);
var products = await uberClient.GetProductsAsync(startLatitude, startLongitude);
```
### GetPriceEstimateAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#price-estimates)
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken);
var prices = await uberClient.GetPriceEstimateAsync(startLatitude, startLongitude, endLatitude, endLongitude);
```
### GetTimeEstimateAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#time-estimates)
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken);
var times = await uberClient.GetTimeEstimateAsync(startLatitude, startLongitude);
```
### GetPromotionAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#promotions)
```
var uberClient = new UberClient(AccessTokenType.Server, serverToken);
var promotion = await uberClient.GetPromotionAsync(startLatitude, startLongitude, endLatitude, endLongitude);
```
### GetUserActivityAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#user-activity-v1-1)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var userActivity = await uberClient.GetUserActivityAsync(0, 50);
```
### GetUserProfileAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#user-profile)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var userProfile = await uberClient.GetUserProfileAsync();
```
### RequestAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#request)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var request = await uberClient.RequestAsync(productId, startLatitude, startLongitude, endLatitude, endLongitude);
```
### GetRequestDetailsAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#request-details)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var requestDetails = await uberClient.GetRequestDetailsAsync(requestId);
```
### GetRequestMapAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#request-map)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var request = await uberClient.GetRequestMapAsync(requestId);
```
### CancelRequestAsync
[Uber Docs](https://developer.uber.com/v1/endpoints/#request-cancel)
```
var uberClient = new UberClient(AccessTokenType.Client, clientToken);
var cancel = await uberClient.CancelRequestAsync(requestId);
```