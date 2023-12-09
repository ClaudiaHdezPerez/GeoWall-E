namespace G_Sharp;

public class GreaterOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public override string ReturnType => "number";

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    private readonly static List<string> compatibility = new()
    {
        "number", "measure", "undefined"
    };

    public GreaterOperation(object left, object right, SyntaxToken operationToken)
    {
        Left = left;
        Right = right;
        OperationToken = operationToken;
    }

    public override bool Check(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        bool leftIsCompatible =  leftType == "number" || leftType == "measure";
        bool rightIsCompatible = rightType == "number" || rightType == "measure";
        bool sameType = leftType == rightType;

        if (!leftIsCompatible || !rightIsCompatible)
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '>' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        if (!sameType && leftType != "undefined" && rightType != "undefined")
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '>' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        return true;
    }

    public override object Evaluate(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (leftType == "measure" && rightType == "measure")
        {
            var measure1 = (Measure) Left;
            var measure2 = (Measure) Right;
            return (measure1.Value > measure2.Value) ? 1 : 0;
        }

        try
        {
            return double.Parse(Left.ToString()!) > double.Parse(Right.ToString()!) ? 1 : 0;
        }

        catch
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '>' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return null!;
        }
    }
}