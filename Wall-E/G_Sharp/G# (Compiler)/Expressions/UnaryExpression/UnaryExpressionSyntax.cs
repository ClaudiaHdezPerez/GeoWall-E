namespace G_Sharp;

#region Expresiones Unarias
public sealed class UnaryExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
    
    public override string ReturnType => "number";

    // Operaciones unarias

    private static readonly Dictionary<SyntaxKind, Func<object, SyntaxToken, ExpressionSyntax>> unaryOperationEvaluation = new()
    {
        [SyntaxKind.PlusToken] = (operand, operation) => new PlusOperation(operand, operation),
        [SyntaxKind.MinusToken] = (operand, operation) => new MinusOperation(operand, operation),
        [SyntaxKind.NotKeyword] = (operand, operation) => new NotOperation(operand, operation)
    };

    #region Constructor
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Operand { get; }

    public UnaryExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax operand)
    {
        OperatorToken = operatorToken;
        Operand = operand;
    }

    #endregion

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        var operand = Operand.Evaluate(scope);

        var operation = unaryOperationEvaluation[OperatorToken.Kind](operand, OperatorToken);
        return operation.Evaluate(scope);
    }

    // Revisión
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

#endregion