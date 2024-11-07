//Copyright Warren Harding 2024.
using System;
using MomentumOptimizerTests;

namespace MomentumOptimizerTestApp
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