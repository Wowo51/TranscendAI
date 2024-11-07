//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace RMSpropOptimizerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize parameters and gradients
            double[] parameters = new double[]
            {
                1.5,
                -2.0,
                3.0
            };
            double[] gradients = new double[]
            {
                0.1,
                -0.2,
                0.3
            };
            // Initialize optimizer
            RMSpropOptimizer optimizer = new RMSpropOptimizer(learningRate: 0.01, decayRate: 0.9, epsilon: 1e-8);
            // Update parameters
            optimizer.UpdateParameters(parameters, gradients);
            // Output updated parameters
            Console.WriteLine("Updated Parameters:");
            foreach (var param in parameters)
            {
                Console.WriteLine(param);
            }
        }
    }
}