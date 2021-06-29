using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    public static class BackFaceCulling
    {
        private static Vector CrossProduct(Vector a, Vector b)
        {
            return new Vector(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
        }
        public static bool isFrontFacing(Vector p1, Vector p2, Vector p3)
        {
            var firstVec = new Vector(p2.X - p1.X, p2.Y - p1.Y,0);
            var secondVec = new Vector(p3.X - p1.X, p3.Y - p1.Y,0);

            var result = CrossProduct(firstVec, secondVec);
            if (result.Z > 0)
                return true;
            return false;
        }

    }
}
