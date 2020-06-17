using System;

namespace P4_Projekt_2
{
    public interface ISingleParameters
    {
        string Cash { get; set; }
        double WhatPercent { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
    }
}
