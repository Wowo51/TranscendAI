// Copyright Warren Harding 2024.
using System;
using System.Text;
using System.Threading.Tasks;
using BayesianOptimizationApp;

namespace BayesianOptimizationTest
{
    public class UnitTests
    {
        public static string Run()
        {
            StringBuilder report = new StringBuilder();
            int totalTests = 5;
            int passedTests = 0;
            Random rand = new Random();
            for (int i = 1; i <= totalTests; i++)
            {
                try
                {
                    // Generate random start parameters within bounds
                    double xStart = rand.NextDouble() * 10;
                    double yStart = rand.NextDouble() * 10;
                    double zStart = rand.NextDouble() * 10;
                    // Define bounds for each parameter
                    double[, ] bounds = new double[, ]
                    {
                        {
                            0,
                            5
                        },
                        {
                            0,
                            5
                        },
                        {
                            0,
                            5
                        }
                    };
                    // Define the objective function
                    Func<double[], double> objectiveFunction = parameters => Math.Pow(parameters[0] - 1, 2) + Math.Pow(parameters[1] - 2, 2) + Math.Pow(parameters[2] - 3, 2) + 4;
                    // Instantiate the optimizer
                    BayesianOptimizer optimizer = new BayesianOptimizer(objectiveFunction, bounds, iterations: 50, parallelism: 2);
                    // Run the optimizer
                    var task = optimizer.OptimizeAsync();
                    task.Wait();
                    var result = task.Result;
                    // Compare the result to the known minimum with adjusted tolerance
                    if (Math.Abs(result.Value - 4) < 1e-2)
                    {
                        report.AppendLine($"Test {i}: Success. Optimized value = {result.Value}");
                        passedTests++;
                    }
                    else
                    {
                        report.AppendLine($"Test {i}: Failure. Optimized value = {result.Value}, Expected = 4");
                    }
                }
                catch (Exception ex)
                {
                    report.AppendLine($"Test {i}: Error occurred - {ex.Message}");
                }
            }

            // Overall result
            report.AppendLine($"Overall Result:\n{passedTests} / {totalTests} tests passed.");
            return report.ToString();
        }
    }
}