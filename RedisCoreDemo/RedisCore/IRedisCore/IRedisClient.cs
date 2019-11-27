using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExtension
{
    public interface IRedisClient : IDisposable
    {
        IDatabase GetConnection();

        RedisValue StringGet(string key);
        T StringGet<T>(string key);
        bool StringSet(string key, object value, TimeSpan? expiry = default(TimeSpan?));

    }
}
