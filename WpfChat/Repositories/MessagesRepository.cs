using WpfChat.Data;
using WpfChat.Model;

namespace WpfChat.Repositories
{
    public interface IMessagesRepository
    {
        ICollection<Message> GetConversation();
        void Add(Message message);
        int SaveChanges();
    }
    public class MessagesRepository : IMessagesRepository
    {
        private readonly AppDbContext _context;
        public MessagesRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Message message)
        {
            _context.Messages.Add(message);
        }

        public ICollection<Message> GetConversation()
        {
            return _context.Messages
                .OrderByDescending(m => m.Time)
                .Select(m => m)
                .ToList();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
