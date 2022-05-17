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
        [HttpPost("CreateSkill")]
        public ActionResult<Skill> CreateSkillForm([FromBody]string skillString)
        {

            //var s1 = JsonSerializer.Deserialize(skillString);
            var t1 = skillString.Split(',');
            Console.WriteLine(t1[0]);
            var skill = new Skill { m_name = t1[0], m_exp = t1[1] };
            m_repo.createSkill(skill);
            return CreatedAtRoute(nameof(GetSkillbyName), new { name = skill.m_name, skill });
        }
        

    }
}
