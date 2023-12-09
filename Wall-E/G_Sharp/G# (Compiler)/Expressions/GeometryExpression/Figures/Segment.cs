using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Segment : Figure, IEquatable<Segment>
{
    private static Dictionary<SyntaxKind, Func<Figure, Figure, FiniteSequence<object>>> intersections = new()
    {
        [SyntaxKind.SegmentToken] = IntersectSegment,
        [SyntaxKind.RayToken] = IntersectRay,
        [SyntaxKind.CircleToken] = IntersectCircle,
        [SyntaxKind.ArcToken] = IntersectArc,
    };
    public override SyntaxKind Kind => SyntaxKind.SegmentToken;
    public Points P1 { get; }
    public Points P2 { get; }
    public float M { get; }
    public float N { get; }
    public override string ReturnType => "segment";

    public Segment(Points p1, Points p2)
    {
        P1 = p1;
        P2 = p2;

        (M, N) = Utilities.LineEquation(P1, P2);
    }

    public override object Evaluate(Scope scope)
    {
        return this;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Segment? other)
    {
        var thisMeasure = Utilities.DistanceBetweenPoints(P1, P2);
        var otherMeasure = Utilities.DistanceBetweenPoints(other!.P1, other.P2);
        return thisMeasure == otherMeasure;
    }

    public override bool Equals(object? obj) => Equals(obj as Segment);
    public override int GetHashCode() => P1.GetHashCode();

    public override SequenceExpressionSyntax PointsInFigure()
    {
        Dictionary<int, object> elements = new();
        object PointsInSegment()
        {
            float x;
            float y;
            do
            {
                x = ParsingSupplies.CreateRandomsCoordinates();
                y = Utilities.PointInLine(M, N, x);
            }
            while (!Utilities.IsInSegment(P1.X, P2.X, x) || !Utilities.IsInSegment(P1.Y, P2.Y, y));

            return new Points(x, y);
        }

        var result = new InfiniteSequence(PointsInSegment, elements)
        {
            valuesType = "point"
        };

        return result;
    }

    public override FiniteSequence<object> Intersect(Figure figure)
    {
        return intersections[figure.Kind](this, figure);
    }

    private static FiniteSequence<object> IntersectSegment(Figure figure1, Figure figure2)
    {
        var segment1 = (Segment)figure1;
        var segment2 = (Segment)figure2;

        Line line1 = new(segment1.P1, segment1.P2);
        Line line2 = new(segment2.P1, segment2.P2);

        var intersect = line1.Intersect(line2);

        if (intersect.Count > 0)
        {
            var point = (Points)intersect[0];

            if (Utilities.IsInSegment(segment1.P1.X, segment1.P2.X, point.X) &&
            Utilities.IsInSegment(segment2.P1.X, segment2.P2.X, point.X))
                return new FiniteSequence<object>(new List<object>() { point });
        }

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectRay(Figure figure1, Figure figure2)
    {
        var segment = (Segment)figure1;
        var ray = (Ray)figure2;

        Segment raySegment = new(ray.P1, ray.End);
        return IntersectSegment(segment, raySegment);
    }

    private static FiniteSequence<object> IntersectCircle(Figure figure1, Figure figure2)
    {
        var segment = (Segment)figure1;
        var circle = (Circle)figure2;

        List<object> list = new();
        Line line = new(segment.P1, segment.P2);

        var intersect = line.Intersect(circle);

        if (intersect.Count > 0)
        {
            for (int i = 0; i < intersect.Count; i++)
            {
                var point = (Points)intersect[i];
                if (Utilities.IsInSegment(segment.P1.X, segment.P2.X, point.X)) list.Add(point);
            }
        }

        return new FiniteSequence<object>(list);
    }

    private static FiniteSequence<object> IntersectArc(Figure figure1, Figure figure2)
    {
        var segment = (Segment)figure1;
        var arc = (Arc)figure2;

        List<object> list = new();
        Line line = new(segment.P1, segment.P2);

        var intersect = line.Intersect(arc);

        if (intersect.Count > 0)
        {
            for (int i = 0; i < intersect.Count; i++)
            {
                var point = (Points)intersect[i];
                if (Utilities.IsInSegment(segment.P1.X, segment.P2.X, point.X)) list.Add(point);
            }
        }

        return new FiniteSequence<object>(list);
    }
}