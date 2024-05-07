using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Ews.Essentials.Data
{
    /// <summary>
    /// rat: ref at. 
    /// ratr: ref at reversed. 
    /// rrat: readonly ref at. 
    /// rratr: readonly ref at reversed. 
    /// </summary>
    public static class FixedListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T2> Select<T1, T2>(this IFixedList<T1> list, Func<T1, T2> predecate)
            where T1 : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                yield return predecate(list[i]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerator<T> Where<T>(this IFixedList<T> list, Func<T, bool> condition)
            where T : unmanaged
        {
            for (int i = 0; i < list.Count; i++)
            {
                T item = list[i];
                if (condition(item))
                    yield return item;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat<T>(this ref FixedList4<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat_rev<T>(this ref FixedList4<T> list, int index)
            where T : unmanaged => ref rat(ref list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rrat<T>(this in FixedList4<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rratr<T>(this in FixedList4<T> list, int index)
            where T : unmanaged => ref rrat(in list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat<T>(this ref FixedList8<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ratr<T>(this ref FixedList8<T> list, int index)
            where T : unmanaged => ref rat(ref list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rrat<T>(this in FixedList8<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rratr<T>(this in FixedList8<T> list, int index)
            where T : unmanaged => ref rrat(in list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat<T>(this ref FixedList16<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ratr<T>(this ref FixedList16<T> list, int index)
            where T : unmanaged => ref rat(ref list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rrat<T>(this in FixedList16<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rratr<T>(this in FixedList16<T> list, int index)
            where T : unmanaged => ref rrat(in list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat<T>(this ref FixedList32<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                case 16: return ref list._16;
                case 17: return ref list._17;
                case 18: return ref list._18;
                case 19: return ref list._19;
                case 20: return ref list._20;
                case 21: return ref list._21;
                case 22: return ref list._22;
                case 23: return ref list._23;
                case 24: return ref list._24;
                case 25: return ref list._25;
                case 26: return ref list._26;
                case 27: return ref list._27;
                case 28: return ref list._28;
                case 29: return ref list._29;
                case 30: return ref list._30;
                case 31: return ref list._31;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ratr<T>(this ref FixedList32<T> list, int index)
            where T : unmanaged => ref rat(ref list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rrat<T>(this in FixedList32<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                case 16: return ref list._16;
                case 17: return ref list._17;
                case 18: return ref list._18;
                case 19: return ref list._19;
                case 20: return ref list._20;
                case 21: return ref list._21;
                case 22: return ref list._22;
                case 23: return ref list._23;
                case 24: return ref list._24;
                case 25: return ref list._25;
                case 26: return ref list._26;
                case 27: return ref list._27;
                case 28: return ref list._28;
                case 29: return ref list._29;
                case 30: return ref list._30;
                case 31: return ref list._31;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rratr<T>(this in FixedList32<T> list, int index)
            where T : unmanaged => ref rrat(in list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T rat<T>(this ref FixedList128<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                case 16: return ref list._16;
                case 17: return ref list._17;
                case 18: return ref list._18;
                case 19: return ref list._19;
                case 20: return ref list._20;
                case 21: return ref list._21;
                case 22: return ref list._22;
                case 23: return ref list._23;
                case 24: return ref list._24;
                case 25: return ref list._25;
                case 26: return ref list._26;
                case 27: return ref list._27;
                case 28: return ref list._28;
                case 29: return ref list._29;
                case 30: return ref list._30;
                case 31: return ref list._31;
                case 32: return ref list._32;
                case 33: return ref list._33;
                case 34: return ref list._34;
                case 35: return ref list._35;
                case 36: return ref list._36;
                case 37: return ref list._37;
                case 38: return ref list._38;
                case 39: return ref list._39;
                case 40: return ref list._40;
                case 41: return ref list._41;
                case 42: return ref list._42;
                case 43: return ref list._43;
                case 44: return ref list._44;
                case 45: return ref list._45;
                case 46: return ref list._46;
                case 47: return ref list._47;
                case 48: return ref list._48;
                case 49: return ref list._49;
                case 50: return ref list._50;
                case 51: return ref list._51;
                case 52: return ref list._52;
                case 53: return ref list._53;
                case 54: return ref list._54;
                case 55: return ref list._55;
                case 56: return ref list._56;
                case 57: return ref list._57;
                case 58: return ref list._58;
                case 59: return ref list._59;
                case 60: return ref list._60;
                case 61: return ref list._61;
                case 62: return ref list._62;
                case 63: return ref list._63;
                case 64: return ref list._64;
                case 65: return ref list._65;
                case 66: return ref list._66;
                case 67: return ref list._67;
                case 68: return ref list._68;
                case 69: return ref list._69;
                case 70: return ref list._70;
                case 71: return ref list._71;
                case 72: return ref list._72;
                case 73: return ref list._73;
                case 74: return ref list._74;
                case 75: return ref list._75;
                case 76: return ref list._76;
                case 77: return ref list._77;
                case 78: return ref list._78;
                case 79: return ref list._79;
                case 80: return ref list._80;
                case 81: return ref list._81;
                case 82: return ref list._82;
                case 83: return ref list._83;
                case 84: return ref list._84;
                case 85: return ref list._85;
                case 86: return ref list._86;
                case 87: return ref list._87;
                case 88: return ref list._88;
                case 89: return ref list._89;
                case 90: return ref list._90;
                case 91: return ref list._91;
                case 92: return ref list._92;
                case 93: return ref list._93;
                case 94: return ref list._94;
                case 95: return ref list._95;
                case 96: return ref list._96;
                case 97: return ref list._97;
                case 98: return ref list._98;
                case 99: return ref list._99;
                case 100: return ref list._100;
                case 101: return ref list._101;
                case 102: return ref list._102;
                case 103: return ref list._103;
                case 104: return ref list._104;
                case 105: return ref list._105;
                case 106: return ref list._106;
                case 107: return ref list._107;
                case 108: return ref list._108;
                case 109: return ref list._109;
                case 110: return ref list._110;
                case 111: return ref list._111;
                case 112: return ref list._112;
                case 113: return ref list._113;
                case 114: return ref list._114;
                case 115: return ref list._115;
                case 116: return ref list._116;
                case 117: return ref list._117;
                case 118: return ref list._118;
                case 119: return ref list._119;
                case 120: return ref list._120;
                case 121: return ref list._121;
                case 122: return ref list._122;
                case 123: return ref list._123;
                case 124: return ref list._124;
                case 125: return ref list._125;
                case 126: return ref list._126;
                case 127: return ref list._127;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ratr<T>(this ref FixedList128<T> list, int index)
            where T : unmanaged => ref rat(ref list, list.Count - 1 - index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rrat<T>(this in FixedList128<T> list, int index)
            where T : unmanaged
        {
            switch (index)
            {
                case 0: return ref list._0;
                case 1: return ref list._1;
                case 2: return ref list._2;
                case 3: return ref list._3;
                case 4: return ref list._4;
                case 5: return ref list._5;
                case 6: return ref list._6;
                case 7: return ref list._7;
                case 8: return ref list._8;
                case 9: return ref list._9;
                case 10: return ref list._10;
                case 11: return ref list._11;
                case 12: return ref list._12;
                case 13: return ref list._13;
                case 14: return ref list._14;
                case 15: return ref list._15;
                case 16: return ref list._16;
                case 17: return ref list._17;
                case 18: return ref list._18;
                case 19: return ref list._19;
                case 20: return ref list._20;
                case 21: return ref list._21;
                case 22: return ref list._22;
                case 23: return ref list._23;
                case 24: return ref list._24;
                case 25: return ref list._25;
                case 26: return ref list._26;
                case 27: return ref list._27;
                case 28: return ref list._28;
                case 29: return ref list._29;
                case 30: return ref list._30;
                case 31: return ref list._31;
                case 32: return ref list._32;
                case 33: return ref list._33;
                case 34: return ref list._34;
                case 35: return ref list._35;
                case 36: return ref list._36;
                case 37: return ref list._37;
                case 38: return ref list._38;
                case 39: return ref list._39;
                case 40: return ref list._40;
                case 41: return ref list._41;
                case 42: return ref list._42;
                case 43: return ref list._43;
                case 44: return ref list._44;
                case 45: return ref list._45;
                case 46: return ref list._46;
                case 47: return ref list._47;
                case 48: return ref list._48;
                case 49: return ref list._49;
                case 50: return ref list._50;
                case 51: return ref list._51;
                case 52: return ref list._52;
                case 53: return ref list._53;
                case 54: return ref list._54;
                case 55: return ref list._55;
                case 56: return ref list._56;
                case 57: return ref list._57;
                case 58: return ref list._58;
                case 59: return ref list._59;
                case 60: return ref list._60;
                case 61: return ref list._61;
                case 62: return ref list._62;
                case 63: return ref list._63;
                case 64: return ref list._64;
                case 65: return ref list._65;
                case 66: return ref list._66;
                case 67: return ref list._67;
                case 68: return ref list._68;
                case 69: return ref list._69;
                case 70: return ref list._70;
                case 71: return ref list._71;
                case 72: return ref list._72;
                case 73: return ref list._73;
                case 74: return ref list._74;
                case 75: return ref list._75;
                case 76: return ref list._76;
                case 77: return ref list._77;
                case 78: return ref list._78;
                case 79: return ref list._79;
                case 80: return ref list._80;
                case 81: return ref list._81;
                case 82: return ref list._82;
                case 83: return ref list._83;
                case 84: return ref list._84;
                case 85: return ref list._85;
                case 86: return ref list._86;
                case 87: return ref list._87;
                case 88: return ref list._88;
                case 89: return ref list._89;
                case 90: return ref list._90;
                case 91: return ref list._91;
                case 92: return ref list._92;
                case 93: return ref list._93;
                case 94: return ref list._94;
                case 95: return ref list._95;
                case 96: return ref list._96;
                case 97: return ref list._97;
                case 98: return ref list._98;
                case 99: return ref list._99;
                case 100: return ref list._100;
                case 101: return ref list._101;
                case 102: return ref list._102;
                case 103: return ref list._103;
                case 104: return ref list._104;
                case 105: return ref list._105;
                case 106: return ref list._106;
                case 107: return ref list._107;
                case 108: return ref list._108;
                case 109: return ref list._109;
                case 110: return ref list._110;
                case 111: return ref list._111;
                case 112: return ref list._112;
                case 113: return ref list._113;
                case 114: return ref list._114;
                case 115: return ref list._115;
                case 116: return ref list._116;
                case 117: return ref list._117;
                case 118: return ref list._118;
                case 119: return ref list._119;
                case 120: return ref list._120;
                case 121: return ref list._121;
                case 122: return ref list._122;
                case 123: return ref list._123;
                case 124: return ref list._124;
                case 125: return ref list._125;
                case 126: return ref list._126;
                case 127: return ref list._127;
                default:
                    throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T rratr<T>(this in FixedList128<T> list, int index)
            where T : unmanaged => ref rrat(in list, list.Count - 1 - index);
    }
}