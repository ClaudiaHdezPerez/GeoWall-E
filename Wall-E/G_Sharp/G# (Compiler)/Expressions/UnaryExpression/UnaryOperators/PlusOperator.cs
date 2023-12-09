namespace G_Sharp;

public class PlusOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

    public override string ReturnType => SemanticChecker.GetType(Operand);

    public object Operand { get; }
    public SyntaxToken OperationToken { get; }

    public PlusOperation(object operand, SyntaxToken operationToken)
    {
        Operand = operand;
        OperationToken = operationToken;
    }

    public override bool Check(Scope scope)
    {
        string operandType = SemanticChecker.GetType(Operand);

        if (operandType != "number" && operandType != "undefined")
        {
            Error.SetError("SEMANTIC", $"Line ' {OperationToken.Line} ' : Operator '+' " +
                            $"can't not be used before '{operandType}'");
            return false;
        }
        
        return true;
    }

    public override object Evaluate(Scope scope)
    {
        if (Operand is null) return null!;
        return + double.Parse(Operand.ToString()!); 
    }
}