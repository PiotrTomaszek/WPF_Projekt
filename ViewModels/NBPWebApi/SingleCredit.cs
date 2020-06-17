using System;
using System.ComponentModel;

namespace P4_Projekt_2
{
    public class SingleCredit : ISingleParameters
    {
        public string Cash { get; set; }
        public double WhatPercent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}
