using Microsoft.AspNetCore.Mvc;
using Store.Services.HandleResponses;

namespace Store.API.Controllers
{
    [Route("errors/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        public ActionResult Error(int Code)
            => NotFound(new Response(Code));

    }
}
