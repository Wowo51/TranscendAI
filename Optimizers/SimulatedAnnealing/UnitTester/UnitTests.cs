//Copyright Warren Harding 2024.
using System;
using SimulatedAnnealingOptimizerApp;

namespace SimulatedAnnealingOptimizerTests
{
    public class UnitTests
    {
        public static string Run()
        {
            var optimizer = new SimulatedAnnealingOptimizer(initialTemperature: 1000, coolingRate: 0.95, maxIterations: 10000);
            string report = "";
            int totalTests = 5;
            int passedTests = 0;
            Random rand = new Random();
            for (int i = 1; i <= totalTests; i++)
            {
                double[] initialSolution = new double[]
                {
                    rand.NextDouble() * 10,
                    rand.NextDouble() * 10,
                    rand.NextDouble() * 10
                };
                Func<double[], double> objectiveFunction = x => Math.Pow(x[0] - 1, 2) + Math.Pow(x[1] - 2, 2) + Math.Pow(x[2] - 3, 2) + 4;
                double[] bestSolution = optimizer.Optimize(initialSolution, objectiveFunction);
                double bestEnergy = objectiveFunction(bestSolution);
                if (Math.Abs(bestEnergy - 4) < 1e-3)
                {
                    report += $"Test {i}: Success. Best Energy: {bestEnergy}\n";
                    passedTests++;
                }
                else
                {
                    report += $"Test {i}: Failure. Best Energy: {bestEnergy}\n";
                }
            }

            report += $"Overall Result: {passedTests}/{totalTests} tests passed.";
            return report;
        }
    }
}