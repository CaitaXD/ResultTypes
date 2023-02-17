﻿using System.Diagnostics.CodeAnalysis;

namespace SMData;

public readonly struct None : IEquatable<None>
{
    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return obj is IOption { HasValue: false };
    }
    public bool Equals(None _) => true;

    public override int GetHashCode()
    {
        return 0;
    }
    public override string ToString()
    {
        return "None";
    }
    public static implicit operator bool(None _) => false;
}
