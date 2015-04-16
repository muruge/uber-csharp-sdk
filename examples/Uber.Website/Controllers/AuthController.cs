using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uber.SDK;
using Uber.Website.Helpers;

namespace Uber.Website.Controllers
{
    public class AuthController : Controller
    {
        public ActionResult Index()
        {
            var scopes = new List<string> { "profile", "history_lite", "request" };

            var uberClient = UberClientHelper.Get();
            var response = uberClient.GetAuthorizeUrl(scopes, Guid.NewGuid().ToString());

            return View("Index", (object)response);
        }

        public async Task<ActionResult> Callback(string code)
        {
            var uberClient = UberClientHelper.Get();
            var accessToken = await uberClient.GetAccessToken(code, "http://localhost:7090/auth/callback");

            if (accessToken == null || string.IsNullOrWhiteSpace(accessToken.Value))
            {
                return RedirectToAction("Index");
            }

            CookieHelper.SetAccessToken(accessToken);

            return View();
        }
    }
}
