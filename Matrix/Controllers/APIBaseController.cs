using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Matrix.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class APIBaseController : ControllerBase
    {
        protected string CurrentUserName
        {
            get
            {
                ClaimsPrincipal principal = HttpContext.User;
                string strUserName = principal.FindFirstValue(ClaimTypes.Name);
                return strUserName;
            }
        }

        protected int CurrentUserId
        {
            get
            {
                ClaimsPrincipal principal = HttpContext.User;
                int strUserId = Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));
                return strUserId;
            }
        }
    }
}
