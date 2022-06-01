using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Extensions
{
	public static class Constants
	{
		/// <summary>Identifies a named client (IHttpClientFactory) configured for fault tolerance</summary>
		public const string _faultTolerantHttpClientName = "fault-tolerant";

		public const string _RedisApplicationName = "STARS_API";
		public const string _SettingsCacheName = "ReportSettings";
		public const string _AttendanceSettingsCacheName = "AttendanceSettings";
	}
}
