//Copyright Warren Harding 2024.
using System;
using System.Linq;
using GeneticAlgorithmOptimizer;

namespace GeneticAlgorithmTest
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "";
            int testCount = 5;
            int passed = 0;
            for (int i = 1; i <= testCount; i++)
            {
                try
                {
                    // Define the fitness function: f(x, y, z) = (x-1)^2 + (y-2)^2 + (z-3)^2 + 4
                    Func<Individual, double> fitnessFunction = individual =>
                    {
                        double x = DecodeGene(individual.Chromosome, 0, 10, 0, 10);
                        double y = DecodeGene(individual.Chromosome, 10, 10, 0, 10);
                        double z = DecodeGene(individual.Chromosome, 20, 10, 0, 10);
                        return Math.Pow(x - 1, 2) + Math.Pow(y - 2, 2) + Math.Pow(z - 3, 2) + 4;
                    };
                    // Initialize GeneticAlgorithm with parameters and custom fitness function
                    GeneticAlgorithm ga = new GeneticAlgorithm(populationSize: 100, chromosomeLength: 30, crossoverRate: 0.7, mutationRate: 0.01, generations: 1000, fitnessFunction);
                    ga.Run();
                    // Access the Population
                    var population = ga.Population;
                    var best = population.OrderBy(ind => ind.Fitness).FirstOrDefault();
                    if (best != null && Math.Abs(best.Fitness - 4) < 1e-3)
                    {
                        report += $"Test {i}: Success. Fitness = {best.Fitness}\n";
                        passed++;
                    }
                    else
                    {
                        report += $"Test {i}: Failure. Fitness = {best.Fitness}\n";
                    }
                }
                catch (Exception ex)
                {
                    report += $"Test {i}: Error - {ex.Message}\n";
                }
            }

            report += $"Passed {passed} out of {testCount} tests.";
            return report;
        }

        private static double DecodeGene(bool[] chromosome, int start, int length, double min, double max)
        {
            if (chromosome == null)
                throw new ArgumentNullException(nameof(chromosome));
            if (start < 0 || length <= 0 || start + length > chromosome.Length)
                throw new ArgumentOutOfRangeException();
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