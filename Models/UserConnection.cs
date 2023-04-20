using System.ComponentModel.DataAnnotations;

namespace LetsChat.Models
{
    public class UserConnection
    {
        [Key]
        public int Id { get; set; }
        public string UserId {  get; set; }
        public string Second_userId { get; set; }
    }
}
