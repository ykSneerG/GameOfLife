using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;


namespace GameOfLife
{

    public sealed partial class MainPage : Page
    {

        private readonly SolidColorBrush ColorDead = new SolidColorBrush(Colors.LightGray);
        private readonly SolidColorBrush ColorLife = new SolidColorBrush(Colors.OrangeRed);

        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();


        private Coord2D Amount = new Coord2D();

        SolidColorBrush[] fills = new SolidColorBrush[1];



        public Dictionary<Coord2D, Rectangle> Squares = new Dictionary<Coord2D, Rectangle>();



        #region Property: (int) Generation

        public int Generation
        {
            get => (int)GetValue(GenerationProperty);
            set => SetValue(GenerationProperty, value);
        }

        private static readonly DependencyProperty GenerationProperty =
            DependencyProperty.Register(
                nameof(Generation),
                typeof(int),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion

        #region Property: (string) Info

        public string Info
        {
            get => (string)GetValue(InfoProperty);
            set => SetValue(InfoProperty, value);
        }

        private static readonly DependencyProperty InfoProperty =
            DependencyProperty.Register(
                nameof(Info),
                typeof(string),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion


        public MainPage()
        {
            InitializeComponent();

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(5);
            dispatcherTimer.Tick += TimerTick;
        }

        private void TimerTick(object sender, object e)
        {
            Generation++;

            NextGeneration();
        }


        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {

            Playarea.Children.Clear();

            Squares.Clear();


            double distance = 0.75;
            _ = int.TryParse(ElemWidth.Text, out int pxWidth);
            double pxdistWidth = pxWidth + distance; ;

            Amount.X = (byte)(Playarea.ActualWidth / pxdistWidth);

            Amount.Y = (byte)(Playarea.ActualHeight / pxdistWidth);

            Info = $"{Amount.X} x {Amount.Y}";



            for (byte x = 0; x < Amount.X; x++)
            {
                for (byte y = 0; y < Amount.Y; y++)
                {
                    Rectangle rect = new Rectangle()
                    {
                        Width = pxWidth,
                        Height = pxWidth,
                        Fill = ColorDead
                    };
                    rect.PointerPressed += Rectangle_PointerPressed;
                    rect.PointerMoved += Rectangle_PointerPressed;

                    Squares.Add(new Coord2D(x, y), rect);

                    Playarea.Children.Add(rect);
                    Canvas.SetLeft(rect, x * pxdistWidth + distance);
                    Canvas.SetTop(rect, y * pxdistWidth + distance);
                }
            }

            ElemTotal.Text = (Amount.X * Amount.Y).ToString();
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (dispatcherTimer.IsEnabled)
            {
                dispatcherTimer.Stop();

            }
            else
            {
                dispatcherTimer.Start();
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            foreach (KeyValuePair<Coord2D, Rectangle> entry in Squares)
            {
                _ = Squares.TryGetValue(entry.Key, out Rectangle rec);

                rec.Fill = ColorDead;
            }

            Generation = 0;
        }

        private void BtnFill_Click(object sender, RoutedEventArgs e)
        {
            ResizeArrayToDict(ref fills, Squares.Count);

            for (int i = 0; i < Squares.Count; i++)
            {

                Random random = new Random();

                int rand = random.Next(0, 8);

                fills[i] = (rand <= 2) ? ColorLife : ColorDead;
            }


            for (int i = 0; i < Squares.Count; i++)
            {
                Squares.ElementAt(i).Value.Fill = fills[i];
            }
        }


        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            rect.Fill = (rect.Fill == ColorDead) ? ColorLife : ColorDead;
        }




        private void NextGeneration()
        {
            Array.Resize(ref fills, Squares.Count);
            //ResizeArrayToDict(ref fills, Squares.Count);

            for (int i = 0; i < Squares.Count; i++)
            {
                int neighbour = 0;

                Coord2D center = Squares.ElementAt(i).Key;

                NeighbourStatus(ref neighbour, center, -1, -1);
                NeighbourStatus(ref neighbour, center, 0, -1);
                NeighbourStatus(ref neighbour, center, 1, -1);

                NeighbourStatus(ref neighbour, center, -1, 0);
                NeighbourStatus(ref neighbour, center, 1, 0);

                NeighbourStatus(ref neighbour, center, -1, 1);
                NeighbourStatus(ref neighbour, center, 0, 1);
                NeighbourStatus(ref neighbour, center, 1, 1);



                Rectangle recEntry = Squares.ElementAt(i).Value;

                if (recEntry.Fill == ColorLife)
                {
                    fills[i] = neighbour < 2 || neighbour > 3 ? ColorDead : ColorLife;
                }

                if (recEntry.Fill == ColorDead)
                {
                    fills[i] = neighbour == 3 ? ColorLife : ColorDead;
                }
            }
            

            for (int i = 0; i < Squares.Count; i++)
            {
                Squares.ElementAt(i).Value.Fill = fills[i];
            }
        }


        private void NeighbourStatus(ref int neighbours, Coord2D center, sbyte nX, sbyte nY)
        {
            if (neighbours > 3)
                return;

            byte x = NeighbourCoordinate(center.X, nX, Amount.X);

            byte y = NeighbourCoordinate(center.Y, nY, Amount.Y);

            _ = Squares.TryGetValue(new Coord2D(x, y), out Rectangle rec);

            if (rec.Fill == ColorLife) 
            {
                neighbours++;
            }
        }


        private static byte NeighbourCoordinate(byte pt, sbyte dist, byte amount)
        {
            int ptdist = pt + dist;

            if (ptdist >= amount)
            {
                ptdist = 0;
            }

            if (ptdist < 0)
            {
                ptdist = amount - 1;
            }

            return (byte)ptdist;
        }

        private static void ResizeArrayToDict(ref SolidColorBrush[] arr, int length)
        {
            if (arr.Length != length)
            {
                arr = new SolidColorBrush[length];
            }
        }


    }
}