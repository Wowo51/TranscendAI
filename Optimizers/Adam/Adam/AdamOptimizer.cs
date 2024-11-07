//Copyright Warren Harding 2024.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdamOptimizerNamespace
{
    public class AdamOptimizer
    {
        private readonly double _learningRate;
        private readonly double _beta1;
        private readonly double _beta2;
        private readonly double _epsilon;
        private int _iteration;
        private readonly ConcurrentDictionary<string, double> _m;
        private readonly ConcurrentDictionary<string, double> _v;
        public AdamOptimizer(double learningRate = 0.001, double beta1 = 0.9, double beta2 = 0.999, double epsilon = 1e-8)
        {
            _learningRate = learningRate;
            _beta1 = beta1;
            _beta2 = beta2;
            _epsilon = epsilon;
            _iteration = 0;
            _m = new ConcurrentDictionary<string, double>();
            _v = new ConcurrentDictionary<string, double>();
        }

        public void InitializeParameters(IEnumerable<string> parameters)
        {
            foreach (var param in parameters)
            {
                _m.TryAdd(param, 0.0);
                _v.TryAdd(param, 0.0);
            }
        }

        public Dictionary<string, double> UpdateParameters(Dictionary<string, double> gradients)
        {
            if (_m.IsEmpty || _v.IsEmpty)
            {
                InitializeParameters(gradients.Keys);
            }

            int currentIteration = System.Threading.Interlocked.Increment(ref _iteration);
            Parallel.ForEach(gradients, gradientPair =>
            {
                var param = gradientPair.Key;
                var grad = gradientPair.Value;
                // Update biased first moment estimate
                _m.AddOrUpdate(param, 0.0, (key, oldValue) => _beta1 * oldValue + (1 - _beta1) * grad);
                // Update biased second raw moment estimate
                _v.AddOrUpdate(param, 0.0, (key, oldValue) => _beta2 * oldValue + (1 - _beta2) * grad * grad);
            });
            var updatedParameters = new ConcurrentDictionary<string, double>();
            Parallel.ForEach(gradients.Keys, param =>
            {
                double mHat = _m[param] / (1 - Math.Pow(_beta1, currentIteration));
                double vHat = _v[param] / (1 - Math.Pow(_beta2, currentIteration));
                double update = _learningRate * mHat / (Math.Sqrt(vHat) + _epsilon);
                updatedParameters[param] = -update;
            });
            return updatedParameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}