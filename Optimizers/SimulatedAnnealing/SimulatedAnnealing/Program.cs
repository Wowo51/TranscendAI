//Copyright Warren Harding 2024.
using System;

namespace SimulatedAnnealingOptimizerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var optimizer = new SimulatedAnnealingOptimizer(initialTemperature: 1000, coolingRate: 0.95, maxIterations: 1000);
            double[] initialSolution = new double[]
            {
                0.0,
                0.0
            }; // Example initial solution
            Func<double[], double> objectiveFunction = x => x[0] * x[0] + x[1] * x[1]; // Example objective function
            var bestSolution = optimizer.Optimize(initialSolution, objectiveFunction);
            Console.WriteLine("Best Solution: " + string.Join(", ", bestSolution));
        }
    }
}