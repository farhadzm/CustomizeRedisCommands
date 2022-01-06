using StackExchange.Redis;
using System.Threading.Tasks;

namespace RedisLuaScript.Repositories
{
    public interface IRedisRepostitory
    {
        Task<int> Increment(string key, int maximum);
    }

    public class RedisRepostitory : IRedisRepostitory
    {
        private readonly IDatabase _database;
        public RedisRepostitory(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }
        public async Task<int> Increment(string key, int maximum)
        {
            string script = @"local count 
                              count = tonumber(redis.call('INCR', @key))
                              if(count > tonumber(@maximum))
                              then
                                 redis.call('DEL', @key) 
                                 return -1
                              else 
                                 return count
                              end";
            LuaScript incrementScript = LuaScript.Prepare(script);

            var result = await _database
                .ScriptEvaluateAsync(incrementScript, new
                {
                    key = new RedisKey(key),
                    maximum
                });

            return (int)result;
        }
    }
}
