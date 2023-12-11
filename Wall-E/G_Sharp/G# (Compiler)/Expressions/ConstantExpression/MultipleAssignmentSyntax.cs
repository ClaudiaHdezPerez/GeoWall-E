using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace G_Sharp;

#region Asignación múltiple
public sealed class MultipleAssignmentSyntax : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.ConstantAssignmentExpression;

    public override string ReturnType => "void expression";

    #region Constructor
    public List<SyntaxToken> Identifiers { get; }
    public SyntaxToken AssignmentToken { get; }
    public ExpressionSyntax Expression { get; }

    public MultipleAssignmentSyntax(
        List<SyntaxToken> identifiers, SyntaxToken assignmentToken, 
        ExpressionSyntax expression
    )
    {
        Identifiers = identifiers;
        AssignmentToken = assignmentToken;
        Expression = expression;
    }

    #endregion

    // Revisión
    public override bool Check(Scope scope)
    {
        foreach (var item in Identifiers)
        {
            string name = item.Text;

            if (!Expression.Check(scope))
                return false;

            if (Expression.ReturnType != "sequence" && Expression.ReturnType != "undefined")
            {
                Error.SetError("SEMANTIC", $"Line '{AssignmentToken.Line}' : " +
                                $"Multiple constants can only be assignment to elements in a sequence");
                return false;
            }

            if (scope.Constants.ContainsKey(name) && name != "_")
            {
                Error.SetError("SYNTAX", $"Line '{item.Line}' : Constant '{name}' is already defined");
                return false;
            }

            if (name != "_")
                scope.Constants[name] = new Constant(Expression);
        }

        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        SequenceExpressionSyntax sequence = (SequenceExpressionSyntax) Expression.Evaluate(scope);

        if (sequence is null)
            return NullEvaluate(scope);

        long count = Identifiers.Count - 1;

        if (sequence.Count >= 0)
            count = long.Parse(sequence.Count.ToString()!);

        for (int i = 0; i < Identifiers.Count - 1; i++)
        {
            string name = Identifiers[i].Text;

            if (i < count && name != "_")
                scope.Constants[name] = new Constant(sequence[i]);

            else if (name != "_")
            {
                scope.Constants[name] = new Constant(null!);
            }
        }

        var lastId = Identifiers.Last().Text;
        if (lastId != "_")
        {
            SequenceExpressionSyntax rest = null!;
            int startIndex = (int)Math.Min(count, Identifiers.Count - 1);

            if (sequence.Count == 0)
                rest = new FiniteSequence<object>(new());

            else if (sequence[startIndex - 1] is not null)
                rest = sequence.RestOfSequence(startIndex);

            scope.Constants[lastId] = new Constant(rest);
        }

        return "";
    }

    private object NullEvaluate(Scope scope)
    {
        for (int i = 0; i < Identifiers.Count; i++)
        {
            string name = Identifiers[i].Text;
            scope.Constants[name] = new Constant(null!);
        }

        return "";
    }
}

#endregion
