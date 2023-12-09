using System.Diagnostics.Tracing;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace G_Sharp;

public static class Evaluator
{
    public static readonly List<object> DefaultFalseValues = new() {
        null!, 0, 0.0
    };

    public static object Evaluate(this Scope scope, ExpressionSyntax node)
    {
        return  node.Evaluate(scope);
    }
}