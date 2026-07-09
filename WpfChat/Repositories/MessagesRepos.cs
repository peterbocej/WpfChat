using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;

using WpfChat.Data;
using WpfChat.Model;

namespace WpfChat.Repositories
{
    public interface IMessagesRepos
    {
        ICollection<Message> GetConversation(string user1, string user2);
        Task<ICollection<Message>> GetConversationAsync(string user1, string user2);
    }
    public class MessagesRepos : IMessagesRepos
    {
        private readonly AppDbContext _context;
        public MessagesRepos(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Message> GetConversation(string user1, string user2)
        {
            return GetConversationAsync(user1, user2).Result;
        }
        public async Task<ICollection<Message>> GetConversationAsync(string user1, string user2)
        {
            string[] users = [user1, user2];
            return await _context.Messages
                .Where(m => (m.From == user1 || m.From == user2) && (m.To == user1 || m.To == user2))
                .OrderBy(m => m.Time)
                .Select(m => m)
                .ToListAsync();
        }
    }
}
