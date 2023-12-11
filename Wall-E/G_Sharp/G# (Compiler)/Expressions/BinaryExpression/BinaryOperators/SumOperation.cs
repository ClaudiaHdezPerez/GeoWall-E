namespace G_Sharp;

#region Operación suma
public class SumOperation : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

    public override string ReturnType => SemanticChecker.GetType(Left);

    private readonly static List<string> compatibility = new()
    {
        "number", "sequence", "measure", "undefined"
    };

    public object Left { get; }
    public object Right { get; }
    public SyntaxToken OperationToken { get; }

    // Constructor
    public SumOperation(object left, object right, SyntaxToken operationToken)
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

        bool leftIsCompatible =  compatibility.Contains(leftType);
        bool rightIsCompatible = compatibility.Contains(rightType);
        bool sameType = leftType == rightType;

        if (!leftIsCompatible || !rightIsCompatible)
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '+' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        if (!sameType && leftType != "undefined" && rightType != "undefined")
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '+' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return false;
        }

        if (leftType == "sequence")
        {
            if (rightType == "undefined")
                return true;

            return SumSequenceCheck(rightType);
        }
        
        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        string leftType = SemanticChecker.GetType(Left);
        string rightType = SemanticChecker.GetType(Right);

        if (leftType == "measure" && rightType == "measure")
        {
            var leftValue = ((Measure)Left).Value;
            var rightValue = ((Measure)Right).Value;
            return new Measure((float)(leftValue + rightValue));
        }

        else if (leftType == "sequence" && (rightType == "sequence" || Right is null))
        {
            return SumSequenceEvaluate(rightType);
        }

        else if (leftType == "undefined" || rightType == "undefined")
            return null!;

        try
        {
            return double.Parse(Left.ToString()!) + double.Parse(Right.ToString()!);
        }

        catch 
        {
            Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '+' can't " +
                            $"be used between '{leftType}' and '{rightType}'");
            return null!;
        }
    }

    #region Suma de secuencias
    private object SumSequenceEvaluate(string type)
    {
        var seq1 = (SequenceExpressionSyntax)Left;
        var seq2 = (SequenceExpressionSyntax)Right;
        List<object> parameters = new();

        if (seq1.Count < 0)
            return Left;

        if (type != "undefined" && seq2.Count < 0)
        {
            Dictionary<int, object> values = new();

            for (int i = 0; i < seq1.Count; i++)
            {
                values[i] = seq1[i];
            }

            if (Right is InfiniteSequence infinite)
                return InfiniteSequence.CreateInifniteSequence(infinite, values);

            return InfiniteSequence.CreateInifniteSequence((InfiniteIntegerSequence)Right, values);
        }

        for (int i = 0; i < seq1.Count; i++)
        {
            parameters.Add(seq1[i]);
        }

        if (type == "undefined")
            parameters.Add(null!);

        else
        {
            for (int i = 0; i < seq2.Count; i++)
            {
                parameters.Add(seq2[i]);
            }
        }

        return new FiniteSequence<object>(parameters);
    }

    private bool SumSequenceCheck(string type)
    {
        if (Left is SequenceExpressionSyntax seq1 && Right is SequenceExpressionSyntax seq2)
        {
            if (seq1.Count == 0 || seq2.Count == 0)
                return true;

            if (type != "undefined")
            {
                var seq1Type = SequenceExpressionSyntax.GetInternalTypeOfSequence(seq1);
                var seq2Type = SequenceExpressionSyntax.GetInternalTypeOfSequence(seq2);


                if (seq1Type != seq2Type)
                {
                    Error.SetError("SEMANTIC", $"Line '{OperationToken.Line}' : Operator '+' can't " +
                                $"be used between 'sequence of {seq1Type}' and " +
                                $"'sequence of {seq2Type}'");
                    return false;
                }
            }
        }

        return true;
    }

    #endregion
}

#endregion