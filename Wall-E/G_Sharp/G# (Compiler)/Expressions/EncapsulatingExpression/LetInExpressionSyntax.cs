namespace G_Sharp;

#region Expresiones Let-in
public sealed class LetInExpressionSyntax : ExpressionSyntax
{
    public override string ReturnType => Instructions.Last().ReturnType;
    public override SyntaxKind Kind => SyntaxKind.LetInExpression;

    #region Constructor
    public SyntaxToken LetToken { get; }
    public List<ExpressionSyntax> Instructions { get; }
    public SyntaxToken InToken { get; }
    

    public LetInExpressionSyntax(
        SyntaxToken letToken, List<ExpressionSyntax> instructions, 
        SyntaxToken inToken, ExpressionSyntax body
    )
    {
        LetToken = letToken;
        Instructions = instructions;
        Instructions.Add(body);
        InToken = inToken;
    }

    #endregion


    // Evaluación
    public override object Evaluate(Scope scope)
    {
        Scope child = GetInternalScope(scope);
        object result = null!;

        foreach (var statement in Instructions)
            result = child.Evaluate(statement);

        scope.DrawingObjects.AddRange(child.DrawingObjects);

        return result;
    }

    // Revisión
    public override bool Check(Scope scope)
    {
        Scope child = GetInternalScope(scope);

        foreach (var statement in Instructions)
        {
            var check = child.Check(statement);
            if (!check) return false;
        }

        return true;
    }

    // Crear scope hijo
    private Scope GetInternalScope(Scope scope)
    {
        Dictionary<string, Constant> internalConstants = new();
        Dictionary<string, Function> internalFunctions = new();

        foreach (string key in scope.Constants.Keys)
            internalConstants[key] = scope.Constants[key];

        foreach (string key in scope.Functions.Keys)
            internalFunctions[key] = scope.Functions[key];

        return new(internalConstants, internalFunctions);
    }
}

#endregion