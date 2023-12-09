using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Arc : Figure, IEquatable<Arc>
{
    private static Dictionary<SyntaxKind, Func<Figure, Figure, FiniteSequence<object>>> intersections = new()
    {
        [SyntaxKind.ArcToken] = IntersectArc,
    };
    public override SyntaxKind Kind => SyntaxKind.ArcToken;
    public Points Center { get; }
    public Points PointRay1 { get; }
    public Points PointRay2 { get; }
    public float Diameter => Radius * 2;
    public float Radius => Measure.Value;
    public Measure Measure { get; }
    public float StartAngle { get; }
    public float SweepAngle { get; }
    public Points IntersectionCircleRay1 { get; }
    public Points IntersectionCircleRay2 { get; }
    public override string ReturnType => "arc";

    public Arc(Points center, Points left, Points right, Measure measure)
    {
        Center = center;
        PointRay1 = left;
        PointRay2 = right;
        Measure = measure;

        (StartAngle, SweepAngle, IntersectionCircleRay1, IntersectionCircleRay2) = Components();
    }

    public override object Evaluate(Scope scope)
    {
        return this;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Arc? other)
    {
        var sameAngle = SweepAngle == other!.SweepAngle;
        return sameAngle && Measure.Equals(other.Measure);
    }

    public override bool Equals(object? obj) => Equals(obj as Arc);
    public override int GetHashCode() => Center.GetHashCode();

    private (float, float, Points, Points) Components()
    {
        Ray r1 = new(Center, PointRay1);
        Ray r2 = new(Center, PointRay2);

        Points point_left = Utilities.IntersectionCircle_Ray(Center, r1.End, Radius, r1.M, r1.N);
        Points point_right = Utilities.IntersectionCircle_Ray(Center, r2.End, Radius, r2.M, r2.N);

        Points cero = new(Center.X + Radius, Center.Y);

        float sweepAngle = Utilities.AngleBetweenVectors(Center, point_left, point_right);
        float angleLeftCero = Utilities.AngleBetweenVectors(Center, point_left, cero);
        float angleRightCero = Utilities.AngleBetweenVectors(Center, point_right, cero);
        float startAngle = 0;

        int quadrant_left = Utilities.GetAngleQuadrant(Center, Radius, point_left);
        int quadrant_right = Utilities.GetAngleQuadrant(Center, Radius, point_right);

        if (quadrant_left == quadrant_right)
        {
            startAngle = quadrant_left < 3 ? Math.Min(angleLeftCero, angleRightCero)
                                             : -Math.Max(angleLeftCero, angleRightCero);
        }

        else
        {
            if (quadrant_left == 1 || quadrant_right == 1)
            {
                startAngle = quadrant_left == 1 ? angleLeftCero : angleRightCero;
                float m = quadrant_left == 1 ? r1.M : r2.M;
                float n = quadrant_left == 1 ? r1.N : r2.N;
                Points point = quadrant_left != 1 ? point_left : point_right;
                float evaluationInRay = Utilities.PointInLine(m, n, point.X);

                if (point.Y <= evaluationInRay)
                {
                    sweepAngle = 360 - sweepAngle;
                }
            }

            else if (quadrant_left == 2 || quadrant_right == 2)
            {
                startAngle = quadrant_left == 2 ? angleLeftCero : angleRightCero;
                float m = quadrant_left == 2 ? r1.M : r2.M;
                float n = quadrant_left == 2 ? r1.N : r2.N;
                Points point = quadrant_left != 2 ? point_left : point_right;
                float evaluationInRay = Utilities.PointInLine(m, n, point.X);

                if (point.Y > evaluationInRay)
                {
                    sweepAngle = 360 - sweepAngle;
                }
            }

            else if (quadrant_left == 3 || quadrant_right == 3)
            {
                startAngle = quadrant_left == 3 ? -angleLeftCero : -angleRightCero;
            }
        }

        return (startAngle, sweepAngle, point_left, point_right);
    }

    public override SequenceExpressionSyntax PointsInFigure()
    {
        Dictionary<int, object> elements = new();

        object PointsInArc()
        {
            var min = Center.X - Radius - 1;
            var max = Center.X + Radius + 1;

            do
            {
                float x = ParsingSupplies.CreateRandomsCoordinates((int)min, (int)max);

                (float y1, float y2) = IsInCircle(x);
                var point1 = new Points(x, y1);
                var point2 = new Points(x, y2);
                var intersect1 = point1.Intersect(this);
                var intersect2 = point2.Intersect(this);

                if ((int)(intersect1.Count) > 0)
                    return point1;

                else if ((int)(intersect2.Count) > 0)
                    return point2;
            }
            while (true);
        }

        var result = new InfiniteSequence(PointsInArc, elements)
        {
            valuesType = "point"
        };

        return result;
    }

    private (float, float) IsInCircle(float x)
    {
        float distance1 = (float)(Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x - Center.X, 2))) + Center.Y;
        float distance2 = (float)(-Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x - Center.X, 2))) + Center.Y;

        return (distance1, distance2);
    }

    public override FiniteSequence<object> Intersect(Figure figure)
    {
        return intersections[figure.Kind](this, figure);
    }

    public static FiniteSequence<object> IntersectArc(Figure figure1, Figure figure2)
    {
        var arc1 = (Arc)figure1;
        var arc2 = (Arc)figure2;

        if (arc1.Equals(arc2))
            return null!;

        List<object> list = new();
        Circle arc1Circle = new(arc1.Center, arc1.Measure);
        Circle arc2Circle = new(arc2.Center, arc2.Measure);

        var intersect = arc1Circle.Intersect(arc2Circle);

        if (intersect is null)
        {
            Segment s1 = new Segment(arc1.PointRay1, arc1.PointRay2);
            Segment s2 = new Segment(arc2.PointRay1, arc2.PointRay2);

            var intersect2 = s1.Intersect(s2);

            return (intersect2.Count > 0) ? null! : new(new());
        }

        if (intersect!.Count > 0)
        {
            for (int i = 0; i < (int)intersect.Count; i++)
            {
                var pointInArc1 = ((Points)intersect[i]).Intersect(arc1);
                var pointInArc2 = ((Points)intersect[i]).Intersect(arc2);

                if (pointInArc1.Count > 0 && pointInArc2.Count > 0)
                    list.Add(intersect[i]);
            }
        }

        return new FiniteSequence<object>(list);
    }
}

