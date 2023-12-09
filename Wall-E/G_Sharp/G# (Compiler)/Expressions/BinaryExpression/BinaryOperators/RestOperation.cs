namespace G_Sharp;

public class RestOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

    public override string ReturnType => SemanticChecker.GetType(Left);

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    public RestOperation(object left, object right, SyntaxToken operationToken)
    {
        Left = left;
        Right = right;
        OperationToken = operationToken;
    }

    private readonly static List<string> compatibility = new()
    {
        "number", "measure", "undefined"
    };

    public override bool Check(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        bool leftIsCompatible =  compatibility.Contains(leftType);
        bool rightIsCompatible = compatibility.Contains(rightType);
        bool sameType = leftType == rightType;

        if (!leftIsCompatible || !rightIsCompatible)
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '-' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        if (!sameType && leftType != "undefined" && rightType != "undefined")
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '-' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        return true;
    }

    public override object Evaluate(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (leftType == "undefined" || rightType == "undefined")
            return null!;

        if (leftType == "measure") 
        {
            var leftValue = ((Measure)Left).Value;
            var rightValue = ((Measure)Right).Value;
            return new Measure((float) Math.Abs(leftValue - rightValue));
        }

        try
        {
            return double.Parse(Left.ToString()!) - double.Parse(Right.ToString()!);
        }

        catch
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '-' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return null!;
        }
    }
}