using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using G_Sharp;
using Microsoft.VisualBasic.Devices;

namespace WallE;
public static class MethodsDrawing
{
    private static List<Draw> Sequences = new();

    private static Dictionary<SyntaxKind, Action<ExpressionSyntax, Graphics, Color, string>> drawings = new()
    {
        [SyntaxKind.PointToken] = DrawingPoint,
        [SyntaxKind.SegmentToken] = DrawingSegment,
        [SyntaxKind.RayToken] = DrawingRay,
        [SyntaxKind.LineToken] = DrawingLine,
        [SyntaxKind.CircleToken] = DrawingCircle,
        [SyntaxKind.ArcToken] = DrawingArc,
        [SyntaxKind.SequenceExpression] = DrawingSequence,
    };

    #region Pintar figuras
    // Método central donde se llama al método específico en dependencia del objeto
    public static List<Draw> DrawFigure(List<Draw> geometries, Graphics graphic)
    {
        Sequences = new();

        // Se iteran por los elementos de la lista de elementos a pintar y en dependencia del tipo se llama al método correspondiente
        for (int i = 0; i < geometries.Count; i++)
        {
            (ExpressionSyntax expression, Color color, string msg) = geometries[i].Geometries;

            try
            {
                if (drawings.TryGetValue(expression.Kind, out Action<ExpressionSyntax, Graphics, Color, string>? value))
                {
                    value(expression, graphic, color, msg);
                }
            }

            catch
            {
                Error.SetError("RUNTIME", "Expression is not drawable");
                return new();
            }
        }

        return Sequences;
    }
    #endregion

    #region Pintar puntos 
    public static void DrawingPoint(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var point = (Points)figure;
        Brush brush = new SolidBrush(color);
        Font font = new("Consolas", 10);

        graphics.DrawString(msg, font, brush, point.X, point.Y);
        graphics.FillEllipse(brush, point.X - 5, point.Y - 5, 10, 10);
    }
    #endregion

    #region Pintar segmentos
    public static void DrawingSegment(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var segment = (Segment)figure;
        Pen pen = new(color)
        {
            Width = 2
        };
        Brush brush = new SolidBrush(color);
        Font font = new("Consolas", 10);
        graphics.DrawString(msg, font, brush, segment.P1.X, segment.P1.Y);
        graphics.DrawLine(pen, segment.P1.X, segment.P1.Y, segment.P2.X, segment.P2.Y);
    }
    #endregion

    #region Pintar rayos
    public static void DrawingRay(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var ray = (Ray)figure;
        Segment s1 = new(ray.P1, ray.P2);
        Segment s2 = new(ray.P2, ray.End);

        DrawingSegment(s1, graphics, color, msg);
        DrawingSegment(s2, graphics, color, "");
    }
    #endregion

    #region Pintar lineas
    public static void DrawingLine(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var line = (Line)figure;
        Segment s1 = new(line.Start, line.P1);
        Segment s2 = new(line.P1, line.P2);
        Segment s3 = new(line.P2, line.End) ;

        DrawingSegment(s1, graphics, color, "");
        DrawingSegment(s2, graphics, color, msg);
        DrawingSegment(s3, graphics, color, "");
    }   
    #endregion

    #region Pintar circunsferncias
    public static void DrawingCircle(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var circle = (Circle)figure;
        Pen pen = new(color)
        {
            Width = 2
        };

        Points extrem = new(circle.Center.X - circle.Radius, circle.Center.Y - circle.Radius);
        graphics.DrawEllipse(pen, extrem.X, extrem.Y, circle.Diameter, circle.Diameter);
        DrawingPoint(circle.Center, graphics, color, msg);
    }
    #endregion

    #region Pintar arcos
    public static void DrawingArc(ExpressionSyntax figure, Graphics graphics, Color color, string msg)
    {
        var arc = (Arc)figure;
        Color componentsColor = Color.DodgerBlue;
        if (color == Color.DodgerBlue)
        {
            componentsColor = Color.Black;
        }

        Pen pen = new(color)
        {
            Width = 5
        };

        Circle circle = new(arc.Center, arc.Measure);
        Ray r1 = new(arc.Center, arc.PointRay1);
        Ray r2 = new(arc.Center, arc.PointRay2);

        Points point_left = Utilities.IntersectionCircle_Ray(arc.Center, r1.End, arc.Radius, r1.M, r1.N);
        Points point_right = Utilities.IntersectionCircle_Ray(arc.Center, r2.End, arc.Radius, r2.M, r2.N);

        Segment segment1 = new(arc.Center, point_left);
        Segment segment2 = new(arc.Center, point_right);

        Points extrem = new(arc.Center.X - arc.Radius, arc.Center.Y - arc.Radius);

        DrawingCircle(circle, graphics, componentsColor, msg);
        DrawingSegment(segment1, graphics, componentsColor, "");
        DrawingSegment(segment2, graphics, componentsColor, "");
        graphics.DrawArc(pen, extrem.X, extrem.Y, arc.Diameter, arc.Diameter, arc.StartAngle, arc.SweepAngle);
    }
    #endregion

    #region Pintar secunecias
    public static void DrawingSequence(ExpressionSyntax expression, Graphics graphics, Color color, string msg)
    {
        if (expression is FiniteSequence<object> finite)
            DrawingFiniteSequence(finite, graphics, color, msg);

        else if(expression is InfiniteSequence infinite)
            DrawingInfiniteSequence(infinite, graphics, color, msg);
    }

    public static void DrawingFiniteSequence(ExpressionSyntax expression, Graphics graphics, Color color, string msg)
    {
        var sequence = (SequenceExpressionSyntax)expression;

        if (sequence is FiniteSequence<object> finite)
        {
            // Se iteran por los elementos de la secuencia
            foreach (var item in finite.Elements)
            {
                var figure = (ExpressionSyntax)item;
                if (drawings.TryGetValue(figure.Kind, out Action<ExpressionSyntax, Graphics, Color, string>? value))
                {
                    value(figure, graphics, color, msg);
                }
            }
        }
    }

    public static void DrawingInfiniteSequence(ExpressionSyntax expression, Graphics graphics, Color color, string msg)
    {
        var sequence = (InfiniteSequence)expression;

        // Se pinta el primer elemento de la secuencia y se devuele el resto a partir del segundo
        var figure = (ExpressionSyntax)sequence[0];
        var drawToken = new SyntaxToken(SyntaxKind.DrawKeyword,0, 0, "draw", "");
        var rest = sequence.RestOfSequence(1);
        var draw = new Draw(drawToken, rest, msg);
        Sequences.Add(draw);

        if (drawings.TryGetValue(figure.Kind, out Action<ExpressionSyntax, Graphics, Color, string>? value))
        {
            value(figure, graphics, color, msg);
        }
    }
    #endregion
}