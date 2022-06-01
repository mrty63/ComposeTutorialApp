﻿using System;

namespace WebApp.Chaos
{
    [Serializable]
    public class OperationChaosSetting
    {
        public Guid Id { get; set; }

        public string OperationKey { get; set; }

        public bool Enabled { get; set; }

        public double InjectionRate { get; set; }

        public int StatusCode { get; set; }

        public int LatencyMs { get; set; }

        public string Exception { get; set; }

        public TimeSpan Duration { get; set; }

        
    }
}
