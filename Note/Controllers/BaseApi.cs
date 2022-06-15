using Microsoft.AspNetCore.Mvc;

namespace Note.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApi : ControllerBase
    {
        public BaseApi()
        {
        }
    }
}