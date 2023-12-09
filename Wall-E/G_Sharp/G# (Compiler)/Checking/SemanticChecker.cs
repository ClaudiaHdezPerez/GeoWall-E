using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;
public static class SemanticChecker
{
    public static bool canImport = true;

    public static bool Check(this Scope scope, ExpressionSyntax node)
    {
        return scope.CheckExpression(node);
    }

    private static bool CheckExpression(this Scope scope, ExpressionSyntax node)
    {
        if (node is not ImportExpressionSyntax)
            canImport = false;

        return node.Check(scope);
    }

    public static string GetType(object obj)
    {
        return obj switch
        {
            double or int or decimal or float or long => "number",
            string => "string",
            null => "undefined",
            ExpressionSyntax expression => expression.ReturnType,
            _ => "undefined expression"
        };
    }
}

