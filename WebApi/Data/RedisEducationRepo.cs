using CV;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;

namespace WebApi.Data
{
    public class RedisEducationRepo : IEducationRepo
    {
        private readonly ConnectionMultiplexer m_redis;
        private IDatabase m_db;
        public RedisEducationRepo(ConnectionMultiplexer redis)
        {
            m_redis = redis;
            m_db = m_redis.GetDatabase(Constants.serverIdEdu);
            Log.Information($"Redis Education Repo Started");
        }
        int IEducationRepo.CountExistingEducation()
        {
            return m_redis.GetServer(Constants.redisHost, Constants.redisPort).Keys(Constants.serverIdEdu).Count();
        }

        void IEducationRepo.CreateEducation(Education inputEdu)
        {
            if (inputEdu == null)
            {
                throw new ArgumentOutOfRangeException(nameof(inputEdu));
            }
            

            var serial_Edu = JsonSerializer.Serialize(inputEdu);
            m_db.StringSet(inputEdu.m_id.ToString(), serial_Edu);
        }

        string IEducationRepo.GetAllEducation()
        {
            List<Education> listEdu = new List<Education>();
            string jsonString;
            {
                ;
                var keys = m_redis.GetServer("redis", 6379).Keys(Constants.serverIdEdu);
                foreach (var key in keys)
                {
                    Education tempEdu = JsonSerializer.Deserialize<Education>(m_db.StringGet(key));
                    listEdu.Add(tempEdu);                    
                }
                jsonString = JsonSerializer.Serialize(listEdu);
                Log.Information($"List of {listEdu.Count} Education made from redis");
            }

            return jsonString;
        }

        string? IEducationRepo.GetEducationById(int id)
        {
            string res = m_db.StringGet(id.ToString());
            if (String.IsNullOrEmpty(res))
            {
                Log.Error($"Invalid Job Id passed to {nameof(IEducationRepo.GetEducationById)}");
                Log.Error($"Invalid Job Id  :{id}");
                return null;
            }
            return res;
        }
    }
}
