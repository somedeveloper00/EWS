using System;
using System.Runtime.CompilerServices;

namespace Ews.Essentials.Data
{
    [System.Diagnostics.DebuggerDisplay("Count = {_count}")]
    [Serializable]
    public struct FixedList128<T> : IFixedList<T> where T : unmanaged
    {
        public int _count;
        public T _0;
        public T _1;
        public T _2;
        public T _3;
        public T _4;
        public T _5;
        public T _6;
        public T _7;
        public T _8;
        public T _9;
        public T _10;
        public T _11;
        public T _12;
        public T _13;
        public T _14;
        public T _15;
        public T _16;
        public T _17;
        public T _18;
        public T _19;
        public T _20;
        public T _21;
        public T _22;
        public T _23;
        public T _24;
        public T _25;
        public T _26;
        public T _27;
        public T _28;
        public T _29;
        public T _30;
        public T _31;
        public T _32;
        public T _33;
        public T _34;
        public T _35;
        public T _36;
        public T _37;
        public T _38;
        public T _39;
        public T _40;
        public T _41;
        public T _42;
        public T _43;
        public T _44;
        public T _45;
        public T _46;
        public T _47;
        public T _48;
        public T _49;
        public T _50;
        public T _51;
        public T _52;
        public T _53;
        public T _54;
        public T _55;
        public T _56;
        public T _57;
        public T _58;
        public T _59;
        public T _60;
        public T _61;
        public T _62;
        public T _63;
        public T _64;
        public T _65;
        public T _66;
        public T _67;
        public T _68;
        public T _69;
        public T _70;
        public T _71;
        public T _72;
        public T _73;
        public T _74;
        public T _75;
        public T _76;
        public T _77;
        public T _78;
        public T _79;
        public T _80;
        public T _81;
        public T _82;
        public T _83;
        public T _84;
        public T _85;
        public T _86;
        public T _87;
        public T _88;
        public T _89;
        public T _90;
        public T _91;
        public T _92;
        public T _93;
        public T _94;
        public T _95;
        public T _96;
        public T _97;
        public T _98;
        public T _99;
        public T _100;
        public T _101;
        public T _102;
        public T _103;
        public T _104;
        public T _105;
        public T _106;
        public T _107;
        public T _108;
        public T _109;
        public T _110;
        public T _111;
        public T _112;
        public T _113;
        public T _114;
        public T _115;
        public T _116;
        public T _117;
        public T _118;
        public T _119;
        public T _120;
        public T _121;
        public T _122;
        public T _123;
        public T _124;
        public T _125;
        public T _126;
        public T _127;

        public readonly int Count => _count;
        public readonly int Capacity => 128;

        public T this[int index]
        {
            readonly get => index switch
            {
                0 => _0,
                1 => _1,
                2 => _2,
                3 => _3,
                4 => _4,
                5 => _5,
                6 => _6,
                7 => _7,
                8 => _8,
                9 => _9,
                10 => _10,
                11 => _11,
                12 => _12,
                13 => _13,
                14 => _14,
                15 => _15,
                16 => _16,
                17 => _17,
                18 => _18,
                19 => _19,
                20 => _20,
                21 => _21,
                22 => _22,
                23 => _23,
                24 => _24,
                25 => _25,
                26 => _26,
                27 => _27,
                28 => _28,
                29 => _29,
                30 => _30,
                31 => _31,
                32 => _32,
                33 => _33,
                34 => _34,
                35 => _35,
                36 => _36,
                37 => _37,
                38 => _38,
                39 => _39,
                40 => _40,
                41 => _41,
                42 => _42,
                43 => _43,
                44 => _44,
                45 => _45,
                46 => _46,
                47 => _47,
                48 => _48,
                49 => _49,
                50 => _50,
                51 => _51,
                52 => _52,
                53 => _53,
                54 => _54,
                55 => _55,
                56 => _56,
                57 => _57,
                58 => _58,
                59 => _59,
                60 => _60,
                61 => _61,
                62 => _62,
                63 => _63,
                64 => _64,
                65 => _65,
                66 => _66,
                67 => _67,
                68 => _68,
                69 => _69,
                70 => _70,
                71 => _71,
                72 => _72,
                73 => _73,
                74 => _74,
                75 => _75,
                76 => _76,
                77 => _77,
                78 => _78,
                79 => _79,
                80 => _80,
                81 => _81,
                82 => _82,
                83 => _83,
                84 => _84,
                85 => _85,
                86 => _86,
                87 => _87,
                88 => _88,
                89 => _89,
                90 => _90,
                91 => _91,
                92 => _92,
                93 => _93,
                94 => _94,
                95 => _95,
                96 => _96,
                97 => _97,
                98 => _98,
                99 => _99,
                100 => _100,
                101 => _101,
                102 => _102,
                103 => _103,
                104 => _104,
                105 => _105,
                106 => _106,
                107 => _107,
                108 => _108,
                109 => _109,
                110 => _110,
                111 => _111,
                112 => _112,
                113 => _113,
                114 => _114,
                115 => _115,
                116 => _116,
                117 => _117,
                118 => _118,
                119 => _119,
                120 => _120,
                121 => _121,
                122 => _122,
                123 => _123,
                124 => _124,
                125 => _125,
                126 => _126,
                127 => _127,
                _ => throw new ArgumentOutOfRangeException(nameof(index))
            };
            set
            {
                switch (index)
                {
                    case 0: _0 = value; break;
                    case 1: _1 = value; break;
                    case 2: _2 = value; break;
                    case 3: _3 = value; break;
                    case 4: _4 = value; break;
                    case 5: _5 = value; break;
                    case 6: _6 = value; break;
                    case 7: _7 = value; break;
                    case 8: _8 = value; break;
                    case 9: _9 = value; break;
                    case 10: _10 = value; break;
                    case 11: _11 = value; break;
                    case 12: _12 = value; break;
                    case 13: _13 = value; break;
                    case 14: _14 = value; break;
                    case 15: _15 = value; break;
                    case 16: _16 = value; break;
                    case 17: _17 = value; break;
                    case 18: _18 = value; break;
                    case 19: _19 = value; break;
                    case 20: _20 = value; break;
                    case 21: _21 = value; break;
                    case 22: _22 = value; break;
                    case 23: _23 = value; break;
                    case 24: _24 = value; break;
                    case 25: _25 = value; break;
                    case 26: _26 = value; break;
                    case 27: _27 = value; break;
                    case 28: _28 = value; break;
                    case 29: _29 = value; break;
                    case 30: _30 = value; break;
                    case 31: _31 = value; break;
                    case 32: _32 = value; break;
                    case 33: _33 = value; break;
                    case 34: _34 = value; break;
                    case 35: _35 = value; break;
                    case 36: _36 = value; break;
                    case 37: _37 = value; break;
                    case 38: _38 = value; break;
                    case 39: _39 = value; break;
                    case 40: _40 = value; break;
                    case 41: _41 = value; break;
                    case 42: _42 = value; break;
                    case 43: _43 = value; break;
                    case 44: _44 = value; break;
                    case 45: _45 = value; break;
                    case 46: _46 = value; break;
                    case 47: _47 = value; break;
                    case 48: _48 = value; break;
                    case 49: _49 = value; break;
                    case 50: _50 = value; break;
                    case 51: _51 = value; break;
                    case 52: _52 = value; break;
                    case 53: _53 = value; break;
                    case 54: _54 = value; break;
                    case 55: _55 = value; break;
                    case 56: _56 = value; break;
                    case 57: _57 = value; break;
                    case 58: _58 = value; break;
                    case 59: _59 = value; break;
                    case 60: _60 = value; break;
                    case 61: _61 = value; break;
                    case 62: _62 = value; break;
                    case 63: _63 = value; break;
                    case 64: _64 = value; break;
                    case 65: _65 = value; break;
                    case 66: _66 = value; break;
                    case 67: _67 = value; break;
                    case 68: _68 = value; break;
                    case 69: _69 = value; break;
                    case 70: _70 = value; break;
                    case 71: _71 = value; break;
                    case 72: _72 = value; break;
                    case 73: _73 = value; break;
                    case 74: _74 = value; break;
                    case 75: _75 = value; break;
                    case 76: _76 = value; break;
                    case 77: _77 = value; break;
                    case 78: _78 = value; break;
                    case 79: _79 = value; break;
                    case 80: _80 = value; break;
                    case 81: _81 = value; break;
                    case 82: _82 = value; break;
                    case 83: _83 = value; break;
                    case 84: _84 = value; break;
                    case 85: _85 = value; break;
                    case 86: _86 = value; break;
                    case 87: _87 = value; break;
                    case 88: _88 = value; break;
                    case 89: _89 = value; break;
                    case 90: _90 = value; break;
                    case 91: _91 = value; break;
                    case 92: _92 = value; break;
                    case 93: _93 = value; break;
                    case 94: _94 = value; break;
                    case 95: _95 = value; break;
                    case 96: _96 = value; break;
                    case 97: _97 = value; break;
                    case 98: _98 = value; break;
                    case 99: _99 = value; break;
                    case 100: _100 = value; break;
                    case 101: _101 = value; break;
                    case 102: _102 = value; break;
                    case 103: _103 = value; break;
                    case 104: _104 = value; break;
                    case 105: _105 = value; break;
                    case 106: _106 = value; break;
                    case 107: _107 = value; break;
                    case 108: _108 = value; break;
                    case 109: _109 = value; break;
                    case 110: _110 = value; break;
                    case 111: _111 = value; break;
                    case 112: _112 = value; break;
                    case 113: _113 = value; break;
                    case 114: _114 = value; break;
                    case 115: _115 = value; break;
                    case 116: _116 = value; break;
                    case 117: _117 = value; break;
                    case 118: _118 = value; break;
                    case 119: _119 = value; break;
                    case 120: _120 = value; break;
                    case 121: _121 = value; break;
                    case 122: _122 = value; break;
                    case 123: _123 = value; break;
                    case 124: _124 = value; break;
                    case 125: _125 = value; break;
                    case 126: _126 = value; break;
                    case 127: _127 = value; break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(index));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly int IndexOf(T item)
        {
            if (_0.Equals(item)) return 0;
            if (_1.Equals(item)) return 1;
            if (_2.Equals(item)) return 2;
            if (_3.Equals(item)) return 3;
            if (_4.Equals(item)) return 4;
            if (_5.Equals(item)) return 5;
            if (_6.Equals(item)) return 6;
            if (_7.Equals(item)) return 7;
            if (_8.Equals(item)) return 8;
            if (_9.Equals(item)) return 9;
            if (_10.Equals(item)) return 10;
            if (_11.Equals(item)) return 11;
            if (_12.Equals(item)) return 12;
            if (_13.Equals(item)) return 13;
            if (_14.Equals(item)) return 14;
            if (_15.Equals(item)) return 15;
            if (_16.Equals(item)) return 16;
            if (_17.Equals(item)) return 17;
            if (_18.Equals(item)) return 18;
            if (_19.Equals(item)) return 19;
            if (_20.Equals(item)) return 20;
            if (_21.Equals(item)) return 21;
            if (_22.Equals(item)) return 22;
            if (_23.Equals(item)) return 23;
            if (_24.Equals(item)) return 24;
            if (_25.Equals(item)) return 25;
            if (_26.Equals(item)) return 26;
            if (_27.Equals(item)) return 27;
            if (_28.Equals(item)) return 28;
            if (_29.Equals(item)) return 29;
            if (_30.Equals(item)) return 30;
            if (_31.Equals(item)) return 31;
            if (_32.Equals(item)) return 32;
            if (_33.Equals(item)) return 33;
            if (_34.Equals(item)) return 34;
            if (_35.Equals(item)) return 35;
            if (_36.Equals(item)) return 36;
            if (_37.Equals(item)) return 37;
            if (_38.Equals(item)) return 38;
            if (_39.Equals(item)) return 39;
            if (_40.Equals(item)) return 40;
            if (_41.Equals(item)) return 41;
            if (_42.Equals(item)) return 42;
            if (_43.Equals(item)) return 43;
            if (_44.Equals(item)) return 44;
            if (_45.Equals(item)) return 45;
            if (_46.Equals(item)) return 46;
            if (_47.Equals(item)) return 47;
            if (_48.Equals(item)) return 48;
            if (_49.Equals(item)) return 49;
            if (_50.Equals(item)) return 50;
            if (_51.Equals(item)) return 51;
            if (_52.Equals(item)) return 52;
            if (_53.Equals(item)) return 53;
            if (_54.Equals(item)) return 54;
            if (_55.Equals(item)) return 55;
            if (_56.Equals(item)) return 56;
            if (_57.Equals(item)) return 57;
            if (_58.Equals(item)) return 58;
            if (_59.Equals(item)) return 59;
            if (_60.Equals(item)) return 60;
            if (_61.Equals(item)) return 61;
            if (_62.Equals(item)) return 62;
            if (_63.Equals(item)) return 63;
            if (_64.Equals(item)) return 64;
            if (_65.Equals(item)) return 65;
            if (_66.Equals(item)) return 66;
            if (_67.Equals(item)) return 67;
            if (_68.Equals(item)) return 68;
            if (_69.Equals(item)) return 69;
            if (_70.Equals(item)) return 70;
            if (_71.Equals(item)) return 71;
            if (_72.Equals(item)) return 72;
            if (_73.Equals(item)) return 73;
            if (_74.Equals(item)) return 74;
            if (_75.Equals(item)) return 75;
            if (_76.Equals(item)) return 76;
            if (_77.Equals(item)) return 77;
            if (_78.Equals(item)) return 78;
            if (_79.Equals(item)) return 79;
            if (_80.Equals(item)) return 80;
            if (_81.Equals(item)) return 81;
            if (_82.Equals(item)) return 82;
            if (_83.Equals(item)) return 83;
            if (_84.Equals(item)) return 84;
            if (_85.Equals(item)) return 85;
            if (_86.Equals(item)) return 86;
            if (_87.Equals(item)) return 87;
            if (_88.Equals(item)) return 88;
            if (_89.Equals(item)) return 89;
            if (_90.Equals(item)) return 90;
            if (_91.Equals(item)) return 91;
            if (_92.Equals(item)) return 92;
            if (_93.Equals(item)) return 93;
            if (_94.Equals(item)) return 94;
            if (_95.Equals(item)) return 95;
            if (_96.Equals(item)) return 96;
            if (_97.Equals(item)) return 97;
            if (_98.Equals(item)) return 98;
            if (_99.Equals(item)) return 99;
            if (_100.Equals(item)) return 100;
            if (_101.Equals(item)) return 101;
            if (_102.Equals(item)) return 102;
            if (_103.Equals(item)) return 103;
            if (_104.Equals(item)) return 104;
            if (_105.Equals(item)) return 105;
            if (_106.Equals(item)) return 106;
            if (_107.Equals(item)) return 107;
            if (_108.Equals(item)) return 108;
            if (_109.Equals(item)) return 109;
            if (_110.Equals(item)) return 110;
            if (_111.Equals(item)) return 111;
            if (_112.Equals(item)) return 112;
            if (_113.Equals(item)) return 113;
            if (_114.Equals(item)) return 114;
            if (_115.Equals(item)) return 115;
            if (_116.Equals(item)) return 116;
            if (_117.Equals(item)) return 117;
            if (_118.Equals(item)) return 118;
            if (_119.Equals(item)) return 119;
            if (_120.Equals(item)) return 120;
            if (_121.Equals(item)) return 121;
            if (_122.Equals(item)) return 122;
            if (_123.Equals(item)) return 123;
            if (_124.Equals(item)) return 124;
            if (_125.Equals(item)) return 125;
            if (_126.Equals(item)) return 126;
            if (_127.Equals(item)) return 127;
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            CheckCapacity(_count + 1);
            this[_count++] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, T item)
        {
            CheckCapacity(index);
            this[index] = item;
            _count = index == _count ? _count + 1 : _count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
        {
            for (int i = index + 1; i < _count; i++)
            {
                this[index - 1] = this[index];
            }
            _count--;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1) return false;
            RemoveAt(index);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                this[i] = default;
            }
            _count = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly void CheckCapacity(int capacity)
        {
            if (capacity > Capacity)
                throw new IndexOutOfRangeException(capacity.ToString());
        }
    }
}