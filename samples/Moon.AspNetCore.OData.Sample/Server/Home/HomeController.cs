using Microsoft.AspNetCore.Mvc;
using Moon.OData;
using Moon.OData.Sql;

namespace Moon.AspNetCore.OData.Sample.Server.Home
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