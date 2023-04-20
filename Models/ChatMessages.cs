using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace LetsChat.Models
{
    public class ChatMessages
    {
        [Key]
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
