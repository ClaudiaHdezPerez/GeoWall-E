namespace G_Sharp;

public class EqualOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public override string ReturnType => "number";

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    public EqualOperation(object left, object right, SyntaxToken operationToken)
    {
        Left = left;
        Right = right;
        OperationToken = operationToken;
    }

    public override bool Check(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (leftType != rightType && 
            leftType != "undefined" && rightType != "undefined"
            )
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '==' can't be " +
                            $"used between '{leftType}' and '{rightType}'");
            return false;
        }
        
        return true;
    }

    public override object Evaluate(Scope scope)
    {
        var leftType = SemanticChecker.GetType(Left);
        var rightType = SemanticChecker.GetType(Right);

        if (leftType == "measure" && rightType == "measure")
        {
            var measure1 = (Measure) Left;
            var measure2 = (Measure) Right;
            return (measure1.Value == measure2.Value) ? 1 : 0;
        }

        if (Left is null && Right is null)
            return 1;

        if (Left is null || Right is null)
            return 0;

        return Left.Equals(Right) ? 1 : 0;
    }
}