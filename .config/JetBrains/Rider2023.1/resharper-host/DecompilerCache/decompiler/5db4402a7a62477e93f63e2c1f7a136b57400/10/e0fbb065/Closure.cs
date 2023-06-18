// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Closure
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Execution;
using System.Collections.Generic;

namespace MoonSharp.Interpreter
{
  /// <summary>A class representing a script function</summary>
  public class Closure : RefIdObject, IScriptPrivateResource
  {
    /// <summary>Shortcut for an empty closure</summary>
    private static ClosureContext emptyClosure = new ClosureContext();

    /// <summary>Gets the entry point location in bytecode .</summary>
    public int EntryPointByteCodeLocation { get; private set; }

    /// <summary>Gets the script owning this function</summary>
    public Script OwnerScript { get; private set; }

    /// <summary>The current closure context</summary>
    internal ClosureContext ClosureContext { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.Closure" /> class.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="idx">The index.</param>
    /// <param name="symbols">The symbols.</param>
    /// <param name="resolvedLocals">The resolved locals.</param>
    internal Closure(
      Script script,
      int idx,
      SymbolRef[] symbols,
      IEnumerable<DynValue> resolvedLocals)
    {
      this.OwnerScript = script;
      this.EntryPointByteCodeLocation = idx;
      if (symbols.Length != 0)
        this.ClosureContext = new ClosureContext(symbols, resolvedLocals);
      else
        this.ClosureContext = Closure.emptyClosure;
    }

    /// <summary>Calls this function with the specified args</summary>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call() => this.OwnerScript.Call((object) this);

    /// <summary>Calls this function with the specified args</summary>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(params object[] args) => this.OwnerScript.Call((object) this, args);

    /// <summary>Calls this function with the specified args</summary>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(params DynValue[] args) => this.OwnerScript.Call((object) this, (object[]) args);

    /// <summary>
    /// Gets a delegate wrapping calls to this scripted function
    /// </summary>
    /// <returns></returns>
    public ScriptFunctionDelegate GetDelegate() => (ScriptFunctionDelegate) (args => this.Call(args).ToObject());

    /// <summary>
    /// Gets a delegate wrapping calls to this scripted function
    /// </summary>
    /// <typeparam name="T">The type of return value of the delegate.</typeparam>
    /// <returns></returns>
    public ScriptFunctionDelegate<T> GetDelegate<T>() => (ScriptFunctionDelegate<T>) (args => this.Call(args).ToObject<T>());

    /// <summary>Gets the number of upvalues in this closure</summary>
    /// <returns>The number of upvalues in this closure</returns>
    public int GetUpvaluesCount() => this.ClosureContext.Count;

    /// <summary>Gets the name of the specified upvalue.</summary>
    /// <param name="idx">The index of the upvalue.</param>
    /// <returns>The upvalue name</returns>
    public string GetUpvalueName(int idx) => this.ClosureContext.Symbols[idx];

    /// <summary>
    /// Gets the value of an upvalue. To set the value, use GetUpvalue(idx).Assign(...);
    /// </summary>
    /// <param name="idx">The index of the upvalue.</param>
    /// <returns>The value of an upvalue </returns>
    public DynValue GetUpvalue(int idx) => this.ClosureContext[idx];

    /// <summary>
    /// Gets the type of the upvalues contained in this closure
    /// </summary>
    /// <returns></returns>
    public Closure.UpvaluesType GetUpvaluesType()
    {
      switch (this.GetUpvaluesCount())
      {
        case 0:
          return Closure.UpvaluesType.None;
        case 1:
          if (this.GetUpvalueName(0) == "_ENV")
            return Closure.UpvaluesType.Environment;
          break;
      }
      return Closure.UpvaluesType.Closure;
    }

    /// <summary>Type of closure based on upvalues</summary>
    public enum UpvaluesType
    {
      /// <summary>
      /// The closure has no upvalues (thus, technically, it's a function and not a closure!)
      /// </summary>
      None,
      /// <summary>The closure has _ENV as its only upvalue</summary>
      Environment,
      /// <summary>
      /// The closure is a "real" closure, with multiple upvalues
      /// </summary>
      Closure,
    }
  }
}
