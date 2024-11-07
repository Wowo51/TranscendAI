using System;

namespace GeneticAlgorithmOptimizer
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<Individual, double> fitnessFunction = individual =>
            {
                if (individual == null || individual.Chromosome == null)
                    throw new ArgumentNullException("Individual or Chromosome is null");
                double x = DecodeGene(individual.Chromosome, 0, 10, 0, 10);
                double y = DecodeGene(individual.Chromosome, 10, 10, 0, 10);
                double z = DecodeGene(individual.Chromosome, 20, 10, 0, 10);
                return Math.Pow(x - 1, 2) + Math.Pow(y - 2, 2) + Math.Pow(z - 3, 2) + 4;
            };
            GeneticAlgorithm ga = new GeneticAlgorithm(populationSize: 100, chromosomeLength: 30, crossoverRate: 0.7, mutationRate: 0.01, generations: 1000, fitnessFunction);
            ga.Run();
        }

        private static double DecodeGene(bool[] chromosome, int start, int length, double min, double max)
        {
            if (chromosome == null)
                throw new ArgumentNullException(nameof(chromosome));
            if (start < 0 || length <= 0 || (start + length) > chromosome.Length)
                throw new ArgumentOutOfRangeException("Invalid start or length for decoding gene.");
            int value = 0;
            for (int i = start; i < start + length; i++)
            {
                value = (value << 1) | (chromosome[i] ? 1 : 0);
            }

            double fraction = (double)value / (Math.Pow(2, length) - 1);
            return min + fraction * (max - min);
        }
    }
}