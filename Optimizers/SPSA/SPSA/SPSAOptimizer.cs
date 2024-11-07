//Copyright Warren Harding 2024.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SPSA
{
    public class SPSAOptimizer
    {
        private readonly Func<double[], double> _objectiveFunction;
        private readonly int _maxIterations;
        private readonly double _a;
        private readonly double _c;
        private readonly double _A;
        private readonly int _numThreads;
        private readonly object _lockObj = new object ();
        public SPSAOptimizer(Func<double[], double> objectiveFunction, int maxIterations = 5000, double a = 0.1, double c = 0.1, double A = 100.0, int numThreads = 4)
        {
            _objectiveFunction = objectiveFunction ?? throw new ArgumentNullException(nameof(objectiveFunction));
            _maxIterations = maxIterations;
            _a = a;
            _c = c;
            _A = A;
            _numThreads = numThreads;
        }

        public double[] Optimize(double[] initialPoint)
        {
            if (initialPoint == null)
                throw new ArgumentNullException(nameof(initialPoint));
            int dim = initialPoint.Length;
            double[] theta = new double[dim];
            Array.Copy(initialPoint, theta, dim);
            double[] gradient = new double[dim];
            for (int k = 0; k < _maxIterations; k++)
            {
                double ak = _a / Math.Pow(k + 1 + _A, 0.602);
                double ck = _c / Math.Pow(k + 1, 0.101);
                // Generate delta vector with random perturbations using thread-safe Random
                double[] delta = new double[dim];
                for (int i = 0; i < dim; i++)
                {
                    delta[i] = Random.Shared.NextDouble() < 0.5 ? -1.0 : 1.0;
                }

                double lossPlus = 0.0;
                double lossMinus = 0.0;
                Parallel.For(0, _numThreads, () => new Tuple<double, double>(0.0, 0.0), (threadId, loopState, localLoss) =>
                {
                    double[] thetaPlus = new double[dim];
                    double[] thetaMinus = new double[dim];
                    for (int i = 0; i < dim; i++)
                    {
                        thetaPlus[i] = theta[i] + ck * delta[i];
                        thetaMinus[i] = theta[i] - ck * delta[i];
                    }

                    double tempPlus = _objectiveFunction(thetaPlus);
                    double tempMinus = _objectiveFunction(thetaMinus);
                    return new Tuple<double, double>(localLoss.Item1 + tempPlus, localLoss.Item2 + tempMinus);
                }, localLoss =>
                {
                    lock (_lockObj)
                    {
                        lossPlus += localLoss.Item1;
                        lossMinus += localLoss.Item2;
                    }
                });
                lossPlus /= _numThreads;
                lossMinus /= _numThreads;
                // Gradient approximation
                for (int i = 0; i < dim; i++)
                {
                    gradient[i] = (lossPlus - lossMinus) / (2.0 * ck * delta[i]);
                }

                // Update theta
                for (int i = 0; i < dim; i++)
                {
                    theta[i] -= ak * gradient[i];
                }

                // Optionally, check for convergence
                if (Math.Abs(_objectiveFunction(theta) - 4.0) < 1e-6)
                {
                    break;
                }
            }

            return theta;
        }
    }
}