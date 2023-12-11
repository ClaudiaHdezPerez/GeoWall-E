using System.Drawing;

namespace G_Sharp;

#region Expresiones binarias
public class BinaryExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public ExpressionSyntax Left { get; }
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Right { get; }
    public override string ReturnType {
        get {
            var operation = binaryOperation[OperatorToken.Kind](Left, Right, OperatorToken);
            return operation.ReturnType;
        }
    }

    // Tipos de operaciones binarias

    private static readonly Dictionary<SyntaxKind, Func<object, object, SyntaxToken, ExpressionSyntax>> binaryOperation = new()
    {
        [SyntaxKind.PlusToken]           = (left, right, operation) => new SumOperation(left, right, operation),
        [SyntaxKind.MinusToken]          = (left, right, operation) => new RestOperation(left, right, operation),
        [SyntaxKind.MultToken]           = (left, right, operation) => new MultOperation(left, right, operation),
        [SyntaxKind.DivisionToken]       = (left, right, operation) => new DivisionOperation(left, right, operation),
        [SyntaxKind.ModToken]            = (left, right, operation) => new ModOperation(left, right, operation),
        [SyntaxKind.AndKeyword]          = (left, right, operation) => new AndOperation(left, right, operation),
        [SyntaxKind.OrKeyword]           = (left, right, operation) => new OrOperation(left, right, operation),
        [SyntaxKind.EqualToken]          = (left, right, operation) => new EqualOperation(left, right, operation),
        [SyntaxKind.DifferentToken]      = (left, right, operation) => new DifferentOperation(left, right, operation),
        [SyntaxKind.GreaterToken]        = (left, right, operation) => new GreaterOperation(left, right, operation),
        [SyntaxKind.LessToken]           = (left, right, operation) => new LessOperation(left, right, operation),
        [SyntaxKind.GreaterOrEqualToken] = (left, right, operation) => new GreaterOrEqualOperation(left, right, operation),
        [SyntaxKind.LessOrEqualToken]    = (left, right, operation) => new LessOrEqualOperation(left, right, operation)
    };

    // Constructor
    public BinaryExpressionSyntax(
        ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right
    )
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        var left = Left.Evaluate(scope);
        var right = Right.Evaluate(scope);

        var operatorKind = OperatorToken.Kind;
        var operation = binaryOperation[operatorKind](left, right, OperatorToken);
        return operation.Evaluate(scope);
    }

    // Revisión
    public override bool Check(Scope scope)
    {
        bool leftIsFine = Left.Check(scope);
        bool rightIsFine = Right.Check(scope);

        
        if (leftIsFine && rightIsFine)
        {
            var operation = binaryOperation[OperatorToken.Kind](Left, Right, OperatorToken);
            return operation.Check(scope);
        }

        return false;
    }
}

#endregion