

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace _3DSphere
{
    public class PhongAlgorithm
    {
        public Vector Source = new Vector(400,400, -300);

        public const double Ii = 255;
        
        public const double Ka = 0.4;
        
        public void MoveSourceHorizontal(int sign, double val)
        {
            Source.X += sign * val;
        }
        public void MoveSourceVertical(int sign, double val)
        {
            Source.Y += sign * val;
        }

       
        public void OuterNormalize_v2(Vector point)
        {
          
            var radius = Math.Sqrt(point.X * point.X + point.Y * point.Y + point.Z * point.Z);
            if (radius != 0 && !Double.IsNaN(radius))
            {
                point.X /= radius;
                point.Y /= radius;
                point.Z /= radius;
                point.D = 0;
            }
            else
            {
                point.X = 0;
                point.Y = 0;
                point.Z = 0;
                point.D = 0;
            }
        }
      
        public Bitmap PhongShading(int m, int n, Bitmap source, Material material, Transformation transformation, Sphere sphere, System.Drawing.Color pixelColor)
        {
            var cameraPoint = new Vector(transformation.camera.PosX, transformation.camera.PosY, transformation.camera.PosZ);
            var radius = sphere.R;

           

           

            var color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            for (int i = 0; i < 2 * m * n; i++)
            {
                var tmpPoints = new List<Point3D>();
                tmpPoints = DrawObject.TriangleTo3DPoint(sphere.triangles[i]);
                var tmpTriangle = sphere.triangles[i];
                if (BackFaceCulling.isFrontFacing(tmpTriangle.vertices[0].Position,
                    tmpTriangle.vertices[1].Position, tmpTriangle.vertices[2].Position))
                {
                    var points = Fill.FillTriangle(tmpPoints);

                    foreach (var point in points)
                    {
                        if (point.X > 0 && point.Y > 0 && DrawObject.isInPicture((int)point.X, 800-1) && DrawObject.isInPicture((int)point.Y, 800-1))
                        {

                            var position = new Vector(point.X, point.Y, point.Z);
                            var normal = new Vector(point.X, point.Y, point.Z);

                            OuterNormalize_v2(normal);

                            var Ir = CalculatePointIntensity(material, normal, position, cameraPoint, Source, pixelColor.R, radius);
                            var Ig = CalculatePointIntensity(material, normal, position, cameraPoint, Source, pixelColor.G, radius);
                            var Ib = CalculatePointIntensity(material, normal, position, cameraPoint, Source, pixelColor.B, radius);

                            var red = Clamp(Ir);
                            var green = Clamp(Ig);
                            var blue = Clamp(Ib);

                            var newColor = System.Windows.Media.Color.FromRgb(red, green, blue);

                            source.SetPixel((int)point.X, (int)point.Y, System.Drawing.Color.FromArgb(red, green, blue));
                        }

                    }
                }
            }
           
            return source;
            






        }
       

        private static byte Clamp(double i)
        {
            if (i < 0)
            {
                return 0;
            }
            else if (i > 255)
            {
                return 255;
            }
            else
            {
                return (byte)i;
            }
        }

        private static Vector ComputeVector(Vector surfacePoint, Vector cameraPoint)
        {
            var denominator = Denominator(cameraPoint, surfacePoint);
            var X = (cameraPoint.X - surfacePoint.X) / denominator;
            var Y = (cameraPoint.Y - surfacePoint.Y) / denominator;
            var Z = (cameraPoint.Z - surfacePoint.Z) / denominator;

            return new Vector(X, Y, Z);
        }
        private static double Denominator(Vector c, Vector v)
        {
            var X = (c.X - v.X);
            var Y = (c.Y - v.Y);
            var Z = (c.Z - v.Z);
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
        private static Vector Direction(Vector n, Vector l)
        {
            var dot = DotProduct(n, l);
            var value = MultiplyByScalar(dot, n);
            value = MultiplyByScalar(2, value);
            return new Vector(value.X - l.X, value.Y - l.Y, value.Z - l.Z);
        }
     
        private static Vector MultiplyByScalar(double a, Vector v)
        {
            return new Vector(a * v.X, a * v.Y, a * v.Z);
        }

        private static double DotProduct(Vector a, Vector b)
        {
            return a.X * b.X + a.Y + b.Y + a.Z * b.Z;
        }
        private static double CalculatePointIntensity(Material material, Vector n, Vector position, Vector cameraPoint, Vector lightSource, byte Ia, double radius)
        {
            
            var v = ComputeVector(position, cameraPoint);
            var l = ComputeVector(position, lightSource);
            var r = Direction(n, l);
            var shader = new PhongAlgorithm();
            shader.OuterNormalize_v2(v);
            shader.OuterNormalize_v2(l);
            shader.OuterNormalize_v2(r);

            var ambient = Ia * Ka;
            var diffuse = material.Kd * Ii * Math.Max(DotProduct(n, l), 0);
            var spectular = material.Ks * Ii * Math.Pow(Math.Max(DotProduct(v, r), 0),material.M);
            var a = ambient + diffuse + spectular;
                
                
            return a;
        }
      

        

    }
}
