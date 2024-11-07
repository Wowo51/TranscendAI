//Copyright Warren Harding 2024.
using System;
using CodeTestRunner;

namespace TestRunnerApp
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