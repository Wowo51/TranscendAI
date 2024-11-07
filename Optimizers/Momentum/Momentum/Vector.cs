//Copyright Warren Harding 2024.
using System;

namespace MomentumOptimizerApp
{
    public class Vector : IVector
    {
        public double[] Values { get; set; } = Array.Empty<double>();
        public int Dimension => Values.Length;

        public Vector()
        {
            Values = Array.Empty<double>();
        }

        public Vector(double[] values)
        {
            Values = values != null ? (double[])values.Clone() : Array.Empty<double>();
        }

        public IVector Add(IVector other)
        {
            if (other is not Vector otherVector)
            {
                throw new ArgumentException("Argument must be of type Vector.", nameof(other));
            }

            if (this.Dimension != otherVector.Dimension)
            {
                throw new InvalidOperationException("Vectors must be of the same dimension.");
            }

            double[] result = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                result[i] = this.Values[i] + otherVector.Values[i];
            }

            return new Vector(result);
        }

        public IVector Subtract(IVector other)
        {
            if (other is not Vector otherVector)
            {
                throw new ArgumentException("Argument must be of type Vector.", nameof(other));
            }

            if (this.Dimension != otherVector.Dimension)
            {
                throw new InvalidOperationException("Vectors must be of the same dimension.");
            }

            double[] result = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                result[i] = this.Values[i] - otherVector.Values[i];
            }

            return new Vector(result);
        }

        public IVector Multiply(double scalar)
        {
            double[] result = new double[Dimension];
            for (int i = 0; i < Dimension; i++)
            {
                result[i] = this.Values[i] * scalar;
            }

            return new Vector(result);
        }

        public double DistanceTo(IVector other)
        {
            if (other is not Vector otherVector)
            {
                throw new ArgumentException("Argument must be of type Vector.", nameof(other));
            }

            if (this.Dimension != otherVector.Dimension)
            {
                throw new InvalidOperationException("Vectors must be of the same dimension.");
            }

            double sum = 0.0;
            for (int i = 0; i < Dimension; i++)
            {
                double diff = this.Values[i] - otherVector.Values[i];
                sum += diff * diff;
            }

            return Math.Sqrt(sum);
        }

        public IVector Clone()
        {
            return new Vector(this.Values);
        }

        public override string ToString()
        {
            return string.Join(", ", Values);
        }
    }
}