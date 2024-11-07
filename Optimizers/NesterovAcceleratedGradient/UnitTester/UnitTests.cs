//Copyright Warren Harding 2024.
using System;
using System.Collections.Generic;
using MyOptimizer;

namespace CodeTestRunner
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "Test Results:\n";
            int testNumber = 1;
            double knownAnswer = 4.0;
            var testParameters = new List<double[]>
            {
                new double[]
                {
                    0.0,
                    0.0,
                    0.0
                },
                new double[]
                {
                    1.0,
                    2.0,
                    3.0
                },
                new double[]
                {
                    -1.0,
                    -2.0,
                    -3.0
                },
                new double[]
                {
                    2.5,
                    3.5,
                    4.5
                },
                new double[]
                {
                    10.0,
                    10.0,
                    10.0
                }
            };
            var optimizer = new NesterovAcceleratedGradientOptimizer();
            double learningRate = 0.01;
            double momentum = 0.9;
            int iterations = 1000;
            foreach (var initialParams in testParameters)
            {
                try
                {
                    double[] optimizedParams = optimizer.Optimize(ObjectiveFunction, initialParams, learningRate, momentum, iterations);
                    double result = ObjectiveFunction(optimizedParams);
                    if (Math.Abs(result - knownAnswer) < 1e-6)
                    {
                        report += $"Test {testNumber}: Success.\n";
                    }
                    else
                    {
                        report += $"Test {testNumber}: Failed. Expected {knownAnswer}, got {result}.\n";
                    }
                }
                catch (Exception ex)
                {
                    report += $"Test {testNumber}: Error - {ex.Message}\n";
                }

                testNumber++;
            }

            // Overall Result
            bool allPassed = !report.Contains("Failed");
            report += $"\nOverall Result: {(allPassed ? "Success" : "Some tests failed")}";
            return report;
        }

        private static double ObjectiveFunction(double[] parameters)
        {
            double x = parameters[0];
            double y = parameters[1];
            double z = parameters[2];
            return Math.Pow(x - 1, 2) + Math.Pow(y - 2, 2) + Math.Pow(z - 3, 2) + 4;
        }
    }
}