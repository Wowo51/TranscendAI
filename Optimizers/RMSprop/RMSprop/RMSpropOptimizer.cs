//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace RMSpropOptimizerApp
{
    public class RMSpropOptimizer
    {
        private double learningRate;
        private double decayRate;
        private double epsilon;
        private double[] cache = new double[0];
        public RMSpropOptimizer(double learningRate = 0.001, double decayRate = 0.9, double epsilon = 1e-8)
        {
            this.learningRate = learningRate;
            this.decayRate = decayRate;
            this.epsilon = epsilon;
        }

        public void UpdateParameters(double[] parameters, double[] gradients)
        {
            if (cache.Length != parameters.Length)
            {
                cache = new double[parameters.Length];
            }

            Parallel.For(0, parameters.Length, i =>
            {
                cache[i] = decayRate * cache[i] + (1 - decayRate) * gradients[i] * gradients[i];
                parameters[i] -= learningRate * gradients[i] / (Math.Sqrt(cache[i]) + epsilon);
            });
        }
    }
}