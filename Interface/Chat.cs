using Azure.Core;
using LetsChat.Models;
using LetsChat.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

namespace LetsChat.Interface
{
    public interface IChat
    {
        Task<ChatResponse> SendMessage(string senderId, string receiverId, string message);

        /*void Connect(String mobile, string RecieverId);*/
        Task<ChatMessageList> FetchMessage(string receiverId, string mobile);
        Task<ChatResponse> CreateStatus(string status, string mobile);
        Task<ContactStatuses> ContactStatus(string mobile);
        Task<Boolean> Connect(string mobile, string receiverId);
    }

    public class Chat : IChat
    {
        public readonly LetsChatDbContext _context;
        private readonly UserManager<ChatUser> _userManager;

        public Chat(LetsChatDbContext context, UserManager<ChatUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ChatResponse> SendMessage(string senderId, string receiverId, string message)
        {
            if (message == null)
                throw new NullReferenceException("Invalid parameter (NULL)");

            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null)
                return new ChatResponse
                {
                    Message = "User does not exist",
                    status = "false"
                };

            var Chatmessage = new ChatMessages
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                TimeStamp = DateTime.Now
            };
            await _context.AddAsync(Chatmessage);
            await _context.SaveChangesAsync();
            return new ChatResponse
            {
                status = "Success"
            };
        }

        public async Task<ChatMessageList> FetchMessage(string receiverId, string mobile)
        {
            var Messages = from m in _context.messages
                           where m.ReceiverId == receiverId | m.ReceiverId == mobile
                           where m.SenderId == mobile | m.SenderId == receiverId
                           select m;

            var MessageList = new ChatMessageList
            {
                ChatMessages = await Messages.ToListAsync(),
            };
            return MessageList;
        }

        public async Task<ChatResponse> CreateStatus(string status, string mobile)
        {
            if (status == null)
                throw new NullReferenceException("Invalid Parameter - Null");

            var user = _userManager.FindByIdAsync(mobile);
            if (user == null)
                return new ChatResponse
                {
                    Message = "User does not exist",
                    status = "error"
                };

            var createStatus = new Status
            {
                userId = mobile,
                status = status,
                expire = DateTime.Now.AddHours(24)
            };

            await _context.AddAsync(createStatus);
            await _context.SaveChangesAsync();
            return new ChatResponse
            {
                Message = "Status Uploaded",
                status = "Success"
            };
        }

        public async Task<ContactStatuses> ContactStatus(string mobile)
        {
            var userStatus = from m in _context.User_status
                             select m;

            List<Status> StatusList = new List<Status>();

            foreach (var status in userStatus)
            {
                var Userconnect = _context.User_connection.FirstOrDefault(m => (m.UserId == status.userId | m.Second_userId == status.userId) & (m.UserId == mobile | m.Second_userId == mobile));
                
                if (Userconnect != null)
                    StatusList.Add(status);
            };

            ContactStatuses contactStatuses1 = new ContactStatuses()
            {
                Status = StatusList,
            };


            return contactStatuses1;
        }

        public async Task<Boolean> Connect(string mobile, string receiverId)
        {
            var connectionExist = _context.User_connection.FirstOrDefault(
                m => (m.UserId == mobile | m.Second_userId == mobile) & (m.UserId == receiverId | m.Second_userId == receiverId)
                );

            if (connectionExist == null)
            {
                var chatmessage = from m in _context.messages
                                  where (m.SenderId == receiverId | m.ReceiverId == receiverId) & (m.ReceiverId == mobile | m.SenderId == mobile)
                                  select m;

                if (chatmessage.Any())
                {
                    var connect = new UserConnection()
                    {
                        UserId = mobile,
                        Second_userId = receiverId
                    };
                    await _context.AddAsync(connect);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            return false;
        }
    }
}

