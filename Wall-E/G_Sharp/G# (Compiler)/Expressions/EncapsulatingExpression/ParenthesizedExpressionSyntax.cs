namespace G_Sharp;

public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    public SyntaxToken OpenParenthesisToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken ClosedParenthesisToken { get; }
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
    public override string ReturnType => Expression.ReturnType;

    public ParenthesizedExpressionSyntax(
        SyntaxToken openParenthesisToken, ExpressionSyntax expression, 
        SyntaxToken closedParenthesisToken
    )
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        ClosedParenthesisToken = closedParenthesisToken;
    }

    public override object Evaluate(Scope scope)
    {
        return Expression.Evaluate(scope);
    }

    public override bool Check(Scope scope)
    {
        return Expression.Check(scope);
    }
}