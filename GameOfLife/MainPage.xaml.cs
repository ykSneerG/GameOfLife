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
                new PropertyMetadata(default, OnLcellsCountPropertyChanged)
                );

        private static void OnLcellsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MainPage myUserControl = dependencyObject as MainPage;

            myUserControl.LcellsCountChanged(e);
        }

        private void LcellsCountChanged(DependencyPropertyChangedEventArgs e)
        {
            LcellsPercent = GolHelper.FractionToPercentage(LcellsCount, Board.AmountOfCells);
        }

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
                new PropertyMetadata(default, OnScellsCountPropertyChanged)
                );

        private static void OnScellsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MainPage myUserControl = dependencyObject as MainPage;

            myUserControl.ScellsCountChanged(e);
        }

        private void ScellsCountChanged(DependencyPropertyChangedEventArgs e)
        {
            ScellsPercent = GolHelper.FractionToPercentage(ScellsCount, Board.AmountOfCells);
        }

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
                new PropertyMetadata(default, OnDcellsCountPropertyChanged)
                );

        private static void OnDcellsCountPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            MainPage myUserControl = dependencyObject as MainPage;

            myUserControl.DcellsCountChanged(e);
        }

        private void DcellsCountChanged(DependencyPropertyChangedEventArgs e)
        {
            DcellsPercent = GolHelper.FractionToPercentage(DcellsCount, Board.AmountOfCells);
        }

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

            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(20);
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


            Info = $"Cells: {Board.AmountOfCells} ({Board.Amount.X}*{Board.Amount.Y})";


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
        }


        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            SetDead(Squares, Squares.Keys.ToArray());

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

                BtnPlay.Content = "PLAY";
            }
            else
            {
                dispatcherTimer.Start();

                BtnPlay.Content = "PAUSE";
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

            InitialGeneration();


            GolHelper.Record(ref GenMinus, GenStart);

            GenerationStop(GenMinus, dispatcherTimer);


            FindLivingCells(Lcells, GenStart.Keys);

            FindSurroundingCells(Scells, Lcells.Keys);


            HashSet<Coord2D> set = GenStart.Keys.ToHashSet();

            GenStart = GolHelper.MergeLeft(Lcells, Scells);

            set.SymmetricExceptWith(GenStart.Keys.ToArray());

            SetDead(Squares, set);


            ColorizeGeneration();

            UpdateInformation();
        }


        private void UpdateInformation()
        {

            LcellsCount = Lcells.Count();

            DcellsCount = Board.AmountOfCells - LcellsCount;

            ScellsCount = GenStart.Count(x => x.Value == Status.Surround);
        }

        private void InitialGeneration()
        {

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


                Coord2D coord2D = GolBoard.NeighbourCell(Board.Amount, center, entry.Item1, entry.Item2);

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
                            GolBoard.NeighbourCell(Board.Amount, center, nx.Item1, nx.Item2),
                            Status.Surround
                            );
                }
            }
        }


        private void ColorizeGeneration()
        {

            foreach (KeyValuePair<Coord2D, Status> entry in GenStart)
            {

                _ = GenMinus[0].TryGetValue(entry.Key, out Status genMinus0Value);

                Squares[entry.Key].Fill = (genMinus0Value == entry.Value && entry.Value == Status.Life) ? ColorLifeStable : GetBrushFromStatus(entry.Value);
            }
        }


        #region STATIC HELPER

        private static void SetDead(Dictionary<Coord2D, Rectangle> squares, IEnumerable<Coord2D> collection)
        {

            foreach (Coord2D entry in collection)
            {
                bool found = squares.TryGetValue(entry, out Rectangle rec);

                if (found)
                {
                    rec.Fill = GolBrush.Dead;
                }
            }
        }

        private static void GenerationStop(Dictionary<Coord2D, Status>[] genMinus, DispatcherTimer timer)
        {

            if (genMinus[2].SequenceEqual(genMinus[0]))
            {
                timer.Stop();
            }
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

        #endregion
    }
}