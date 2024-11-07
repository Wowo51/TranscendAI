//Copyright Warren Harding 2024.
using System;
using SPSA;

namespace SPSA.Tests
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "";
            int totalTests = 5;
            int passedTests = 0;
            Random rand = new Random();
            Func<double[], double> objectiveFunction = (paramsArray) => Math.Pow(paramsArray[0] - 1, 2) + Math.Pow(paramsArray[1] - 2, 2) + Math.Pow(paramsArray[2] - 3, 2) + 4;
            SPSAOptimizer optimizer = new SPSAOptimizer(objectiveFunction);
            for (int i = 1; i <= totalTests; i++)
            {
                try
                {
                    // Generate random initial point
                    double[] initialPoint = new double[3];
                    for (int j = 0; j < 3; j++)
                    {
                        initialPoint[j] = rand.NextDouble() * 10 - 5; // Random value between -5 and 5
                    }

                    // Optimize
                    double[] optimizedPoint = optimizer.Optimize(initialPoint);
                    // Evaluate objective function at optimized point
                    double finalValue = objectiveFunction(optimizedPoint);
                    // Check if the final value is approximately 4
                    if (Math.Abs(finalValue - 4) < 1e-3)
                    {
                        report += $"Test {i}: Success. Final objective value: {finalValue:F6}\n";
                        passedTests++;
                    }
                    else
                    {
                        report += $"Test {i}: Failure. Final objective value: {finalValue:F6}\n";
                    }
                }
                catch (Exception ex)
                {
                    report += $"Test {i}: Error - {ex.Message}\n";
                }
            }

            // Overall result
            report += $"\nOverall Result: {passedTests} out of {totalTests} tests passed.";
            return report;
        }
    }
}