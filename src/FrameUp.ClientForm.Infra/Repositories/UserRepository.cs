using MongoDB.Driver;
using FrameUp.ClientForm.Domain.Entities;
using FrameUp.ClientForm.Domain.Interfaces;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Infra.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient client)
        {
            var database = client.GetDatabase("YourDatabaseName"); //to do
            _users = database.GetCollection<User>("Users");
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _users.Find(filter).AnyAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await _users.Find(filter).SingleOrDefaultAsync();
        }
    }
}
