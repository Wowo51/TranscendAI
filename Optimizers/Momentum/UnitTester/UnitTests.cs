//Copyright Warren Harding 2024.
using System;
using MomentumOptimizerApp;

namespace MomentumOptimizerTests
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "";
            int totalTests = 5;
            int passedTests = 0;
            var rand = new Random(); // Moved outside the loop to ensure different seeds
            for (int i = 1; i <= totalTests; i++)
            {
                try
                {
                    // Generate random start parameters
                    double x = rand.NextDouble() * 10; // Random between 0 and 10
                    double y = rand.NextDouble() * 10;
                    double z = rand.NextDouble() * 10;
                    Vector initialParams = new Vector(new double[] { x, y, z });
                    // Define the objective function
                    Func<Vector, double> objectiveFunction = (Vector parameters) =>
                    {
                        if (parameters.Values == null || parameters.Values.Length < 3)
                            throw new ArgumentException("Parameters must have at least three dimensions.");
                        double a = parameters.Values[0];
                        double b = parameters.Values[1];
                        double c = parameters.Values[2];
                        return Math.Pow(a - 1, 2) + Math.Pow(b - 2, 2) + Math.Pow(c - 3, 2) + 4;
                    };
                    // Create optimizer instance
                    var optimizer = new MomentumOptimizer<Vector>(learningRate: 0.01, momentum: 0.9, maxIterations: 1000, tolerance: 1e-6);
                    // Optimize
                    Vector optimalParams = optimizer.Optimize(objectiveFunction, initialParams);
                    // Evaluate the function at optimized parameters
                    double optimizedValue = objectiveFunction(optimalParams);
                    // Compare to known answer
                    double expected = 4.0;
                    double tolerance = 1e-4;
                    if (Math.Abs(optimizedValue - expected) < tolerance)
                    {
                        report += $"Test {i}: Success. Optimized Value = {optimizedValue:F4}\n";
                        passedTests++;
                    }
                    else
                    {
                        report += $"Test {i}: Failure. Optimized Value = {optimizedValue:F4}, Expected = {expected}\n";
                    }
                }
                catch (Exception ex)
                {
                    report += $"Test {i}: Exception occurred - {ex.Message}\n";
                }
            }

            report += $"\nPassed {passedTests} out of {totalTests} tests.";
            return report;
        }
    }
}