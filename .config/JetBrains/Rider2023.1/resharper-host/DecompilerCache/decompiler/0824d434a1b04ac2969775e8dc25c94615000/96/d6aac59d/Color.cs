// Decompiled with JetBrains decompiler
// Type: Raylib_cs.Color
// Assembly: Raylib-cs, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0824D434-A1B0-4AC2-9697-75E8DC25C946
// Assembly location: /home/cadenz/.nuget/packages/raylib-cs/4.5.0.1/lib/net6.0/Raylib-cs.dll
// XML documentation location: /home/cadenz/.nuget/packages/raylib-cs/4.5.0.1/lib/net6.0/Raylib-cs.xml

using System;
using System.Runtime.CompilerServices;

namespace Raylib_cs
{
  /// <summary>Color type, RGBA (32bit)</summary>
  public struct Color
  {
    public byte r;
    public byte g;
    public byte b;
    public byte a;
    public static readonly Color LIGHTGRAY = new Color(200, 200, 200, (int) byte.MaxValue);
    public static readonly Color GRAY = new Color(130, 130, 130, (int) byte.MaxValue);
    public static readonly Color DARKGRAY = new Color(80, 80, 80, (int) byte.MaxValue);
    public static readonly Color YELLOW = new Color(253, 249, 0, (int) byte.MaxValue);
    public static readonly Color GOLD = new Color((int) byte.MaxValue, 203, 0, (int) byte.MaxValue);
    public static readonly Color ORANGE = new Color((int) byte.MaxValue, 161, 0, (int) byte.MaxValue);
    public static readonly Color PINK = new Color((int) byte.MaxValue, 109, 194, (int) byte.MaxValue);
    public static readonly Color RED = new Color(230, 41, 55, (int) byte.MaxValue);
    public static readonly Color MAROON = new Color(190, 33, 55, (int) byte.MaxValue);
    public static readonly Color GREEN = new Color(0, 228, 48, (int) byte.MaxValue);
    public static readonly Color LIME = new Color(0, 158, 47, (int) byte.MaxValue);
    public static readonly Color DARKGREEN = new Color(0, 117, 44, (int) byte.MaxValue);
    public static readonly Color SKYBLUE = new Color(102, 191, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color BLUE = new Color(0, 121, 241, (int) byte.MaxValue);
    public static readonly Color DARKBLUE = new Color(0, 82, 172, (int) byte.MaxValue);
    public static readonly Color PURPLE = new Color(200, 122, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color VIOLET = new Color(135, 60, 190, (int) byte.MaxValue);
    public static readonly Color DARKPURPLE = new Color(112, 31, 126, (int) byte.MaxValue);
    public static readonly Color BEIGE = new Color(211, 176, 131, (int) byte.MaxValue);
    public static readonly Color BROWN = new Color((int) sbyte.MaxValue, 106, 79, (int) byte.MaxValue);
    public static readonly Color DARKBROWN = new Color(76, 63, 47, (int) byte.MaxValue);
    public static readonly Color WHITE = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color BLACK = new Color(0, 0, 0, (int) byte.MaxValue);
    public static readonly Color BLANK = new Color(0, 0, 0, 0);
    public static readonly Color MAGENTA = new Color((int) byte.MaxValue, 0, (int) byte.MaxValue, (int) byte.MaxValue);
    public static readonly Color RAYWHITE = new Color(245, 245, 245, (int) byte.MaxValue);

    public Color(byte r, byte g, byte b, byte a)
    {
      this.r = r;
      this.g = g;
      this.b = b;
      this.a = a;
    }

    public Color(int r, int g, int b, int a)
    {
      this.r = Convert.ToByte(r);
      this.g = Convert.ToByte(g);
      this.b = Convert.ToByte(b);
      this.a = Convert.ToByte(a);
    }

    public override string ToString()
    {
      DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 4);
      interpolatedStringHandler.AppendLiteral("{R:");
      interpolatedStringHandler.AppendFormatted<byte>(this.r);
      interpolatedStringHandler.AppendLiteral(" G:");
      interpolatedStringHandler.AppendFormatted<byte>(this.g);
      interpolatedStringHandler.AppendLiteral(" B:");
      interpolatedStringHandler.AppendFormatted<byte>(this.b);
      interpolatedStringHandler.AppendLiteral(" A:");
      interpolatedStringHandler.AppendFormatted<byte>(this.a);
      interpolatedStringHandler.AppendLiteral("}");
      return interpolatedStringHandler.ToStringAndClear();
    }
  }
}
