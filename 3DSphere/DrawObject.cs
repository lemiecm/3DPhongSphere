using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace _3DSphere
{
    public static class DrawObject
    {
        public static Bitmap InitializePoints(int m, int n, int r, Bitmap bitmap, Sphere spherePoints)
        {
           
            using (Graphics g = Graphics.FromImage(bitmap))
            {
              

                for (int i = 0; i < m*n+2;i++)
                {
                  
                    var x1 = (float)(spherePoints.vertices[i].Position.X );
                    var y1 = (float)(spherePoints.vertices[i].Position.Y );
                    var x2 = (float)(spherePoints.vertices[i].Position.X+1 );
                    var y2 = (float)(spherePoints.vertices[i].Position.Y+1 );

                    g.DrawLine(Pens.White, x1,y1, x2, y2);
                }
                
            }
            return bitmap;
        }
        public static Bitmap WireTriangles(int m, int n, Bitmap bitmap, Sphere sphere)
        {
           
            
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < 2*m * n ; i++)
                {

                    var tmpPoints = new PointF[3];
                    var tmpTriangle = sphere.triangles[i];
                    if(BackFaceCulling.isFrontFacing(tmpTriangle.vertices[0].Position, tmpTriangle.vertices[1].Position, tmpTriangle.vertices[2].Position))
                        tmpPoints = TriangleToPoint(sphere.triangles[i]);
                        g.DrawPolygon(Pens.White, tmpPoints);


                }

            }
            return bitmap;
        }
        
        public static Bitmap ColorTriangles(int m, int n, Bitmap bitmap, Sphere sphere)
        {
            SolidBrush blueBrush = new SolidBrush(System.Drawing.Color.Blue);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < 2 * m * n; i++)
                {

                    var tmpPoints = new PointF[3];
                    tmpPoints = TriangleToPoint(sphere.triangles[i]);
                    var tmpTriangle = sphere.triangles[i];
                    if (BackFaceCulling.isFrontFacing(tmpTriangle.vertices[0].Position, tmpTriangle.vertices[1].Position, tmpTriangle.vertices[2].Position))
                        g.FillPolygon(blueBrush, tmpPoints);
                    
                      
                }

            }
            return bitmap;
        }
        
       
        private static PointF[] TriangleToPoint(Triangle triangle)
        {
            PointF[] points = new PointF[3];
            for(int i =0; i< 3;i++)
            {
                var tmpPoint = new PointF((float)(triangle.vertices[i].Position.X),
                    (float)(triangle.vertices[i].Position.Y));
                points[i] = tmpPoint;
            }
            return points;
        }
        public static List<Point3D> TriangleTo3DPoint(Triangle triangle)
        {
            List<Point3D> points = new List<Point3D>();
            for (int i = 0; i < 3; i++)
            {
                var tmpPoint = new Point3D(triangle.vertices[i].Position.X,
                    triangle.vertices[i].Position.Y, triangle.vertices[i].Position.Z);
                points.Add(tmpPoint);
            }
            return points;
        }
        public static Bitmap Color3DTriangles(int m, int n, Bitmap bitmap, Sphere sphere)
        {
            SolidBrush blueBrush = new SolidBrush(System.Drawing.Color.Blue);
          
            using (Graphics g = Graphics.FromImage(bitmap))
            {


                for (int i = 0; i < 2 * m * n; i++)
                {

                    var tmpPoints = new List<Point3D>();
                    tmpPoints = TriangleTo3DPoint(sphere.triangles[i]);
                    var tmpTriangle = sphere.triangles[i];
                    if (BackFaceCulling.isFrontFacing(tmpTriangle.vertices[0].Position,
                        tmpTriangle.vertices[1].Position, tmpTriangle.vertices[2].Position))
                    {
                        var points = Fill.FillTriangle(tmpPoints);
                        foreach(var point in points)
                        {
                            g.DrawLine(Pens.Blue, (float)point.X, (float)point.Y, (float)point.X+1, (float)point.Y+1);

                        }
                    }
                        


                }

            }
            

           
            return bitmap;
        }
  

        public static BitmapSource DrawSimpleRec(int m, int n, BitmapSource source, Sphere sphere, Transformation transformation)
        {
            var cameraPoint = new Vector(transformation.camera.PosX, transformation.camera.PosY, transformation.camera.PosZ);

            // Calculate stride of source
            int stride = (source.PixelWidth * source.Format.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * source.PixelHeight;
            byte[] data = new byte[length];

            // Copy source image pixels to the data array
            source.CopyPixels(data, stride, 0);

            var color = System.Windows.Media.Color.FromRgb(0, 0, 255);
            for (int i = 0; i < 2 * m * n; i++)
            {

                var tmpPoints = new List<Point3D>();
                tmpPoints = TriangleTo3DPoint(sphere.triangles[i]);
                var tmpTriangle = sphere.triangles[i];
                if (BackFaceCulling.isFrontFacing(tmpTriangle.vertices[0].Position,
                    tmpTriangle.vertices[1].Position, tmpTriangle.vertices[2].Position))
                {
                    var points = Fill.FillTriangle(tmpPoints);


                    foreach (var point in points)
                    {
                        putPixel((int)point.X, (int)point.Y, color, stride, source.PixelWidth, source.PixelHeight, data);
                    }
                    
                }



            }
            
           

            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                source.PixelWidth, source.PixelHeight,
                96, 96, PixelFormats.Bgra32,
                null, data, stride);
        }

        public static bool isInPicture(int x, int width)
        {
            if (x >= 0 && x <= width)
                return true;
            else
                return false;

        }
        public static void putPixel(int x, int y, System.Windows.Media.Color color, int stride, int width, int height, byte[] data)
        {
            if (isInPicture(x, width) && isInPicture(y, height))
            {
                try
                {
                    data[4 * x + y * stride] = color.B;
                    data[4 * x + 1 + y * stride] = color.G;
                    data[4 * x + 2 + y * stride] = color.R;
                }
                catch (IndexOutOfRangeException e)  // CS0168
                {
                }
            }

        }
        public static System.Windows.Media.Color GetPixel(byte[] data, double ax, double ay, int stride)
        {
            int x = (int)ax;
            int y = (int)ay;

            var tmpIndex = x * 4 + stride * y;
            var tmpColor = new System.Windows.Media.Color();
            try
            {
                var tmpBlue = data[tmpIndex];
                var tmpGreen = data[tmpIndex + 1];
                var tmpRed = data[tmpIndex + 2];
                tmpColor = System.Windows.Media.Color.FromRgb(tmpRed, tmpGreen, tmpBlue);
            }
            catch (IndexOutOfRangeException e)  // CS0168
            {

            }

            return tmpColor;
        }
    }
}
