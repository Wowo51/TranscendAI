//Copyright Warren Harding 2024.
using System;
using BayesianOptimizationTest;

namespace BayesianOptimizationRunner
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