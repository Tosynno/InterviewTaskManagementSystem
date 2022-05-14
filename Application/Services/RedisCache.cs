using Application.Data;
using Application.Respository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class RedisCache : IRedisCache
    {
        private readonly IRedisDataContext _context;
        public RedisCache(IRedisDataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<string> GetOTP(string useremail)
        {
            var basket = await _context
                                .Redis
                                .StringGetAsync(useremail);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<string>(basket);
        }

        public async Task<bool> PostOTP(string useremail, string value)
        {
            var basket = await _context
                               .Redis.StringSetAsync(useremail, value, TimeSpan.FromDays(1));
            if (!basket)
            {
                return false;
            }
            return basket;
        }
    }
}
