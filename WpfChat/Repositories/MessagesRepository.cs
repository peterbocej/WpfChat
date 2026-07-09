using Microsoft.EntityFrameworkCore;

using WpfChat.Data;
using WpfChat.Model;

namespace WpfChat.Repositories
{
    public interface IMessagesRepository
    {
        ICollection<Message> GetConversation();
        Task<ICollection<Message>> GetConversationAsync();
    }
    public class MessagesRepository : IMessagesRepository
    {
        private readonly AppDbContext _context;
        public MessagesRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICollection<Message> GetConversation()
        {
            return GetConversationAsync().Result;
        }
        public async Task<ICollection<Message>> GetConversationAsync()
        {
            return await _context.Messages
                .OrderBy(m => m.Time)
                .Select(m => m)
                .ToListAsync();
        }
    }
}
