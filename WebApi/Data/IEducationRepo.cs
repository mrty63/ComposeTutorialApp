using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CV;


namespace WebApi.Data
{
    public interface IEducationRepo
    {
        public void CreateEducation(Education edu);
        public string? GetEducationById(int id);
        public int CountExistingEducation();
        public string GetAllEducation();

    }
}
