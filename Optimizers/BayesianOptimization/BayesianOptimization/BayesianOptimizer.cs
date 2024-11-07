// Copyright Warren Harding 2024.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BayesianOptimizationApp
{
    public class BayesianOptimizer
    {
        private readonly Func<double[], double> _objectiveFunction;
        private readonly double[, ] _bounds;
        private readonly int _iterations;
        private readonly int _parallelism;
        private readonly List<double[]> _samples = new List<double[]>();
        private readonly List<double> _values = new List<double>();
        private readonly object _lock = new object ();
        public BayesianOptimizer(Func<double[], double> objectiveFunction, double[, ] bounds, int iterations = 100, int parallelism = 4)
        {
            _objectiveFunction = objectiveFunction ?? throw new ArgumentNullException(nameof(objectiveFunction));
            _bounds = bounds ?? throw new ArgumentNullException(nameof(bounds));
            if (_bounds.GetLength(1) != 2)
                throw new ArgumentException("Bounds must have exactly two columns for lower and upper limits.", nameof(bounds));
            if (iterations <= 0)
                throw new ArgumentOutOfRangeException(nameof(iterations), "Iterations must be greater than zero.");
            _iterations = iterations;
            _parallelism = parallelism > 0 ? parallelism : throw new ArgumentOutOfRangeException(nameof(parallelism), "Parallelism must be greater than zero.");
        }

        public async Task<OptimizationResult> OptimizeAsync()
        {
            // Initial sampling
            InitializeSamples();
            for (int i = 0; i < _iterations; i++)
            {
                // Fit Gaussian Process
                var gp = new GaussianProcess(_samples, _values);
                // Acquire next points based on parallelism
                var tasks = new List<Task<(double[] Point, double Value)>>();
                for (int p = 0; p < _parallelism; p++)
                {
                    var nextPoint = gp.AcquireNextPoint(_bounds);
                    var task = Task.Run(() =>
                    {
                        double value = _objectiveFunction(nextPoint);
                        return (nextPoint, value);
                    });
                    tasks.Add(task);
                }

                var results = await Task.WhenAll(tasks);
                lock (_lock)
                {
                    foreach (var result in results)
                    {
                        _samples.Add(result.Point);
                        _values.Add(result.Value);
                    }
                }
            }

            // Find the best sample
            if (_values.Count == 0 || _samples.Count == 0)
                throw new InvalidOperationException("No samples were evaluated.");
            int bestIndex = _values.IndexOf(_values.Min());
            return new OptimizationResult
            {
                Parameters = _samples[bestIndex],
                Value = _values[bestIndex]
            };
        }

        private void InitializeSamples()
        {
            // Increased initial samples for better GP modeling
            var rand = new Random(0);
            int dimensions = _bounds.GetLength(0);
            int initialSamples = Math.Max(10, dimensions * 2);
            for (int i = 0; i < initialSamples; i++)
            {
                var sample = new double[dimensions];
                for (int d = 0; d < dimensions; d++)
                {
                    sample[d] = _bounds[d, 0] + rand.NextDouble() * (_bounds[d, 1] - _bounds[d, 0]);
                }

                double value = _objectiveFunction(sample);
                _samples.Add(sample);
                _values.Add(value);
            }
        }
    }

    public class OptimizationResult
    {
        public double[] Parameters { get; set; } = Array.Empty<double>();
        public double Value { get; set; }
    }
}