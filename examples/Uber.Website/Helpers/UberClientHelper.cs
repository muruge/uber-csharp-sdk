using System.Configuration;
using Uber.SDK;
using Uber.SDK.Models;

namespace Uber.Website.Helpers
{
    public class UberClientHelper
    {
        public static UberClient Get(string clientToken = null)
        {
            var serverToken = ConfigurationManager.AppSettings["UBER.ServerToken"];
            var clientId = ConfigurationManager.AppSettings["UBER.ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["UBER.ClientSecret"];
            var baseUri = ConfigurationManager.AppSettings["UBER.BaseUri"];

            return  string.IsNullOrWhiteSpace(clientToken) 
                ? new UberClient(AccessTokenType.Server, serverToken, clientId, clientSecret, baseUri)
                : new UberClient(AccessTokenType.Client, clientToken, clientId, clientSecret, baseUri);
        }

        public static UberSandboxClient GetSandbox(string clientToken)
        {
            var serverToken = ConfigurationManager.AppSettings["UBER.ServerToken"];
            var clientId = ConfigurationManager.AppSettings["UBER.ClientId"];
            var clientSecret = ConfigurationManager.AppSettings["UBER.ClientSecret"];
            var baseUri = ConfigurationManager.AppSettings["UBER.BaseUri"];

            return string.IsNullOrWhiteSpace(clientToken)
                ? new UberSandboxClient(AccessTokenType.Server, serverToken, clientId, clientSecret, baseUri)
                : new UberSandboxClient(AccessTokenType.Client, clientToken, clientId, clientSecret, baseUri);
        }
    }
}