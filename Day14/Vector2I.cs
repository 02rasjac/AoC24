namespace Day14;

public struct Vector2I(int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    public static Vector2I operator +(Vector2I a, Vector2I b)
    {
        return new Vector2I(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2I operator -(Vector2I a, Vector2I b)
    {
        return new Vector2I(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2I operator -(Vector2I a)
    {
        return new Vector2I(-a.X, -a.Y);
    }

    public static int Dot(Vector2I a, Vector2I b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    public static Vector2I ElementProduct(Vector2I a, Vector2I b)
    {
        return new Vector2I(a.X * b.X, a.Y * b.Y);
    }

    public static Vector2I operator *(Vector2I a, int scalar)
    {
        return new Vector2I(a.X * scalar, a.Y * scalar);
    }

    public static Vector2I operator /(Vector2I a, int scalar)
    {
        return new Vector2I(a.X / scalar, a.Y / scalar);
    }
}