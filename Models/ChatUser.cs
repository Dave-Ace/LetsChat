using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LetsChat.Models
{
    public class ChatUser : IdentityUser
    {
        [Key]
        public string UserId { get; set; }
        public string displayName { get; set; }
        public string? about { get; set; }
    }
}
