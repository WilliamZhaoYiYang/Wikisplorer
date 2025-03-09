namespace Wikisplorer
{
    public class Line
    {
        private Point start;
        private Point end;
        private Color color;

        public Point Start
        {
            get { return start; }
            set { start = value; }
        }

        public Point End
        {
            get { return end; }
            set { end = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public Line(Point Start, Point End, Color Color)
        {
            start = Start;
            end = End;
            color = Color;
        }
    }
}
