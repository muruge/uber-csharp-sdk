using System.Web;
using Newtonsoft.Json;
using Uber.SDK.Models;

namespace Uber.Website.Helpers
{
    public class CookieHelper
    {
        private const string COOKIE_KEY = "UBER|ACCESSTOKEN";

        public static AccessToken GetAccessToken()
        {
            var cookie = HttpContext.Current.Request.Cookies.Get(COOKIE_KEY);

            return cookie != null && !string.IsNullOrWhiteSpace(cookie.Value)
                ? JsonConvert.DeserializeObject<AccessToken>(cookie.Value)
                : null;
        }

        public static void SetAccessToken(AccessToken accessToken)
        {
            var accessTokenJson = JsonConvert.SerializeObject(accessToken);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(COOKIE_KEY, accessTokenJson));
        }
    }
}