// Copyright Warren Harding 2024.
using System;
using System.Collections.Generic;

namespace SwarmNamespace
{
    public static class SwarmOptimizer
    {
        private static int _dimensions;
        private static int _swarmSize;
        private static List<Particle> _particles;
        public static double[] GlobalBestPosition { get; private set; } = null;
        public static double GlobalBestValue { get; private set; } = double.MaxValue;

        private static readonly Random RandomGenerator = new Random();
        public static void Initialize(int dimensions, int swarmSize)
        {
            _dimensions = dimensions;
            _swarmSize = swarmSize;
            GlobalBestPosition = new double[_dimensions];
            _particles = new List<Particle>();
            InitializeParticles();
        }

        private static void InitializeParticles()
        {
            for (int i = 0; i < _swarmSize; i++)
            {
                var position = new double[_dimensions];
                var velocity = new double[_dimensions];
                for (int d = 0; d < _dimensions; d++)
                {
                    position[d] = RandomGenerator.NextDouble() * 100 - 50; // Initialize between -50 and 50
                    velocity[d] = RandomGenerator.NextDouble() * 10 - 5;
                }

                _particles.Add(new Particle(position, velocity));
            }
        }

        public static double[] Optimize(Func<double[], double> objectiveFunction, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                foreach (var particle in _particles)
                {
                    double value = objectiveFunction(particle.Position);
                    if (value < particle.BestValue)
                    {
                        particle.BestValue = value;
                        particle.BestPosition = (double[])particle.Position.Clone();
                        if (value < GlobalBestValue)
                        {
                            GlobalBestValue = value;
                            GlobalBestPosition = (double[])particle.Position.Clone();
                        }
                    }
                }

                foreach (var particle in _particles)
                {
                    UpdateVelocity(particle);
                    UpdatePosition(particle);
                }
            }

            return GlobalBestPosition;
        }

        private static void UpdateVelocity(Particle particle)
        {
            for (int d = 0; d < particle.Position.Length; d++)
            {
                double r1 = RandomGenerator.NextDouble();
                double r2 = RandomGenerator.NextDouble();
                double cognitive = 2.0 * r1 * (particle.BestPosition[d] - particle.Position[d]);
                double social = 2.0 * r2 * (GlobalBestPosition[d] - particle.Position[d]);
                particle.Velocity[d] = cognitive + social;
            }
        }

        private static void UpdatePosition(Particle particle)
        {
            for (int d = 0; d < particle.Position.Length; d++)
            {
                particle.Position[d] += particle.Velocity[d];
                // Boundary check to prevent particles from exiting the search space
                if (particle.Position[d] > 100)
                    particle.Position[d] = 100;
                if (particle.Position[d] < -100)
                    particle.Position[d] = -100;
            }
        }
    }
}