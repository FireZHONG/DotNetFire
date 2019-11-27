using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExtension
{
    public class RedisClient : IRedisClient
    {
        private IConnectionMultiplexer _redisMultiplexer;
        IDatabase _redisConnection;

        public RedisClient(IConnectionMultiplexer redisMultiplexer, string connectionString = null)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _redisMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            }
            else
            {
                _redisMultiplexer = redisMultiplexer;
            }
            _redisConnection = _redisMultiplexer.GetDatabase();
        }

        public IDatabase GetConnection()
        {
            return _redisConnection;
        }

        /// <summary>
        /// get key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public RedisValue StringGet(string key)
        {
            return _redisConnection.StringGet(key);
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        public T StringGet<T>(string key)
        {
            var val = _redisConnection.StringGet(key);
            return JsonConvert.DeserializeObject<T>(val);
        }

        /// <summary>
        /// set key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public bool StringSet(string key, object value, TimeSpan? expiry = default(TimeSpan?))
        {
            return _redisConnection.StringSet(key, value.ToString(), expiry);
        }

        public void Dispose()
        {
            if (_redisMultiplexer != null)
            {
                _redisConnection = null;
                _redisMultiplexer.Dispose();
            }
        }
    }


    public class ClusterOption
    {
        public string Password { get; set; }
        public IList<string> EndPoints { get; set; }
    }
}
