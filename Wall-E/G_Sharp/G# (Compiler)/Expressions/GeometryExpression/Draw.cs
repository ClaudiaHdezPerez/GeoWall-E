using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace G_Sharp;

public class Draw : ExpressionSyntax
{
    private static List<string> drawableExpressions = new()
    {
        "point", "line", "segment", "ray", "circle", "arc", "sequence", "undefined"
    };
    public (ExpressionSyntax, Color, string) Geometries { get; }
    public override SyntaxKind Kind => SyntaxKind.DrawExpression;

    public SyntaxToken DrawToken { get; }
    public ExpressionSyntax Parameters { get; }
    public Color Color { get; }
    public string Msg { get; }

    public override string ReturnType => "void expression";

    public Draw(SyntaxToken drawToken, ExpressionSyntax parameters, Color color, string msg)
    {
        DrawToken = drawToken;
        Parameters = parameters;
        Color = color;
        Msg = msg;
        Geometries = new();
    }

    public Draw((ExpressionSyntax, Color, string) geometries, ExpressionSyntax parameters, Color color)
    { 
        Geometries = geometries;
        Parameters = parameters;
        Color = color;
    }

    public override object Evaluate(Scope scope)
    {
        (ExpressionSyntax, Color, string) geometries = (null!, Color.White, null!);

        var value = Parameters.Evaluate(scope);

        if (value is SequenceExpressionSyntax sequence)
            geometries = (sequence, Color, Msg);

        else if (value is ExpressionSyntax val)
        {
            if ((int)val.Kind <= 28 && (int)val.Kind >= 22)
                geometries = (val, Color, Msg);
        }

        scope.DrawingObjects.Add(new(geometries, Parameters, Color));

        return "";
    }

    public override bool Check(Scope scope)
    {
        if (!Parameters.Check(scope))
            return false;

        if (Parameters is SequenceExpressionSyntax sequence)
        {
            string type = SequenceExpressionSyntax.GetInternalTypeOfSequence(sequence);
            if (type.Contains(' '))
                type = type[type.LastIndexOf(" ")..];

            if (!drawableExpressions.Contains(type))
            {
                Error.SetError("SEMANTIC", $"Line '{DrawToken.Line}' : {Parameters} is not a drawable object");
                return false;
            }
        }

        else if (!drawableExpressions.Contains(SemanticChecker.GetType(Parameters)))
        {
            Error.SetError("SEMANTIC", $"Line '{DrawToken.Line}' : {Parameters.ReturnType} is not a drawable object");
            return false;
        }

        return true;
    }
}
