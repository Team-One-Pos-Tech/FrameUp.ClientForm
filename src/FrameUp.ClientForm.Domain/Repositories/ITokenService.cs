using FrameUp.ClientForm.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameUp.ClientForm.Domain.Repositories
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
