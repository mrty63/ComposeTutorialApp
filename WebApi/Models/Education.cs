using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Education
    {
		public string m_title { get; set; }
		public string m_school { get; set; }
		public DateTime m_start { get; set; }
		public DateTime m_end { get; set; }
		public string m_grade { get; set; }
	}
}
