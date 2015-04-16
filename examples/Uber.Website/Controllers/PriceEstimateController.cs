using System.Threading.Tasks;
using System.Web.Mvc;
using Uber.SDK;
using Uber.Website.Helpers;

namespace Uber.Website.Controllers
{
    public class PriceEstimateController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(float startlat, float startlng, float endlat, float endlng)
        {
            var uberClient = UberClientHelper.Get();
            var prices = await uberClient.GetPriceEstimateAsync(startlat, startlng, endlat, endlng);

            return View("Results", prices.Data);
        }
    }
}