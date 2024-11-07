//Copyright Warren Harding 2024.
// Copyright Warren Harding 2024.
using System;
using PSOTest;

namespace PSOTestRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            UnitTests tests = new UnitTests();
            string report = tests.Run();
            Console.WriteLine(report);
        }
    }
}