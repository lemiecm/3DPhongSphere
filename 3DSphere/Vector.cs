using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double D { get; set; }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
            D = 1;
        }

        /// <summary>
        /// Normalizacja wektora
        /// </summary>
        public void Normalize(double radius)
        {
           
            if (radius != 0 && !Double.IsNaN(radius))
            {
                this.X = X / radius;
                this.Y = Y / radius;
                this.Z = Z / radius;
                this.D = 0;
            }
            else
            {
                X = 0;
                Y = 0;
                Z = 0;
                D = 0;
            }
            //var length = Math.Sqrt(X * X + Y * Y + Z * Z);
            //if (length != 0 && !Double.IsNaN(length))
            //{
            //    X = X / length;
            //    Y = Y / length;
            //    Z = Z / length;
            //}
            //else
            //{
            //    X = 0;
            //    Y = 0;
            //    Z = 0;
            //}
        }
        public void MyNormalize()
        {
            var radius = Math.Sqrt(X * X + Y * Y + Z * Z);
            if (radius != 0 && !Double.IsNaN(radius))
            {
                this.X = X / radius;
                this.Y = Y / radius;
                this.Z = Z / radius;
                this.D = 0;
            }
            else
            {
                X = 0;
                Y = 0;
                Z = 0;
                D = 0;
            }
            //var length = Math.Sqrt(X * X + Y * Y + Z * Z);
            //if (length != 0 && !Double.IsNaN(length))
            //{
            //    X = X / length;
            //    Y = Y / length;
            //    Z = Z / length;
            //}
            //else
            //{
            //    X = 0;
            //    Y = 0;
            //    Z = 0;
            //}
        }

        /// <summary>
        /// Mnożenie wektora przez wektor
        /// </summary>
        /// <param name="vector">Wektor</param>
        /// <returns>Liczba wynikowa</returns>
        public double Multiply(Vector vector)
        {
            return vector.X * X + vector.Y * Y + vector.Z * Z;
        }

        /// <summary>
        /// Mnożenie wektora przez liczbę
        /// </summary>
        /// <param name="value">Liczba</param>
        /// <returns>Nowy wektor</returns>
        public Vector Multiply(double value)
        {
            return new Vector(X * value, Y * value, Z * value);
        }
    }
}

