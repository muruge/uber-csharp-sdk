using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Uber.SDK;
using Uber.Website.Helpers;

namespace Uber.Website.Controllers
{
    public class RequestController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var userActivity = await uberClient.GetUserActivityAsync(0, 50);

            return View(userActivity.Data);
        }

        public async Task<ActionResult> Show(string requestId)
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var request = await uberClient.GetRequestDetailsAsync(requestId);

            return View(request.Data);
        }

        public async Task<ActionResult> Create()
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var products = await uberClient.GetProductsAsync(-37.8602828f, 145.079616f);
            ViewBag.ProductId = products.Data.Products.First().ProductId;

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(string productId, float startlat, float startlng, float endlat, float endlng)
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var request = await uberClient.RequestAsync(productId, startlat, startlng, endlat, endlng);

            return RedirectToAction("Show", new { requestId = request.Data.RequestId });
        }

        public async Task<ActionResult> Cancel(string requestId)
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var request = await uberClient.CancelRequestAsync(requestId);

            return View(request.Data);
        }

        public async Task<ActionResult> Map(string requestId)
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            var request = await uberClient.GetRequestMapAsync(requestId);

            return View(request.Data);
        }

        public async Task<ActionResult> UpdateStatus(string requestId, string status)
        {
            var auth = CookieHelper.GetAccessToken();
            if (auth == null)
            {
                return RedirectToAction("Index", "Auth");
            }

            var uberClient = UberClientHelper.Get(auth.Value);
            await uberClient.UpdateRequestStatus(requestId, status);

            return RedirectToAction("Show", new { requestId = requestId });
        }
    }
}
