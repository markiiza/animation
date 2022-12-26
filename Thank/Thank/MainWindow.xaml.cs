using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Thank
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool rendering = false;
        public MainWindow()
        {
            
            InitializeComponent();
            ImageUri = "fon.jpg";
            

        }

        private void cmdStart_Clicked(object sender, RoutedEventArgs e)
        {
            if (!rendering)
            {
                ellipses.Clear();
                canvas.Children.Clear();

                CompositionTarget.Rendering += RenderFrame;
                rendering = true;
            }

        }
        private void cmdStop_Clicked(object sender, RoutedEventArgs e)
        {
            StopRendering();
        }

        private void StopRendering()
        {
            CompositionTarget.Rendering -= RenderFrame;
            rendering = false;
        }

        private List<EllipseInfo> ellipses = new List<EllipseInfo>();

        private double accelerationY = 0.00000001;
        private int minStartingSpeed = 10;
        private int maxStartingSpeed = 400;
        private double speedRatio = 0.01;
        private int minEllipses = 600;
        private int maxEllipses = 2000;
        private int ellipseRadius = 5;

        private void RenderFrame(object sender, EventArgs e)
        {
            if (ellipses.Count == 0)
            {
                // Animation just started. Create the ellipses.
                int halfCanvasWidth = (int)canvas.ActualWidth / 2;

                Random rand = new Random();
                int ellipseCount = rand.Next(minEllipses, maxEllipses + 1);
                for (int i = 0; i < ellipseCount; i++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Fill = Brushes.White;
                    ellipse.Width = ellipseRadius;
                    ellipse.Height = ellipseRadius;
                    Canvas.SetLeft(ellipse, halfCanvasWidth + rand.Next(-halfCanvasWidth, halfCanvasWidth));
                    Canvas.SetTop(ellipse, 0);
                    canvas.Children.Add(ellipse);

                    EllipseInfo info = new EllipseInfo(ellipse, speedRatio * rand.Next(minStartingSpeed, maxStartingSpeed));
                    ellipses.Add(info);
                }
            }
            else
            {
                for (int i = ellipses.Count - 1; i >= 0; i--)
                {
                    EllipseInfo info = ellipses[i];
                    double top = Canvas.GetTop(info.Ellipse);
                    Canvas.SetTop(info.Ellipse, top + 1 * info.VelocityY);

                    if (top >= (canvas.ActualHeight - ellipseRadius * 2 - 10))
                    {
                        // This circle has reached the bottom.
                        // Stop animating it.
                        ellipses.Remove(info);
                    }
                    else
                    {
                        // Increase the velocity.
                        info.VelocityY += accelerationY;
                    }

                    if (ellipses.Count == 0)
                    {
                        // End the animation.
                        // There's no reason to keep calling this method
                        // if it has no work to do.
                        StopRendering();
                    }
                }
            }


        }
        private string imageUri;
        public string ImageUri
        {
            get
            {
                return imageUri;
            }
            set
            {
                imageUri = value;
                OnPropChanged("ImageUri");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropChanged(string prop)
        {
            var ev = PropertyChanged;
            if (ev == null) return;
            ev(this, new PropertyChangedEventArgs(prop));
        }
        public TranslateTransform trans = new TranslateTransform();
        private void mouse(object sender, MouseEventArgs e)
        {
            Random rnd = new Random();
            int x = rnd.Next(-100, 100);
            int y = rnd.Next(-100, 100);
            
            kartinka.RenderTransform = trans;
            DoubleAnimation daX = new DoubleAnimation( kartinka.ActualHeight + x, TimeSpan.FromSeconds(0.2));
            kartinka.RenderTransform.BeginAnimation(TranslateTransform.XProperty, daX);
            DoubleAnimation daY = new DoubleAnimation( kartinka.ActualHeight + y, TimeSpan.FromSeconds(0.2));
            kartinka.RenderTransform.BeginAnimation(TranslateTransform.YProperty, daY);
        }
    }

    public class EllipseInfo
    {
        public Ellipse Ellipse
        {
            get; set;
        }

        public double VelocityY
        {
            get; set;
        }

        public EllipseInfo(Ellipse ellipse, double velocityY)
        {
            VelocityY = velocityY;
            Ellipse = ellipse;
        }
    }
}

