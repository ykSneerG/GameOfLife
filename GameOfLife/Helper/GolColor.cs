using Windows.UI;
using Windows.UI.Xaml.Media;

namespace GameOfLife
{
    public static class GolColor
    {

        public static Color Dead = Color.FromArgb(255, 210, 210, 210);

        public static Color Next = Color.FromArgb(255, 190, 190, 190);

        public static Color Stable = Color.FromArgb(175, 255, 0, 0);

        public static Color Life = Color.FromArgb(255, 255, 0, 0);

    }

    public static class GolBrush
    {

        public static readonly Brush Dead = new SolidColorBrush(GolColor.Dead);

        public static readonly Brush Next = new SolidColorBrush(GolColor.Next);

        public static readonly Brush Stable = new SolidColorBrush(GolColor.Stable);

        public static readonly Brush Life = new SolidColorBrush(GolColor.Life);

    }
}