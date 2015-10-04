using Microsoft.AspNet.Mvc;
using Moon.OData;
using Moon.OData.Sql;

namespace Moon.AspNet.OData.Sample.Server.Home
{
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index(ODataOptions<Entity> options)
        {
            return View(new ODataSqlQuery(
                "SELECT FROM Entities WHERE OwnerId = @p0",
                10456, options
            ));
        }
    }
}