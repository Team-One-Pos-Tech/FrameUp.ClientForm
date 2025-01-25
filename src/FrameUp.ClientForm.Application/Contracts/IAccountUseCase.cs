using FrameUp.ClientForm.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Application.Contracts
{
    public interface IAccountUseCase
    {
        Task<ServiceResponse> RegisterAsync(RegisterRequest request);
        Task<bool> UserExistsAsync(string email);
        Task<ServiceResponse> LoginAsync(LoginRequest request);
        Task<bool> ValidateAccountAsync(string email);
    }
}
