using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly ISkillRepo m_repo;

        public SkillsController(ISkillRepo repo)
        {
            m_repo = repo;
        }

        [HttpGet("{name}", Name = "GetSkillbyName")]
        public ActionResult<string> GetSkillbyName(string name)
        {
            var skill = m_repo.GetSkillByName(name);
            if (name != null)
            {
                return Ok(skill);
            }
            return NotFound();
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            var response = m_repo.CheckHelloExists();
            if (response)
            {
                return Ok("true");
            }
            return NotFound("false");
        }

        [HttpPost]
        public ActionResult<Skill> CreateSkill(Skill skill)
        {
            m_repo.createSkill(skill);
            return CreatedAtRoute(nameof(GetSkillbyName), new { name = skill.m_name, skill });
        }
    }
}
