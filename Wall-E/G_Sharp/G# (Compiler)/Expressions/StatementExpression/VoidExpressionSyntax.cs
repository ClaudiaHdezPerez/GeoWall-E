using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public class VoidExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.VoidExpression;

    public override string ReturnType => "void expression";

    public override bool Check(Scope scope)
    {
        return true; ;
    }

    public override object Evaluate(Scope scope)
    {
        return "";
    }
}
