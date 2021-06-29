using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    public class Triangle
    {
        public Vertex [] vertices { get; set; }
        public Triangle(Vertex v1, Vertex v2, Vertex v3)
        {
            vertices = new Vertex[3];
            vertices[0] = v1;
            vertices[1] = v2;
            vertices[2] = v3;

        }
    }
}
