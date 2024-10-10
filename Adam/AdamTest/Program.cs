//Copyright Warren Harding 2024.
using System;
using AdamTestApp.Tests;

namespace AdamTestApp
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