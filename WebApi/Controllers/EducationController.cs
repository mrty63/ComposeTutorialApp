using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Data;
using CV;
using Serilog;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly IEducationRepo m_repo;
        public EducationController(IEducationRepo repo)
        {
            m_repo = repo;
        }

        [HttpGet("Ping")]
        public ActionResult Ping()
        {
            return Ok("pong");
        }

        [HttpPost("CreateEducation")]
        public ActionResult<bool> CreateEducation([FromBody] string eduString)
        {
            if(eduString == null)
            {
                Log.Error($"Cannot Create Null Education");
                return BadRequest();
            }
            Education edu = JsonSerializer.Deserialize<Education>(eduString);
            edu.m_id = m_repo.CountExistingEducation();

            m_repo.CreateEducation(edu);
            Log.Information($"Created Education {edu.m_title} at {edu.m_school}");
            return Created("./index", true);

        }
        [HttpGet("GetAllEducation")]
        public ActionResult<string> GetAllEducation()
        {
            var result = m_repo.GetAllEducation();
            if(result != null)
            {
                return Ok(result);

            }
            return NotFound();
        }
    }
}
