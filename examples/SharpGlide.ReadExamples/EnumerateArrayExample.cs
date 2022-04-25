using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpGlide.ReadExamples
{
    public class EnumerateArrayExample
    {
        public static readonly List<decimal> Data = new();
        
        public static void Run()
        {
            InitData();
            SetupFlowConfig();
        }

        private static void SetupFlowConfig()
        {
           
        }

        private static void InitData()
        {
            Data.AddRange(Enumerable.Range(0, 1000).Select(Convert.ToDecimal));
        }
    }
}