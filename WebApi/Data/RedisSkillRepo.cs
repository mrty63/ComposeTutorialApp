using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Data
{
    public class RedisSkillRepo : ISkillRepo
    {
        private readonly ConnectionMultiplexer m_redis;
        private IDatabase m_db;


        public RedisSkillRepo(ConnectionMultiplexer redis)
        {
            m_redis = redis;
            m_db = m_redis.GetDatabase();

        }
        void ISkillRepo.createSkill(Skill skill)
        {
            if(skill == null)
            {
                throw new ArgumentOutOfRangeException(nameof(skill));
            }

            var serial_skill = JsonSerializer.Serialize(skill);
            m_db.StringSet(skill.m_name, serial_skill);
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
    }
}
