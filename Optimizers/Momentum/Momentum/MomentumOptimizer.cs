//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace MomentumOptimizerApp
{
    public class MomentumOptimizer<T>
        where T : IVector, new()
    {
        private readonly double _learningRate;
        private readonly double _momentum;
        private readonly int _maxIterations;
        private readonly double _tolerance;
        public MomentumOptimizer(double learningRate, double momentum, int maxIterations, double tolerance)
        {
            _learningRate = learningRate;
            _momentum = momentum;
            _maxIterations = maxIterations;
            _tolerance = tolerance;
        }

        public T Optimize(Func<T, double> objectiveFunction, T initialParams)
        {
            IVector paramsCurrent = initialParams.Clone();
            IVector velocity = new Vector(new double[initialParams.Dimension]);
            for (int i = 0; i < _maxIterations; i++)
            {
                T gradient = ComputeGradient(objectiveFunction, (T)paramsCurrent);
                velocity = velocity.Multiply(_momentum).Subtract(gradient.Multiply(_learningRate));
                IVector paramsNext = paramsCurrent.Add(velocity);
                if (paramsNext.DistanceTo(paramsCurrent) < _tolerance)
                {
                    break;
                }

                paramsCurrent = paramsNext;
            }

            return (T)paramsCurrent;
        }

        private T ComputeGradient(Func<T, double> objectiveFunction, T parameters)
        {
            double epsilon = 1e-8;
            T gradient = new T();
            gradient.Values = new double[parameters.Dimension];
            Parallel.For(0, parameters.Dimension, i =>
            {
                T paramsPlus = (T)parameters.Clone();
                paramsPlus.Values[i] += epsilon;
                double fPlus = objectiveFunction(paramsPlus);
                T paramsMinus = (T)parameters.Clone();
                paramsMinus.Values[i] -= epsilon;
                double fMinus = objectiveFunction(paramsMinus);
                double grad = (fPlus - fMinus) / (2 * epsilon);
                gradient.Values[i] = grad;
            });
            return gradient;
        }
    }
}