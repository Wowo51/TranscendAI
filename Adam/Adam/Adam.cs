// Copyright Warren Harding 2024.
using System;

namespace EthosWork.Adam
{
    public class Adam
    {
        private readonly AdamParameters _parameters;
        public Adam(AdamParameters parameters)
        {
            _parameters = parameters;
        }

        public OptimizationResult FindMinimum(Func<double[], double> function, double[] initialPoint)
        {
            return AdamOptimizer.FindMinimum(function, initialPoint, _parameters);
        }
    }
}