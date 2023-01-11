using System;
using Store.Core.Entities;

namespace StoreApi.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user, IList<string> roles, IConfiguration config);
    }
}

