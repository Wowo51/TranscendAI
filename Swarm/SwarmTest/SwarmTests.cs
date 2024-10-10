//Copyright Warren Harding 2024.
using System;
using SwarmNamespace;

namespace SwarmTestApp
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "";
            bool allPassed = true;
            // Test 1: Check if FindMinima returns expected values within two decimal places
            double[] expected =
            {
                1.0,
                2.0,
                3.0
            };
            Func<double[], double> objectiveFunction = (x) =>
            {
                double sum = 0.0;
                for (int i = 0; i < x.Length; i++)
                {
                    sum += Math.Pow(x[i] - expected[i], 2);
                }

                return sum;
            };
            int dimensions = 3;
            int swarmSize = 30;
            int iterations = 100;
            double[] result = Swarm.FindMinima(objectiveFunction, dimensions, swarmSize, iterations);
            bool passed = true;
            if (result.Length != expected.Length)
            {
                passed = false;
                report += "Test 1 Failed: Result length does not match expected length.\n";
            }
            else
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    if (Math.Abs(result[i] - expected[i]) > 0.01)
                    {
                        passed = false;
                        report += $"Test 1 Failed: Dimension {i + 1} expected {expected[i]}, got {result[i]:F2}.\n";
                    }
                }
            }

            if (passed)
            {
                report += "Test 1 Passed: Swarm found the known minima within two decimal places.\n";
            }
            else
            {
                allPassed = false;
            }

            // Calculate differences
            if (result.Length == expected.Length)
            {
                double difference = 0.0;
                for (int i = 0; i < expected.Length; i++)
                {
                    difference += Math.Abs(result[i] - expected[i]);
                }

                report += $"Difference between result and known minima: {difference:F2}\n";
            }

            // Overall result
            if (allPassed)
            {
                report += "Overall Result: SUCCESS. All tests passed successfully.";
            }
            else
            {
                report += "Overall Result: FAILURE. Some tests failed.";
            }

            return report;
        }
    }
}