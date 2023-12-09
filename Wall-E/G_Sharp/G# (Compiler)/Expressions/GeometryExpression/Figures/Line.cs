using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Line : Figure, IEquatable<Line>
{
    private static readonly Dictionary<SyntaxKind, Func<Figure, Figure, FiniteSequence<object>>> intersections = new()
    {
        [SyntaxKind.LineToken] = IntersectLine,
        [SyntaxKind.SegmentToken] = IntersectSegment,
        [SyntaxKind.RayToken] = IntersectRay,
        [SyntaxKind.CircleToken] = IntersectCircle,
        [SyntaxKind.ArcToken] = IntersectArc,
    };

    public override SyntaxKind Kind => SyntaxKind.LineToken;
    public Points P1 { get; }
    public Points P2 { get; }
    public Points Start { get; }
    public Points End { get; }
    public float M { get; }
    public float N { get; }
    public override string ReturnType => "line";

    public Line(Points p1, Points p2)
    {
        P1 = p1;
        P2 = p2;
        (M, N) = Utilities.LineEquation(P1, P2);

        float x_start = P1.X - 50000;
        float x_end = P2.X + 50000;
        float y_start = Utilities.PointInLine(M, N, x_start);
        float y_end = Utilities.PointInLine(M, N, x_end);

        if (float.IsInfinity(M))
        {
            x_start = p1.X;
            x_end = p1.X;
            y_start = p1.Y - 5000;
            y_end = p2.Y + 5000;
        }

        Start = new Points(x_start, y_start);
        End = new Points(x_end, y_end); 
    }

    public override object Evaluate(Scope scope)
    {
        return this;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Line? other)
    {
        return M.Equals(other!.M) && N.Equals(other!.N);
    }

    public override bool Equals(object? obj) => Equals(obj as Line);
    public override int GetHashCode() => P1.GetHashCode();

    public override SequenceExpressionSyntax PointsInFigure()
    {
        Dictionary<int, object> elements = new();
        object PointsInLine()
        {
            float x;
            float y;
            do
            {
                x = ParsingSupplies.CreateRandomsCoordinates();
                y = Utilities.PointInLine(M, N, x);
            }
            while (!Utilities.IsInSegment(Start.X, End.X, x) || !Utilities.IsInSegment(Start.Y, End.Y, y));

            return new Points(x, y);
        }

        var result = new InfiniteSequence(PointsInLine, elements)
        {
            valuesType = "point"
        };

        return result;
    }

    public override FiniteSequence<object> Intersect(Figure figure)
    {
        return intersections[figure.Kind](this, figure);
    }

    private static FiniteSequence<object> IntersectLine(Figure figure1, Figure figure2)
    {
        var l1 = (Line)figure1;
        var l2 = (Line)figure2;

        if (l1.Equals(l2))
            return null!;

        if (l1.M == l2.M || float.IsInfinity(l1.M) && float.IsInfinity(l2.M))
            return new FiniteSequence<object>(new List<object>());

        float x = (l2.N - l1.N) / (l1.M - l2.M);

        var m = l1.M;
        var n = l1.N;

        if (float.IsNaN(x))
        {
            if (float.IsInfinity(m))
            {
                m = l2.M;
                n = l2.N;
                x = l1.P1.X;
            }

            else x = l2.P1.X;            
        }

        float y = Utilities.PointInLine(m, n, x);
        Points point = new(x, y);

        return new FiniteSequence<object>(new List<object>() { point });
    }

    private static FiniteSequence<object> IntersectSegment(Figure figure1, Figure figure2)
    {
        var line = (Line)figure1;
        var segment = (Segment)figure2;

        Line line1 = new(segment.P1, segment.P2);
        var intersect = IntersectLine(line, line1);

        if (intersect is null)
            return null!;

        if (intersect.Count > 0 && intersect is FiniteSequence<object> sequence)
        {
            var point = (Points)sequence[0];
            if (Utilities.IsInSegment(segment.P1.X, segment.P2.X, point.X))
            {
                return new FiniteSequence<object>(new List<object>() { point });
            }
        }

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectRay(Figure figure1, Figure figure2)
    {
        var line = (Line)figure1;
        var ray = (Ray)figure2;

        Segment segment = new(ray.P1, ray.End);
        return IntersectSegment(line, segment);
    }

    private static FiniteSequence<object> IntersectCircle(Figure figure1, Figure figure2)
    {
        var line = (Line)figure1;
        var circle = (Circle)figure2;

        var distance = Utilities.DistancePointLine(circle.Center, line);

        if (distance > circle.Radius)
            return new FiniteSequence<object>(new List<object>());

        var a = 1 + Math.Pow(line.M, 2);
        var b = 2 * line.M * (line.N - circle.Center.Y) - 2 * circle.Center.X;
        var c = Math.Pow(circle.Center.X, 2) + Math.Pow(line.N - circle.Center.Y, 2) - Math.Pow(circle.Radius, 2);

        var D = Math.Pow(b, 2) - 4 * a * c;

        var x1 = (float)((-b + Math.Sqrt(D)) / (2 * a));
        float y1 = Utilities.PointInLine(line.M, line.N, x1);

        Points p1 = new(x1, y1);
        if (D == 0)
            return new FiniteSequence<object>(new List<object>() { p1 });

        var x2 = (float)((-b - Math.Sqrt(D)) / (2 * a));
        float y2 = Utilities.PointInLine(line.M, line.N, x2);

        Points p2 = new(x2, y2);


        return new FiniteSequence<object>(new List<object>() { p1, p2 });
    }

    private static FiniteSequence<object> IntersectArc(Figure figure1, Figure figure2)
    {
        var line = (Line)figure1;
        var arc = (Arc)figure2;

        List<object> list = new();
        Circle arcCircle = new(arc.Center, arc.Measure);

        var intersect = IntersectCircle(line, arcCircle);

        if (intersect.Count > 0)
        {
            for (int i = 0; i < intersect.Count; i++)
            {
                var pointInArc = ((Points)intersect[i]).Intersect(arc);
                if ((int)pointInArc.Count > 0)
                    list.Add(intersect[i]);
            }
        }

        return new FiniteSequence<object>(list);
    }
}
