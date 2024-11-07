//Copyright Warren Harding 2024.
using System;
using ParticleSwarmOptimizer;

namespace PSOTest
{
    public class UnitTests
    {
        public string Run()
        {
            string report = "";
            bool allTestsPassed = true;
            // Define the expected fitness value
            double expectedFitness = 4.0;
            Random rand = new Random();
            for (int i = 1; i <= 5; i++)
            {
                // Generate random swarm size between 20 and 50
                int swarmSize = rand.Next(20, 51);
                int dimensions = 3; // As per the fitness function requirement
                // Generate random max iterations between 500 and 1500
                int maxIterations = rand.Next(500, 1501);
                // Create and initialize the swarm
                Swarm swarm = new Swarm(swarmSize, dimensions, maxIterations);
                swarm.Initialize();
                swarm.Optimize();
                // Retrieve the best fitness from the swarm
                double actualFitness = swarm.GetGlobalBestFitness();
                // Compare the actual fitness with the expected fitness
                if (Math.Abs(actualFitness - expectedFitness) < 1e-6)
                {
                    report += $"Test {i}: Success\n";
                }
                else
                {
                    report += $"Test {i}: Failure - Expected Fitness: {expectedFitness}, Actual Fitness: {actualFitness}\n";
                    allTestsPassed = false;
                }
            }

            // Aggregate the overall result
            report += "\nOverall Result: ";
            report += allTestsPassed ? "All tests passed." : "Some tests failed.";
            return report;
        }
    }
}