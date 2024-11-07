//Copyright Warren Harding 2024.
using System;
using System.Text;
using RMSpropOptimizerApp;

namespace RMSpropTest
{
    public class UnitTests
    {
        public static string Run()
        {
            StringBuilder report = new StringBuilder();
            bool allPassed = true;
            Random rand = new Random();
            for (int i = 1; i <= 5; i++)
            {
                // Generate random starting parameters
                double x = rand.NextDouble() * 10;
                double y = rand.NextDouble() * 10;
                double z = rand.NextDouble() * 10;
                double[] parameters = new double[]
                {
                    x,
                    y,
                    z
                };
                // Define function f and gradients
                Func<double[], double> f = (p) => Math.Pow(p[0] - 1, 2) + Math.Pow(p[1] - 2, 2) + Math.Pow(p[2] - 3, 2) + 4;
                Func<double[], double[]> gradients = (p) => new double[]
                {
                    2 * (p[0] - 1),
                    2 * (p[1] - 2),
                    2 * (p[2] - 3)
                };
                // Initialize optimizer
                RMSpropOptimizer optimizer = new RMSpropOptimizer(learningRate: 0.1, decayRate: 0.9, epsilon: 1e-8);
                // Optimization loop
                int maxIterations = 1000;
                for (int iter = 0; iter < maxIterations; iter++)
                {
                    double[] currentGradients = gradients(parameters);
                    optimizer.UpdateParameters(parameters, currentGradients);
                    double currentF = f(parameters);
                    if (currentF <= 4.0001)
                    {
                        break;
                    }
                }

                double finalF = f(parameters);
                if (Math.Abs(finalF - 4.0) <= 0.0001)
                {
                    report.AppendLine($"Test {i}: Success. Final f(x,y,z) = {finalF}");
                }
                else
                {
                    report.AppendLine($"Test {i}: Failed. Final f(x,y,z) = {finalF}");
                    allPassed = false;
                }
            }

            report.AppendLine($"Overall Result: {(allPassed ? "Success" : "Failure")}");
            return report.ToString();
        }
    }
}