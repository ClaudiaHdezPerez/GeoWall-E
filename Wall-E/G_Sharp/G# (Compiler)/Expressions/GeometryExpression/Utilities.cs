namespace G_Sharp;
public static class Utilities
{
    public static float PointInLine(float m, float n, float x)
    {
        var result = m * x + n;

        if (float.IsInfinity(m))
        {
            result = ParsingSupplies.CreateRandomsCoordinates();
        }

        return result;
    }

    public static (float, float) LineEquation(Points p1, Points p2)
    {

        float m = (p1.Y - p2.Y) / (p1.X - p2.X);
        float n = p1.Y - m * p1.X;

        return (m, n);
    }

    public static Points IntersectionCircle_Ray(Points center, Points ray_end, float radio, float m, float n)
    {
        double x_c = center.X;
        double y_c = center.Y;
        double a = Math.Pow(m, 2) + 1;
        double b = 2 * (x_c - m * (n - y_c));
        double c = Math.Pow(n - y_c, 2) - Math.Pow(radio, 2) + Math.Pow(x_c, 2);

        double D = Math.Pow(b, 2) - 4 * a * c;

        double x_1 = (b + Math.Sqrt(D)) / (2 * a);
        double x_2 = (b - Math.Sqrt(D)) / (2 * a);

        if (IsInSegment(x_c, ray_end.X, x_1))
        {
            float y_1 = PointInLine(m, n, (float)x_1);
            return new Points((float)x_1, y_1);
        }
        else if (IsInSegment(x_c, ray_end.X, x_2))
        {
            float y_2 = PointInLine(m, n, (float)x_2);
            return new Points((float)x_2, y_2); 
        }

        return new Points(0,0);
    }

    public static bool IsInSegment(double x1, double x2, double x_point)
    {
        var errorRange = 0.5;
        if (Math.Abs(x_point - x2) <= errorRange || Math.Abs(x_point - x1) <= errorRange)
            return true;

        double razon = (x_point - x1) / (x2 - x_point);
        return razon >= 0;
    }

    public static float DistanceBetweenPoints(Points p1, Points p2)
    {
        double x_1 = p1.X;
        double y_1 = p1.Y;
        double x_2 = p2.X;
        double y_2 = p2.Y;

        double distance = Math.Sqrt(Math.Pow(x_1 - x_2, 2) + Math.Pow(y_1 - y_2, 2));

        return (float)distance;
    }

    public static float AngleBetweenVectors(Points center, Points p1, Points p2)
    {
        (float, float) v1 = (p1.X - center.X, p1.Y - center.Y);
        (float, float) v2 = (p2.X - center.X, p2.Y - center.Y);
        float scalarProduct = v1.Item1 * v2.Item1 + v1.Item2 * v2.Item2;
        float distance_v1 = DistanceBetweenPoints(center, p1);
        float distance_v2 = DistanceBetweenPoints(center, p2);

        float angle = (float) Math.Acos(scalarProduct / (distance_v1 * distance_v2));
        angle = (float) (angle / Math.PI) * 180;

        return angle;
    }

    public static int GetAngleQuadrant(Points center, float radio, Points point)
    {
        Points right = new(center.X + radio, center.Y);
        Points up = new(center.X, center.Y - radio);

        if (point.X == up.X)
            return (point.Y == up.Y) ? 4 : 2;

        if (point.Y == right.Y)
            return (point.X == right.X) ? 1 : 3;

        if (point.X > up.X)
            return (point.Y > right.Y) ? 1 : 4;

        else
            return (point.Y > right.Y) ? 2 : 3;
    }

    public static float DistancePointLine(Points point, Line line)
    {
        var enumerador = (float)Math.Abs(line.M * point.X - point.Y + line.N);
        var denominador = (float)Math.Sqrt(Math.Pow(line.M, 2) + 1);

        return enumerador / denominador;
    }
}
