using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NTS.Redis
{
    public class RedisCacheService
    {
        internal readonly IServer Server;
        internal readonly IDatabase Database;
        private readonly IRedisCacheConnectionFactory redisCacheConnectionFactory;

        public RedisCacheService(IRedisCacheConnectionFactory redisCacheConnectionFactory)
        {
            this.redisCacheConnectionFactory = redisCacheConnectionFactory;
            this.Database = this.redisCacheConnectionFactory.Connection().GetDatabase();
            this.Server = this.redisCacheConnectionFactory.Connection().GetServer(this.redisCacheConnectionFactory.Connection().GetEndPoints().First());
        }

        public bool Exists(string key)
        {
            return this.Database.KeyExists(key, CommandFlags.None);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return this.Database.KeyExistsAsync(key, CommandFlags.None);
        }

        public bool Remove(string key)
        {
            return this.Database.KeyDelete(key, CommandFlags.None);
        }

        public Task<bool> RemoveAsync(string key)
        {
            return this.Database.KeyDeleteAsync(key, CommandFlags.None);
        }

        public T Get<T>(string key)
        {
            RedisValue value = this.Database.StringGet(key, CommandFlags.None);
            if (!value.HasValue)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            RedisValue value = await this.Database.StringGetAsync(key, CommandFlags.None);
            T result;
            if (!value.HasValue)
            {
                result = default(T);
            }
            else
            {
                result = JsonConvert.DeserializeObject<T>(value);
            }
            return result;
        }

        public bool Add<T>(string key, T value)
        {
            string value2 = JsonConvert.SerializeObject(value);
            return this.Database.StringSet(key, value2, null, When.Always, CommandFlags.None);
        }

        public bool Add<T>(string key, T value, DateTimeOffset expiresAt)
        {
            string value2 = JsonConvert.SerializeObject(value);
            TimeSpan expireTime = expiresAt.Subtract(DateTimeOffset.Now);
            return this.Database.StringSet(key, value2, new TimeSpan?(expireTime), When.Always, CommandFlags.None);
        }

        public bool Add<T>(string key, T value, TimeSpan expiresIn)
        {
            string value2 = JsonConvert.SerializeObject(value);
            return this.Database.StringSet(key, value2, new TimeSpan?(expiresIn), When.Always, CommandFlags.None);
        }

        public bool Replace<T>(string key, T value)
        {
            return this.Add<T>(key, value);
        }

        public bool Replace<T>(string key, T value, DateTimeOffset expiresAt)
        {
            return this.Add<T>(key, value, expiresAt);
        }

        public bool Replace<T>(string key, T value, TimeSpan expiresIn)
        {
            return this.Add<T>(key, value, expiresIn);
        }

        public bool AddAll<T>(IList<Tuple<string, T>> items)
        {
            KeyValuePair<RedisKey, RedisValue>[] values = (
                from item in items
                select new KeyValuePair<RedisKey, RedisValue>(item.Item1, JsonConvert.SerializeObject(item.Item2))).ToArray<KeyValuePair<RedisKey, RedisValue>>();
            return this.Database.StringSet(values, When.Always, CommandFlags.None);
        }

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items)
        {
            KeyValuePair<RedisKey, RedisValue>[] values = (
                from item in items
                select new KeyValuePair<RedisKey, RedisValue>(item.Item1, JsonConvert.SerializeObject(item.Item2))).ToArray<KeyValuePair<RedisKey, RedisValue>>();
            return await this.Database.StringSetAsync(values, When.Always, CommandFlags.None);
        }
    }
}
