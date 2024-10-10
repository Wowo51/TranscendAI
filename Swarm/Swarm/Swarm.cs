// Copyright Warren Harding 2024.
using System;

namespace SwarmNamespace
{
    public static class Swarm
    {
        public static double[] FindMinima(Func<double[], double> objectiveFunction, int dimensions, int swarmSize, int iterations)
        {
            SwarmOptimizer.Initialize(dimensions, swarmSize);
            return SwarmOptimizer.Optimize(objectiveFunction, iterations);
        }
    }
}