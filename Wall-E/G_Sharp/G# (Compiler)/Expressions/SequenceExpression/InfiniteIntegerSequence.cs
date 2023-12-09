using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace G_Sharp;

public sealed class InfiniteIntegerSequence : SequenceExpressionSyntax
{
    public override object this[int index] => First + index;
    
    public double First { get; }
    private double Last { get; }

    public override long Count => Last == long.MaxValue ? -1 : long.Parse((Last  + 1 - First).ToString());

    public override string ValuesType => "number";

    public InfiniteIntegerSequence(double first, double last)
    {
        First = first;
        Last = last;
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public override object Evaluate(Scope scope)
    {
        return new InfiniteIntegerSequence(First, Last);
    }

    public override SequenceExpressionSyntax RestOfSequence(int startIndex)
    {
        return new InfiniteIntegerSequence(startIndex, Last);
    }
}

