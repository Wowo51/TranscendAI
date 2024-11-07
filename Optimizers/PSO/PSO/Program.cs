//Copyright Warren Harding 2024.
using System;

namespace ParticleSwarmOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            int swarmSize = 30;
            int dimensions = 30;
            int maxIterations = 1000;
            Swarm swarm = new Swarm(swarmSize, dimensions, maxIterations);
            swarm.Initialize();
            swarm.Optimize();
        }
    }
}