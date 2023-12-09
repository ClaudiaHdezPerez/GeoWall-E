using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Circle : Figure, IEquatable<Circle>
{
    private static Dictionary<SyntaxKind, Func<Figure, Figure, FiniteSequence<object>>> intersections = new()
    {
        [SyntaxKind.CircleToken] = IntersectCircle,
        [SyntaxKind.ArcToken] = IntersectArc,
    };

    public override SyntaxKind Kind => SyntaxKind.CircleToken;
    public Points Center { get; }
    public float Diameter => Radius * 2;
    public float Radius => Measure.Value;
    public Measure Measure { get; }
    public override string ReturnType => "circle";

    public Circle(Points center, Measure measure)
    {
        Center = center;
        Measure = measure;
    }

    public override object Evaluate(Scope scope)
    {
        return this;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Circle? other)
    {
        return Center.Equals(other!.Center) && Measure.Equals(other!.Measure);
    }

    public override bool Equals(object? obj) => Equals(obj as Circle);
    public override int GetHashCode() => Center.GetHashCode();

    public override SequenceExpressionSyntax PointsInFigure()
    {
        Dictionary<int, object> elements = new();
        object PointsInCircle()
        {
            float x;
            float[] y;
            Random random = new();
            var position = random.Next(2);
            x = ParsingSupplies.CreateRandomsCoordinates((int)(Center.X - Radius - 1), (int)(Center.X + Radius + 1));
            y = IsInCircle(x);

            return new Points(x, y[position]);
        }

        var result = new InfiniteSequence(PointsInCircle, elements)
        {
            valuesType = "point"
        };

        return result;
    }

    private float[] IsInCircle(float x)
    {
        float distance1 = (float)(Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x - Center.X, 2))) + Center.Y;
        float distance2 = (float)(-Math.Sqrt(Math.Pow(Radius, 2) - Math.Pow(x - Center.X, 2))) + Center.Y;
        float[] result = { distance1, distance2 };

        return result;
    }

    public override FiniteSequence<object> Intersect(Figure figure)
    {
        return intersections[figure.Kind](this, figure);
    }

    private static FiniteSequence<object> IntersectCircle(Figure figure1, Figure figure2)
    {
        var c1 = (Circle)figure1;
        var c2 = (Circle)figure2;

        var a1 = c1.Center.X;
        var b1 = c1.Center.Y;
        var r1 = c1.Radius;
        var a2 = c2.Center.X;
        var b2 = c2.Center.Y;
        var r2 = c2.Radius;

        if (c1.Equals(c2) && r1.Equals(r2))
            return null!;

        var distanceBetweenCenters = Utilities.DistanceBetweenPoints(c1.Center, c2.Center);
        var sumRadious = c1.Radius + c2.Radius;

        if (distanceBetweenCenters > sumRadious || distanceBetweenCenters < Math.Abs(r1 - r2))
            return new FiniteSequence<object>(new List<object>());

        var a = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(distanceBetweenCenters, 2)) / (2 * distanceBetweenCenters);
        var h = Math.Sqrt(Math.Pow(r1, 2) - Math.Pow(a, 2));

        var x3 = a1 + a * (a2 - a1) / distanceBetweenCenters;
        var y3 = b1 + a * (b2 - b1) / distanceBetweenCenters;

        var x1 = (float)(x3 + h * (b2 - b1) / distanceBetweenCenters);
        var y1 = (float)(y3 - h * (a2 - a1) / distanceBetweenCenters);

        var x2 = (float)(x3 - h * (b2 - b1) / distanceBetweenCenters);
        var y2 = (float)(y3 + h * (a2 - a1) / distanceBetweenCenters);

        Points intersect1 = new(x1, y1);
        Points intersect2 = new(x2, y2);

        return new FiniteSequence<object>(new List<object>() { intersect1, intersect2 });
    }

    private static FiniteSequence<object> IntersectArc(Figure figure1, Figure figure2)
    {
        var circle = (Circle)figure1;
        var arc = (Arc)figure2;

        if (arc.Center.Equals(circle.Center) && arc.Measure.Equals(circle.Measure))
            return null!;

        List<object> list = new();
        Circle arcCircle = new(arc.Center, arc.Measure);

        var intersect = IntersectCircle(circle, arcCircle);

        if (intersect.Count > 0)
        {
            for (int i = 0; i < intersect.Count; i++)
            {
                var pointInArc = ((Points)intersect[i]).Intersect(arc);
                if (pointInArc.Count > 0)
                    list.Add(intersect[i]);
            }
        }

        return new FiniteSequence<object>(list);
    }
}
