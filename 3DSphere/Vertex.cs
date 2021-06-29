using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    public class Vertex
    {
       public Vector Position { get; set; }
       public Vector Normal { get; set; }

        
        public Vertex(double X, double Y, double Z, double radius)
        {
            Position = new Vector(X, Y, Z);
            Normal = new Vector(X, Y, Z);
            Normal.Normalize(radius);
        }
     
    }
}
