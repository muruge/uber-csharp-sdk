using System.Threading.Tasks;
using System.Web.Mvc;
using Uber.SDK;
using Uber.Website.Helpers;

namespace Uber.Website.Controllers
{
    public class UserController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var userProfile = await uberClient.GetUserProfileAsync();

            return View(userProfile.Data);
        }
    }
}