using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LetsChat.Models
{
    public class LetsChatDbContext : IdentityDbContext<ChatUser>
    {
        public LetsChatDbContext(DbContextOptions<LetsChatDbContext> options) : base(options)
        {

        }
        public DbSet<ChatUser> user { get; set; }

        public DbSet<ChatMessages> messages { get; set; }
        public DbSet<Status> User_status { get; set; }
        public DbSet<UserConnection> User_connection { get; set; }
    }

    /*public class WhatsappContext : DbContext
    {
        public WhatsappContext(DbContextOptions<WhatsappContext> options) : base(options)
        {

        }
        
    }*/
}

