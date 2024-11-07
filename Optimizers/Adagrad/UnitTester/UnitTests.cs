//Copyright Warren Harding 2024.
using System;
using Optimizers;

namespace OptimizerTestProgram.Tests
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "Test Report:\n";
            bool overallSuccess = true;
            // Define test cases with random start parameters
            var testCases = new (double x, double y, double z)[]
            {
                (3.5, -1.2, 4.8),
                (0.0, 2.0, 5.0),
                (1.5, 2.5, 2.5),
                (2.0, 3.0, 1.0),
                (-1.0, 4.0, 0.0)
            };
            int testNumber = 1;
            foreach (var testCase in testCases)
            {
                try
                {
                    // Initialize optimizer with parameter size 3
                    var optimizer = new AdagradOptimizer(parameterSize: 3);
                    // Initial weights
                    double[] weights = new double[]
                    {
                        testCase.x,
                        testCase.y,
                        testCase.z
                    };
                    // Define gradients (derivatives of the function)
                    Func<double[], double[]> computeGradients = currentWeights => new double[]
                    {
                        2 * (currentWeights[0] - 1),
                        2 * (currentWeights[1] - 2),
                        2 * (currentWeights[2] - 3)
                    };
                    // Perform optimization
                    double[] optimizedWeights = optimizer.Optimize(weights, computeGradients);
                    // Compute function value after optimization
                    double functionValue = Math.Pow(optimizedWeights[0] - 1, 2) + Math.Pow(optimizedWeights[1] - 2, 2) + Math.Pow(optimizedWeights[2] - 3, 2) + 4;
                    // Check if the function value is approximately 4
                    if (Math.Abs(functionValue - 4) < 1e-3)
                    {
                        report += $"Test {testNumber}: Success\n";
                    }
                    else
                    {
                        report += $"Test {testNumber}: Failure - Expected 4, Got {functionValue}\n";
                        overallSuccess = false;
                    }
                }
                catch (Exception ex)
                {
                    report += $"Test {testNumber}: Exception - {ex.Message}\n";
                    overallSuccess = false;
                }

                testNumber++;
            }

            report += $"\nOverall Result: {(overallSuccess ? "Success" : "Failure")}";
            return report;
        }
    }
}