// Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace BayesianOptimizationApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var optimizer = new BayesianOptimizer(objectiveFunction: ObjectiveFunction, bounds: new double[, ] { { 0, 10 }, { 0, 10 }, { 0, 10 } }, iterations: 50, parallelism: 4);
                var result = await optimizer.OptimizeAsync();
                if (result.Parameters.Length >= 3)
                {
                    Console.WriteLine($"Optimal Parameters: x = {result.Parameters[0]}, y = {result.Parameters[1]}, z = {result.Parameters[2]}");
                }
                else
                {
                    Console.WriteLine("Optimal Parameters not found.");
                }

                Console.WriteLine($"Optimal Value: {result.Value}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during optimization: {ex.Message}");
            }
        }

        static double ObjectiveFunction(double[] parameters)
        {
            // Objective function as per Specification
            if (parameters == null || parameters.Length < 3)
                throw new ArgumentException("Parameters must have at least three elements.", nameof(parameters));
            double x = parameters[0];
            double y = parameters[1];
            double z = parameters[2];
            return Math.Pow(x - 1, 2) + Math.Pow(y - 2, 2) + Math.Pow(z - 3, 2) + 4;
        }
    }
}