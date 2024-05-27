using System;
using System.Diagnostics;
using UnityEngine;

namespace Ews.Essentials.Data.FixedStrings
{
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString8 : IFixedString
    {
        public flist8<char> buffer;

        public FixedString8(string str)
        {
#pragma warning disable IDE0017 // Simplify object initialization
            buffer = new();
            buffer.Count = Mathf.Min(Capacity - 1, str.Length);
#pragma warning restore IDE0017 // Simplify object initialization

            for (int i = 0; i < buffer.Count; i++)
                buffer[i] = str[i];
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 8;

        public static implicit operator string(FixedString8 value) => value.ToString();
        public static implicit operator FixedString8(string value) => new(value);
        public override readonly string ToString() => new(buffer.AsSpan());
    }

    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString32 : IFixedString
    {
        public flist32<char> buffer;

        public FixedString32(string str)
        {
#pragma warning disable IDE0017 // Simplify object initialization
            buffer = new();
            buffer.Count = Mathf.Min(Capacity - 1, str.Length);
#pragma warning restore IDE0017 // Simplify object initialization

            for (int i = 0; i < buffer.Count; i++)
                buffer[i] = str[i];
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 32;

        public static implicit operator string(FixedString32 value) => value.ToString();
        public static implicit operator FixedString32(string value) => new(value);
        public override readonly string ToString() => new(buffer.AsSpan());
    }

    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString128 : IFixedString
    {
        public flist128<char> buffer;

        public FixedString128(string str)
        {
#pragma warning disable IDE0017 // Simplify object initialization
            buffer = new();
            buffer.Count = Mathf.Min(Capacity - 1, str.Length);
#pragma warning restore IDE0017 // Simplify object initialization

            for (int i = 0; i < buffer.Count; i++)
                buffer[i] = str[i];
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 128;

        public static implicit operator string(FixedString128 value) => value.ToString();
        public static implicit operator FixedString128(string value) => new(value);
        public override readonly string ToString() => new(buffer.AsSpan());
    }
}