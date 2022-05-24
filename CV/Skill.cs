using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CV
{
    public class Skill
    {
        [Required]
        public string m_name
        {
            get; set;
        }

        [Required]
        public string m_exp
        {
            get; set;
        }
    }
}
