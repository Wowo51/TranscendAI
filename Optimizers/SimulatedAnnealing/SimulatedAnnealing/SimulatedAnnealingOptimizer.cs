//Copyright Warren Harding 2024.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SimulatedAnnealingOptimizerApp
{
    public class SimulatedAnnealingOptimizer
    {
        private readonly double _initialTemperature;
        private readonly double _coolingRate;
        private readonly int _maxIterations;
        private static readonly ThreadLocal<Random> RandomGenerator = new ThreadLocal<Random>(() => new Random());
        public SimulatedAnnealingOptimizer(double initialTemperature, double coolingRate, int maxIterations)
        {
            _initialTemperature = initialTemperature;
            _coolingRate = coolingRate;
            _maxIterations = maxIterations;
        }

        public double[] Optimize(double[] initialSolution, Func<double[], double> objectiveFunction, int parallelRuns = 4)
        {
            if (initialSolution == null)
                throw new ArgumentNullException(nameof(initialSolution));
            if (objectiveFunction == null)
                throw new ArgumentNullException(nameof(objectiveFunction));
            double[] bestSolution = (double[])initialSolution.Clone();
            double bestEnergy = objectiveFunction(bestSolution);
            object lockObj = new object ();
            Parallel.For(0, parallelRuns, i =>
            {
                double[] currentSolution = (double[])initialSolution.Clone();
                double currentEnergy = objectiveFunction(currentSolution);
                double[] localBestSolution = (double[])currentSolution.Clone();
                double localBestEnergy = currentEnergy;
                double temperature = _initialTemperature;
                for (int j = 1; j <= _maxIterations; j++)
                {
                    if (temperature <= 0)
                        break;
                    double[] newSolution = GetNewSolution(currentSolution);
                    double newEnergy = objectiveFunction(newSolution);
                    double acceptanceProbability = Math.Exp((currentEnergy - newEnergy) / temperature);
                    double randValue = RandomGenerator.Value!.NextDouble();
                    if (newEnergy < currentEnergy || acceptanceProbability > randValue)
                    {
                        currentSolution = newSolution;
                        currentEnergy = newEnergy;
                        if (newEnergy < localBestEnergy)
                        {
                            localBestSolution = newSolution;
                            localBestEnergy = newEnergy;
                        }
                    }

                    temperature *= _coolingRate;
                }

                lock (lockObj)
                {
                    if (localBestEnergy < bestEnergy)
                    {
                        bestSolution = localBestSolution;
                        bestEnergy = localBestEnergy;
                    }
                }
            });
            return bestSolution;
        }

        private double[] GetNewSolution(double[] currentSolution)
        {
            double[] newSolution = new double[currentSolution.Length];
            double stepSize = 0.01;
            for (int j = 0; j < currentSolution.Length; j++)
            {
                newSolution[j] = currentSolution[j] + (RandomGenerator.Value!.NextDouble() - 0.5) * stepSize;
            }

            return newSolution;
        }
    }
}