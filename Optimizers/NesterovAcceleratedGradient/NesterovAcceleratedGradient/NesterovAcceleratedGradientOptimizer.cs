//Copyright Warren Harding 2024.
using System;
using System.Threading.Tasks;

namespace MyOptimizer
{
    public class NesterovAcceleratedGradientOptimizer
    {
        public double[] Optimize(Func<double[], double> objectiveFunction, double[] initialParameters, double learningRate, double momentum, int iterations)
        {
            if (objectiveFunction == null)
                throw new ArgumentNullException(nameof(objectiveFunction));
            if (initialParameters == null)
                throw new ArgumentNullException(nameof(initialParameters));
            double[] parameters = (double[])initialParameters.Clone();
            double[] velocity = new double[parameters.Length];
            for (int i = 0; i < iterations; i++)
            {
                double[] lookahead = new double[parameters.Length];
                for (int j = 0; j < parameters.Length; j++)
                {
                    lookahead[j] = parameters[j] + momentum * velocity[j];
                }

                // Compute gradient at lookahead
                double[] gradient = ComputeGradient(objectiveFunction, lookahead);
                for (int j = 0; j < parameters.Length; j++)
                {
                    velocity[j] = momentum * velocity[j] - learningRate * gradient[j];
                    parameters[j] += velocity[j];
                }
            }

            return parameters;
        }

        private double[] ComputeGradient(Func<double[], double> objectiveFunction, double[] parameters)
        {
            if (objectiveFunction == null)
                throw new ArgumentNullException(nameof(objectiveFunction));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));
            double epsilon = 1e-8;
            double[] gradient = new double[parameters.Length];
            double baseValue = objectiveFunction(parameters);
            Parallel.For(0, parameters.Length, j =>
            {
                double[] paramsPlus = (double[])parameters.Clone();
                paramsPlus[j] += epsilon;
                double valuePlus = objectiveFunction(paramsPlus);
                gradient[j] = (valuePlus - baseValue) / epsilon;
            });
            return gradient;
        }
    }
}