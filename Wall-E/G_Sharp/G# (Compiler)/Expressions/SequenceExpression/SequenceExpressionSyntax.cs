using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public abstract class SequenceExpressionSyntax : ExpressionSyntax, IEquatable<SequenceExpressionSyntax>
{
    public override SyntaxKind Kind => SyntaxKind.SequenceExpression;
    public abstract long Count { get; }
    public override string ReturnType => "sequence";
    public abstract string ValuesType { get; }
    public abstract object this[int index] { get; }
    public abstract SequenceExpressionSyntax RestOfSequence(int startIndex);

    public bool Equals(SequenceExpressionSyntax? other)
    {
        if (this is SequenceExpressionSyntax seq1 && other is SequenceExpressionSyntax seq2)
        {
            if (seq1.Count != seq2.Count)
                return false;

            for (int i = 0; i < seq1.Count; i++)
            {
                if (!seq1[i].Equals(seq2[i])) 
                    return false;
            }

            if (seq1.Count < 0)
            {
                if (seq1 is InfiniteIntegerSequence infinite1 && 
                    seq2 is InfiniteIntegerSequence infinite2
                   ) 
                {
                    return infinite1.First.Equals(infinite2.First);
                }

                return false;
            }
        }

        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as SequenceExpressionSyntax);

    public override int GetHashCode()
    {
        return Count.GetHashCode();
    }

    public static string GetInternalTypeOfSequence(SequenceExpressionSyntax sequence)
    {
        var seqType = sequence.ValuesType;
        var seqInternedType = seqType;

        while (seqType == "sequence")
        {
            sequence = (SequenceExpressionSyntax)sequence[0];
            seqInternedType += $" of {sequence.ValuesType}";

            seqType = sequence.ValuesType;
        }

        return seqInternedType;
    }
}