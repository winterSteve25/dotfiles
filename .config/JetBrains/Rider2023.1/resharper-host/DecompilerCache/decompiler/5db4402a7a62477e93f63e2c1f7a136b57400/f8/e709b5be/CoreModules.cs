// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.CoreModules
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using System;

namespace MoonSharp.Interpreter
{
  /// <summary>
  /// Enumeration (combinable as flags) of all the standard library modules
  /// </summary>
  [Flags]
  public enum CoreModules
  {
    /// <summary>
    /// Value used to specify no modules to be loaded (equals 0).
    /// </summary>
    None = 0,
    /// <summary>
    /// The basic methods. Includes "assert", "collectgarbage", "error", "print", "select", "type", "tonumber" and "tostring".
    /// </summary>
    Basic = 64, // 0x00000040
    /// <summary>
    /// The global constants: "_G", "_VERSION" and "_MOONSHARP".
    /// </summary>
    GlobalConsts = 1,
    /// <summary>The table iterators: "next", "ipairs" and "pairs".</summary>
    TableIterators = 2,
    /// <summary>
    /// The metatable methods : "setmetatable", "getmetatable", "rawset", "rawget", "rawequal" and "rawlen".
    /// </summary>
    Metatables = 4,
    /// <summary>The string package</summary>
    String = 8,
    /// <summary>
    /// The load methods: "load", "loadsafe", "loadfile", "loadfilesafe", "dofile" and "require"
    /// </summary>
    LoadMethods = 16, // 0x00000010
    /// <summary>The table package</summary>
    Table = 32, // 0x00000020
    /// <summary>The error handling methods: "pcall" and "xpcall"</summary>
    ErrorHandling = 128, // 0x00000080
    /// <summary>The math package</summary>
    Math = 256, // 0x00000100
    /// <summary>The coroutine package</summary>
    Coroutine = 512, // 0x00000200
    /// <summary>The bit32 package</summary>
    Bit32 = 1024, // 0x00000400
    /// <summary>
    /// The time methods of the "os" package: "clock", "difftime", "date" and "time"
    /// </summary>
    OS_Time = 2048, // 0x00000800
    /// <summary>
    /// The methods of "os" package excluding those listed for OS_Time. These are not supported under Unity.
    /// </summary>
    OS_System = 4096, // 0x00001000
    /// <summary>
    /// The methods of "io" and "file" packages. These are not supported under Unity.
    /// </summary>
    IO = 8192, // 0x00002000
    /// <summary>The "debug" package (it has limited support)</summary>
    Debug = 16384, // 0x00004000
    /// <summary>The "dynamic" package (introduced by MoonSharp).</summary>
    Dynamic = 32768, // 0x00008000
    /// <summary>The "json" package (introduced by MoonSharp).</summary>
    Json = 65536, // 0x00010000
    /// <summary>
    /// A sort of "hard" sandbox preset, including string, math, table, bit32 packages, constants and table iterators.
    /// </summary>
    Preset_HardSandbox = Bit32 | Math | Table | String | TableIterators | GlobalConsts | Basic, // 0x0000056B
    /// <summary>
    /// A softer sandbox preset, adding metatables support, error handling, coroutine, time functions, json parsing and dynamic evaluations.
    /// </summary>
    Preset_SoftSandbox = Preset_HardSandbox | Json | Dynamic | OS_Time | Coroutine | ErrorHandling | Metatables, // 0x00018FEF
    /// <summary>
    /// The default preset. Includes everything except "debug" as now.
    /// Beware that using this preset allows scripts unlimited access to the system.
    /// </summary>
    Preset_Default = Preset_SoftSandbox | IO | OS_System | LoadMethods, // 0x0001BFFF
    /// <summary>
    /// The complete package.
    /// Beware that using this preset allows scripts unlimited access to the system.
    /// </summary>
    Preset_Complete = Preset_Default | Debug, // 0x0001FFFF
  }
}
