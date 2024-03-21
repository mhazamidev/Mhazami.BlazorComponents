using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Mhazami.Utility;

public class QRCodeGenerator
{
    private char[] alphanumEncTable = new char[45]
    {
      '0',
      '1',
      '2',
      '3',
      '4',
      '5',
      '6',
      '7',
      '8',
      '9',
      'A',
      'B',
      'C',
      'D',
      'E',
      'F',
      'G',
      'H',
      'I',
      'J',
      'K',
      'L',
      'M',
      'N',
      'O',
      'P',
      'Q',
      'R',
      'S',
      'T',
      'U',
      'V',
      'W',
      'X',
      'Y',
      'Z',
      ' ',
      '$',
      '%',
      '*',
      '+',
      '-',
      '.',
      '/',
      ':'
    };
    private int[] capacityBaseValues = new int[640]
    {
      41,
      25,
      17,
      10,
      34,
      20,
      14,
      8,
      27,
      16,
      11,
      7,
      17,
      10,
      7,
      4,
      77,
      47,
      32,
      20,
      63,
      38,
      26,
      16,
      48,
      29,
      20,
      12,
      34,
      20,
      14,
      8,
      (int) sbyte.MaxValue,
      77,
      53,
      32,
      101,
      61,
      42,
      26,
      77,
      47,
      32,
      20,
      58,
      35,
      24,
      15,
      187,
      114,
      78,
      48,
      149,
      90,
      62,
      38,
      111,
      67,
      46,
      28,
      82,
      50,
      34,
      21,
      (int) byte.MaxValue,
      154,
      106,
      65,
      202,
      122,
      84,
      52,
      144,
      87,
      60,
      37,
      106,
      64,
      44,
      27,
      322,
      195,
      134,
      82,
      (int) byte.MaxValue,
      154,
      106,
      65,
      178,
      108,
      74,
      45,
      139,
      84,
      58,
      36,
      370,
      224,
      154,
      95,
      293,
      178,
      122,
      75,
      207,
      125,
      86,
      53,
      154,
      93,
      64,
      39,
      461,
      279,
      192,
      118,
      365,
      221,
      152,
      93,
      259,
      157,
      108,
      66,
      202,
      122,
      84,
      52,
      552,
      335,
      230,
      141,
      432,
      262,
      180,
      111,
      312,
      189,
      130,
      80,
      235,
      143,
      98,
      60,
      652,
      395,
      271,
      167,
      513,
      311,
      213,
      131,
      364,
      221,
      151,
      93,
      288,
      174,
      119,
      74,
      772,
      468,
      321,
      198,
      604,
      366,
      251,
      155,
      427,
      259,
      177,
      109,
      331,
      200,
      137,
      85,
      883,
      535,
      367,
      226,
      691,
      419,
      287,
      177,
      489,
      296,
      203,
      125,
      374,
      227,
      155,
      96,
      1022,
      619,
      425,
      262,
      796,
      483,
      331,
      204,
      580,
      352,
      241,
      149,
      427,
      259,
      177,
      109,
      1101,
      667,
      458,
      282,
      871,
      528,
      362,
      223,
      621,
      376,
      258,
      159,
      468,
      283,
      194,
      120,
      1250,
      758,
      520,
      320,
      991,
      600,
      412,
      254,
      703,
      426,
      292,
      180,
      530,
      321,
      220,
      136,
      1408,
      854,
      586,
      361,
      1082,
      656,
      450,
      277,
      775,
      470,
      322,
      198,
      602,
      365,
      250,
      154,
      1548,
      938,
      644,
      397,
      1212,
      734,
      504,
      310,
      876,
      531,
      364,
      224,
      674,
      408,
      280,
      173,
      1725,
      1046,
      718,
      442,
      1346,
      816,
      560,
      345,
      948,
      574,
      394,
      243,
      746,
      452,
      310,
      191,
      1903,
      1153,
      792,
      488,
      1500,
      909,
      624,
      384,
      1063,
      644,
      442,
      272,
      813,
      493,
      338,
      208,
      2061,
      1249,
      858,
      528,
      1600,
      970,
      666,
      410,
      1159,
      702,
      482,
      297,
      919,
      557,
      382,
      235,
      2232,
      1352,
      929,
      572,
      1708,
      1035,
      711,
      438,
      1224,
      742,
      509,
      314,
      969,
      587,
      403,
      248,
      2409,
      1460,
      1003,
      618,
      1872,
      1134,
      779,
      480,
      1358,
      823,
      565,
      348,
      1056,
      640,
      439,
      270,
      2620,
      1588,
      1091,
      672,
      2059,
      1248,
      857,
      528,
      1468,
      890,
      611,
      376,
      1108,
      672,
      461,
      284,
      2812,
      1704,
      1171,
      721,
      2188,
      1326,
      911,
      561,
      1588,
      963,
      661,
      407,
      1228,
      744,
      511,
      315,
      3057,
      1853,
      1273,
      784,
      2395,
      1451,
      997,
      614,
      1718,
      1041,
      715,
      440,
      1286,
      779,
      535,
      330,
      3283,
      1990,
      1367,
      842,
      2544,
      1542,
      1059,
      652,
      1804,
      1094,
      751,
      462,
      1425,
      864,
      593,
      365,
      3517,
      2132,
      1465,
      902,
      2701,
      1637,
      1125,
      692,
      1933,
      1172,
      805,
      496,
      1501,
      910,
      625,
      385,
      3669,
      2223,
      1528,
      940,
      2857,
      1732,
      1190,
      732,
      2085,
      1263,
      868,
      534,
      1581,
      958,
      658,
      405,
      3909,
      2369,
      1628,
      1002,
      3035,
      1839,
      1264,
      778,
      2181,
      1322,
      908,
      559,
      1677,
      1016,
      698,
      430,
      4158,
      2520,
      1732,
      1066,
      3289,
      1994,
      1370,
      843,
      2358,
      1429,
      982,
      604,
      1782,
      1080,
      742,
      457,
      4417,
      2677,
      1840,
      1132,
      3486,
      2113,
      1452,
      894,
      2473,
      1499,
      1030,
      634,
      1897,
      1150,
      790,
      486,
      4686,
      2840,
      1952,
      1201,
      3693,
      2238,
      1538,
      947,
      2670,
      1618,
      1112,
      684,
      2022,
      1226,
      842,
      518,
      4965,
      3009,
      2068,
      1273,
      3909,
      2369,
      1628,
      1002,
      2805,
      1700,
      1168,
      719,
      2157,
      1307,
      898,
      553,
      5253,
      3183,
      2188,
      1347,
      4134,
      2506,
      1722,
      1060,
      2949,
      1787,
      1228,
      756,
      2301,
      1394,
      958,
      590,
      5529,
      3351,
      2303,
      1417,
      4343,
      2632,
      1809,
      1113,
      3081,
      1867,
      1283,
      790,
      2361,
      1431,
      983,
      605,
      5836,
      3537,
      2431,
      1496,
      4588,
      2780,
      1911,
      1176,
      3244,
      1966,
      1351,
      832,
      2524,
      1530,
      1051,
      647,
      6153,
      3729,
      2563,
      1577,
      4775,
      2894,
      1989,
      1224,
      3417,
      2071,
      1423,
      876,
      2625,
      1591,
      1093,
      673,
      6479,
      3927,
      2699,
      1661,
      5039,
      3054,
      2099,
      1292,
      3599,
      2181,
      1499,
      923,
      2735,
      1658,
      1139,
      701,
      6743,
      4087,
      2809,
      1729,
      5313,
      3220,
      2213,
      1362,
      3791,
      2298,
      1579,
      972,
      2927,
      1774,
      1219,
      750,
      7089,
      4296,
      2953,
      1817,
      5596,
      3391,
      2331,
      1435,
      3993,
      2420,
      1663,
      1024,
      3057,
      1852,
      1273,
      784
    };
    private int[] capacityECCBaseValues = new int[960]
    {
      19,
      7,
      1,
      19,
      0,
      0,
      16,
      10,
      1,
      16,
      0,
      0,
      13,
      13,
      1,
      13,
      0,
      0,
      9,
      17,
      1,
      9,
      0,
      0,
      34,
      10,
      1,
      34,
      0,
      0,
      28,
      16,
      1,
      28,
      0,
      0,
      22,
      22,
      1,
      22,
      0,
      0,
      16,
      28,
      1,
      16,
      0,
      0,
      55,
      15,
      1,
      55,
      0,
      0,
      44,
      26,
      1,
      44,
      0,
      0,
      34,
      18,
      2,
      17,
      0,
      0,
      26,
      22,
      2,
      13,
      0,
      0,
      80,
      20,
      1,
      80,
      0,
      0,
      64,
      18,
      2,
      32,
      0,
      0,
      48,
      26,
      2,
      24,
      0,
      0,
      36,
      16,
      4,
      9,
      0,
      0,
      108,
      26,
      1,
      108,
      0,
      0,
      86,
      24,
      2,
      43,
      0,
      0,
      62,
      18,
      2,
      15,
      2,
      16,
      46,
      22,
      2,
      11,
      2,
      12,
      136,
      18,
      2,
      68,
      0,
      0,
      108,
      16,
      4,
      27,
      0,
      0,
      76,
      24,
      4,
      19,
      0,
      0,
      60,
      28,
      4,
      15,
      0,
      0,
      156,
      20,
      2,
      78,
      0,
      0,
      124,
      18,
      4,
      31,
      0,
      0,
      88,
      18,
      2,
      14,
      4,
      15,
      66,
      26,
      4,
      13,
      1,
      14,
      194,
      24,
      2,
      97,
      0,
      0,
      154,
      22,
      2,
      38,
      2,
      39,
      110,
      22,
      4,
      18,
      2,
      19,
      86,
      26,
      4,
      14,
      2,
      15,
      232,
      30,
      2,
      116,
      0,
      0,
      182,
      22,
      3,
      36,
      2,
      37,
      132,
      20,
      4,
      16,
      4,
      17,
      100,
      24,
      4,
      12,
      4,
      13,
      274,
      18,
      2,
      68,
      2,
      69,
      216,
      26,
      4,
      43,
      1,
      44,
      154,
      24,
      6,
      19,
      2,
      20,
      122,
      28,
      6,
      15,
      2,
      16,
      324,
      20,
      4,
      81,
      0,
      0,
      254,
      30,
      1,
      50,
      4,
      51,
      180,
      28,
      4,
      22,
      4,
      23,
      140,
      24,
      3,
      12,
      8,
      13,
      370,
      24,
      2,
      92,
      2,
      93,
      290,
      22,
      6,
      36,
      2,
      37,
      206,
      26,
      4,
      20,
      6,
      21,
      158,
      28,
      7,
      14,
      4,
      15,
      428,
      26,
      4,
      107,
      0,
      0,
      334,
      22,
      8,
      37,
      1,
      38,
      244,
      24,
      8,
      20,
      4,
      21,
      180,
      22,
      12,
      11,
      4,
      12,
      461,
      30,
      3,
      115,
      1,
      116,
      365,
      24,
      4,
      40,
      5,
      41,
      261,
      20,
      11,
      16,
      5,
      17,
      197,
      24,
      11,
      12,
      5,
      13,
      523,
      22,
      5,
      87,
      1,
      88,
      415,
      24,
      5,
      41,
      5,
      42,
      295,
      30,
      5,
      24,
      7,
      25,
      223,
      24,
      11,
      12,
      7,
      13,
      589,
      24,
      5,
      98,
      1,
      99,
      453,
      28,
      7,
      45,
      3,
      46,
      325,
      24,
      15,
      19,
      2,
      20,
      253,
      30,
      3,
      15,
      13,
      16,
      647,
      28,
      1,
      107,
      5,
      108,
      507,
      28,
      10,
      46,
      1,
      47,
      367,
      28,
      1,
      22,
      15,
      23,
      283,
      28,
      2,
      14,
      17,
      15,
      721,
      30,
      5,
      120,
      1,
      121,
      563,
      26,
      9,
      43,
      4,
      44,
      397,
      28,
      17,
      22,
      1,
      23,
      313,
      28,
      2,
      14,
      19,
      15,
      795,
      28,
      3,
      113,
      4,
      114,
      627,
      26,
      3,
      44,
      11,
      45,
      445,
      26,
      17,
      21,
      4,
      22,
      341,
      26,
      9,
      13,
      16,
      14,
      861,
      28,
      3,
      107,
      5,
      108,
      669,
      26,
      3,
      41,
      13,
      42,
      485,
      30,
      15,
      24,
      5,
      25,
      385,
      28,
      15,
      15,
      10,
      16,
      932,
      28,
      4,
      116,
      4,
      117,
      714,
      26,
      17,
      42,
      0,
      0,
      512,
      28,
      17,
      22,
      6,
      23,
      406,
      30,
      19,
      16,
      6,
      17,
      1006,
      28,
      2,
      111,
      7,
      112,
      782,
      28,
      17,
      46,
      0,
      0,
      568,
      30,
      7,
      24,
      16,
      25,
      442,
      24,
      34,
      13,
      0,
      0,
      1094,
      30,
      4,
      121,
      5,
      122,
      860,
      28,
      4,
      47,
      14,
      48,
      614,
      30,
      11,
      24,
      14,
      25,
      464,
      30,
      16,
      15,
      14,
      16,
      1174,
      30,
      6,
      117,
      4,
      118,
      914,
      28,
      6,
      45,
      14,
      46,
      664,
      30,
      11,
      24,
      16,
      25,
      514,
      30,
      30,
      16,
      2,
      17,
      1276,
      26,
      8,
      106,
      4,
      107,
      1000,
      28,
      8,
      47,
      13,
      48,
      718,
      30,
      7,
      24,
      22,
      25,
      538,
      30,
      22,
      15,
      13,
      16,
      1370,
      28,
      10,
      114,
      2,
      115,
      1062,
      28,
      19,
      46,
      4,
      47,
      754,
      28,
      28,
      22,
      6,
      23,
      596,
      30,
      33,
      16,
      4,
      17,
      1468,
      30,
      8,
      122,
      4,
      123,
      1128,
      28,
      22,
      45,
      3,
      46,
      808,
      30,
      8,
      23,
      26,
      24,
      628,
      30,
      12,
      15,
      28,
      16,
      1531,
      30,
      3,
      117,
      10,
      118,
      1193,
      28,
      3,
      45,
      23,
      46,
      871,
      30,
      4,
      24,
      31,
      25,
      661,
      30,
      11,
      15,
      31,
      16,
      1631,
      30,
      7,
      116,
      7,
      117,
      1267,
      28,
      21,
      45,
      7,
      46,
      911,
      30,
      1,
      23,
      37,
      24,
      701,
      30,
      19,
      15,
      26,
      16,
      1735,
      30,
      5,
      115,
      10,
      116,
      1373,
      28,
      19,
      47,
      10,
      48,
      985,
      30,
      15,
      24,
      25,
      25,
      745,
      30,
      23,
      15,
      25,
      16,
      1843,
      30,
      13,
      115,
      3,
      116,
      1455,
      28,
      2,
      46,
      29,
      47,
      1033,
      30,
      42,
      24,
      1,
      25,
      793,
      30,
      23,
      15,
      28,
      16,
      1955,
      30,
      17,
      115,
      0,
      0,
      1541,
      28,
      10,
      46,
      23,
      47,
      1115,
      30,
      10,
      24,
      35,
      25,
      845,
      30,
      19,
      15,
      35,
      16,
      2071,
      30,
      17,
      115,
      1,
      116,
      1631,
      28,
      14,
      46,
      21,
      47,
      1171,
      30,
      29,
      24,
      19,
      25,
      901,
      30,
      11,
      15,
      46,
      16,
      2191,
      30,
      13,
      115,
      6,
      116,
      1725,
      28,
      14,
      46,
      23,
      47,
      1231,
      30,
      44,
      24,
      7,
      25,
      961,
      30,
      59,
      16,
      1,
      17,
      2306,
      30,
      12,
      121,
      7,
      122,
      1812,
      28,
      12,
      47,
      26,
      48,
      1286,
      30,
      39,
      24,
      14,
      25,
      986,
      30,
      22,
      15,
      41,
      16,
      2434,
      30,
      6,
      121,
      14,
      122,
      1914,
      28,
      6,
      47,
      34,
      48,
      1354,
      30,
      46,
      24,
      10,
      25,
      1054,
      30,
      2,
      15,
      64,
      16,
      2566,
      30,
      17,
      122,
      4,
      123,
      1992,
      28,
      29,
      46,
      14,
      47,
      1426,
      30,
      49,
      24,
      10,
      25,
      1096,
      30,
      24,
      15,
      46,
      16,
      2702,
      30,
      4,
      122,
      18,
      123,
      2102,
      28,
      13,
      46,
      32,
      47,
      1502,
      30,
      48,
      24,
      14,
      25,
      1142,
      30,
      42,
      15,
      32,
      16,
      2812,
      30,
      20,
      117,
      4,
      118,
      2216,
      28,
      40,
      47,
      7,
      48,
      1582,
      30,
      43,
      24,
      22,
      25,
      1222,
      30,
      10,
      15,
      67,
      16,
      2956,
      30,
      19,
      118,
      6,
      119,
      2334,
      28,
      18,
      47,
      31,
      48,
      1666,
      30,
      34,
      24,
      34,
      25,
      1276,
      30,
      20,
      15,
      61,
      16
    };
    private int[] alignmentPatternBaseValues = new int[280]
    {
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      6,
      18,
      0,
      0,
      0,
      0,
      0,
      6,
      22,
      0,
      0,
      0,
      0,
      0,
      6,
      26,
      0,
      0,
      0,
      0,
      0,
      6,
      30,
      0,
      0,
      0,
      0,
      0,
      6,
      34,
      0,
      0,
      0,
      0,
      0,
      6,
      22,
      38,
      0,
      0,
      0,
      0,
      6,
      24,
      42,
      0,
      0,
      0,
      0,
      6,
      26,
      46,
      0,
      0,
      0,
      0,
      6,
      28,
      50,
      0,
      0,
      0,
      0,
      6,
      30,
      54,
      0,
      0,
      0,
      0,
      6,
      32,
      58,
      0,
      0,
      0,
      0,
      6,
      34,
      62,
      0,
      0,
      0,
      0,
      6,
      26,
      46,
      66,
      0,
      0,
      0,
      6,
      26,
      48,
      70,
      0,
      0,
      0,
      6,
      26,
      50,
      74,
      0,
      0,
      0,
      6,
      30,
      54,
      78,
      0,
      0,
      0,
      6,
      30,
      56,
      82,
      0,
      0,
      0,
      6,
      30,
      58,
      86,
      0,
      0,
      0,
      6,
      34,
      62,
      90,
      0,
      0,
      0,
      6,
      28,
      50,
      72,
      94,
      0,
      0,
      6,
      26,
      50,
      74,
      98,
      0,
      0,
      6,
      30,
      54,
      78,
      102,
      0,
      0,
      6,
      28,
      54,
      80,
      106,
      0,
      0,
      6,
      32,
      58,
      84,
      110,
      0,
      0,
      6,
      30,
      58,
      86,
      114,
      0,
      0,
      6,
      34,
      62,
      90,
      118,
      0,
      0,
      6,
      26,
      50,
      74,
      98,
      122,
      0,
      6,
      30,
      54,
      78,
      102,
      126,
      0,
      6,
      26,
      52,
      78,
      104,
      130,
      0,
      6,
      30,
      56,
      82,
      108,
      134,
      0,
      6,
      34,
      60,
      86,
      112,
      138,
      0,
      6,
      30,
      58,
      86,
      114,
      142,
      0,
      6,
      34,
      62,
      90,
      118,
      146,
      0,
      6,
      30,
      54,
      78,
      102,
      126,
      150,
      6,
      24,
      50,
      76,
      102,
      128,
      154,
      6,
      28,
      54,
      80,
      106,
      132,
      158,
      6,
      32,
      58,
      84,
      110,
      136,
      162,
      6,
      26,
      54,
      82,
      110,
      138,
      166,
      6,
      30,
      58,
      86,
      114,
      142,
      170
    };
    private int[] remainderBits = new int[40]
    {
      0,
      7,
      7,
      7,
      7,
      7,
      0,
      0,
      0,
      0,
      0,
      0,
      0,
      3,
      3,
      3,
      3,
      3,
      3,
      3,
      4,
      4,
      4,
      4,
      4,
      4,
      4,
      3,
      3,
      3,
      3,
      3,
      3,
      3,
      0,
      0,
      0,
      0,
      0,
      0
    };
    private List<QRCodeGenerator.AlignmentPattern> alignmentPatternTable;
    private List<QRCodeGenerator.ECCInfo> capacityECCTable;
    private List<QRCodeGenerator.VersionInfo> capacityTable;
    private List<QRCodeGenerator.Antilog> galoisField;
    private Dictionary<char, int> alphanumEncDict;

    public QRCodeGenerator()
    {
        this.CreateAntilogTable();
        this.CreateAlphanumEncDict();
        this.CreateCapacityTable();
        this.CreateCapacityECCTable();
        this.CreateAlignmentPatternTable();
    }

    public QRCodeGenerator.QRCode CreateQrCode(
      string plainText,
      QRCodeGenerator.ECCLevel eccLevel)
    {
        QRCodeGenerator.EncodingMode encodingFromPlaintext = this.GetEncodingFromPlaintext(plainText);
        int version = this.GetVersion(plainText, encodingFromPlaintext, eccLevel);
        string str1 = this.DecToBin((int)encodingFromPlaintext, 4) + this.DecToBin(plainText.Length, this.GetCountIndicatorLength(version, encodingFromPlaintext)) + this.PlainTextToBinary(plainText, encodingFromPlaintext);
        QRCodeGenerator.ECCInfo eccInfo = this.capacityECCTable.Where<QRCodeGenerator.ECCInfo>((Func<QRCodeGenerator.ECCInfo, bool>)(x => x.Version == version && x.ErrorCorrectionLevel.Equals((object)eccLevel))).Single<QRCodeGenerator.ECCInfo>();
        int length = eccInfo.TotalDataCodewords * 8;
        int val1 = length - str1.Length;
        if (val1 > 0)
            str1 += new string('0', Math.Min(val1, 4));
        if (str1.Length % 8 != 0)
            str1 += new string('0', 8 - str1.Length % 8);
        while (str1.Length < length)
            str1 += "1110110000010001";
        if (str1.Length > length)
            str1 = str1.Substring(0, length);
        List<QRCodeGenerator.CodewordBlock> codewordBlockList = new List<QRCodeGenerator.CodewordBlock>();
        for (int index = 0; index < eccInfo.BlocksInGroup1; ++index)
        {
            string bitString = str1.Substring(index * eccInfo.CodewordsInGroup1 * 8, eccInfo.CodewordsInGroup1 * 8);
            codewordBlockList.Add(new QRCodeGenerator.CodewordBlock()
            {
                BitString = bitString,
                BlockNumber = index + 1,
                GroupNumber = 1,
                CodeWords = this.BinaryStringToBitBlockList(bitString),
                ECCWords = this.CalculateECCWords(bitString, eccInfo)
            });
        }
        string str2 = str1.Substring(eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8);
        for (int index = 0; index < eccInfo.BlocksInGroup2; ++index)
        {
            string bitString = str2.Substring(index * eccInfo.CodewordsInGroup2 * 8, eccInfo.CodewordsInGroup2 * 8);
            codewordBlockList.Add(new QRCodeGenerator.CodewordBlock()
            {
                BitString = bitString,
                BlockNumber = index + 1,
                GroupNumber = 2,
                CodeWords = this.BinaryStringToBitBlockList(bitString),
                ECCWords = this.CalculateECCWords(bitString, eccInfo)
            });
        }
        StringBuilder stringBuilder = new StringBuilder();
        for (int index = 0; index < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); ++index)
        {
            foreach (QRCodeGenerator.CodewordBlock codewordBlock in codewordBlockList)
            {
                if (codewordBlock.CodeWords.Count > index)
                    stringBuilder.Append(codewordBlock.CodeWords[index]);
            }
        }
        for (int index = 0; index < eccInfo.ECCPerBlock; ++index)
        {
            foreach (QRCodeGenerator.CodewordBlock codewordBlock in codewordBlockList)
                stringBuilder.Append(codewordBlock.ECCWords[index]);
        }
        stringBuilder.Append(new string('0', this.remainderBits[version - 1]));
        string data = stringBuilder.ToString();
        QRCodeGenerator.QRCode qrCode = new QRCodeGenerator.QRCode(version);
        List<Rectangle> blockedModules = new List<Rectangle>();
        QRCodeGenerator.ModulePlacer.PlaceFinderPatterns(ref qrCode, ref blockedModules);
        QRCodeGenerator.ModulePlacer.ReserveSeperatorAreas(qrCode.ModuleMatrix.Count, ref blockedModules);
        QRCodeGenerator.ModulePlacer.PlaceAlignmentPatterns(ref qrCode, this.alignmentPatternTable.Where<QRCodeGenerator.AlignmentPattern>((Func<QRCodeGenerator.AlignmentPattern, bool>)(x => x.Version == version)).Select<QRCodeGenerator.AlignmentPattern, List<Point>>((Func<QRCodeGenerator.AlignmentPattern, List<Point>>)(x => x.PatternPositions)).First<List<Point>>(), ref blockedModules);
        QRCodeGenerator.ModulePlacer.PlaceTimingPatterns(ref qrCode, ref blockedModules);
        QRCodeGenerator.ModulePlacer.PlaceDarkModule(ref qrCode, version, ref blockedModules);
        QRCodeGenerator.ModulePlacer.ReserveVersionAreas(qrCode.ModuleMatrix.Count, version, ref blockedModules);
        QRCodeGenerator.ModulePlacer.PlaceDataWords(ref qrCode, data, ref blockedModules);
        int maskVersion = QRCodeGenerator.ModulePlacer.MaskCode(ref qrCode, version, ref blockedModules);
        string formatString = this.GetFormatString(eccLevel, maskVersion);
        QRCodeGenerator.ModulePlacer.PlaceFormat(ref qrCode, formatString);
        if (version >= 7)
        {
            string versionString = this.GetVersionString(version);
            QRCodeGenerator.ModulePlacer.PlaceVersion(ref qrCode, versionString);
        }
        QRCodeGenerator.ModulePlacer.AddQuietZone(ref qrCode);
        return qrCode;
    }

    private string GetFormatString(QRCodeGenerator.ECCLevel level, int maskVersion)
    {
        string str1 = "10100110111";
        string str2 = "101010000010010";
        string str3;
        switch (level)
        {
            case QRCodeGenerator.ECCLevel.L:
                str3 = "01";
                break;
            case QRCodeGenerator.ECCLevel.M:
                str3 = "00";
                break;
            case QRCodeGenerator.ECCLevel.Q:
                str3 = "11";
                break;
            default:
                str3 = "10";
                break;
        }
        string str4 = str3 + this.DecToBin(maskVersion, 3);
        string str5 = str4.PadRight(15, '0');
        char[] chArray1 = new char[1] { '0' };
        string str6;
        string str7;
        char[] chArray2;
        for (str6 = str5.TrimStart(chArray1); str6.Length > 10; str6 = str7.TrimStart(chArray2))
        {
            StringBuilder stringBuilder = new StringBuilder();
            str1 = str1.PadRight(str6.Length, '0');
            for (int index = 0; index < str6.Length; ++index)
                stringBuilder.Append((System.Convert.ToInt32(str6[index]) ^ System.Convert.ToInt32(str1[index])).ToString());
            str7 = stringBuilder.ToString();
            chArray2 = new char[1] { '0' };
        }
        string str8 = str6.PadLeft(10, '0');
        string str9 = str4 + str8;
        StringBuilder stringBuilder1 = new StringBuilder();
        for (int index = 0; index < str9.Length; ++index)
            stringBuilder1.Append((System.Convert.ToInt32(str9[index]) ^ System.Convert.ToInt32(str2[index])).ToString());
        return stringBuilder1.ToString();
    }

    private string GetVersionString(int version)
    {
        string str1 = "1111100100101";
        string bin = this.DecToBin(version, 6);
        string str2 = bin.PadRight(18, '0');
        char[] chArray1 = new char[1] { '0' };
        string str3;
        string str4;
        char[] chArray2;
        for (str3 = str2.TrimStart(chArray1); str3.Length > 12; str3 = str4.TrimStart(chArray2))
        {
            StringBuilder stringBuilder = new StringBuilder();
            str1 = str1.PadRight(str3.Length, '0');
            for (int index = 0; index < str3.Length; ++index)
                stringBuilder.Append((System.Convert.ToInt32(str3[index]) ^ System.Convert.ToInt32(str1[index])).ToString());
            str4 = stringBuilder.ToString();
            chArray2 = new char[1] { '0' };
        }
        string str5 = str3.PadLeft(12, '0');
        return bin + str5;
    }

    private List<string> CalculateECCWords(string bitString, QRCodeGenerator.ECCInfo eccInfo)
    {
        int eccPerBlock = eccInfo.ECCPerBlock;
        QRCodeGenerator.Polynom messagePolynom = this.CalculateMessagePolynom(bitString);
        QRCodeGenerator.Polynom generatorPolynom = this.CalculateGeneratorPolynom(eccPerBlock);
        for (int index = 0; index < messagePolynom.PolyItems.Count; ++index)
            messagePolynom.PolyItems[index] = new QRCodeGenerator.PolynomItem()
            {
                Coefficient = messagePolynom.PolyItems[index].Coefficient,
                Exponent = messagePolynom.PolyItems[index].Exponent + eccPerBlock
            };
        int num = messagePolynom.PolyItems[0].Exponent - generatorPolynom.PolyItems[0].Exponent;
        for (int index = 0; index < generatorPolynom.PolyItems.Count; ++index)
            generatorPolynom.PolyItems[index] = new QRCodeGenerator.PolynomItem()
            {
                Coefficient = generatorPolynom.PolyItems[index].Coefficient,
                Exponent = generatorPolynom.PolyItems[index].Exponent + num
            };
        QRCodeGenerator.Polynom polynom = messagePolynom;
        for (int lowerExponentBy = 0; lowerExponentBy < messagePolynom.PolyItems.Count; ++lowerExponentBy)
        {
            QRCodeGenerator.Polynom decNotation = this.ConvertToDecNotation(this.MultiplyGeneratorPolynomByLeadterm(generatorPolynom, this.ConvertToAlphaNotation(polynom).PolyItems[0], lowerExponentBy));
            polynom = this.XORPolynoms(polynom, decNotation);
        }
        return polynom.PolyItems.Select<QRCodeGenerator.PolynomItem, string>((Func<QRCodeGenerator.PolynomItem, string>)(x => this.DecToBin(x.Coefficient, 8))).ToList<string>();
    }

    private QRCodeGenerator.Polynom ConvertToAlphaNotation(
      QRCodeGenerator.Polynom poly)
    {
        QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
        for (int index = 0; index < poly.PolyItems.Count; ++index)
            polynom.PolyItems.Add(new QRCodeGenerator.PolynomItem()
            {
                Coefficient = poly.PolyItems[index].Coefficient != 0 ? this.GetAlphaExpFromIntVal(poly.PolyItems[index].Coefficient) : 0,
                Exponent = poly.PolyItems[index].Exponent
            });
        return polynom;
    }

    private QRCodeGenerator.Polynom ConvertToDecNotation(QRCodeGenerator.Polynom poly)
    {
        QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
        for (int index = 0; index < poly.PolyItems.Count; ++index)
            polynom.PolyItems.Add(new QRCodeGenerator.PolynomItem()
            {
                Coefficient = this.GetIntValFromAlphaExp(poly.PolyItems[index].Coefficient),
                Exponent = poly.PolyItems[index].Exponent
            });
        return polynom;
    }

    private int GetVersion(
      string plainText,
      QRCodeGenerator.EncodingMode encMode,
      QRCodeGenerator.ECCLevel eccLevel)
    {
        return this.capacityTable.Where<QRCodeGenerator.VersionInfo>((Func<QRCodeGenerator.VersionInfo, bool>)(x => x.Details.Where<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>)(y => y.ErrorCorrectionLevel == eccLevel && y.CapacityDict[encMode] >= System.Convert.ToInt32(plainText.Length))).Count<QRCodeGenerator.VersionInfoDetails>() > 0)).Select(x =>
        {
            var data = new
            {
                version = x.Version,
                capacity = x.Details.Where<QRCodeGenerator.VersionInfoDetails>((Func<QRCodeGenerator.VersionInfoDetails, bool>)(y => y.ErrorCorrectionLevel == eccLevel)).Single<QRCodeGenerator.VersionInfoDetails>().CapacityDict[encMode]
            };
            return data;
        }).Min(x => x.version);
    }

    private QRCodeGenerator.EncodingMode GetEncodingFromPlaintext(string plainText)
    {
        if (plainText.All<char>((Func<char, bool>)(c => "0123456789".Contains<char>(c))))
            return QRCodeGenerator.EncodingMode.Numeric;
        return plainText.All<char>((Func<char, bool>)(c => ((IEnumerable<char>)this.alphanumEncTable).Contains<char>(c))) ? QRCodeGenerator.EncodingMode.Alphanumeric : QRCodeGenerator.EncodingMode.Byte;
    }

    private QRCodeGenerator.Polynom CalculateMessagePolynom(string bitString)
    {
        QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
        for (int index = bitString.Length / 8 - 1; index >= 0; --index)
        {
            polynom.PolyItems.Add(new QRCodeGenerator.PolynomItem()
            {
                Coefficient = this.BinToDec(bitString.Substring(0, 8)),
                Exponent = index
            });
            bitString = bitString.Remove(0, 8);
        }
        return polynom;
    }

    private QRCodeGenerator.Polynom CalculateGeneratorPolynom(int numEccWords)
    {
        QRCodeGenerator.Polynom polynomBase = new QRCodeGenerator.Polynom();
        polynomBase.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>)new QRCodeGenerator.PolynomItem[2]
        {
        new QRCodeGenerator.PolynomItem()
        {
          Coefficient = 0,
          Exponent = 1
        },
        new QRCodeGenerator.PolynomItem()
        {
          Coefficient = 0,
          Exponent = 0
        }
        });
        for (int index = 1; index <= numEccWords - 1; ++index)
        {
            QRCodeGenerator.Polynom polynomMultiplier = new QRCodeGenerator.Polynom();
            polynomMultiplier.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>)new QRCodeGenerator.PolynomItem[2]
            {
          new QRCodeGenerator.PolynomItem()
          {
            Coefficient = 0,
            Exponent = 1
          },
          new QRCodeGenerator.PolynomItem()
          {
            Coefficient = index,
            Exponent = 0
          }
            });
            polynomBase = this.MultiplyAlphaPolynoms(polynomBase, polynomMultiplier);
        }
        return polynomBase;
    }

    private List<string> BinaryStringToBitBlockList(string bitString)
    {
        const int blockSize = 8;
        var numberOfBlocks = (int)Math.Ceiling(bitString.Length / (double)blockSize);
        var blocklist = new List<string>(numberOfBlocks);

        for (int i = 0; i < bitString.Length; i += blockSize)
        {
            blocklist.Add(bitString.Substring(i, blockSize));
        }

        return blocklist;
    }

    private int BinToDec(string binStr)
    {
        return System.Convert.ToInt32(binStr, 2);
    }

    private string DecToBin(int decNum)
    {
        return System.Convert.ToString(decNum, 2);
    }

    private string DecToBin(int decNum, int padLeftUpTo)
    {
        return this.DecToBin(decNum).PadLeft(padLeftUpTo, '0');
    }

    private int GetCountIndicatorLength(int version, QRCodeGenerator.EncodingMode encMode)
    {
        if (version < 10)
        {
            if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Numeric))
                return 10;
            return encMode.Equals((object)QRCodeGenerator.EncodingMode.Alphanumeric) ? 9 : 8;
        }
        if (version < 27)
        {
            if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Numeric))
                return 12;
            if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Alphanumeric))
                return 11;
            return encMode.Equals((object)QRCodeGenerator.EncodingMode.Byte) ? 16 : 10;
        }
        if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Numeric))
            return 14;
        if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Alphanumeric))
            return 13;
        return encMode.Equals((object)QRCodeGenerator.EncodingMode.Byte) ? 16 : 12;
    }

    private bool IsValidISO(string input)
    {
        string b = Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(input));
        return string.Equals(input, b);
    }

    private string PlainTextToBinary(string plainText, QRCodeGenerator.EncodingMode encMode)
    {
        if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Numeric))
            return this.PlainTextToBinaryNumeric(plainText);
        if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Alphanumeric))
            return this.PlainTextToBinaryAlphanumeric(plainText);
        if (encMode.Equals((object)QRCodeGenerator.EncodingMode.Byte))
            return this.PlainTextToBinaryByte(plainText);
        return string.Empty;
    }

    private string PlainTextToBinaryNumeric(string plainText)
    {
        string empty = string.Empty;
        for (; plainText.Length >= 3; plainText = plainText.Substring(3))
        {
            int int32 = System.Convert.ToInt32(plainText.Substring(0, 3));
            empty += this.DecToBin(int32, 10);
        }
        if (plainText.Length == 2)
        {
            int int32 = System.Convert.ToInt32(plainText.Substring(0, plainText.Length));
            empty += this.DecToBin(int32, 7);
        }
        else if (plainText.Length == 1)
        {
            int int32 = System.Convert.ToInt32(plainText.Substring(0, plainText.Length));
            empty += this.DecToBin(int32, 4);
        }
        return empty;
    }

    private string PlainTextToBinaryAlphanumeric(string plainText)
    {
        string empty = string.Empty;
        for (; plainText.Length >= 2; plainText = plainText.Substring(2))
        {
            string str = plainText.Substring(0, 2);
            int decNum = this.alphanumEncDict[str[0]] * 45 + this.alphanumEncDict[str[1]];
            empty += this.DecToBin(decNum, 11);
        }
        if (plainText.Length > 0)
            empty += this.DecToBin(this.alphanumEncDict[plainText[0]], 6);
        return empty;
    }

    private string PlainTextToBinaryByte(string plainText)
    {
        byte[] numArray = new byte[1];
        string empty = string.Empty;
        foreach (byte num in !this.IsValidISO(plainText) ? Encoding.UTF8.GetBytes(plainText) : Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText))
            empty += this.DecToBin((int)num, 8);
        return empty;
    }

    private QRCodeGenerator.Polynom XORPolynoms(
      QRCodeGenerator.Polynom messagePolynom,
      QRCodeGenerator.Polynom resPolynom)
    {
        QRCodeGenerator.Polynom polynom1 = new QRCodeGenerator.Polynom();
        QRCodeGenerator.Polynom polynom2;
        QRCodeGenerator.Polynom polynom3;
        if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
        {
            polynom2 = messagePolynom;
            polynom3 = resPolynom;
        }
        else
        {
            polynom2 = resPolynom;
            polynom3 = messagePolynom;
        }
        for (int index = 0; index < polynom2.PolyItems.Count; ++index)
            polynom1.PolyItems.Add(new QRCodeGenerator.PolynomItem()
            {
                Coefficient = polynom2.PolyItems[index].Coefficient ^ (polynom3.PolyItems.Count > index ? polynom3.PolyItems[index].Coefficient : 0),
                Exponent = messagePolynom.PolyItems[0].Exponent - index
            });
        polynom1.PolyItems.RemoveAt(0);
        return polynom1;
    }

    private QRCodeGenerator.Polynom MultiplyGeneratorPolynomByLeadterm(
      QRCodeGenerator.Polynom genPolynom,
      QRCodeGenerator.PolynomItem leadTerm,
      int lowerExponentBy)
    {
        QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
        foreach (QRCodeGenerator.PolynomItem polyItem in genPolynom.PolyItems)
            polynom.PolyItems.Add(new QRCodeGenerator.PolynomItem()
            {
                Coefficient = (polyItem.Coefficient + leadTerm.Coefficient) % (int)byte.MaxValue,
                Exponent = polyItem.Exponent - lowerExponentBy
            });
        return polynom;
    }

    private QRCodeGenerator.Polynom MultiplyAlphaPolynoms(
      QRCodeGenerator.Polynom polynomBase,
      QRCodeGenerator.Polynom polynomMultiplier)
    {
        QRCodeGenerator.Polynom polynom = new QRCodeGenerator.Polynom();
        foreach (QRCodeGenerator.PolynomItem polyItem1 in polynomMultiplier.PolyItems)
        {
            foreach (QRCodeGenerator.PolynomItem polyItem2 in polynomBase.PolyItems)
                polynom.PolyItems.Add(new QRCodeGenerator.PolynomItem()
                {
                    Coefficient = this.ShrinkAlphaExp(polyItem1.Coefficient + polyItem2.Coefficient),
                    Exponent = polyItem1.Exponent + polyItem2.Exponent
                });
        }
        IEnumerable<int> exponentsToGlue = polynom.PolyItems.GroupBy<QRCodeGenerator.PolynomItem, int>((Func<QRCodeGenerator.PolynomItem, int>)(x => x.Exponent)).Where<IGrouping<int, QRCodeGenerator.PolynomItem>>((Func<IGrouping<int, QRCodeGenerator.PolynomItem>, bool>)(x => x.Count<QRCodeGenerator.PolynomItem>() > 1)).Select<IGrouping<int, QRCodeGenerator.PolynomItem>, int>((Func<IGrouping<int, QRCodeGenerator.PolynomItem>, int>)(x => x.First<QRCodeGenerator.PolynomItem>().Exponent));
        List<QRCodeGenerator.PolynomItem> polynomItemList = new List<QRCodeGenerator.PolynomItem>();
        using (IEnumerator<int> enumerator = exponentsToGlue.GetEnumerator())
        {
            while (enumerator.MoveNext())
            {
                int exponent = enumerator.Current;
                QRCodeGenerator.PolynomItem polynomItem1 = new QRCodeGenerator.PolynomItem();
                polynomItem1.Exponent = exponent;
                int intVal = 0;
                foreach (QRCodeGenerator.PolynomItem polynomItem2 in polynom.PolyItems.Where<QRCodeGenerator.PolynomItem>((Func<QRCodeGenerator.PolynomItem, bool>)(x => x.Exponent == exponent)))
                    intVal ^= this.GetIntValFromAlphaExp(polynomItem2.Coefficient);
                polynomItem1.Coefficient = this.GetAlphaExpFromIntVal(intVal);
                polynomItemList.Add(polynomItem1);
            }
        }
        polynom.PolyItems.RemoveAll((Predicate<QRCodeGenerator.PolynomItem>)(x => exponentsToGlue.Contains<int>(x.Exponent)));
        polynom.PolyItems.AddRange((IEnumerable<QRCodeGenerator.PolynomItem>)polynomItemList);
        polynom.PolyItems = polynom.PolyItems.OrderByDescending<QRCodeGenerator.PolynomItem, int>((Func<QRCodeGenerator.PolynomItem, int>)(x => x.Exponent)).ToList<QRCodeGenerator.PolynomItem>();
        return polynom;
    }

    private int GetIntValFromAlphaExp(int exp)
    {
        return this.galoisField.Where<QRCodeGenerator.Antilog>((Func<QRCodeGenerator.Antilog, bool>)(alog => alog.ExponentAlpha == exp)).Select<QRCodeGenerator.Antilog, int>((Func<QRCodeGenerator.Antilog, int>)(alog => alog.IntegerValue)).First<int>();
    }

    private int GetAlphaExpFromIntVal(int intVal)
    {
        return this.galoisField.Where<QRCodeGenerator.Antilog>((Func<QRCodeGenerator.Antilog, bool>)(alog => alog.IntegerValue == intVal)).Select<QRCodeGenerator.Antilog, int>((Func<QRCodeGenerator.Antilog, int>)(alog => alog.ExponentAlpha)).First<int>();
    }

    private int ShrinkAlphaExp(int alphaExp)
    {
        return (int)((double)(alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
    }

    private void CreateAlphanumEncDict()
    {
        this.alphanumEncDict = new Dictionary<char, int>();
        ((IEnumerable<char>)this.alphanumEncTable).ToList<char>().Select((x, i) =>
        {
            var data = new { Chr = x, Index = i };
            return data;
        }).ToList().ForEach(x => this.alphanumEncDict.Add(x.Chr, x.Index));
    }

    private void CreateAlignmentPatternTable()
    {
        this.alignmentPatternTable = new List<QRCodeGenerator.AlignmentPattern>();
        for (int index1 = 0; index1 < 280; index1 += 7)
        {
            List<Point> pointList = new List<Point>();
            for (int index2 = 0; index2 < 7; ++index2)
            {
                if (this.alignmentPatternBaseValues[index1 + index2] != 0)
                {
                    for (int index3 = 0; index3 < 7; ++index3)
                    {
                        if (this.alignmentPatternBaseValues[index1 + index3] != 0)
                        {
                            Point point = new Point(this.alignmentPatternBaseValues[index1 + index2] - 2, this.alignmentPatternBaseValues[index1 + index3] - 2);
                            if (!pointList.Contains(point))
                                pointList.Add(point);
                        }
                    }
                }
            }
            this.alignmentPatternTable.Add(new QRCodeGenerator.AlignmentPattern()
            {
                Version = (index1 + 7) / 7,
                PatternPositions = pointList
            });
        }
    }

    private void CreateCapacityECCTable()
    {
        this.capacityECCTable = new List<QRCodeGenerator.ECCInfo>();
        for (int index = 0; index < 960; index += 24)
            this.capacityECCTable.AddRange((IEnumerable<QRCodeGenerator.ECCInfo>)new QRCodeGenerator.ECCInfo[4]
            {
          new QRCodeGenerator.ECCInfo()
          {
            Version = (index + 24) / 24,
            ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.L,
            TotalDataCodewords = this.capacityECCBaseValues[index],
            ECCPerBlock = this.capacityECCBaseValues[index + 1],
            BlocksInGroup1 = this.capacityECCBaseValues[index + 2],
            CodewordsInGroup1 = this.capacityECCBaseValues[index + 3],
            BlocksInGroup2 = this.capacityECCBaseValues[index + 4],
            CodewordsInGroup2 = this.capacityECCBaseValues[index + 5]
          },
          new QRCodeGenerator.ECCInfo()
          {
            Version = (index + 24) / 24,
            ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.M,
            TotalDataCodewords = this.capacityECCBaseValues[index + 6],
            ECCPerBlock = this.capacityECCBaseValues[index + 7],
            BlocksInGroup1 = this.capacityECCBaseValues[index + 8],
            CodewordsInGroup1 = this.capacityECCBaseValues[index + 9],
            BlocksInGroup2 = this.capacityECCBaseValues[index + 10],
            CodewordsInGroup2 = this.capacityECCBaseValues[index + 11]
          },
          new QRCodeGenerator.ECCInfo()
          {
            Version = (index + 24) / 24,
            ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.Q,
            TotalDataCodewords = this.capacityECCBaseValues[index + 12],
            ECCPerBlock = this.capacityECCBaseValues[index + 13],
            BlocksInGroup1 = this.capacityECCBaseValues[index + 14],
            CodewordsInGroup1 = this.capacityECCBaseValues[index + 15],
            BlocksInGroup2 = this.capacityECCBaseValues[index + 16],
            CodewordsInGroup2 = this.capacityECCBaseValues[index + 17]
          },
          new QRCodeGenerator.ECCInfo()
          {
            Version = (index + 24) / 24,
            ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.H,
            TotalDataCodewords = this.capacityECCBaseValues[index + 18],
            ECCPerBlock = this.capacityECCBaseValues[index + 19],
            BlocksInGroup1 = this.capacityECCBaseValues[index + 20],
            CodewordsInGroup1 = this.capacityECCBaseValues[index + 21],
            BlocksInGroup2 = this.capacityECCBaseValues[index + 22],
            CodewordsInGroup2 = this.capacityECCBaseValues[index + 23]
          }
            });
    }

    private void CreateCapacityTable()
    {
        this.capacityTable = new List<QRCodeGenerator.VersionInfo>();
        for (int index = 0; index < 640; index += 16)
            this.capacityTable.Add(new QRCodeGenerator.VersionInfo()
            {
                Version = (index + 16) / 16,
                Details = new List<QRCodeGenerator.VersionInfoDetails>()
          {
            new QRCodeGenerator.VersionInfoDetails()
            {
              ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.L,
              CapacityDict = new Dictionary<QRCodeGenerator.EncodingMode, int>()
              {
                {
                  QRCodeGenerator.EncodingMode.Numeric,
                  this.capacityBaseValues[index]
                },
                {
                  QRCodeGenerator.EncodingMode.Alphanumeric,
                  this.capacityBaseValues[index + 1]
                },
                {
                  QRCodeGenerator.EncodingMode.Byte,
                  this.capacityBaseValues[index + 2]
                },
                {
                  QRCodeGenerator.EncodingMode.Kanji,
                  this.capacityBaseValues[index + 3]
                }
              }
            },
            new QRCodeGenerator.VersionInfoDetails()
            {
              ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.M,
              CapacityDict = new Dictionary<QRCodeGenerator.EncodingMode, int>()
              {
                {
                  QRCodeGenerator.EncodingMode.Numeric,
                  this.capacityBaseValues[index + 4]
                },
                {
                  QRCodeGenerator.EncodingMode.Alphanumeric,
                  this.capacityBaseValues[index + 5]
                },
                {
                  QRCodeGenerator.EncodingMode.Byte,
                  this.capacityBaseValues[index + 6]
                },
                {
                  QRCodeGenerator.EncodingMode.Kanji,
                  this.capacityBaseValues[index + 7]
                }
              }
            },
            new QRCodeGenerator.VersionInfoDetails()
            {
              ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.Q,
              CapacityDict = new Dictionary<QRCodeGenerator.EncodingMode, int>()
              {
                {
                  QRCodeGenerator.EncodingMode.Numeric,
                  this.capacityBaseValues[index + 8]
                },
                {
                  QRCodeGenerator.EncodingMode.Alphanumeric,
                  this.capacityBaseValues[index + 9]
                },
                {
                  QRCodeGenerator.EncodingMode.Byte,
                  this.capacityBaseValues[index + 10]
                },
                {
                  QRCodeGenerator.EncodingMode.Kanji,
                  this.capacityBaseValues[index + 11]
                }
              }
            },
            new QRCodeGenerator.VersionInfoDetails()
            {
              ErrorCorrectionLevel = QRCodeGenerator.ECCLevel.H,
              CapacityDict = new Dictionary<QRCodeGenerator.EncodingMode, int>()
              {
                {
                  QRCodeGenerator.EncodingMode.Numeric,
                  this.capacityBaseValues[index + 12]
                },
                {
                  QRCodeGenerator.EncodingMode.Alphanumeric,
                  this.capacityBaseValues[index + 13]
                },
                {
                  QRCodeGenerator.EncodingMode.Byte,
                  this.capacityBaseValues[index + 14]
                },
                {
                  QRCodeGenerator.EncodingMode.Kanji,
                  this.capacityBaseValues[index + 15]
                }
              }
            }
          }
            });
    }

    private void CreateAntilogTable()
    {
        this.galoisField = new List<QRCodeGenerator.Antilog>();
        for (int index = 0; index < 256; ++index)
        {
            int num = (int)Math.Pow(2.0, (double)index);
            if (index > 7)
                num = this.galoisField[index - 1].IntegerValue * 2;
            if (num > (int)byte.MaxValue)
                num ^= 285;
            this.galoisField.Add(new QRCodeGenerator.Antilog()
            {
                ExponentAlpha = index,
                IntegerValue = num
            });
        }
    }

    private static class ModulePlacer
    {
        public static void AddQuietZone(ref QRCodeGenerator.QRCode qrCode)
        {
            bool[] values = new bool[qrCode.ModuleMatrix.Count + 8];
            for (int index = 0; index < values.Length; ++index)
                values[index] = false;
            for (int index = 0; index < 4; ++index)
                qrCode.ModuleMatrix.Insert(0, new BitArray(values));
            for (int index = 0; index < 4; ++index)
                qrCode.ModuleMatrix.Add(new BitArray(values));
            for (int index = 4; index < qrCode.ModuleMatrix.Count - 4; ++index)
            {
                bool[] flagArray = new bool[4];
                List<bool> boolList = new List<bool>((IEnumerable<bool>)flagArray);
                foreach (bool flag in qrCode.ModuleMatrix[index])
                    boolList.Add(flag);
                boolList.AddRange((IEnumerable<bool>)flagArray);
                qrCode.ModuleMatrix[index] = new BitArray(boolList.ToArray());
            }
        }

        public static void PlaceVersion(ref QRCodeGenerator.QRCode qrCode, string versionStr)
        {
            int count = qrCode.ModuleMatrix.Count;
            string str = new string(versionStr.Reverse<char>().ToArray<char>());
            for (int index1 = 0; index1 < 6; ++index1)
            {
                for (int index2 = 0; index2 < 3; ++index2)
                {
                    qrCode.ModuleMatrix[index2 + count - 11][index1] = str[index1 * 3 + index2] == '1';
                    qrCode.ModuleMatrix[index1][index2 + count - 11] = str[index1 * 3 + index2] == '1';
                }
            }
        }

        public static void PlaceFormat(ref QRCodeGenerator.QRCode qrCode, string formatStr)
        {
            int count = qrCode.ModuleMatrix.Count;
            string str = new string(formatStr.Reverse<char>().ToArray<char>());
            int[,] numArray = new int[15, 4]
            {
          {
            8,
            0,
            count - 1,
            8
          },
          {
            8,
            1,
            count - 2,
            8
          },
          {
            8,
            2,
            count - 3,
            8
          },
          {
            8,
            3,
            count - 4,
            8
          },
          {
            8,
            4,
            count - 5,
            8
          },
          {
            8,
            5,
            count - 6,
            8
          },
          {
            8,
            7,
            count - 7,
            8
          },
          {
            8,
            8,
            count - 8,
            8
          },
          {
            7,
            8,
            8,
            count - 7
          },
          {
            5,
            8,
            8,
            count - 6
          },
          {
            4,
            8,
            8,
            count - 5
          },
          {
            3,
            8,
            8,
            count - 4
          },
          {
            2,
            8,
            8,
            count - 3
          },
          {
            1,
            8,
            8,
            count - 2
          },
          {
            0,
            8,
            8,
            count - 1
          }
            };
            for (int index = 0; index < 15; ++index)
            {
                Point point1 = new Point(numArray[index, 0], numArray[index, 1]);
                Point point2 = new Point(numArray[index, 2], numArray[index, 3]);
                qrCode.ModuleMatrix[point1.Y][point1.X] = str[index] == '1';
                qrCode.ModuleMatrix[point2.Y][point2.X] = str[index] == '1';
            }
        }

        public static int MaskCode(
          ref QRCodeGenerator.QRCode qrCode,
          int version,
          ref List<Rectangle> blockedModules)
        {
            string patternName = string.Empty;
            int num1 = 0;
            int count = qrCode.ModuleMatrix.Count;
            foreach (MethodInfo method in typeof(QRCodeGenerator.ModulePlacer.MaskPattern).GetMethods())
            {
                if (method.Name.Length == 8 && method.Name.Substring(0, 7) == "Pattern")
                {
                    QRCodeGenerator.QRCode qrCode1 = new QRCodeGenerator.QRCode(version);
                    for (int index1 = 0; index1 < count; ++index1)
                    {
                        for (int index2 = 0; index2 < count; ++index2)
                            qrCode1.ModuleMatrix[index1][index2] = qrCode.ModuleMatrix[index1][index2];
                    }
                    for (int x = 0; x < count; ++x)
                    {
                        for (int y = 0; y < count; ++y)
                        {
                            if (!QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            {
                                BitArray bitArray;
                                int index;
                                int num2 = ((bitArray = qrCode1.ModuleMatrix[y])[index = x] ? 1 : 0) ^ ((bool)method.Invoke((object)null, new object[2]
                                {
                    (object) x,
                    (object) y
                                }) ? 1 : 0);
                                bitArray[index] = num2 != 0;
                            }
                        }
                    }
                    int num3 = QRCodeGenerator.ModulePlacer.MaskPattern.Score(ref qrCode1);
                    if (string.IsNullOrEmpty(patternName) || num1 > num3)
                    {
                        patternName = method.Name;
                        num1 = num3;
                    }
                }
            }
            MethodInfo methodInfo = ((IEnumerable<MethodInfo>)typeof(QRCodeGenerator.ModulePlacer.MaskPattern).GetMethods()).Where<MethodInfo>((Func<MethodInfo, bool>)(x => x.Name == patternName)).First<MethodInfo>();
            for (int x = 0; x < count; ++x)
            {
                for (int y = 0; y < count; ++y)
                {
                    if (!QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                    {
                        BitArray bitArray;
                        int index;
                        int num2 = ((bitArray = qrCode.ModuleMatrix[y])[index = x] ? 1 : 0) ^ ((bool)methodInfo.Invoke((object)null, new object[2]
                        {
                (object) x,
                (object) y
                        }) ? 1 : 0);
                        bitArray[index] = num2 != 0;
                    }
                }
            }
            return System.Convert.ToInt32(methodInfo.Name.Substring(methodInfo.Name.Length - 1, 1)) - 1;
        }

        public static void PlaceDataWords(
          ref QRCodeGenerator.QRCode qrCode,
          string data,
          ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            bool flag = true;
            Queue<bool> datawords = new Queue<bool>();
            data.ToList<char>().ForEach((Action<char>)(x => datawords.Enqueue(x != '0')));
            for (int x = count - 1; x >= 0; x -= 2)
            {
                if (x == 7 || x == 6)
                    x = 5;
                for (int index = 1; index <= count; ++index)
                {
                    if (flag)
                    {
                        int y = count - index;
                        if (datawords.Count > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x] = datawords.Dequeue();
                        if (datawords.Count > 0 && x > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x - 1] = datawords.Dequeue();
                    }
                    else
                    {
                        int y = index - 1;
                        if (datawords.Count > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x] = datawords.Dequeue();
                        if (datawords.Count > 0 && x > 0 && !QRCodeGenerator.ModulePlacer.IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x - 1] = datawords.Dequeue();
                    }
                }
                flag = !flag;
            }
        }

        public static void ReserveSeperatorAreas(int size, ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange((IEnumerable<Rectangle>)new Rectangle[6]
            {
          new Rectangle(7, 0, 1, 8),
          new Rectangle(0, 7, 7, 1),
          new Rectangle(0, size - 8, 8, 1),
          new Rectangle(7, size - 7, 1, 7),
          new Rectangle(size - 8, 0, 1, 8),
          new Rectangle(size - 7, 7, 7, 1)
            });
        }

        public static void ReserveVersionAreas(
          int size,
          int version,
          ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange((IEnumerable<Rectangle>)new Rectangle[6]
            {
          new Rectangle(8, 0, 1, 6),
          new Rectangle(8, 7, 1, 1),
          new Rectangle(0, 8, 6, 1),
          new Rectangle(7, 8, 2, 1),
          new Rectangle(size - 8, 8, 8, 1),
          new Rectangle(8, size - 7, 1, 7)
            });
            if (version < 7)
                return;
            blockedModules.AddRange((IEnumerable<Rectangle>)new Rectangle[2]
            {
          new Rectangle(size - 11, 0, 3, 6),
          new Rectangle(0, size - 11, 6, 3)
            });
        }

        public static void PlaceDarkModule(
          ref QRCodeGenerator.QRCode qrCode,
          int version,
          ref List<Rectangle> blockedModules)
        {
            qrCode.ModuleMatrix[4 * version + 9][8] = true;
            blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
        }

        public static void PlaceFinderPatterns(
          ref QRCodeGenerator.QRCode qrCode,
          ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            int[] numArray = new int[6]
            {
          0,
          0,
          count - 7,
          0,
          0,
          count - 7
            };
            for (int index1 = 0; index1 < 6; index1 += 2)
            {
                for (int index2 = 0; index2 < 7; ++index2)
                {
                    for (int index3 = 0; index3 < 7; ++index3)
                    {
                        if ((index2 != 1 && index2 != 5 || (index3 <= 0 || index3 >= 6)) && (index2 <= 0 || index2 >= 6 || index3 != 1 && index3 != 5))
                            qrCode.ModuleMatrix[index3 + numArray[index1 + 1]][index2 + numArray[index1]] = true;
                    }
                }
                blockedModules.Add(new Rectangle(numArray[index1], numArray[index1 + 1], 7, 7));
            }
        }

        public static void PlaceAlignmentPatterns(
          ref QRCodeGenerator.QRCode qrCode,
          List<Point> alignmentPatternLocations,
          ref List<Rectangle> blockedModules)
        {
            foreach (Point alignmentPatternLocation in alignmentPatternLocations)
            {
                Rectangle r1 = new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5);
                bool flag = false;
                foreach (Rectangle r2 in blockedModules)
                {
                    if (QRCodeGenerator.ModulePlacer.Intersects(r1, r2))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    for (int index1 = 0; index1 < 5; ++index1)
                    {
                        for (int index2 = 0; index2 < 5; ++index2)
                        {
                            int num;
                            if (index2 != 0 && index2 != 4)
                            {
                                switch (index1)
                                {
                                    case 0:
                                    case 4:
                                        break;
                                    case 2:
                                        num = index2 != 2 ? 1 : 0;
                                        goto label_16;
                                    default:
                                        num = 1;
                                        goto label_16;
                                }
                            }
                            num = 0;
                        label_16:
                            if (num == 0)
                                qrCode.ModuleMatrix[alignmentPatternLocation.Y + index2][alignmentPatternLocation.X + index1] = true;
                        }
                    }
                    blockedModules.Add(new Rectangle(alignmentPatternLocation.X, alignmentPatternLocation.Y, 5, 5));
                }
            }
        }

        public static void PlaceTimingPatterns(
          ref QRCodeGenerator.QRCode qrCode,
          ref List<Rectangle> blockedModules)
        {
            int count = qrCode.ModuleMatrix.Count;
            for (int index = 8; index < count - 8; ++index)
            {
                if (index % 2 == 0)
                {
                    qrCode.ModuleMatrix[6][index] = true;
                    qrCode.ModuleMatrix[index][6] = true;
                }
            }
            blockedModules.AddRange((IEnumerable<Rectangle>)new Rectangle[2]
            {
          new Rectangle(6, 8, 1, count - 16),
          new Rectangle(8, 6, count - 16, 1)
            });
        }

        private static bool Intersects(Rectangle r1, Rectangle r2)
        {
            return r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height;
        }

        private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
        {
            bool flag = false;
            foreach (Rectangle blockedModule in blockedModules)
            {
                if (QRCodeGenerator.ModulePlacer.Intersects(blockedModule, r1))
                    flag = true;
            }
            return flag;
        }

        private static class MaskPattern
        {
            public static bool Pattern1(int x, int y)
            {
                return (x + y) % 2 == 0;
            }

            public static bool Pattern2(int x, int y)
            {
                return y % 2 == 0;
            }

            public static bool Pattern3(int x, int y)
            {
                return x % 3 == 0;
            }

            public static bool Pattern4(int x, int y)
            {
                return (x + y) % 3 == 0;
            }

            public static bool Pattern5(int x, int y)
            {
                return (y / 2 + x / 3) % 2 == 0;
            }

            public static bool Pattern6(int x, int y)
            {
                return x * y % 2 + x * y % 3 == 0;
            }

            public static bool Pattern7(int x, int y)
            {
                return (x * y % 2 + x * y % 3) % 2 == 0;
            }

            public static bool Pattern8(int x, int y)
            {
                return ((x + y) % 2 + x * y % 3) % 2 == 0;
            }

            public static int Score(ref QRCodeGenerator.QRCode qrCode)
            {
                int num1 = 0;
                int count = qrCode.ModuleMatrix.Count;
                for (int index1 = 0; index1 < count; ++index1)
                {
                    int num2 = 0;
                    int num3 = 0;
                    bool flag1 = qrCode.ModuleMatrix[index1][0];
                    bool flag2 = qrCode.ModuleMatrix[0][index1];
                    for (int index2 = 0; index2 < count; ++index2)
                    {
                        if (qrCode.ModuleMatrix[index1][index2] == flag1)
                            ++num2;
                        else
                            num2 = 1;
                        if (num2 == 5)
                            num1 += 3;
                        else if (num2 > 5)
                            ++num1;
                        flag1 = qrCode.ModuleMatrix[index1][index2];
                        if (qrCode.ModuleMatrix[index2][index1] == flag2)
                            ++num3;
                        else
                            num3 = 1;
                        if (num3 == 5)
                            num1 += 3;
                        else if (num3 > 5)
                            ++num1;
                        flag2 = qrCode.ModuleMatrix[index2][index1];
                    }
                }
                for (int index1 = 0; index1 < count - 1; ++index1)
                {
                    for (int index2 = 0; index2 < count - 1; ++index2)
                    {
                        if (qrCode.ModuleMatrix[index1][index2] == qrCode.ModuleMatrix[index1][index2 + 1] && qrCode.ModuleMatrix[index1][index2] == qrCode.ModuleMatrix[index1 + 1][index2] && qrCode.ModuleMatrix[index1][index2] == qrCode.ModuleMatrix[index1 + 1][index2 + 1])
                            num1 += 3;
                    }
                }
                for (int index1 = 0; index1 < count; ++index1)
                {
                    for (int index2 = 0; index2 < count - 10; ++index2)
                    {
                        if (qrCode.ModuleMatrix[index1][index2] && !qrCode.ModuleMatrix[index1][index2 + 1] && (qrCode.ModuleMatrix[index1][index2 + 2] && qrCode.ModuleMatrix[index1][index2 + 3]) && (qrCode.ModuleMatrix[index1][index2 + 4] && !qrCode.ModuleMatrix[index1][index2 + 5] && (qrCode.ModuleMatrix[index1][index2 + 6] && !qrCode.ModuleMatrix[index1][index2 + 7])) && (!qrCode.ModuleMatrix[index1][index2 + 8] && !qrCode.ModuleMatrix[index1][index2 + 9] && !qrCode.ModuleMatrix[index1][index2 + 10]) || !qrCode.ModuleMatrix[index1][index2] && !qrCode.ModuleMatrix[index1][index2 + 1] && (!qrCode.ModuleMatrix[index1][index2 + 2] && !qrCode.ModuleMatrix[index1][index2 + 3]) && (qrCode.ModuleMatrix[index1][index2 + 4] && !qrCode.ModuleMatrix[index1][index2 + 5] && (qrCode.ModuleMatrix[index1][index2 + 6] && qrCode.ModuleMatrix[index1][index2 + 7])) && (qrCode.ModuleMatrix[index1][index2 + 8] && !qrCode.ModuleMatrix[index1][index2 + 9]) && qrCode.ModuleMatrix[index1][index2 + 10])
                            num1 += 40;
                        if (qrCode.ModuleMatrix[index2][index1] && !qrCode.ModuleMatrix[index2 + 1][index1] && (qrCode.ModuleMatrix[index2 + 2][index1] && qrCode.ModuleMatrix[index2 + 3][index1]) && (qrCode.ModuleMatrix[index2 + 4][index1] && !qrCode.ModuleMatrix[index2 + 5][index1] && (qrCode.ModuleMatrix[index2 + 6][index1] && !qrCode.ModuleMatrix[index2 + 7][index1])) && (!qrCode.ModuleMatrix[index2 + 8][index1] && !qrCode.ModuleMatrix[index2 + 9][index1] && !qrCode.ModuleMatrix[index2 + 10][index1]) || !qrCode.ModuleMatrix[index2][index2] && !qrCode.ModuleMatrix[index2 + 1][index1] && (!qrCode.ModuleMatrix[index2 + 2][index1] && !qrCode.ModuleMatrix[index2 + 3][index1]) && (qrCode.ModuleMatrix[index2 + 4][index1] && !qrCode.ModuleMatrix[index2 + 5][index1] && (qrCode.ModuleMatrix[index2 + 6][index1] && qrCode.ModuleMatrix[index2 + 7][index1])) && (qrCode.ModuleMatrix[index2 + 8][index1] && !qrCode.ModuleMatrix[index2 + 9][index1]) && qrCode.ModuleMatrix[index2 + 10][index1])
                            num1 += 40;
                    }
                }
                int num4 = 0;
                foreach (BitArray bitArray in qrCode.ModuleMatrix)
                {
                    foreach (bool flag in bitArray)
                    {
                        if (flag)
                            ++num4;
                    }
                }
                int num5 = num4 / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count) * 100;
                return num5 % 5 != 0 ? num1 + Math.Min(Math.Abs((int)Math.Floor((Decimal)num5 / new Decimal(5)) - 50) / 5, Math.Abs((int)Math.Floor((Decimal)num5 / new Decimal(5)) + 5 - 50) / 5) * 10 : num1 + Math.Min(Math.Abs(num5 - 55) / 5, Math.Abs(num5 - 45) / 5) * 10;
            }
        }
    }

    public enum ECCLevel
    {
        L,
        M,
        Q,
        H,
    }

    private enum EncodingMode
    {
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 4,
        ECI = 7,
        Kanji = 8,
    }

    private struct AlignmentPattern
    {
        public int Version;
        public List<Point> PatternPositions;
    }

    private struct CodewordBlock
    {
        public int GroupNumber;
        public int BlockNumber;
        public string BitString;
        public List<string> CodeWords;
        public List<string> ECCWords;
    }

    private struct ECCInfo
    {
        public int Version;
        public QRCodeGenerator.ECCLevel ErrorCorrectionLevel;
        public int TotalDataCodewords;
        public int ECCPerBlock;
        public int BlocksInGroup1;
        public int CodewordsInGroup1;
        public int BlocksInGroup2;
        public int CodewordsInGroup2;
    }

    private struct VersionInfo
    {
        public int Version;
        public List<QRCodeGenerator.VersionInfoDetails> Details;
    }

    private struct VersionInfoDetails
    {
        public QRCodeGenerator.ECCLevel ErrorCorrectionLevel;
        public Dictionary<QRCodeGenerator.EncodingMode, int> CapacityDict;
    }

    private struct Antilog
    {
        public int ExponentAlpha;
        public int IntegerValue;
    }

    private struct PolynomItem
    {
        public int Coefficient;
        public int Exponent;
    }

    private class Polynom
    {
        public Polynom()
        {
            this.PolyItems = new List<QRCodeGenerator.PolynomItem>();
        }

        public List<QRCodeGenerator.PolynomItem> PolyItems { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            this.PolyItems.ForEach((Action<QRCodeGenerator.PolynomItem>)(x => sb.Append("a^" + (object)x.Coefficient + "*x^" + (object)x.Exponent + " + ")));
            return sb.ToString().TrimEnd(' ', '+');
        }
    }

    public class QRCode
    {
        private int version;

        public List<BitArray> ModuleMatrix { get; set; }

        public QRCode(int version)
        {
            this.version = version;
            int length = this.ModulesPerSideFromVersion(version);
            this.ModuleMatrix = new List<BitArray>();
            for (int index = 0; index < length; ++index)
                this.ModuleMatrix.Add(new BitArray(length));
        }

        public Bitmap GetGraphic(int pixelsPerModule)
        {
            int num = this.ModuleMatrix.Count * pixelsPerModule;
            Bitmap bitmap = new Bitmap(num, num);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            for (int x = 0; x < num; x += pixelsPerModule)
            {
                for (int y = 0; y < num; y += pixelsPerModule)
                {
                    if (this.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1])
                        graphics.FillRectangle(Brushes.Black, new Rectangle(x, y, pixelsPerModule, pixelsPerModule));
                    else
                        graphics.FillRectangle(Brushes.White, new Rectangle(x, y, pixelsPerModule, pixelsPerModule));
                }
            }
            graphics.Save();
            return bitmap;
        }

        private int ModulesPerSideFromVersion(int version)
        {
            return 21 + (version - 1) * 4;
        }
    }
}