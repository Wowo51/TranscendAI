// Copyright Warren Harding 2024.
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace BayesianOptimizationApp
{
    public class GaussianProcess
    {
        private readonly List<double[]> _X;
        private readonly List<double> _y;
        private readonly double _lengthScale;
        private readonly double _sigmaF;
        private Matrix<double> _kernelMatrix = Matrix<double>.Build.Dense(0, 0);
        private Matrix<double> _kernelInverse = Matrix<double>.Build.Dense(0, 0);
        private bool _isTrained = false;
        public GaussianProcess(List<double[]> X, List<double> y, double lengthScale = 1.0, double sigmaF = 1.0)
        {
            _X = X ?? throw new ArgumentNullException(nameof(X));
            _y = y ?? throw new ArgumentNullException(nameof(y));
            if (_X.Count != _y.Count)
                throw new ArgumentException("The number of samples and values must be equal.", nameof(y));
            if (_X.Count == 0)
                throw new ArgumentException("Training data cannot be empty.", nameof(X));
            _lengthScale = lengthScale;
            _sigmaF = sigmaF;
            ComputeKernelMatrix();
            _isTrained = true;
        }

        private void ComputeKernelMatrix()
        {
            int n = _X.Count;
            _kernelMatrix = Matrix<double>.Build.Dense(n, n, 0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    double value = Kernel(_X[i], _X[j]);
                    _kernelMatrix[i, j] = value;
                    _kernelMatrix[j, i] = value;
                }
            }

            // Add noise term to the diagonal for numerical stability
            double noise = 1e-6;
            for (int i = 0; i < n; i++)
            {
                _kernelMatrix[i, i] += noise;
            }

            // Use LU decomposition instead of Cholesky for better numerical stability with non-positive definite matrices
            var lu = _kernelMatrix.LU();
            _kernelInverse = lu.Solve(Matrix<double>.Build.DenseIdentity(n));
        }

        public double[] AcquireNextPoint(double[, ] bounds)
        {
            if (!_isTrained)
                throw new InvalidOperationException("Gaussian Process is not trained.");
            // Implement Expected Improvement acquisition function
            // Increased number of samples for better acquisition
            int dimensions = bounds.GetLength(0);
            Random rand = new Random();
            double bestValue = _y.Min();
            double[] bestPoint = new double[dimensions];
            double maxEI = double.MinValue;
            for (int i = 0; i < 1000; i++)
            {
                var point = new double[dimensions];
                for (int d = 0; d < dimensions; d++)
                {
                    point[d] = bounds[d, 0] + rand.NextDouble() * (bounds[d, 1] - bounds[d, 0]);
                }

                var mu = PredictMean(point);
                var sigma = PredictStdDev(point);
                double ei = ExpectedImprovement(mu, sigma, bestValue);
                if (ei > maxEI)
                {
                    maxEI = ei;
                    bestPoint = point;
                }
            }

            // If no improvement found, return a random point
            if (maxEI == double.MinValue)
            {
                for (int d = 0; d < dimensions; d++)
                {
                    bestPoint[d] = bounds[d, 0] + rand.NextDouble() * (bounds[d, 1] - bounds[d, 0]);
                }
            }

            return bestPoint;
        }

        private double PredictMean(double[] point)
        {
            // Implement Gaussian Process predictive mean
            int n = _X.Count;
            Vector<double> k = Vector<double>.Build.Dense(n, 0);
            for (int i = 0; i < n; i++)
            {
                k[i] = Kernel(_X[i], point);
            }

            Vector<double> yVector = Vector<double>.Build.DenseOfEnumerable(_y);
            Vector<double> kernelY = _kernelInverse * yVector;
            double prediction = k.DotProduct(kernelY);
            return prediction;
        }

        private double PredictStdDev(double[] point)
        {
            // Implement Gaussian Process predictive standard deviation
            int n = _X.Count;
            Vector<double> k = Vector<double>.Build.Dense(n, 0);
            for (int i = 0; i < n; i++)
            {
                k[i] = Kernel(_X[i], point);
            }

            double kernelPoint = Kernel(point, point);
            double variance = kernelPoint - k.DotProduct(_kernelInverse * k);
            return Math.Sqrt(Math.Max(variance, 0));
        }

        private double Kernel(double[] x1, double[] x2)
        {
            double sum = 0.0;
            for (int i = 0; i < x1.Length; i++)
            {
                double diff = x1[i] - x2[i];
                sum += diff * diff;
            }

            return _sigmaF * Math.Exp(-0.5 * sum / (_lengthScale * _lengthScale));
        }

        private double ExpectedImprovement(double mu, double sigma, double best)
        {
            if (sigma <= 0)
                return 0;
            double z = (best - mu) / sigma;
            return (best - mu) * CDF(z) + sigma * PDF(z);
        }

        private double PDF(double z)
        {
            return Math.Exp(-0.5 * z * z) / Math.Sqrt(2 * Math.PI);
        }

        private double CDF(double z)
        {
            return 0.5 * (1.0 + Erf(z / Math.Sqrt(2)));
        }

        private double Erf(double x)
        {
            // Approximation of the error function
            // Abramowitz and Stegun formula 7.1.26
            double t = 1.0 / (1.0 + 0.5 * Math.Abs(x));
            double tau = t * Math.Exp(-x * x - 1.26551223 + 1.00002368 * t + 0.37409196 * Math.Pow(t, 2) + 0.09678418 * Math.Pow(t, 3) - 0.18628806 * Math.Pow(t, 4) + 0.27886807 * Math.Pow(t, 5) - 1.13520398 * Math.Pow(t, 6) + 1.48851587 * Math.Pow(t, 7) - 0.82215223 * Math.Pow(t, 8) + 0.17087277 * Math.Pow(t, 9));
            return x >= 0 ? 1 - tau : tau - 1;
        }
    }
}