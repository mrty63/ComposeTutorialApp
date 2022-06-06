using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV;

namespace WebApi.Data
{
    public interface IJobRepo
    {
        public void CreateJob(Job inputJob);
        public string? GetJobById(int id);
        public int CountExistingJobs();
        public string GetAllJobs();
    }
}
