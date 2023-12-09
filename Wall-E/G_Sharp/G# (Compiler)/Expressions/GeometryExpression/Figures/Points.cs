using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Points : Figure, IEquatable<Points>
{
    private static readonly Dictionary<SyntaxKind, Func<Figure, Figure, FiniteSequence<object>>> intersections = new()
    {
        [SyntaxKind.PointToken] = IntersectPoint,
        [SyntaxKind.LineToken] = IntersectLine,
        [SyntaxKind.SegmentToken] = IntersectSegment,
        [SyntaxKind.RayToken] = IntersectRay,
        [SyntaxKind.CircleToken] = IntersectCircle,
        [SyntaxKind.ArcToken] = IntersectArc,
    };

    public override SyntaxKind Kind => SyntaxKind.PointToken;
    public float X { get; }
    public float Y { get; }
    public override string ReturnType => "point";

    public Points(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override object Evaluate(Scope scope)
    {
        return this;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Points? other)
    {
        return X.Equals(other!.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj) => Equals(obj as Points);

    public override int GetHashCode() => X.GetHashCode();

    public override SequenceExpressionSyntax PointsInFigure()
    {
        List<Points> point = new() { this };
        return new FiniteSequence<Points>(point);
    }

    public override FiniteSequence<object> Intersect(Figure figure)
    {
        return intersections[figure.Kind](this, figure);
    }

    public static FiniteSequence<object> IntersectPoint(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var other = (Points)figure2;

        if (point.Equals(other))
            return new FiniteSequence<object>(new List<object>() { point });

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectLine(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var line = (Line)figure2;

        var y1 = Utilities.PointInLine(line.M, line.N, point.X);
        var y2 = point.Y;
        var errorRange = 0.5;
        if (Math.Abs(y1 - y2) <= errorRange)
            return new FiniteSequence<object>(new List<object>() { point });

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectSegment(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var segment = (Segment)figure2;

        var y1 = Utilities.PointInLine(segment.M, segment.N, point.X);
        var y2 = point.Y;
        var errorRange = 0.5;
        if (Math.Abs(y1 - y2) <= errorRange && Utilities.IsInSegment(segment.P1.X, segment.P2.X, point.X))
            return new FiniteSequence<object>(new List<object>() { point });

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectRay(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var ray = (Ray)figure2;

        Segment raySegment = new(ray.P1, ray.End);
        return IntersectSegment(point, raySegment);
    }

    private static FiniteSequence<object> IntersectCircle(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var circle = (Circle)figure2;

        var distance = Utilities.DistanceBetweenPoints(point, circle.Center);
        var radius = circle.Radius;
        var errorRange = 0.5;
        if (Math.Abs(distance - radius) <= errorRange)
            return new FiniteSequence<object>(new List<object>() { point });

        return new FiniteSequence<object>(new List<object>());
    }

    private static FiniteSequence<object> IntersectArc(Figure figure1, Figure figure2)
    {
        var point = (Points)figure1;
        var arc = (Arc)figure2;

        var distance = Utilities.DistanceBetweenPoints(point, arc.Center);
        var radius = arc.Radius;
        var errorRange = 0.5;
        if (Math.Abs(distance - radius) > errorRange)
            return new FiniteSequence<object>(new List<object>());

        Segment centerPoint = new(arc.Center, point);
        Segment chord = new(arc.IntersectionCircleRay1, arc.IntersectionCircleRay2);
        bool intersect = chord.Intersect(centerPoint).Count > 0;

        if (Math.Abs(arc.SweepAngle) >= 180 && !intersect)
            return new FiniteSequence<object>(new List<object>() { point });

        if (Math.Abs(arc.SweepAngle) < 180 && intersect)
            return new FiniteSequence<object>(new List<object>() { point });


        return new FiniteSequence<object>(new List<object>());
    }
}
