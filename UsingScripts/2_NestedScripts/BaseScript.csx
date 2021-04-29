class Point 
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static Point operator + (Point lhs, Point rhs)
    {
        lhs.X += rhs.X;
        lhs.Y += rhs.Y;
        return lhs;
    }

    public override string ToString()
    {
        return $"P = ({X},{Y})";
    }
}