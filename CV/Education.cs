using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CV
{
    public class Education
    {

        [Required]
        public string m_title { get; set; }
        [Required]
        public string m_school { get; set; }
        [Required]
        public DateTime m_start { get; set; }
        [Required]
        public DateTime m_end { get; set; }
        [Required]
        public string m_grade { get; set; }
        
    }
}
