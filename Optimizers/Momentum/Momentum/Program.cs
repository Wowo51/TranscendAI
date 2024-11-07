//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace MomentumOptimizerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var optimizer = new MomentumOptimizer<Vector>(learningRate: 0.01, momentum: 0.9, maxIterations: 1000, tolerance: 1e-6);
            Vector initialParams = new Vector(new double[] { 0.0, 0.0 });
            Vector optimalParams = optimizer.Optimize(ObjectiveFunction, initialParams);
            Console.WriteLine($"Optimal Parameters: {optimalParams}");
        }

        static double ObjectiveFunction(Vector parameters)
        {
            // Example: Rosenbrock function
            double x = parameters.Values[0];
            double y = parameters.Values[1];
            return Math.Pow(1 - x, 2) + 100 * Math.Pow(y - x * x, 2);
        }
    }
}