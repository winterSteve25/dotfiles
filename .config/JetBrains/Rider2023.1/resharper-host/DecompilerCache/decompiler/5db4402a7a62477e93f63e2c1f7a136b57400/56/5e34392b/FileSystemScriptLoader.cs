// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Loaders.FileSystemScriptLoader
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using System.IO;

namespace MoonSharp.Interpreter.Loaders
{
  /// <summary>
  /// A script loader loading scripts directly from the file system (does not go through platform object)
  /// </summary>
  public class FileSystemScriptLoader : ScriptLoaderBase
  {
    /// <summary>Checks if a script file exists.</summary>
    /// <param name="name">The script filename.</param>
    /// <returns></returns>
    public override bool ScriptFileExists(string name) => File.Exists(name);

    /// <summary>
    /// Opens a file for reading the script code.
    /// It can return either a string, a byte[] or a Stream.
    /// If a byte[] is returned, the content is assumed to be a serialized (dumped) bytecode. If it's a string, it's
    /// assumed to be either a script or the output of a string.dump call. If a Stream, autodetection takes place.
    /// </summary>
    /// <param name="file">The file.</param>
    /// <param name="globalContext">The global context.</param>
    /// <returns>A string, a byte[] or a Stream.</returns>
    public override object LoadFile(string file, Table globalContext) => (object) new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
  }
}
