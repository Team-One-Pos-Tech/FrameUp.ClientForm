using FrameUp.ClientForm.Application.Contracts;
using FrameUp.ClientForm.Application.Models;
using FrameUp.ClientForm.Domain.Entities;
using FrameUp.ClientForm.Domain.Interfaces;
using FrameUp.ClientForm.Domain.Repositories;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Application.UsesCases
{
    public class AccountUseCase : IAccountUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AccountUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<ServiceResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _userRepository.UserExistsAsync(request.Email))
                return new ServiceResponse { IsSuccess = false, Message = "User already exists." };

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var user = new User { Name = request.Name, Email = request.Email, Password = hashedPassword };

            await _userRepository.AddUserAsync(user);
            return new ServiceResponse { IsSuccess = true, Message = "User registered successfully." };
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _userRepository.UserExistsAsync(email);
        }

        public async Task<ServiceResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.Password))
                return new ServiceResponse { IsSuccess = false, Message = "Invalid credentials." };

            var token = _tokenService.GenerateToken(user);
            return new ServiceResponse { IsSuccess = true, Message = "Login successful.", Token = token };
        }

        public async Task<bool> ValidateAccountAsync(string email)
        {
            return await _userRepository.UserExistsAsync(email);
        }
    }
}
