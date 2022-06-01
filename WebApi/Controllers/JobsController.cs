using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Data;
using CV;

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
            

            jstr.m_id = m_repo.CountExistingJobs();
            
            m_repo.CreateJob(jstr);
            Console.WriteLine($"Created job {jstr.m_id} at company {jstr.m_company}");
            return Created("./index", true);

            //return CreatedAtRoute(nameof(CreateJob), new { name = jstr.m_id, jstr });
        }

        [HttpGet("Ping")]
        public  ActionResult Ping()
        {
            return Ok("pong");
            //return Problem();
        }


    }
}
