namespace G_Sharp;

public sealed class UnaryExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Operand { get; }
    public override string ReturnType => "number";

    private static readonly Dictionary<SyntaxKind, Func<object, SyntaxToken, ExpressionSyntax>> unaryOperationEvaluation = new()
    {
        [SyntaxKind.PlusToken] = (operand, operation) => new PlusOperation(operand, operation),
        [SyntaxKind.MinusToken] = (operand, operation) => new MinusOperation(operand, operation),
        [SyntaxKind.NotKeyword] = (operand, operation) => new NotOperation(operand, operation)
    };

    public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
    {
        OperatorToken = operatorToken;
        Operand = operand;
    }

    public override object Evaluate(Scope scope)
    {
        var operand = Operand.Evaluate(scope);

        var operation = unaryOperationEvaluation[OperatorToken.Kind](operand, OperatorToken);
        return operation.Evaluate(scope);
    }

    public override bool Check(Scope scope)
    {
        bool operandIsFine = Operand.Check(scope);
        if (operandIsFine)
        {
            var operation = unaryOperationEvaluation[OperatorToken.Kind](Operand, OperatorToken);
            return operation.Check(scope);
        }

        return false;
    }
}