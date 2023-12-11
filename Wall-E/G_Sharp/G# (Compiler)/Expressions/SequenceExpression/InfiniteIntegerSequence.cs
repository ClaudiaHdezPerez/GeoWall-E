using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace G_Sharp;

#region Secuencias infinitas de enteros
public sealed class InfiniteIntegerSequence : SequenceExpressionSyntax
{
    public override object this[int index] => First + index;
    
    public override long Count => Last == long.MaxValue ? -1 : long.Parse((Last  + 1 - First).ToString());

    public override string ValuesType => "number";

    #region Constructor
    public double First { get; }
    private double Last { get; }
    public InfiniteIntegerSequence(double first, double last)
    {
        First = first;
        Last = last;
    }

    #endregion


    // Revisión
    public override bool Check(Scope scope)
    {
        return true;
    }

    // Evaluación
    public override object Evaluate(Scope scope)
    {
        return new InfiniteIntegerSequence(First, Last);
    }

    // Obtener el resto de la secuencia a partir de un n
    public override SequenceExpressionSyntax RestOfSequence(int startIndex)
    {
        return new InfiniteIntegerSequence(startIndex, Last);
    }
}

#endregion

