using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

#region Expresiones void
public class VoidExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.VoidExpression;

    public override string ReturnType => "void expression";

    // Revisión
    public override bool Check(Scope scope)
    {
        return true; ;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        return "";
    }
}

#endregion
