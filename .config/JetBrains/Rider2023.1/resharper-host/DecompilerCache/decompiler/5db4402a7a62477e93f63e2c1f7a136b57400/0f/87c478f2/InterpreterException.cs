// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.InterpreterException
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Debugging;
using System;
using System.Collections.Generic;

namespace MoonSharp.Interpreter
{
  /// <summary>Base type of all exceptions thrown in MoonSharp</summary>
  public class InterpreterException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.InterpreterException" /> class.
    /// </summary>
    /// <param name="ex">The ex.</param>
    protected InterpreterException(Exception ex, string message)
      : base(message, ex)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.InterpreterException" /> class.
    /// </summary>
    /// <param name="ex">The ex.</param>
    protected InterpreterException(Exception ex)
      : base(ex.Message, ex)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.InterpreterException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    protected InterpreterException(string message)
      : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.InterpreterException" /> class.
    /// </summary>
    /// <param name="format">The format.</param>
    /// <param name="args">The arguments.</param>
    protected InterpreterException(string format, params object[] args)
      : base(string.Format(format, args))
    {
    }

    /// <summary>
    /// Gets the instruction pointer of the execution (if it makes sense)
    /// </summary>
    public int InstructionPtr { get; internal set; }

    /// <summary>Gets the interpreter call stack.</summary>
    public IList<WatchItem> CallStack { get; internal set; }

    /// <summary>
    /// Gets the decorated message (error message plus error location in script) if possible.
    /// </summary>
    public string DecoratedMessage { get; internal set; }

    /// <summary>
    /// Gets or sets a value indicating whether the message should not be decorated
    /// </summary>
    public bool DoNotDecorateMessage { get; set; }

    internal void DecorateMessage(Script script, SourceRef sref, int ip = -1)
    {
      if (!string.IsNullOrEmpty(this.DecoratedMessage))
        return;
      if (this.DoNotDecorateMessage)
        this.DecoratedMessage = this.Message;
      else if (sref != null)
        this.DecoratedMessage = string.Format("{0}: {1}", (object) sref.FormatLocation(script), (object) this.Message);
      else
        this.DecoratedMessage = string.Format("bytecode:{0}: {1}", (object) ip, (object) this.Message);
    }

    /// <summary>Rethrows this instance if</summary>
    /// <returns></returns>
    public virtual void Rethrow()
    {
    }
  }
}
