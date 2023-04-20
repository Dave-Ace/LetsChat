using LetsChat.Hubs;
using LetsChat.Interface;
using LetsChat.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;

namespace LetsChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class Chat : Controller
    {   

        private IChat _chat;
        private readonly IHubContext<ChatHub> _hubContext;
        public Chat(IChat chat, IHubContext<ChatHub> hubContext)
        {
            _chat = chat;
            _hubContext = hubContext;
        }

        //api/chat
        [HttpPost]
        public async Task<IActionResult> ChatAPI(string receiverId, string message)
        {
            string mobile=null;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(accessToken);
            var claims = decodedValue.Claims;
            foreach (var i in claims)
            {
                if (i.Type == "mobile")
                {
                    mobile = i.Value;
                }
            }
            /*var mobile = from i in claims
                         where i.Type == "mobile"
                         select i.Value;*/
            var result = await _chat.SendMessage(mobile, receiverId, message);
            var connected = await _chat.Connect(mobile, receiverId);

            if (result.status == "Success")
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", mobile.ToString(), message);
            
            return Ok(result);
        }

        //api/chat/fetch
        [HttpGet("FetchMessage")]
        public async Task<IActionResult> FetchMessages(string receiverId)
        {
            string mobile = null;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(accessToken);
            var claims = decodedValue.Claims;
            foreach (var i in claims)
            {
                if (i.Type == "mobile")
                {
                    mobile = i.Value;
                }
            }
            /*var mobile = from i in claims
                         where i.Type == "mobile"
                         select i.Value;*/
            var result = await _chat.FetchMessage(receiverId, mobile);
            await _hubContext.Clients.All.SendAsync("fetchMessages", result);
            return Ok(result);

        }

        //api/chat/CreateStatus
        [HttpPost("CreateStatus")]
        public async Task<IActionResult> CreateStatus(string status)
        {
            string mobile = null;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(accessToken);
            var claims = decodedValue.Claims;
            foreach (var i in claims)
            {
                if (i.Type == "mobile")
                {
                    mobile = i.Value;
                }
            }
            var result = await _chat.CreateStatus(status, mobile);
            return Ok(result);
        }

        //api/chat/ContactStatus/
        [HttpGet("ContactStatus")]
        public async Task<IActionResult> ContactStatus()
        {
            string mobile = null;
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(accessToken);
            var claims = decodedValue.Claims;
            foreach (var i in claims)
            {
                if (i.Type == "mobile")
                {
                    mobile = i.Value;
                }
            }
            var result = await _chat.ContactStatus(mobile);
            return Ok(result);
        }
    }
}
