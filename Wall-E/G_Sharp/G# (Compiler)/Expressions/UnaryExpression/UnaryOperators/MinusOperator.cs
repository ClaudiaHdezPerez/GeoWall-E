namespace G_Sharp;

#region Operación unaria de resta
public class MinusOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.UnaryExpression;

    public override string ReturnType => SemanticChecker.GetType(Operand);

    #region Constructor
    public object Operand { get; }
    public SyntaxToken OperationToken { get; }

    public MinusOperation(object operand, SyntaxToken operationToken)
    {
        Operand = operand;
        OperationToken = operationToken;
    }

    #endregion

    // Revisión
    public override bool Check(Scope scope)
    {
        string operandType = SemanticChecker.GetType(Operand);

        if (operandType != "number" && operandType != "undefined")
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '-' " +
                            $"can't not be used before '{operandType}'");
            return false;
        }
        
        return true;
    }


    // Evaluación
    public override object Evaluate(Scope scope)
    {
        if (Operand is null) return null!;
        return - double.Parse(Operand.ToString()!);
    }
}

#endregion