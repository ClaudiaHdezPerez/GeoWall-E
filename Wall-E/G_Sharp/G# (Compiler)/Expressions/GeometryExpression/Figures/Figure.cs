using System.Drawing;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
namespace G_Sharp;

// Interfaz para las figuras
public abstract class Figure : ExpressionSyntax
{
    public abstract SequenceExpressionSyntax PointsInFigure();
    public abstract FiniteSequence<object> Intersect(Figure figure);
}