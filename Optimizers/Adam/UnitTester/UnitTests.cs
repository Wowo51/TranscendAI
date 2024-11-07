//Copyright Warren Harding 2024.
using System;
using System.Linq;
using System.Threading.Tasks;
using AdamOptimizerNamespace;
using System.Collections.Generic;

namespace AdamOptimizerTestNamespace
{
    public class UnitTests
    {
        public static string Run()
        {
            var report = "";
            bool allPassed = true;
            var rand = new Random();
            for (int i = 1; i <= 5; i++)
            {
                var optimizer = new AdamOptimizer();
                // Generate random start parameters
                double x = rand.NextDouble() * 10;
                double y = rand.NextDouble() * 10;
                double z = rand.NextDouble() * 10;
                // Define the function to minimize: f(x,y,z) = (x-1)^2 + (y-2)^2 + (z-3)^2 + 4
                Func<double, double, double, double> function = (a, b, c) => Math.Pow(a - 1, 2) + Math.Pow(b - 2, 2) + Math.Pow(c - 3, 2) + 4;
                // Define the gradients
                Func<double, double, double, Dictionary<string, double>> gradients = (a, b, c) =>
                {
                    return new Dictionary<string, double>
                    {
                        {
                            "x",
                            2 * (a - 1)
                        },
                        {
                            "y",
                            2 * (b - 2)
                        },
                        {
                            "z",
                            2 * (c - 3)
                        }
                    };
                };
                // Initialize parameters
                var parameters = new Dictionary<string, double>
                {
                    {
                        "x",
                        x
                    },
                    {
                        "y",
                        y
                    },
                    {
                        "z",
                        z
                    }
                };
                optimizer.InitializeParameters(parameters.Keys);
                // Optimization loop
                int maxIterations = 10000;
                for (int iteration = 0; iteration < maxIterations; iteration++)
                {
                    var grads = gradients(parameters["x"], parameters["y"], parameters["z"]);
                    var updates = optimizer.UpdateParameters(grads);
                    // Apply updates
                    foreach (var param in updates)
                    {
                        parameters[param.Key] += param.Value;
                    }

                    // Check convergence
                    double currentValue = function(parameters["x"], parameters["y"], parameters["z"]);
                    if (currentValue <= 4 + 1e-3)
                    {
                        break;
                    }
                }

                double finalValue = function(parameters["x"], parameters["y"], parameters["z"]);
                bool passed = Math.Abs(finalValue - 4) <= 1e-3;
                report += $"Test {i}: Final Function Value = {finalValue:F6}. " + (passed ? "Passed." : "Failed.") + "\n";
                if (!passed)
                    allPassed = false;
            }

            report += $"Overall Result: " + (allPassed ? "All tests passed." : "Some tests failed.");
            return report;
        }
    }
}