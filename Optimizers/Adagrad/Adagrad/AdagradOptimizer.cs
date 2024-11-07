//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace Optimizers
{
    public class AdagradOptimizer
    {
        private readonly double learningRate;
        private readonly double epsilon;
        private double[] accumulatedSquares;
        public AdagradOptimizer(int parameterSize, double learningRate = 0.1, double epsilon = 1e-8)
        {
            if (parameterSize <= 0)
                throw new ArgumentException("Parameter size must be greater than zero.", nameof(parameterSize));
            this.learningRate = learningRate;
            this.epsilon = epsilon;
            this.accumulatedSquares = new double[parameterSize];
        }

        public double[] Optimize(double[] weights, Func<double[], double[]> computeGradients)
        {
            if (weights == null)
                throw new ArgumentNullException(nameof(weights));
            if (computeGradients == null)
                throw new ArgumentNullException(nameof(computeGradients));
            bool converged = false;
            int maxIterations = 10000;
            double convergenceThreshold = 1e-6;
            int iteration = 0;
            object lockObj = new object ();
            while (!converged && iteration < maxIterations)
            {
                converged = true;
                double[] gradients = computeGradients(weights);
                if (gradients.Length != weights.Length)
                    throw new ArgumentException("Weights and gradients must be of the same length.");
                Parallel.For(0, weights.Length, i =>
                {
                    double gradient = gradients[i];
                    accumulatedSquares[i] += gradient * gradient;
                    double adjustedLearningRate = learningRate / Math.Sqrt(accumulatedSquares[i] + epsilon);
                    double update = adjustedLearningRate * gradient;
                    weights[i] -= update;
                    if (Math.Abs(update) > convergenceThreshold)
                    {
                        lock (lockObj)
                        {
                            converged = false;
                        }
                    }
                });
                iteration++;
            }

            return weights;
        }
    }
}