using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Job
    {
        public int m_id { get; set; }
        public string m_title { get; set; }
        public string m_company { get; set; }
        public DateTime m_start { get; set; }
        public DateTime? m_end { get; set; }
    }
}
