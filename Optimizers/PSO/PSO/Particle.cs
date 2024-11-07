//Copyright Warren Harding 2024.
using System;

namespace ParticleSwarmOptimizer
{
    public class Particle
    {
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }
        public double[] BestPosition { get; set; }
        public double BestFitness { get; set; }

        private Random random;
        public Particle(int dimensions, Random random)
        {
            this.random = random;
            Position = new double[dimensions];
            Velocity = new double[dimensions];
            BestPosition = new double[dimensions];
            Initialize();
        }

        private void Initialize()
        {
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] = random.NextDouble() * 10 - 5; // Initialize between -5 and 5
                Velocity[i] = random.NextDouble() - 0.5; // Initialize between -0.5 and 0.5
                BestPosition[i] = Position[i];
            }

            BestFitness = EvaluateFitness(Position);
        }

        public void UpdatePosition()
        {
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] += Velocity[i];
            }

            double fitness = EvaluateFitness(Position);
            if (fitness < BestFitness)
            {
                BestFitness = fitness;
                Array.Copy(Position, BestPosition, Position.Length);
            }
        }

        private double EvaluateFitness(double[] position)
        {
            if (position.Length < 3)
            {
                throw new ArgumentException("Position must have at least 3 dimensions.");
            }

            // Fitness function: (x-1)^2 + (y-2)^2 + (z-3)^2 + 4
            double fitness = Math.Pow(position[0] - 1, 2) + Math.Pow(position[1] - 2, 2) + Math.Pow(position[2] - 3, 2) + 4;
            return fitness;
        }
    }
}