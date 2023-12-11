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
    public override SyntaxKind Kind => SyntaxKind.DrawExpression;
    public override string ReturnType => "void expression";

    #region Constructores de la clase Draw
    public SyntaxToken DrawToken { get; }
    public ExpressionSyntax Parameters { get; }
    public (ExpressionSyntax, Color, string) Geometries { get; }
    public Color Color => Colors.ColorDraw!.Peek();
    public string Msg { get; }
    public Draw(SyntaxToken drawToken, ExpressionSyntax parameters, string msg)
    {
        DrawToken = drawToken;
        Parameters = parameters;
        Msg = msg;
        Geometries = new();
    }

    public Draw((ExpressionSyntax, Color, string) geometries, ExpressionSyntax parameters, Color color)
    { 
        Geometries = geometries;
        Parameters = parameters;
    }
    #endregion

    #region Evaluación
    public override object Evaluate(Scope scope)
    {
        (ExpressionSyntax, Color, string) geometries = (null!, Color.White, null!);

        // Evalua de forma independiente los parametros a dibujar
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
    #endregion

    #region Revisión 
    public override bool Check(Scope scope)
    {
        // Se revisa que los parámetros de forma independiente sean correctos
        if (!Parameters.Check(scope))
            return false;

        // En caso de ser secuencia se verifica de que tipo es
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
        // En caso de que sea secuencia o no se verifica que sean objetos "dibujables"
        else if (!drawableExpressions.Contains(SemanticChecker.GetType(Parameters)))
        {
            Error.SetError("SEMANTIC", $"Line '{DrawToken.Line}' : {Parameters.ReturnType} is not a drawable object");
            return false;
        }

        return true;
    }
    #endregion
}
