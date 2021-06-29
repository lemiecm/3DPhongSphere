using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _3DSphere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sphere globalSphere { get; set; } 
        public int M { get; set; }
        public int N { get; set; }

        private static DispatcherTimer dispatcherTimer;

        public Transformation transformation = new Transformation();
        public Camera camera = new Camera();
        public PhongAlgorithm shader = new PhongAlgorithm();
        public Material material = new Material();
        public MainWindow()
        {
            InitializeComponent();
            if (MyImage.Source == null)
            {
               
                MyImage.Source = InitializeImage(800, 800);

            }
            int m = 30;
            int n = 30;
            int r = 180;
            M = m;
            N = n;
            
            
            var spherePoints = new Sphere(m, n, r);
            globalSphere = spherePoints;
            Operations.TransformWithCamera(globalSphere, transformation,shader);
           
            material = new Material
            {
                Ks = 0.25,
                Kd = 0.75,
                M = 0.8
            };
            Shading();
            SetTimer();
            
        }

        private System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                var bitmap2 = new Bitmap(bitmap);
                bitmap2.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public BitmapSource InitializeImage(int width, int height)
        {

            // Calculate stride of source
            int stride = (width * PixelFormats.Bgra32.BitsPerPixel + 7) / 8;

            // Create data array to hold source pixel data
            int length = stride * height;
            byte[] data = new byte[length];



            // Loop for inversion(alpha is skipped)
            for (int i = 0; i < length; i += 4)
            {
                data[i] = 0;
                data[i + 1] = 0;
                data[i + 2] = 0;
                data[i + 3] = 255;

            }

            // Create a new BitmapSource from the inverted pixel buffer
            return BitmapSource.Create(
                width, height,
                96, 96, PixelFormats.Bgra32,
                null, data, stride);
        }
        private void Redraw()
        {
            ////MyImage.Source = InitializeImage(Convert.ToInt32(MyImage.Width), Convert.ToInt32(MyImage.Height));
            //MyImage.Source = InitializeImage(800, 800);
            //Bitmap tmpBitmap = BitmapFromSource((BitmapSource)MyImage.Source);
            //tmpBitmap = DrawObject.Color3DTriangles(M, N, tmpBitmap, globalSphere);
            //tmpBitmap = DrawObject.WireTriangles(M, N, tmpBitmap, globalSphere);
            //MyImage.Source = BitmapToImageSource(tmpBitmap);

            //RedrawThreeD();

            Shading();

        }
        private void RedrawThreeD()
        {
            MyImage.Source = InitializeImage(800, 800);
          
            MyImage.Source = DrawObject.DrawSimpleRec(M, N, (BitmapSource)MyImage.Source, globalSphere, transformation);
        }
        private void Shading()
        {
            MyImage.Source = InitializeImage(800, 800);
            
           
            //MyImage.Source = shader.PhongShading(M, N, (BitmapSource)MyImage.Source, material, transformation, globalSphere);
            Bitmap tmpBitmap = BitmapFromSource((BitmapSource)MyImage.Source);

            tmpBitmap = shader.PhongShading(M, N, tmpBitmap, material, transformation, globalSphere,System.Drawing.Color.Blue);
            tmpBitmap = DrawObject.WireTriangles(M, N, tmpBitmap, globalSphere);
            MyImage.Source = BitmapToImageSource(tmpBitmap);

        }
       
        private void RotateAroundX(double angle = Math.PI / 6)
        {
            var trans = new Transformation();
            transformation.beta += angle;

            Operations.TransformWithCamera(globalSphere, transformation, shader);

            Redraw();
        }
        private void RotateAroundY(double angle=Math.PI/6)
        {
            var trans = new Transformation();
            transformation.alpha += angle;

            Operations.TransformWithCamera(globalSphere, transformation, shader);

            Redraw();
        }
        private void RotateAroundZ(double angle = Math.PI / 6)
        {
            var trans = new Transformation();
            transformation.gamma += angle;

            Operations.TransformWithCamera(globalSphere, transformation, shader);

            Redraw();
        }
        private void MoveSphereHorizontal(int sign, double val = 20)
        {
            //var trans = new Transformation();
            transformation.hori += sign * val;
            //Operations.ResetVertices(globalSphere);
            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();

        }
        private void MoveSphereVertical(int sign, double val = 20)
        {
            var trans = new Transformation();
            trans.vert = sign * val;
            transformation.vert += sign * val;

            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();
        }
        private void MoveCameraHorizontal(int sign, double val = 10)
        {
            //var trans = new Transformation();
            transformation.camera.PosX  += sign * val;
            //transformation.camera.targetX += sign * val;

            //Operations.ResetVertices(globalSphere);
            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();

        }
        private void MoveCameraVertical(int sign, double val = 10)
        {
            //var trans = new Transformation();
            //trans.vert = sign * val;
            transformation.camera.PosY += sign * val;
            //transformation.camera.targetY += sign * val;

            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();
        }
        private void MoveLightHorizontal(int sign, double val = 10)
        {
            shader.MoveSourceHorizontal(sign,val);
            //Operations.TransformWithCamera(globalSphere, transformation);
            Redraw();

        }
        private void MoveLightVertical(int sign, double val = 10)
        {
            shader.MoveSourceVertical(sign, val);
            //Operations.TransformWithCamera(globalSphere, transformation);
            Redraw();
        }
        private void RotatingAnimation(double angle = Math.PI / 6)
        {
            //var trans = new Transformation();
            transformation.beta += Math.PI / 36;
            transformation.alpha += Math.PI / 36;
            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();


        }
        private void SetTimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RotatingAnimation();
        }
        private void MyImage_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.X:
                    if (dispatcherTimer.IsEnabled)
                    {
                        dispatcherTimer.Stop();

                    }
                    RotateAroundX();
                    break;
                case Key.Y:
                    if (dispatcherTimer.IsEnabled)
                    {
                        dispatcherTimer.Stop();

                    }
                    RotateAroundY();
                    break;
                case Key.Z:
                    if (dispatcherTimer.IsEnabled)
                    {
                        dispatcherTimer.Stop();

                    }
                    RotateAroundZ();
                    break;
                case Key.R:
                    if (!dispatcherTimer.IsEnabled)
                    {
                        dispatcherTimer.Start();

                    }
                    else
                    {
                        dispatcherTimer.Stop();

                    }
                    break;
                case Key.Up:
                    MoveCameraVertical(1);
                    break;
                case Key.Down:
                    MoveCameraVertical(-1);
                    break;
                case Key.Left:
                    MoveCameraHorizontal(1);
                    break;
                case Key.Right:
                    MoveCameraHorizontal(-1);
                    break;
                case Key.W:
                    MoveSphereVertical(-1);
                    break;
                case Key.S:
                    MoveSphereVertical(1);
                    break;
                case Key.A:
                    MoveSphereHorizontal(-1);
                    break;
                case Key.D:
                    MoveSphereHorizontal(1);
                    break;
                case Key.I:
                    MoveLightVertical(-1);
                    break;
                case Key.K:
                    MoveLightVertical(1);
                    break;
                case Key.J:
                    MoveLightHorizontal(-1);
                    break;
                case Key.L:
                    MoveLightHorizontal(1);
                    break;


            }


        }

        private void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Operations.TransformWithCamera(globalSphere, transformation, shader);
            Redraw();
        }

        private void mattButton_Checked(object sender, RoutedEventArgs e)
        {
            material = new Material
            {
                Ks = 0.5,
                Kd = 0.5,
                M = 0.4
            };
            Redraw();
        }

        private void metalicButton_Checked(object sender, RoutedEventArgs e)
        {
            material = new Material
            {
                Ks = 0.25,
                Kd = 0.75,
                M = 0.4
            };
            Redraw();
        }
    }
}
