//Copyright Warren Harding 2024.
namespace MomentumOptimizerApp
{
    public interface IVector
    {
        double[] Values { get; set; }

        int Dimension { get; }

        IVector Add(IVector other);
        IVector Subtract(IVector other);
        IVector Multiply(double scalar);
        double DistanceTo(IVector other);
        IVector Clone();
    }
}