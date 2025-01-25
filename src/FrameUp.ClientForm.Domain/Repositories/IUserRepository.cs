using FrameUp.ClientForm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
