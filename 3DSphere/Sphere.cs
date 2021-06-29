using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    public class Sphere
    {
        public Vertex[] vertices { get; set; }
        public Triangle[] triangles { get; set; }
        public int R { get; set; }
        public double ShiftX { get; set; }
        public double ShiftY { get; set; }
        public Sphere(int m, int n, int r)
        {
            R = r;
            
            VerticesInitialize(m, n, r);
            TriangleInitialize(m, n);
        }
        public void VerticesInitialize(int m, int n, int r)
        {
            int size = m * n + 2;
            vertices = new Vertex[size];
            vertices[0] = new Vertex(0, r, 0, r);
            vertices[m * n + 1] = new Vertex(0, -r, 0, r);
            for(int i =0; i<= n-1;i++)
            {
                for(int j =0; j<=m-1;j++)
                {
                    var XZcommonPart = Math.Sin(Math.PI / (n + 1) * (i + 1));
                    var X = r * Math.Cos((2 * Math.PI) / m * j) * XZcommonPart;
                    var Y = r * Math.Cos(Math.PI / (n + 1) * (i + 1));
                    var Z = r * Math.Sin(2 * Math.PI / m * j) * XZcommonPart;
                    vertices[i * m + j + 1] = new Vertex(X, Y, Z, r);
                }
            }
        }
        public void TriangleInitialize(int m, int n)
        {
            int size = m * n * 2;
            triangles = new Triangle[size];
            for(int i =0; i <=m-1;i++ )
            {
              
                if(i!=m-1)
                {
                    triangles[i] = new Triangle(vertices[0], vertices[i + 2], vertices[i + 1]);
                    triangles[(2 * n - 1) * m + i] = new Triangle(vertices[m * n + 1], vertices[(n - 1) * m + i + 1], vertices[(n - 1) * m + i + 2]);
                }
                else
                {
                    triangles[i] = new Triangle(vertices[0], vertices[1], vertices[m]);
                    triangles[(2 * n - 1) * m + i] = new Triangle(vertices[m * n + 1], vertices[m * n], vertices[(n - 1) * m + 1]);
                }
                
            }
            for (int i = 0; i <= n - 2; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    if (j != m)
                    {
                        triangles[(2 * i + 1) * m + j - 1] = new Triangle(vertices[i * m + j], vertices[i * m + j + 1], vertices[(i + 1) * m + j + 1]);
                        triangles[(2 * i + 2) * m + j - 1] = new Triangle(vertices[i * m + j], vertices[(i + 1) * m + j + 1], vertices[(i + 1) * m + j]);
                    }
                    else
                    {
                        triangles[(2 * i + 1) * m + j - 1] = new Triangle(vertices[(i + 1) * m], vertices[i * m + 1], vertices[(i + 1) * m + 1]);
                        triangles[(2 * i + 2) * m + j - 1] = new Triangle(vertices[(i + 1) * m], vertices[(i + 1) * m + 1], vertices[(i + 2) * m]);
                    }
                }
               
               
            }
        }
    }
}
