// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.SyntaxErrorException
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Debugging;
using MoonSharp.Interpreter.Tree;
using System;

namespace MoonSharp.Interpreter
{
  /// <summary>Exception for all parsing/lexing errors.</summary>
  public class SyntaxErrorException : InterpreterException
  {
    internal Token Token { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether this exception was caused by premature stream termination (that is, unexpected EOF).
    /// This can be used in REPL interfaces to tell between unrecoverable errors and those which can be recovered by extra input.
    /// </summary>
    public bool IsPrematureStreamTermination { get; set; }

    internal SyntaxErrorException(Token t, string format, params object[] args)
      : base(format, args)
    {
      this.Token = t;
    }

    internal SyntaxErrorException(Token t, string message)
      : base(message)
    {
      this.Token = t;
    }

    internal SyntaxErrorException(
      Script script,
      SourceRef sref,
      string format,
      params object[] args)
      : base(format, args)
    {
      this.DecorateMessage(script, sref);
    }

    internal SyntaxErrorException(Script script, SourceRef sref, string message)
      : base(message)
    {
      this.DecorateMessage(script, sref);
    }

    private SyntaxErrorException(SyntaxErrorException syntaxErrorException)
      : base((Exception) syntaxErrorException, syntaxErrorException.DecoratedMessage)
    {
      this.Token = syntaxErrorException.Token;
      this.DecoratedMessage = this.Message;
    }

    internal void DecorateMessage(Script script)
    {
      if (this.Token == null)
        return;
      this.DecorateMessage(script, this.Token.GetSourceRef(false));
    }

    /// <summary>Rethrows this instance if</summary>
    /// <returns></returns>
    public override void Rethrow()
    {
      if (Script.GlobalOptions.RethrowExceptionNested)
        throw new SyntaxErrorException(this);
    }
  }
}
