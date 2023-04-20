using LetsChat.Hubs;
using LetsChat.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LetsChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Authentication : ControllerBase
    {
        private IAuthentication _authentication;
        private readonly IHubContext<ChatHub> _hubContext;

        public Authentication(IAuthentication authentication, IHubContext<ChatHub> hubContext)
        {
            _authentication = authentication;
            _hubContext = hubContext;
        }

        // GET: api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> LoginASync(string Mobile)
        {
            if (Mobile != null)
            {
                var result = await _authentication.LoginUserAsync(Mobile);
                if (result.success)
                {
                    await _hubContext.Clients.All.SendAsync("LoggedIn", Mobile, result.Message);
                    return Ok(result);
                }
                return BadRequest(result);
            }
            return BadRequest("Invalid Parameter"); //status code = 400
        }

        
    }
}
