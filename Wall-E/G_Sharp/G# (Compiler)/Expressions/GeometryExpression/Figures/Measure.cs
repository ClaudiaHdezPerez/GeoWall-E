﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Sharp;

public sealed class Measure : ExpressionSyntax
{
    public override SyntaxKind Kind => SyntaxKind.MeasureToken;
    public float Value { get; }
    public override string ReturnType => "measure";

    public Measure(Points p1, Points p2)
    {
        Value = Utilities.DistanceBetweenPoints(p1, p2);
    }

    public Measure(float value)
    {
        Value = value;
    }

    public override object Evaluate(Scope scope)
    {
        return new Measure(Value);
    }

    public override bool Check(Scope scope)
    {
        return true;
    }

    public bool Equals(Measure? other)
    {
        return Value.Equals(other!.Value);
    }

    public override bool Equals(object? obj) => Equals(obj as Measure);

    public override int GetHashCode() => Value.GetHashCode();
}
