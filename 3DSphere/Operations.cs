using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace _3DSphere
{
    // Source: https://github.com/Walczi123/Computer-Graphics-Projects
    public static class Operations
    {
        public static void TransformWithCamera(Sphere sphere, Transformation transformation, PhongAlgorithm shader)
        {
            double a = transformation.alpha;
            double b = transformation.beta;
            double g = transformation.gamma;
            double s = transformation.camera.width * (1 / Math.Tan(Math.PI / 8));

            Matrix4x4 Rz = new Matrix4x4((float)Math.Cos(g), (float)Math.Sin(g), 0, 0,
                                      (float)-Math.Sin(g), (float)Math.Cos(g), 0, 0,
                                      0, 0, 1, 0,
                                      0, 0, 0, 1);
            Matrix4x4 Ry = new Matrix4x4((float)Math.Cos(a), 0, (float)Math.Sin(a), 0,
                                        0, 1, 0, 0,
                                        (float)-Math.Sin(a), 0, (float)Math.Cos(a), 0,
                                        0, 0, 0, 1);
            Matrix4x4 Rx = new Matrix4x4(1, 0, 0, 0,
                                        0, (float)Math.Cos(b), (float)Math.Sin(b), 0,
                                        0, (float)-Math.Sin(b), (float)Math.Cos(b), 0,
                                        0, 0, 0, 1);
           
            Matrix4x4 P = new Matrix4x4(-(float)s, 0, (float)(transformation.camera.width), 0,
                                        0, (float)s, (float)(transformation.camera.height), 0,
                                        0, 0, 0, 1,
                                        0, 0, 1, 0);
            Matrix4x4 M = transformation.camera.Matrix();
            Matrix4x4 result1 = Rx * Ry*Rz;
           
            Matrix4x4 result2 = P * M;
            
            
            double x, y, z, d;
            ResetVertices(sphere);
            for (int i = 0; i < sphere.vertices.Length; i++)
            {
               
                //rotate about the origin
                x = sphere.vertices[i].Position.X;
                y = sphere.vertices[i].Position.Y;
                z = sphere.vertices[i].Position.Z;
                d = sphere.vertices[i].Position.D;


                sphere.vertices[i].Position.X = result1.M11 * x + result1.M12 * y + result1.M13 * z + result1.M14 * d;
                sphere.vertices[i].Position.Y = result1.M21 * x + result1.M22 * y + result1.M23 * z + result1.M24 * d;
                sphere.vertices[i].Position.Z = result1.M31 * x + result1.M32 * y + result1.M33 * z + result1.M34 * d;
                sphere.vertices[i].Position.D = result1.M41 * x + result1.M42 * y + result1.M43 * z + result1.M44 * d;

                sphere.vertices[i].Position.X += transformation.hori;
                sphere.vertices[i].Position.Y += transformation.vert;

                //projection and distance
                x = sphere.vertices[i].Position.X;
                y = sphere.vertices[i].Position.Y;
                z = sphere.vertices[i].Position.Z;
                d = sphere.vertices[i].Position.D;

                sphere.vertices[i].Position .X = result2.M11 * x + result2.M12 * y + result2.M13 * z + result2.M14 * d;
                sphere.vertices[i].Position .Y = result2.M21 * x + result2.M22 * y + result2.M23 * z + result2.M24 * d;
                sphere.vertices[i].Position .Z = result2.M31 * x + result2.M32 * y + result2.M33 * z + result2.M34 * d;
                sphere.vertices[i].Position .D = result2.M41 * x + result2.M42 * y + result2.M43 * z + result2.M44 * d;

                sphere.vertices[i].Position.X /= sphere.vertices[i].Position.D;
                sphere.vertices[i].Position.Y /= sphere.vertices[i].Position.D;
                sphere.vertices[i].Position.Z /= sphere.vertices[i].Position.D;
            }
        }

        public static double LengthOfVector((double, double, double) vec)
        {
            return Math.Pow(Math.Pow(vec.Item1, 2) + Math.Pow(vec.Item2, 2) + Math.Pow(vec.Item3, 2), (double)1 / 3);
        }

        public static (double, double, double) CrossProdOfVectors((double, double, double) vec1, (double, double, double) vec2)
        {
            (double, double, double) result = ((vec1.Item2 * vec2.Item3) - (vec1.Item3 * vec2.Item2),
                                            (vec1.Item3 * vec2.Item1) - (vec1.Item1 * vec2.Item3),
                                            (vec1.Item1 * vec2.Item2) - (vec1.Item2 * vec2.Item1));
            return result;
        }
        public static double MultiplicationOfVectors((double, double, double) vec1, (double, double, double) vec2)
        {
            double result = vec1.Item1 * vec2.Item1 + vec1.Item2 * vec2.Item2 + vec1.Item3 * vec2.Item3;
            return result;
        }
        public static void ResetVertices(Sphere sphere)
        {
            for (int i = 0; i < sphere.vertices.Length; i++)
            {
                var x = sphere.vertices[i].Normal.X;
                var y = sphere.vertices[i].Normal.Y;
                var z = sphere.vertices[i].Normal.Z;
                var d = sphere.vertices[i].Normal.D;
                sphere.vertices[i].Position.X = x* sphere.R;
                sphere.vertices[i].Position.Y= y * sphere.R;
                sphere.vertices[i].Position.Z = z * sphere.R;
                sphere.vertices[i].Position.D = 1;


            }
        }
    }
}
