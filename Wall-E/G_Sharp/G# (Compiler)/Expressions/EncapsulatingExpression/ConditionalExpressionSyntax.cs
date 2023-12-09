namespace G_Sharp;

public sealed class ConditionalExpressionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.ConditionalExpression;

    public SyntaxToken IfKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxToken ThenKeyword { get; }
    public ExpressionSyntax BodyTrue { get; }
    public SyntaxToken ElseKeyword { get; }
    public ExpressionSyntax BodyFalse { get; }
    public override string ReturnType => BodyTrue.ReturnType;

    public ConditionalExpressionSyntax(
        SyntaxToken ifKeyword, ExpressionSyntax condition, SyntaxToken thenKeyword, 
        ExpressionSyntax bodyTrue, SyntaxToken elseKeyword, ExpressionSyntax bodyFalse
    )
    {
        IfKeyword = ifKeyword;
        Condition = condition;
        ThenKeyword = thenKeyword;
        BodyTrue = bodyTrue;
        ElseKeyword = elseKeyword;
        BodyFalse = bodyFalse;

    }

    public override object Evaluate(Scope scope)
    {
        var condition = Condition.Evaluate(scope);
        string conditionType = SemanticChecker.GetType(condition);
        
        if (conditionType == "sequence")
        {   
            var seq = (SequenceExpressionSyntax)condition;
            condition = seq.Count <= 0 ? null : seq.Count;
        }

        if (Evaluator.DefaultFalseValues.Contains(condition!))
            return BodyFalse.Evaluate(scope);
        
        return BodyTrue.Evaluate(scope);
    }

    public override bool Check(Scope scope)
    {
        var conditionIsFine = Condition.Check(scope);
        var bodyTrueIsFine = BodyTrue.Check(scope);
        var bodyFalseIsFine = BodyFalse.Check(scope);

        if (!conditionIsFine || !bodyTrueIsFine || !bodyFalseIsFine)
        {
            return false;
        }

        var bodyFalseType = BodyFalse.ReturnType;
        var bodyTrueType = BodyTrue.ReturnType;
        var sameType = bodyTrueType.Equals(bodyFalseType);

        if (!sameType && bodyFalseType != "undefined" && bodyTrueType != "undefined")
        {
            Error.SetError("SEMANTIC", "Conditional must return the same type of value in " +
                            "the 'then' clause and the 'else' clause");
            return false;
        }

        return true;
    }
}