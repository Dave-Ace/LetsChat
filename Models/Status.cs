using System.ComponentModel.DataAnnotations;

namespace LetsChat.Models
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string userId { get; set; }
        public string status { get; set; }
        public DateTime expire { get; set; }
    }
}
