//Copyright Warren Harding 2024.
using System;

namespace EthosWork.Adam
{
    public static class AdamOptimizer
    {
        public static OptimizationResult FindMinimum(Func<double[], double> function, double[] initialPoint, AdamParameters parameters)
        {
            int dim = initialPoint.Length;
            double[] m = new double[dim];
            double[] v = new double[dim];
            double[] point = (double[])initialPoint.Clone();
            double learningRate = parameters.LearningRate;
            double beta1 = parameters.Beta1;
            double beta2 = parameters.Beta2;
            double epsilon = parameters.Epsilon;
            int iterations = parameters.MaxIterations;
            for (int t = 1; t <= iterations; t++)
            {
                double[] grad = NumericalGradient(function, point);
                for (int i = 0; i < dim; i++)
                {
                    m[i] = beta1 * m[i] + (1 - beta1) * grad[i];
                    v[i] = beta2 * v[i] + (1 - beta2) * grad[i] * grad[i];
                    double mHat = m[i] / (1 - Math.Pow(beta1, t));
                    double vHat = v[i] / (1 - Math.Pow(beta2, t));
                    point[i] -= learningRate * mHat / (Math.Sqrt(vHat) + epsilon);
                }
            }

            double minimumValue = function(point);
            return new OptimizationResult
            {
                MinimumPoint = point,
                MinimumValue = minimumValue
            };
        }

        private static double[] NumericalGradient(Func<double[], double> function, double[] point, double h = 1e-8)
        {
            int dim = point.Length;
            double[] grad = new double[dim];
            double[] pointForward = (double[])point.Clone();
            double[] pointBackward = (double[])point.Clone();
            for (int i = 0; i < dim; i++)
            {
                pointForward[i] += h;
                pointBackward[i] -= h;
                double fForward = function(pointForward);
                double fBackward = function(pointBackward);
                grad[i] = (fForward - fBackward) / (2 * h);
                pointForward[i] = point[i];
                pointBackward[i] = point[i];
            }

            return grad;
        }
    }
}