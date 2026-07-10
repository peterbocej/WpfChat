using WpfChat.WebApi.Data;
using WpfChat.Domain.Model;

using Microsoft.EntityFrameworkCore;

namespace WpfChat.WebApi.Repositories
{
    public interface IMessagesRepository
    {
        Task<ICollection<Message>> GetMessagesAsync();
        Task<Message?> GetMessageAsync(int id);
        Task<Message?> AddAsync(Message message);
        Task<int> SaveChangesAsync();
        Task<ICollection<Message>> GetLastMessagesAsync(int lastId);
    }
    public class MessagesRepository : IMessagesRepository
    {
        private readonly AppDbContext _context;
        public MessagesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> AddAsync(Message message)
        {
            var retMsg = await _context.Messages.AddAsync(message);
            if (retMsg == null || retMsg.State != EntityState.Added)
                throw new ApplicationException("Error adding message.");

            return retMsg.Entity;
        }

        public async Task<ICollection<Message>> GetLastMessagesAsync(int lastId)
        {
            return await _context.Messages
                .Where(m => m.MessageId > lastId)
                .OrderByDescending(m => m.Time)
                .Select(m => m)
                .ToListAsync();
        }

        public async Task<Message?> GetMessageAsync(int id)
        {
            return await _context.Messages.FindAsync(id);
        }

        public async Task<ICollection<Message>> GetMessagesAsync()
        {
            return await _context.Messages
                .OrderByDescending(m => m.Time)
                .Select(m => m)
                .ToListAsync();
        }



        public async Task<int> SaveChangesAsync()
        {
            var ret = await _context.SaveChangesAsync();
            if (ret == 0)
                throw new ApplicationException("Error saving message to db.");

            return ret;
        }
    }
}
