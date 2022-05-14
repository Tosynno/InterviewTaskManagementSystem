using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Respository
{
    public interface IRedisCache
    {
        Task<string> GetOTP(string Phone);
        Task<bool> PostOTP(string phone, string value);
    }
}
