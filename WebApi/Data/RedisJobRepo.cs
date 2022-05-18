using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi;
using System.Text.Json;

namespace WebApi.Data
{
    public class RedisJobRepo : IJobRepo
    {
        private readonly ConnectionMultiplexer m_redis;
        private IDatabase m_db;
        public RedisJobRepo(ConnectionMultiplexer redis)
        {
            m_redis = redis;
            m_db = m_redis.GetDatabase(Constants.serverIdJob);
        }

        void IJobRepo.CreateJob(Job inputJob)
        {
            if (inputJob == null)
            {
                throw new ArgumentOutOfRangeException(nameof(inputJob));
            }

            var job = JsonSerializer.Serialize(inputJob);
            Console.WriteLine(inputJob);
            m_db.StringSet(inputJob.m_id.ToString(), job);

        }
        string? IJobRepo.GetJobById(int id)
        {
            
            string res = m_db.StringGet(id.ToString());
            if(String.IsNullOrEmpty(res))
            {
                Console.WriteLine("Invalid Job Id passed to GetJobbyID");
            }
            return res;
        }
    }
}
