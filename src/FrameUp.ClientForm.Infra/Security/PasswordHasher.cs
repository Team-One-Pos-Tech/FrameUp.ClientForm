using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Infra.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
        public bool VerifyPassword(string hashedPassword, string plainPassword) => BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
