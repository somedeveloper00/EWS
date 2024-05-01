using System;
using System.Diagnostics;

namespace Ews.Essentials.Data.FixedStrings
{
    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString8 : IFixedString
    {
        public FixedList8<char> buffer;

        public FixedString8(string str)
        {
            buffer = new();
            for (int i = 0; i < str.Length; i++)
            {
                buffer[i] = str[i];
            }
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 8;

        public static implicit operator string(FixedString8 value) => value.ToString();
        public static implicit operator FixedString8(string value) => new(value);

        public override readonly string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < buffer.Count; i++)
            {
                sb.Append(buffer.rrefat(i));
            }
            return sb.ToString();
        }
    }

    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString32 : IFixedString
    {
        public FixedList32<char> buffer;

        public FixedString32(string str)
        {
            buffer = new();
            for (int i = 0; i < str.Length; i++)
            {
                buffer[i] = str[i];
            }
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 32;

        public static implicit operator string(FixedString32 value) => value.ToString();
        public static implicit operator FixedString32(string value) => new(value);

        public override readonly string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < buffer.Count; i++)
            {
                sb.Append(buffer.rrefat(i));
            }
            return sb.ToString();
        }
    }

    [DebuggerDisplay("{" + nameof(ToString) + "(),nq}")]
    [Serializable]
    public struct FixedString128 : IFixedString
    {
        public FixedList128<char> buffer;

        public FixedString128(string str)
        {
            buffer = new();
            for (int i = 0; i < str.Length; i++)
            {
                buffer[i] = str[i];
            }
        }

        public readonly char this[int index] => buffer[index];
        public readonly int Length => buffer.Count;
        public readonly int Capacity => 128;

        public static implicit operator string(FixedString128 value) => value.ToString();
        public static implicit operator FixedString128(string value) => new(value);

        public override readonly string ToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 0; i < buffer.Count; i++)
            {
                sb.Append(buffer.rrefat(i));
            }
            return sb.ToString();
        }
    }
}