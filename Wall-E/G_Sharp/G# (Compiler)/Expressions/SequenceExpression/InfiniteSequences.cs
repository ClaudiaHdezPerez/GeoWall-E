using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class InfiniteSequence : SequenceExpressionSyntax
{
    public override long Count => -1;
    public Func<object> Func { get; }
    private Dictionary<int, object> Elements { get; }

    public string valuesType = "undefined";
    public override string ValuesType => valuesType;

    public override object this[int index]
    {
        get
        {
            if (!Elements.ContainsKey(index))
                Elements[index] = Func();

            return Elements[index];
        }
    }

    public InfiniteSequence(Func<object> func, Dictionary<int, object> elements)
    {
        Func = func;
        Elements = elements;
    }

    public override bool Check(Scope scope)
    {
        if (Elements.Count == 0) return true;

        string type = SemanticChecker.GetType(Elements[0]);

        foreach (var element in Elements)
        {
            if (SemanticChecker.GetType(element) != type)
                return false;
        }

        return true;
    }

    public override object Evaluate(Scope scope)
    {
        return null!;
    }

    public override SequenceExpressionSyntax RestOfSequence(int startIndex)
    {
        for (int i = 0; i < startIndex; i++)
            Elements.Remove(i);

        return new InfiniteSequence(Func, Elements);
    }

    public static object CreateInifniteSequence(InfiniteSequence sequence, Dictionary<int, object> values)
    {
        var func = sequence.Func;
        return new InfiniteSequence(func, values);
    }

    public static object CreateInifniteSequence(InfiniteIntegerSequence sequence, Dictionary<int, object> values)
    {
        var first = sequence.First;
        Func<object> func = () => first++;
        return new InfiniteSequence(func, values);
    }
}