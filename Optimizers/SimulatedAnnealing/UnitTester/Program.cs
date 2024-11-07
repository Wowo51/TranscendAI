//Copyright Warren Harding 2024.
using System;
using SimulatedAnnealingOptimizerTests;

namespace SimulatedAnnealingTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string report = UnitTests.Run();
            Console.WriteLine(report);
        }
    }
}