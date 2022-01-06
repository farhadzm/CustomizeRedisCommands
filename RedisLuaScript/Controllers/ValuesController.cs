using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisLuaScript.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisLuaScript.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IRedisRepostitory _redisRepostitory;

        public ValuesController(IRedisRepostitory redisRepostitory)
        {
            _redisRepostitory = redisRepostitory;
        }
        [HttpGet("{key}/{count}")]
        public async Task<IActionResult> Get(string key, int count)
        {
            var result = await _redisRepostitory.Increment(key, count);
            return Ok(result);
        }
    }
}
