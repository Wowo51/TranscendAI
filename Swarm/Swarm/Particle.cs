// Copyright Warren Harding 2024.
namespace SwarmNamespace
{
    public class Particle
    {
        public double[] Position { get; set; }
        public double[] Velocity { get; set; }
        public double[] BestPosition { get; set; }
        public double BestValue { get; set; }

        public Particle(double[] position, double[] velocity)
        {
            Position = position;
            Velocity = velocity;
            BestPosition = (double[])position.Clone();
            BestValue = double.MaxValue;
        }
    }
}