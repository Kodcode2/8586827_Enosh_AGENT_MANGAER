using AgentTargetRest.Dto;
using AgentTargetRest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace AgentTargetRest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IJwtService jwtService) : Controller
    {
        private static readonly ImmutableList<string> allowedNames = [
            "enosh", "avi"
        ];

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginDto loginDto) =>
            allowedNames.Contains(loginDto.Name)
                ? Ok(jwtService.CreateToken(loginDto.Name))
                : BadRequest();

        [Authorize]
        [HttpGet("protected")]
        public ActionResult<string> Protected()
        {
            return Ok("Yay!!");
        }
    }
}

/*public class LoginController(IJwtService jwtService) : Controller
{
    [HttpPost("create-token")]
    public ActionResult<string> CreateToken([FromBody] LoginDto login)
    {
        return Ok(jwtService.GenerateToken(login));
    }
}
*/