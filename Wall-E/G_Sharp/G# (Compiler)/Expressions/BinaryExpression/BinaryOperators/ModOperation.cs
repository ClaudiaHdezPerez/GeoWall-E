namespace G_Sharp;

#region Operación Módulo
public class ModOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
    public override string ReturnType => "number";

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    private readonly static List<string> compatibility = new()
    {
        "number", "undefined"
    };

    // Constructor
    public ModOperation(object left, object right, SyntaxToken operationToken)
    {
        Left = left;
        Right = right;
        OperationToken = operationToken;
    }

    // Revisión
    public override bool Check(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (!compatibility.Contains(leftType) || !compatibility.Contains(rightType))
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '%' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }
        
        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (leftType == "undefined" || rightType == "undefined")
            return null!;

        if ((double)Right == 0)
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Division by '0' is not defined");
            return 0;
        }

        try
        {
            return double.Parse(Left.ToString()!) % double.Parse(Right.ToString()!);
        }
        catch 
        { 
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '%' can't " + 
                                $"be used between '{leftType}' and '{rightType}'"); 
            return null!; }
        }
}

#endregion