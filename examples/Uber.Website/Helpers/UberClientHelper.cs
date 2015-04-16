using System.Configuration;
using Uber.SDK;
using Uber.SDK.Models;

namespace Uber.Website.Helpers
{
    public class UberClientHelper
    {
        public static UberAuthenticationClient GetAuth()
        {
            var clientId = ConfigurationManager.AppSettings["UBER.ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["UBER.ClientSecret"];

            return new UberAuthenticationClient(clientId, clientSecret);
        }

        public static UberSandboxClient Get(string clientToken = null)
        {
            var serverToken = ConfigurationManager.AppSettings["UBER.ServerToken"];

            return string.IsNullOrWhiteSpace(clientToken)
                ? new UberSandboxClient(AccessTokenType.Server, serverToken)
                : new UberSandboxClient(AccessTokenType.Client, clientToken);
        }
    }
}