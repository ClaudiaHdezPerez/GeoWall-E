namespace G_Sharp;

public class OrOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public override string ReturnType => "number";

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    public OrOperation(object left, object right, SyntaxToken operationToken)
    {
        Left = left;
        Right = right;
        OperationToken = operationToken;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public override object Evaluate(Scope scope)
    {
        var leftType = SemanticChecker.GetType(Left);
        var rightType = SemanticChecker.GetType(Right);

        object left = Left;
        object right = Right;

        if (leftType == "sequence")
        {
            var seq = (SequenceExpressionSyntax)Left;
            left = seq.Count <= 0 ? null! : seq.Count;
        }

        if (rightType == "sequence")
        {
            var seq = (SequenceExpressionSyntax)Right;
            right = seq.Count <= 0 ? null! : seq.Count;
        }

        bool leftIsFalse = Evaluator.DefaultFalseValues.Contains(left);
        bool rightIsFalse = Evaluator.DefaultFalseValues.Contains(right);

        return (leftIsFalse && rightIsFalse) ? 0 : 1;
    }
}