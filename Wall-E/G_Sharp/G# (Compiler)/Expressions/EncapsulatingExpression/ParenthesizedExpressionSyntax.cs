namespace G_Sharp;

#region Expresiones entre paréntesis
public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;
    public override string ReturnType => Expression.ReturnType;

    #region Constructor
    public SyntaxToken OpenParenthesisToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken ClosedParenthesisToken { get; }
    
    public ParenthesizedExpressionSyntax(
        SyntaxToken openParenthesisToken, ExpressionSyntax expression, 
        SyntaxToken closedParenthesisToken
    )
    {
        OpenParenthesisToken = openParenthesisToken;
        Expression = expression;
        ClosedParenthesisToken = closedParenthesisToken;
    }

    #endregion

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        return Expression.Evaluate(scope);
    }

    // Revisión
    public override bool Check(Scope scope)
    {
        return Expression.Check(scope);
    }
}

#endregion