//Copyright Warren Harding 2024.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithmOptimizer
{
    public class GeneticAlgorithm
    {
        private int PopulationSize;
        private int ChromosomeLength;
        private double CrossoverRate;
        private double MutationRate;
        private int Generations;
        public ConcurrentBag<Individual> Population { get; private set; }

        private static readonly System.Threading.ThreadLocal<Random> rand = new System.Threading.ThreadLocal<Random>(() => new Random());
        private Func<Individual, double> FitnessFunction;
        public GeneticAlgorithm(int populationSize, int chromosomeLength, double crossoverRate, double mutationRate, int generations, Func<Individual, double> fitnessFunction)
        {
            if (fitnessFunction == null)
                throw new ArgumentNullException(nameof(fitnessFunction));
            PopulationSize = populationSize;
            ChromosomeLength = chromosomeLength;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            Generations = generations;
            FitnessFunction = fitnessFunction;
            Population = new ConcurrentBag<Individual>();
        }

        public void Run()
        {
            InitializePopulation();
            EvaluateFitness();
            for (int generation = 0; generation < Generations; generation++)
            {
                var newPopulation = new ConcurrentBag<Individual>();
                // Elitism: retain the best individual
                var elite = Population.OrderBy(ind => ind.Fitness).FirstOrDefault();
                if (elite != null)
                {
                    newPopulation.Add(new Individual(elite.Chromosome) { Fitness = elite.Fitness });
                }

                Parallel.For(0, (PopulationSize / 2) - 1, i =>
                {
                    Individual parent1 = Selection();
                    Individual parent2 = Selection();
                    Individual child1, child2;
                    Crossover(parent1, parent2, out child1, out child2);
                    Mutation(child1);
                    Mutation(child2);
                    newPopulation.Add(child1);
                    newPopulation.Add(child2);
                });
                Population = newPopulation;
                EvaluateFitness();
                if (generation % 100 == 0)
                {
                    var best = Population.OrderBy(ind => ind.Fitness).FirstOrDefault();
                    if (best != null)
                    {
                        Console.WriteLine($"Generation {generation}: Best Fitness = {best.Fitness}");
                    }
                }
            }

            var finalBest = Population.OrderBy(ind => ind.Fitness).FirstOrDefault();
            if (finalBest != null)
            {
                Console.WriteLine($"Final Best Fitness = {finalBest.Fitness}");
            }
        }

        private void InitializePopulation()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Population.Add(new Individual(ChromosomeLength, rand.Value));
            }
        }

        private void EvaluateFitness()
        {
            Parallel.ForEach(Population, individual =>
            {
                individual.CalculateFitness(FitnessFunction);
            });
        }

        private Individual Selection()
        {
            // Tournament selection
            int tournamentSize = 5;
            var tournament = new List<Individual>();
            var populationList = Population.ToList();
            int populationCount = populationList.Count;
            if (populationCount == 0)
                throw new InvalidOperationException("Population is empty.");
            for (int i = 0; i < tournamentSize; i++)
            {
                int index = rand.Value.Next(populationCount);
                tournament.Add(populationList[index]);
            }

            return tournament.OrderBy(ind => ind.Fitness).FirstOrDefault();
        }

        private void Crossover(Individual parent1, Individual parent2, out Individual child1, out Individual child2)
        {
            if (parent1 == null || parent2 == null)
            {
                throw new ArgumentNullException("Parents cannot be null");
            }

            if (rand.Value.NextDouble() < CrossoverRate)
            {
                int crossoverPoint = rand.Value.Next(1, ChromosomeLength - 1);
                bool[] child1Chromosome = new bool[ChromosomeLength];
                bool[] child2Chromosome = new bool[ChromosomeLength];
                for (int i = 0; i < ChromosomeLength; i++)
                {
                    if (i < crossoverPoint)
                    {
                        child1Chromosome[i] = parent1.Chromosome[i];
                        child2Chromosome[i] = parent2.Chromosome[i];
                    }
                    else
                    {
                        child1Chromosome[i] = parent2.Chromosome[i];
                        child2Chromosome[i] = parent1.Chromosome[i];
                    }
                }

                child1 = new Individual(child1Chromosome);
                child2 = new Individual(child2Chromosome);
            }
            else
            {
                child1 = new Individual(parent1.Chromosome);
                child2 = new Individual(parent2.Chromosome);
            }
        }

        private void Mutation(Individual individual)
        {
            if (individual == null)
                throw new ArgumentNullException(nameof(individual));
            for (int i = 0; i < ChromosomeLength; i++)
            {
                if (rand.Value.NextDouble() < MutationRate)
                {
                    individual.Chromosome[i] = !individual.Chromosome[i];
                }
            }
        }
    }
}