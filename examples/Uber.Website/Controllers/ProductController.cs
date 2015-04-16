using System.Threading.Tasks;
using System.Web.Mvc;
using Uber.SDK;
using Uber.Website.Helpers;

namespace Uber.Website.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(float startlat, float startlng)
        {
            var uberClient = UberClientHelper.Get();
            var products = await uberClient.GetProductsAsync(startlat, startlng);

            return View("Results", products.Data);
        }
    }
}