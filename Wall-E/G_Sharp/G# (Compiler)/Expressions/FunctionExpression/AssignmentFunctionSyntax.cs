using System.Xml.Linq;

namespace G_Sharp;

#region Asignación de funciones
public sealed class AssignmentFunctionSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.AssignmentFunctionExpression;
    public override string ReturnType => "void expression";

    #region  Constructor
    public SyntaxToken FunctionIdentifierToken { get; }
    public List<ExpressionSyntax> IdentifiersToken { get; }
    public SyntaxToken AssignmentToken { get; }
    public ExpressionSyntax Expression { get; }
    

    public AssignmentFunctionSyntax(
        SyntaxToken functionIdentifierToken, List<ExpressionSyntax> identifiersToken,
        SyntaxToken assignmentToken, ExpressionSyntax expression
    )
    {
        FunctionIdentifierToken = functionIdentifierToken;
        IdentifiersToken = identifiersToken;
        AssignmentToken = assignmentToken;
        Expression = expression;
    }

    #endregion

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        return "";
    }

    // Revisión
    public override bool Check(Scope scope)
    {
        string name = FunctionIdentifierToken.Text;
        var body = Expression;
        var parameters = IdentifiersToken;

        if (!scope.Functions.ContainsKey(name))
            scope.Functions[name] = new Function(body, parameters);

        else
        {
            Error.SetError("SYNTAX", $"Line '{FunctionIdentifierToken.Line}' : Function '{name}' " +
                            $"is already defined");
            return false;
        }

        return true;
    }
}

#endregion