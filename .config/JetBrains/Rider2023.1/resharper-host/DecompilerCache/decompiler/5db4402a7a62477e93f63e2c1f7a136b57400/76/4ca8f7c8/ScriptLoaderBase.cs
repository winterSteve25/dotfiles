// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Loaders.ScriptLoaderBase
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using System;
using System.Collections.Generic;
using System.Linq;

namespace MoonSharp.Interpreter.Loaders
{
  /// <summary>
  /// A base implementation of IScriptLoader, offering resolution of module names.
  /// </summary>
  public abstract class ScriptLoaderBase : IScriptLoader
  {
    /// <summary>Checks if a script file exists.</summary>
    /// <param name="name">The script filename.</param>
    /// <returns></returns>
    public abstract bool ScriptFileExists(string name);

    /// <summary>
    /// Opens a file for reading the script code.
    /// It can return either a string, a byte[] or a Stream.
    /// If a byte[] is returned, the content is assumed to be a serialized (dumped) bytecode. If it's a string, it's
    /// assumed to be either a script or the output of a string.dump call. If a Stream, autodetection takes place.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="globalContext">The global context.</param>
    /// <returns>A string, a byte[] or a Stream.</returns>
    public abstract object LoadFile(string file, Table globalContext);

    /// <summary>Resolves the name of a module on a set of paths.</summary>
    /// <param name="modname">The modname.</param>
    /// <param name="paths">The paths.</param>
    /// <returns></returns>
    protected virtual string ResolveModuleName(string modname, string[] paths)
    {
      if (paths == null)
        return (string) null;
      modname = modname.Replace('.', '/');
      foreach (string path in paths)
      {
        string name = path.Replace("?", modname);
        if (this.ScriptFileExists(name))
          return name;
      }
      return (string) null;
    }

    /// <summary>
    /// Resolves the name of a module to a filename (which will later be passed to OpenScriptFile).
    /// The resolution happens first on paths included in the LUA_PATH global variable (if and only if
    /// the IgnoreLuaPathGlobal is false), and - if the variable does not exist - by consulting the
    /// ScriptOptions.ModulesPaths array. Override to provide a different behaviour.
    /// </summary>
    /// <param name="modname">The modname.</param>
    /// <param name="globalContext">The global context.</param>
    /// <returns></returns>
    public virtual string ResolveModuleName(string modname, Table globalContext)
    {
      if (!this.IgnoreLuaPathGlobal)
      {
        DynValue dynValue = globalContext.RawGet("LUA_PATH");
        if (dynValue != null && dynValue.Type == DataType.String)
          return this.ResolveModuleName(modname, ScriptLoaderBase.UnpackStringPaths(dynValue.String));
      }
      return this.ResolveModuleName(modname, this.ModulePaths);
    }

    /// <summary>
    /// Gets or sets the modules paths used by the "require" function. If null, the default paths are used (using
    /// environment variables etc.).
    /// </summary>
    public string[] ModulePaths { get; set; }

    /// <summary>
    /// Unpacks a string path in a form like "?;?.lua" to an array
    /// </summary>
    public static string[] UnpackStringPaths(string str) => ((IEnumerable<string>) str.Split(new char[1]
    {
      ';'
    }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (s => s.Trim())).Where<string>((Func<string, bool>) (s => !string.IsNullOrEmpty(s))).ToArray<string>();

    /// <summary>Gets the default environment paths.</summary>
    public static string[] GetDefaultEnvironmentPaths()
    {
      string[] environmentPaths = (string[]) null;
      if (environmentPaths == null)
      {
        string environmentVariable1 = Script.GlobalOptions.Platform.GetEnvironmentVariable("MOONSHARP_PATH");
        if (!string.IsNullOrEmpty(environmentVariable1))
          environmentPaths = ScriptLoaderBase.UnpackStringPaths(environmentVariable1);
        if (environmentPaths == null)
        {
          string environmentVariable2 = Script.GlobalOptions.Platform.GetEnvironmentVariable("LUA_PATH");
          if (!string.IsNullOrEmpty(environmentVariable2))
            environmentPaths = ScriptLoaderBase.UnpackStringPaths(environmentVariable2);
        }
        if (environmentPaths == null)
          environmentPaths = ScriptLoaderBase.UnpackStringPaths("?;?.lua");
      }
      return environmentPaths;
    }

    /// <summary>Resolves a filename [applying paths, etc.]</summary>
    /// <param name="filename">The filename.</param>
    /// <param name="globalContext">The global context.</param>
    /// <returns></returns>
    public virtual string ResolveFileName(string filename, Table globalContext) => filename;

    /// <summary>
    /// Gets or sets a value indicating whether the LUA_PATH global is checked or not to get the path where modules are contained.
    /// If true, the LUA_PATH global is NOT checked.
    /// </summary>
    public bool IgnoreLuaPathGlobal { get; set; }
  }
}
