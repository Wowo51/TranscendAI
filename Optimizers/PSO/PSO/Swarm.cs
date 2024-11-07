//Copyright Warren Harding 2024.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace ParticleSwarmOptimizer
{
    public class Swarm
    {
        private int swarmSize;
        private int dimensions;
        private int maxIterations;
        private List<Particle> particles;
        private double[] globalBestPosition;
        private double globalBestFitness;
        private static ThreadLocal<Random> threadLocalRandom = new ThreadLocal<Random>(() => new Random());
        private double inertia = 0.5;
        private double cognitive = 1.5;
        private double social = 1.5;
        public Swarm(int swarmSize, int dimensions, int maxIterations)
        {
            this.swarmSize = swarmSize;
            this.dimensions = dimensions;
            this.maxIterations = maxIterations;
            particles = new List<Particle>();
            globalBestPosition = new double[dimensions];
            globalBestFitness = double.MaxValue;
        }

        public void Initialize()
        {
            for (int i = 0; i < swarmSize; i++)
            {
                Particle p = new Particle(dimensions, new Random());
                particles.Add(p);
                if (p.BestFitness < globalBestFitness)
                {
                    globalBestFitness = p.BestFitness;
                    Array.Copy(p.BestPosition, globalBestPosition, dimensions);
                }
            }
        }

        public void Optimize()
        {
            for (int iter = 0; iter < maxIterations; iter++)
            {
                Parallel.ForEach(particles, p =>
                {
                    for (int d = 0; d < dimensions; d++)
                    {
                        double r1 = threadLocalRandom.Value.NextDouble();
                        double r2 = threadLocalRandom.Value.NextDouble();
                        p.Velocity[d] = inertia * p.Velocity[d] + cognitive * r1 * (p.BestPosition[d] - p.Position[d]) + social * r2 * (globalBestPosition[d] - p.Position[d]);
                    }

                    p.UpdatePosition();
                });
                // Update global best
                foreach (var p in particles)
                {
                    if (p.BestFitness < globalBestFitness)
                    {
                        lock (this)
                        {
                            if (p.BestFitness < globalBestFitness)
                            {
                                globalBestFitness = p.BestFitness;
                                Array.Copy(p.BestPosition, globalBestPosition, dimensions);
                            }
                        }
                    }
                }

                if (iter % 100 == 0)
                {
                    Console.WriteLine($"Iteration {iter}, Best Fitness: {globalBestFitness}");
                }
            }

            Console.WriteLine($"Optimization completed. Best Fitness: {globalBestFitness}");
        }

        public double GetGlobalBestFitness()
        {
            return globalBestFitness;
        }
    }
}