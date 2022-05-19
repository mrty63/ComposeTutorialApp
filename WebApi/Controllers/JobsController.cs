using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobRepo m_repo;
        public JobsController(IJobRepo repo)
        {
            m_repo = repo;
        }

        [HttpGet("{id}", Name = "GetSkillbyName")]
        public ActionResult<string> GetJobById(int id)
        {
            string? job = m_repo.GetJobById(id);
            if (String.IsNullOrEmpty(job))
            {
                return Ok(job);
            }


            return NotFound();
        }

        [HttpPost("CreateJob")]
        public ActionResult<Job> CreateJob([FromBody] string jobString)
        //public ActionResult<Job> CreateJob(string jobString)
        {
            Console.WriteLine(jobString);
            Job jstr = JsonSerializer.Deserialize<Job>(jobString);
            //var s1 = JsonSerializer.Deserialize(skillString);
            //var t1 = skillString.Split(',');
            ///Console.WriteLine(t1[0]);
            //var skill = new Skill { m_name = t1[0], m_exp = t1[1] };
            //var jo = jstr;
            
            m_repo.CreateJob(jstr);
            Console.WriteLine($"Created job {jstr.m_id} at company {jstr.m_company}");
            return CreatedAtRoute(nameof(CreateJob), new { name = jstr.m_id, jstr });
        }

       
    }
}
