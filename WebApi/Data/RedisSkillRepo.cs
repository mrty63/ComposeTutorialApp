//using Newtonsoft.Json;
using CV;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
//using WebApi.Models;

namespace WebApi.Data
{
    public class RedisSkillRepo : ISkillRepo
    {
        private readonly ConnectionMultiplexer m_redis;
        //private readonly Microsoft.Extensions.Logging.ILogger m_logger;
        private IDatabase m_db;
        private static int serverId = 1;

        public RedisSkillRepo(ConnectionMultiplexer redis/*, ILogger<RedisJobRepo> logger*/)
        {
            m_redis = redis;
            m_db = m_redis.GetDatabase(serverId);
            //m_logger = logger;

        }
        void ISkillRepo.createSkill(Skill skill)
        {
            if(skill == null)
            {
                throw new ArgumentOutOfRangeException(nameof(skill));
            }

            //var serial_skill = JsonConvert.SerializeObject(skill);
            var serial_skill = JsonSerializer.Serialize(skill);
            //m_db.StringSet(skill.m_name, serial_skill);
            m_db.StringSet(skill.m_name, skill.m_exp);
            var message = $"skill created at {DateTime.Now.ToString()}";
            //m_logger.LogInformation(message);
            //using (m_logger.BeginScope($"\nCreated Skill at {DateTime.Now.ToLongTimeString()}"))
            //{
            //    m_logger.LogInformation("Skill called {0} created ", skill.m_name);
            //}
            Log.Logger.Debug(message);
        }

        bool ISkillRepo.CheckHelloExists()       //check if hello: there is in the redis database else insert it
        {
            
            var res = m_db.StringGet("hello");
            //bool present = false;
            if(!res.HasValue)
            {
                m_db.StringSet("hello", "there");


                return false;
            }
            return true;
            //throw new NotImplementedException("Check Hello is not implemented yet");
        }

        string? ISkillRepo.GetSkillByName(string name)
        {
            if (name == null)
            {
                Console.WriteLine("invalid name passed to ");
                throw new ArgumentOutOfRangeException(nameof(name));
            }

            var skill = m_db.StringGet(name);
            return skill;
            //if (!string.IsNullOrEmpty(name))
            //{
            //    return JsonSerializer.Deserialize<Skill>(skill);
            //}
            //return null;
        }
        string ISkillRepo.GetAllSkills()
        {
            List<Skill> listSkills = new List<Skill>();
            {
                var keys = m_redis.GetServer("redis", 6379).Keys(serverId);
                foreach(var key in keys)
                {
                    listSkills.Add(new Skill { m_name = key.ToString(), m_exp = m_db.StringGet(key) });
                }
            }
            string jsonString = JsonSerializer.Serialize(listSkills);
            //Console.WriteLine(jsonString);
            //using (m_logger.BeginScope($"\nListing all Skills at {DateTime.Now.ToLongTimeString()}"))
            {
                //m_logger.LogCritical
                var message = $"List made";
                var message2 = $"{listSkills.Count} skills were found in redis";
                //m_logger.LogCritical(message);
                Log.Logger.Debug(message);
                //m_logger.LogCritical(message2);
                Log.Logger.Debug(message2);
            }


            return jsonString;

        }
    }
}
