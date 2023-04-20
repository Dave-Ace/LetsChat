using LetsChat.Models;
using LetsChat.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LetsChat.Interface
{
    public interface IAuthentication
    {
        Task<AuthResponseManager> LoginUserAsync(string Mobile);
    }

    public class Authentication : IAuthentication
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly IConfiguration _configuration;

        public Authentication(UserManager<ChatUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseManager> LoginUserAsync(string Mobile)
        {
            if (Mobile == null)
                throw new NullReferenceException("Mobile is NUll");
           
            var getUser = await _userManager.FindByIdAsync(Mobile);
           if (getUser == null)
            {
                var Newuser = new ChatUser
                {
                    Id = Mobile,
                    about = "Hey! There I am using Whatsapp",
                    PhoneNumber = Mobile,
                    displayName = "user" + Mobile,
                    UserName = "user" + Mobile
                };

                var result = await _userManager.CreateAsync(Newuser);
            }
            var user = await _userManager.FindByIdAsync(Mobile);


            var claims = new[]
            {
                new Claim("UserName", user.UserName),
                new Claim("mobile", user.PhoneNumber),
                new Claim("displayName", user.displayName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:key"]));

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                issuer: "https://localhost:7186/",
                audience: "https://localhost:7186/"
                );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseManager
            {
                Message = tokenAsString,
                success = true,
                Expires = token.ValidTo
            };
        }
    }

}
