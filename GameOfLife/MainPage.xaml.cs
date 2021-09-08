using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;


namespace GameOfLife
{

    public sealed partial class MainPage : Page
    {

        private static Brush ColorLifeStable { get; set; } = new SolidColorBrush();
        private static Brush ColorSurround { get; set; } = new SolidColorBrush();


        private readonly DispatcherTimer dispatcherTimer = new DispatcherTimer();


        //private Coord2D Amount = new Coord2D();
        private GolBoard Board = new GolBoard();


        public Dictionary<Coord2D, Rectangle> Squares = new Dictionary<Coord2D, Rectangle>();

        private Dictionary<Coord2D, Status> GenStart = new Dictionary<Coord2D, Status>();

        private Dictionary<Coord2D, Status>[] GenMinus = new Dictionary<Coord2D, Status>[3];


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


        #region Property: (Dictionary<Coord2D, Status>) Lcells

        public Dictionary<Coord2D, Status> Scells
        {
            get => (Dictionary<Coord2D, Status>)GetValue(ScellsProperty);
            set => SetValue(ScellsProperty, value);
        }

        private static readonly DependencyProperty ScellsProperty =
            DependencyProperty.Register(
                nameof(Scells),
                typeof(Dictionary<Coord2D, Status>),
                typeof(MainPage),
                new PropertyMetadata(new Dictionary<Coord2D, Status>())
                );

        #endregion

        #region Property: (int) ScellsCount

        public int ScellsCount
        {
            get => (int)GetValue(ScellsCountProperty);
            set => SetValue(ScellsCountProperty, value);
        }

        private static readonly DependencyProperty ScellsCountProperty =
            DependencyProperty.Register(
                nameof(ScellsCount),
                typeof(int),
                typeof(MainPage),
                new PropertyMetadata(default)
                );

        #endregion

        #region Property: (int) ScellsPercent

        public double ScellsPercent
        {
            get => (double)GetValue(ScellsPercentProperty);
            set => SetValue(ScellsPercentProperty, value);
        }

        private static readonly DependencyProperty ScellsPercentProperty =
            DependencyProperty.Register(
                nameof(ScellsPercent),
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

            for (int i = 0; i < GenMinus.Length; i++)
            {
                GenMinus[i] = new Dictionary<Coord2D, Status>();
            }

            ColorLifeStable = GolBrush.Life;
            ColorSurround = GolBrush.Dead;
        }

        private void TimerTick(object sender, object e)
        {
            NextGeneration();
        }

        #region UI: BUTTON

        private void BtnSet_Click(object sender, RoutedEventArgs e)
        {

            Playarea.Children.Clear();

            Squares.Clear();


            double distance = 0.75;
            _ = int.TryParse(ElemWidth.Text, out int pxWidth);
            double pxdistWidth = pxWidth + distance; ;

            Board.SetBoardWidth((byte)(Playarea.ActualWidth / pxdistWidth));
            Board.SetBoardHeight((byte)(Playarea.ActualHeight / pxdistWidth));

            //Board.Amount = new Coord2D(
            //    (byte)(Playarea.ActualWidth / pxdistWidth),
            //    (byte)(Playarea.ActualHeight / pxdistWidth)
            //    );


            Info = $"Area: {Board.AmountOfCells} cells";


            for (byte x = 0; x < Board.Amount.X; x++)
            {
                for (byte y = 0; y < Board.Amount.Y; y++)
                {
                    Rectangle rect = new Rectangle()
                    {
                        Width = pxWidth,
                        Height = pxWidth,
                        Fill = GolBrush.Dead
                    };
                    rect.PointerPressed += Rectangle_PointerPressed;
                    //rect.PointerMoved += Rectangle_PointerPressed;

                    Squares.Add(new Coord2D(x, y), rect);

                    Playarea.Children.Add(rect);
                    Canvas.SetLeft(rect, x * pxdistWidth + distance);
                    Canvas.SetTop(rect, y * pxdistWidth + distance);
                }
            }

            ElemTotal.Text = $"Total: {Board.AmountOfCells}";
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


            IEnumerable<Coord2D> coord2Ds = Board.RandomCells(percentSlider.Value);

            foreach (Coord2D entry in coord2Ds)
            {
                Squares[entry].Fill = GolBrush.Life;
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

        #region UI: TOGGLE_SWITCH

        private void ToggleSwitchStable_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            dispatcherTimer.Stop();


            foreach (var entry in Squares)
            {
                if (Squares[entry.Key].Fill == GolBrush.Stable)
                {
                    Squares[entry.Key].Fill = GolBrush.Life;
                }
            }

            dispatcherTimer.Start();

            ColorLifeStable = toggleSwitch.IsOn ? GolBrush.Stable : GolBrush.Life;
        }

        private void ToggleSwitchSurround_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;

            ColorSurround = toggleSwitch.IsOn ? GolBrush.Next : GolBrush.Dead;
        }


        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Rectangle rect = sender as Rectangle;

            rect.Fill = (rect.Fill == GolBrush.Dead) ? GolBrush.Life : GolBrush.Dead;
        }

        #endregion


        private void NextGeneration()
        {

            Generation++;


            if (Lcells.Count == 0)
            {

                GenStart.Clear();

                foreach (KeyValuePair<Coord2D, Rectangle> entry in Squares)
                {
                    if (entry.Value.Fill == GolBrush.Life || entry.Value.Fill == ColorLifeStable)
                    {
                        _ = GenStart.TryAdd(entry.Key, Status.Life);
                    }
                    if (entry.Value.Fill == ColorSurround)
                    {
                        _ = GenStart.TryAdd(entry.Key, Status.Surround);
                    }
                }
            }
            else
            {
                GenStart = GolHelper.MergeLeft(Lcells, Scells);
            }
            

            GolHelper.Record(ref GenMinus, GenStart);

            GenerationStop(GenMinus, dispatcherTimer);


            FindLivingCells(Lcells, GenStart.Keys);

            FindSurroundingCells(Scells, Lcells.Keys);


            ColorizeGeneration();
        }


        private void FindLivingCells(Dictionary<Coord2D, Status> alive, IEnumerable<Coord2D> coord2Ds)
        {

            alive.Clear();

            foreach (Coord2D coord2D in coord2Ds)
            {
                if (alive.ContainsKey(coord2D))
                {
                    continue;
                }

                if (IsCellAlive(coord2D))
                {
                    _ = alive.TryAdd(coord2D, Status.Life);
                }
            }
        }

        private bool IsCellAlive(Coord2D center)
        {

            bool alive = false;

            int neighbour = 0;


            foreach(Tuple<sbyte, sbyte> entry in GolHelper.NextCells)
            {

                if (neighbour > 3)
                {
                    break;
                }


                Coord2D coord2D = NeighbourCell(Board.Amount, center, entry.Item1, entry.Item2);

                if (Squares[coord2D].Fill == GolBrush.Life || Squares[coord2D].Fill == ColorLifeStable)
                {
                    neighbour++;
                }
            }


            if (neighbour == 3 || (Squares[center].Fill == GolBrush.Life && neighbour == 2) || (Squares[center].Fill == ColorLifeStable && neighbour == 2))
            {
                alive = true;
            }

            return alive;
        }

        private void FindSurroundingCells(Dictionary<Coord2D, Status> surround, IEnumerable<Coord2D> coord2Ds)
        {

            surround.Clear();

            foreach (Coord2D center in coord2Ds)
            {
                foreach (Tuple<sbyte, sbyte> nx in GolHelper.NextCells)
                {
                    _ = surround.TryAdd(
                            NeighbourCell(Board.Amount, center, nx.Item1, nx.Item2),
                            Status.Surround
                            );
                }
            }
        }


        private void ColorizeGeneration()
        {

            SetDead(GenStart.Keys.ToArray());


            Dictionary<Coord2D, Status> mergedCells = GolHelper.MergeLeft(Lcells, Scells);

            foreach (KeyValuePair<Coord2D, Status> entry in mergedCells)
            {
                
                Brush brush = GetBrushFromStatus(entry.Value);

                //if (Squares[entry.Key].Fill != brush)
                //{
                //    Squares[entry.Key].Fill = brush;
                //}

                _ = GenStart.TryGetValue(entry.Key, out Status statusOld);

                if (statusOld == entry.Value && entry.Value == Status.Life)
                {
                    Squares[entry.Key].Fill = ColorLifeStable;
                }
                else
                {
                    Squares[entry.Key].Fill = brush;
                }
            }


            #region INFORMATION update

            int amount = Board.Amount.X * Board.Amount.Y;

            LcellsCount = Lcells.Count();

            LcellsPercent = Math.Round(LcellsCount / (double)amount * 100, 1);

            DcellsCount = amount - LcellsCount;

            DcellsPercent = Math.Round(DcellsCount / (double)amount * 100, 1);

            ScellsCount = mergedCells.Count(x => x.Value == Status.Surround);

            ScellsPercent = Math.Round(ScellsCount / (double)amount * 100, 1);

            #endregion
        }

        private void SetDead(IEnumerable<Coord2D> collection)
        {
            foreach (Coord2D entry in collection)
            {
                _ = Squares.TryGetValue(entry, out Rectangle rec);

                rec.Fill = GolBrush.Dead;
            }
        }


        private static void GenerationStop(Dictionary<Coord2D, Status>[] genMinus, DispatcherTimer timer)
        {

            if (genMinus[2].SequenceEqual(genMinus[0]))
            {
                timer.Stop();
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


        private static Brush GetBrushFromStatus(Status status)
        {

            switch (status)
            {
                case Status.Life:
                    return GolBrush.Life;
                case Status.Surround:
                    return ColorSurround;
                case Status.Dead:
                    return GolBrush.Dead;
                default:
                    return GolBrush.Dead;
            }
        }

    }
}