//Copyright Warren Harding 2024.
using System;
using EthosWork.Adam;

namespace AdamTestApp.Tests
{
    public class UnitTests
    {
        public static string Run()
        {
            string report = "";
            bool allTestsPassed = true;
            // Define Adam parameters
            AdamParameters parameters = new AdamParameters
            {
                LearningRate = 0.1,
                Beta1 = 0.9,
                Beta2 = 0.999,
                Epsilon = 1e-8,
                MaxIterations = 10000
            };
            Adam adam = new Adam(parameters);
            // Test 1
            var test1Function = new Func<double[], double>(point => Math.Pow(point[0] - 1, 2) + Math.Pow(point[1] + 2, 2) + 4);
            double[] test1Initial = new double[]
            {
                0,
                0
            };
            var test1Result = adam.FindMinimum(test1Function, test1Initial);
            double test1ExpectedValue = 4;
            double test1ValueDifference = Math.Abs(test1Result.MinimumValue - test1ExpectedValue);
            bool test1Passed = test1ValueDifference <= 0.01;
            report += $"Test 1 - Function 1: {(test1Passed ? "Passed" : "Failed")}, Difference: {test1ValueDifference:F4}\n";
            if (!test1Passed)
                allTestsPassed = false;
            // Test 2
            var test2Function = new Func<double[], double>(point => Math.Pow(point[0] + 3, 2) + Math.Pow(point[1] - 1, 2) + 7);
            double[] test2Initial = new double[]
            {
                0,
                0
            };
            var test2Result = adam.FindMinimum(test2Function, test2Initial);
            double test2ExpectedValue = 7;
            double test2ValueDifference = Math.Abs(test2Result.MinimumValue - test2ExpectedValue);
            bool test2Passed = test2ValueDifference <= 0.01;
            report += $"Test 2 - Function 2: {(test2Passed ? "Passed" : "Failed")}, Difference: {test2ValueDifference:F4}\n";
            if (!test2Passed)
                allTestsPassed = false;
            // Test 3
            var test3Function = new Func<double[], double>(point => Math.Pow(point[0] - 5, 2) + Math.Pow(point[1] + 4, 2) + 10);
            double[] test3Initial = new double[]
            {
                0,
                0
            };
            var test3Result = adam.FindMinimum(test3Function, test3Initial);
            double test3ExpectedValue = 10;
            double test3ValueDifference = Math.Abs(test3Result.MinimumValue - test3ExpectedValue);
            bool test3Passed = test3ValueDifference <= 0.01;
            report += $"Test 3 - Function 3: {(test3Passed ? "Passed" : "Failed")}, Difference: {test3ValueDifference:F4}\n";
            if (!test3Passed)
                allTestsPassed = false;
            // Overall Result
            report += $"\nOverall Result: {(allTestsPassed ? "All tests passed." : "Some tests failed.")}";
            return report;
        }
    }
}