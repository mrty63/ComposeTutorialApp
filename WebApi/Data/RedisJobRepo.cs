using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV;
using WebApi;
using System.Text.Json;

namespace WebApi.Data
{
    public class RedisJobRepo : IJobRepo
    {
        private readonly ConnectionMultiplexer m_redis;

        private IDatabase m_db;
        //Logger m_log;
        public RedisJobRepo(ConnectionMultiplexer redis)
        {
            m_redis = redis;
            m_db = m_redis.GetDatabase(Constants.serverIdJob);
            //m_log = log;
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
        int IJobRepo.CountExistingJobs()
        {
            return m_redis.GetServer(Constants.redisHost,Constants.redisPort).Keys(Constants.serverIdJob).Count();
            
        }
    }
}
