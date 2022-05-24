using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using WebApi.Models;
using CV;

namespace WebApi.Data
{
    public interface ISkillRepo
    {
        void createSkill(Skill skill);
        //Skill? GetSkillByName(string name);
        string? GetSkillByName(string name);
        bool CheckHelloExists();
        string GetAllSkills();
    }
}
