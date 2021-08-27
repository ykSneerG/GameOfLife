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

        private readonly SolidColorBrush ColorDead = new SolidColorBrush(Color.FromArgb(255, 210, 210, 210));
        private readonly SolidColorBrush ColorSurround = new SolidColorBrush(Color.FromArgb(255, 190, 190, 190));
        private readonly SolidColorBrush ColorLife = new SolidColorBrush(Colors.Red);
        

        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();


        private Coord2D Amount = new Coord2D();


        public Dictionary<Coord2D, Rectangle> Squares = new Dictionary<Coord2D, Rectangle>();

        private Dictionary<Coord2D, Status> Scells = new Dictionary<Coord2D, Status>();

        private Dictionary<Coord2D, Status> GenOld = new Dictionary<Coord2D, Status>();


        #region Property: (Dictionary<Coord2D, Status>) Lcells

        public Dictionary<Coord2D, Status> Lcells
        {
            get => (Dictionary<Coord2D, Status>)GetValue(LcellsProperty);
            set => SetValue(LcellsProperty, value);
        }

        private static readonly DependencyProperty LcellsProperty =
            DependencyProperty.Register(
                nameof(Lcells),
                typeof(Dictionary<Coord2D, Status>),
                typeof(MainPage),
                new PropertyMetadata(new Dictionary<Coord2D, Status>())
                );

        #endregion


        #region Property: (int) LcellsCount

        public int LcellsCount
        {
            get => (int)GetValue(LcellsCountProperty);
            set => SetValue(LcellsCountProperty, value);
        }

        private static readonly DependencyProperty LcellsCountProperty =
            DependencyProperty.Register(
                nameof(LcellsCount),
                typeof(int),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion

        #region Property: (int) LcellsPercent

        public double LcellsPercent
        {
            get => (double)GetValue(LcellsPercentProperty);
            set => SetValue(LcellsPercentProperty, value);
        }

        private static readonly DependencyProperty LcellsPercentProperty =
            DependencyProperty.Register(
                nameof(LcellsPercent),
                typeof(double),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion


        #region Property: (int) DcellsCount

        public int DcellsCount
        {
            get => (int)GetValue(DcellsCountProperty);
            set => SetValue(DcellsCountProperty, value);
        }

        private static readonly DependencyProperty DcellsCountProperty =
            DependencyProperty.Register(
                nameof(DcellsCount),
                typeof(int),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion

        #region Property: (int) DcellsPercent

        public double DcellsPercent
        {
            get => (double)GetValue(DcellsPercentProperty);
            set => SetValue(DcellsPercentProperty, value);
        }

        private static readonly DependencyProperty DcellsPercentProperty =
            DependencyProperty.Register(
                nameof(DcellsPercent),
                typeof(double),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion


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
            NextGeneration();
        }

        #region BUTTON

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {

            Playarea.Children.Clear();

            Squares.Clear();


            double distance = 0.75;
            _ = int.TryParse(ElemWidth.Text, out int pxWidth);
            double pxdistWidth = pxWidth + distance; ;

            Amount.X = (byte)(Playarea.ActualWidth / pxdistWidth);

            Amount.Y = (byte)(Playarea.ActualHeight / pxdistWidth);

            Info = $"Area: {Amount.X} x {Amount.Y} cells";



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
                    //rect.PointerMoved += Rectangle_PointerPressed;

                    Squares.Add(new Coord2D(x, y), rect);

                    Playarea.Children.Add(rect);
                    Canvas.SetLeft(rect, x * pxdistWidth + distance);
                    Canvas.SetTop(rect, y * pxdistWidth + distance);
                }
            }

            ElemTotal.Text = "Total: " + (Amount.X * Amount.Y).ToString();
        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            SetDead(Squares.Keys.ToArray());

            Generation = 0;
        }

        private void BtnFill_Click(object sender, RoutedEventArgs e)
        {

            Generation = 0;

            Lcells.Clear();
            Scells.Clear();
            GenOld.Clear();


            Random random = new Random();

            for (byte x = 0; x < Amount.X; x++)
            {
                for (byte y = 0; y < Amount.Y; y++)
                {
                    int rand = random.Next(0, 9);

                    if (rand <= 2)
                    {
                        Squares[new Coord2D(x, y)].Fill = ColorLife;
                    }
                }
            }

            NextGeneration();
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

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            NextGeneration();
        }

        #endregion

        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            rect.Fill = (rect.Fill == ColorDead) ? ColorLife : ColorDead;
        }



        private void NextGeneration()
        {
            Generation++;

            // ####################

            if (Lcells.Count == 0)
            {
                foreach(KeyValuePair<Coord2D, Rectangle> entry in Squares)
                {
                    if(entry.Value.Fill == ColorLife)
                    {
                        _ = GenOld.TryAdd(entry.Key, Status.Life);
                    }
                    if (entry.Value.Fill == ColorSurround)
                    {
                        _ = GenOld.TryAdd(entry.Key, Status.Surround);
                    }
                }
            }
            else
            {
                GenOld = GolHelper.MergeLeft(Lcells, Scells);
            }
            
            // ####################


            Lcells.Clear();

            foreach (Coord2D coord2D in GenOld.Keys)
            {
                NeighbourAnalyse(coord2D);
            }
           

            FindSurroundingCells();

            ColorizeGeneration();
        }


        private void NeighbourAnalyse(Coord2D center)
        {

            if (Lcells.ContainsKey(center))
            {
                return;
            }

            int neighbour = 0;

            foreach(Tuple<sbyte, sbyte> entry in GolHelper.NextCells)
            {

                if (neighbour > 3)
                {
                    break;
                }


                Coord2D coord2D = NeighbourCell(Amount, center, entry.Item1, entry.Item2);

                if (Squares[coord2D].Fill == ColorLife)
                {
                    neighbour++;
                }
            }


            if (neighbour == 3 || (Squares[center].Fill == ColorLife && neighbour == 2))
            {
                _ = Lcells.TryAdd(center, Status.Life);
            }
        }


        private void FindSurroundingCells()
        {

            Scells.Clear();

            foreach (Coord2D center in Lcells.Keys)
            {
                foreach (Tuple<sbyte, sbyte> nx in GolHelper.NextCells)
                {
                    _ = Scells.TryAdd(
                            NeighbourCell(Amount, center, nx.Item1, nx.Item2), 
                            Status.Surround
                            );
                }
            }
        }

        private void ColorizeGeneration()
        {

            SetDead(GenOld.Keys.ToArray());


            #region INFORMATION update

            int amount = Amount.X * Amount.Y;

            LcellsCount = Lcells.Count();

            LcellsPercent = Math.Round(LcellsCount / (double)amount * 100, 1);

            DcellsCount = amount - LcellsCount;

            DcellsPercent = Math.Round(DcellsCount / (double)amount * 100, 1);

            #endregion


            Dictionary<Coord2D, Status> mergedCells = GolHelper.MergeLeft(Lcells, Scells);

            foreach (KeyValuePair<Coord2D, Status> entry in mergedCells)
            {
                Brush brush;

                switch (entry.Value)
                {
                    case Status.Life:
                        brush = ColorLife;
                        break;
                    case Status.Surround:
                        brush = ColorSurround;
                        break;
                    default:
                        brush = ColorDead;
                        break;
                }

                Squares[entry.Key].Fill = brush;
            }
        }





        private static Coord2D NeighbourCell(Coord2D amount, Coord2D center, sbyte nX, sbyte nY)
        {

            byte x = NeighbourCoordinate(center.X, nX, amount.X);

            byte y = NeighbourCoordinate(center.Y, nY, amount.Y);

            return new Coord2D(x, y);
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


        private void SetDead(IEnumerable<Coord2D> collection)
        {
            foreach (Coord2D entry in collection)
            {
                _ = Squares.TryGetValue(entry, out Rectangle rec);

                rec.Fill = ColorDead;
            }
        }

    }
}