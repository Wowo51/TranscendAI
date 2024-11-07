//Copyright Warren Harding 2024.
using System;

namespace GeneticAlgorithmOptimizer
{
    public class Individual
    {
        public bool[] Chromosome { get; set; }
        public double Fitness { get; set; }

        public Individual(int length, Random random)
        {
            if (random == null)
                throw new ArgumentNullException(nameof(random));
            Chromosome = new bool[length];
            for (int i = 0; i < length; i++)
            {
                Chromosome[i] = random.NextDouble() > 0.5;
            }
        }

        public Individual(bool[] chromosome)
        {
            if (chromosome == null)
                throw new ArgumentNullException(nameof(chromosome));
            Chromosome = new bool[chromosome.Length];
            Array.Copy(chromosome, Chromosome, chromosome.Length);
        }

        public void CalculateFitness(Func<Individual, double> fitnessFunction)
        {
            if (fitnessFunction == null)
                throw new ArgumentNullException(nameof(fitnessFunction));
            Fitness = fitnessFunction(this);
        }
    }
}