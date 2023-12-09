using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class FiniteSequence<T> : SequenceExpressionSyntax
{
    public List<T> Elements { get; }

    public override long Count => Elements.Count;
    public override string ValuesType
    {
        get
        {
            if (Elements.Count == 0) return "undefined";
            return SemanticChecker.GetType(Elements[0]!);
        }
    }

    public override object this[int index] => Elements[index]!;

    public FiniteSequence(List<T> elements)
    {
        Elements = elements;
    }


    public override object Evaluate(Scope scope)
    {
        List<object> values = new();

        foreach (var item in Elements as List<ExpressionSyntax>)
        {
            values.Add(item.Evaluate(scope));
        }

        return new FiniteSequence<object>(values);
    }

    public override bool Check(Scope scope)
    {
        var elements = Elements as List<ExpressionSyntax>;
        if (Count.Equals(0)) return true;

        if (!elements![0].Check(scope)) return false;
        string type = "";

        if (SemanticChecker.GetType(elements[0]) == "sequence")
            type = "sequence of " + GetInternalTypeOfSequence((SequenceExpressionSyntax)elements[0]);

        else
            type = SemanticChecker.GetType(Elements[0]!);

        if (type == "void expression")
        {
            Error.SetError("SEMANTIC", "Sequence can't contain void expressions");
            return false;
        }

        for(int i = 1; i < elements.Count; i++)
        {
            if (!elements[i].Check(scope)) return false;

            var elementType = "";
            if (SemanticChecker.GetType(elements[i]) == "sequence")
                elementType = "sequence of " + GetInternalTypeOfSequence((SequenceExpressionSyntax)elements[i]);

            else
                elementType = SemanticChecker.GetType(elements[i]);

            if (elementType != type && type != "undefined" && elementType != "undefined")
            {
                Error.SetError("SEMANTIC", $"Elements in sequence must have the same type");
                return false;
            }
                
        }

        return true;
    }

    public override SequenceExpressionSyntax RestOfSequence(int startIndex)
    {
        List<object> elements = new();

        for (int i = startIndex; i < Elements.Count; i++)
            elements.Add(Elements[i]!);

        return new FiniteSequence<object>(elements);
    }
}
