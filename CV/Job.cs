using System;
using System.ComponentModel.DataAnnotations;

namespace CV
{
    public class Job
    {
        public int m_id { get; set; }
        [Required]
        public string m_title { get; set; }
        [Required]
        public string m_company { get; set; }
        [Required]
        public DateTime m_start { get; set; }
        public DateTime? m_end { get; set; }
    }
}