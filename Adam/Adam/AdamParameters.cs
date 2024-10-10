//Copyright Warren Harding 2024.
namespace EthosWork.Adam
{
    public class AdamParameters
    {
        public double LearningRate { get; set; } = 0.001;
        public double Beta1 { get; set; } = 0.9;
        public double Beta2 { get; set; } = 0.999;
        public double Epsilon { get; set; } = 1e-8;
        public int MaxIterations { get; set; } = 1000;
    }
}