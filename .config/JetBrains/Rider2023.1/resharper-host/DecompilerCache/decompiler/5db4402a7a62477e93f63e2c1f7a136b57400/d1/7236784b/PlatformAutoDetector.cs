// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Platforms.PlatformAutoDetector
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Loaders;
using System;
using System.Linq.Expressions;

namespace MoonSharp.Interpreter.Platforms
{
  /// <summary>
  /// A static class offering properties for autodetection of system/platform details
  /// </summary>
  public static class PlatformAutoDetector
  {
    private static bool? m_IsRunningOnAOT;
    private static bool m_AutoDetectionsDone;

    /// <summary>
    /// Gets a value indicating whether this instance is running on mono.
    /// </summary>
    public static bool IsRunningOnMono { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is running on a CLR4 compatible implementation
    /// </summary>
    public static bool IsRunningOnClr4 { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is running on Unity-3D
    /// </summary>
    public static bool IsRunningOnUnity { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has been built as a Portable Class Library
    /// </summary>
    public static bool IsPortableFramework { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has been compiled natively in Unity (as opposite to importing a DLL).
    /// </summary>
    public static bool IsUnityNative { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance has been compiled natively in Unity AND is using IL2CPP
    /// </summary>
    public static bool IsUnityIL2CPP { get; private set; }

    /// <summary>
    /// Gets a value indicating whether this instance is running a system using Ahead-Of-Time compilation
    /// and not supporting JIT.
    /// </summary>
    public static bool IsRunningOnAOT
    {
      get
      {
        if (!PlatformAutoDetector.m_IsRunningOnAOT.HasValue)
        {
          try
          {
            ((Expression<Func<int>>) (() => 5)).Compile();
            PlatformAutoDetector.m_IsRunningOnAOT = new bool?(false);
          }
          catch (Exception ex)
          {
            PlatformAutoDetector.m_IsRunningOnAOT = new bool?(true);
          }
        }
        return PlatformAutoDetector.m_IsRunningOnAOT.Value;
      }
    }

    private static void AutoDetectPlatformFlags()
    {
      if (PlatformAutoDetector.m_AutoDetectionsDone)
        return;
      PlatformAutoDetector.IsRunningOnMono = Type.GetType("Mono.Runtime") != null;
      PlatformAutoDetector.IsRunningOnClr4 = Type.GetType("System.Lazy`1") != null;
      PlatformAutoDetector.m_AutoDetectionsDone = true;
    }

    internal static IPlatformAccessor GetDefaultPlatform()
    {
      PlatformAutoDetector.AutoDetectPlatformFlags();
      return PlatformAutoDetector.IsRunningOnUnity ? (IPlatformAccessor) new LimitedPlatformAccessor() : (IPlatformAccessor) new DotNetCorePlatformAccessor();
    }

    internal static IScriptLoader GetDefaultScriptLoader()
    {
      PlatformAutoDetector.AutoDetectPlatformFlags();
      return PlatformAutoDetector.IsRunningOnUnity ? (IScriptLoader) new UnityAssetsScriptLoader() : (IScriptLoader) new FileSystemScriptLoader();
    }
  }
}
