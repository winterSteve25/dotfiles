// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Script
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Debugging;
using MoonSharp.Interpreter.Diagnostics;
using MoonSharp.Interpreter.Execution.VM;
using MoonSharp.Interpreter.IO;
using MoonSharp.Interpreter.Platforms;
using MoonSharp.Interpreter.Tree.Expressions;
using MoonSharp.Interpreter.Tree.Fast_Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoonSharp.Interpreter
{
  /// <summary>
  /// This class implements a MoonSharp scripting session. Multiple Script objects can coexist in the same program but cannot share
  /// data among themselves unless some mechanism is put in place.
  /// </summary>
  public class Script : IScriptPrivateResource
  {
    /// <summary>The version of the MoonSharp engine</summary>
    public const string VERSION = "2.0.0.0";
    /// <summary>The Lua version being supported</summary>
    public const string LUA_VERSION = "5.2";
    private Processor m_MainProcessor;
    private ByteCode m_ByteCode;
    private List<SourceCode> m_Sources = new List<SourceCode>();
    private Table m_GlobalTable;
    private IDebugger m_Debugger;
    private Table[] m_TypeMetatables = new Table[6];

    /// <summary>
    /// Initializes the <see cref="T:MoonSharp.Interpreter.Script" /> class.
    /// </summary>
    static Script()
    {
      Script.GlobalOptions = new ScriptGlobalOptions();
      Script.DefaultOptions = new ScriptOptions()
      {
        DebugPrint = (Action<string>) (s => Script.GlobalOptions.Platform.DefaultPrint(s)),
        DebugInput = (Func<string, string>) (s => Script.GlobalOptions.Platform.DefaultInput(s)),
        CheckThreadAccess = true,
        ScriptLoader = PlatformAutoDetector.GetDefaultScriptLoader(),
        TailCallOptimizationThreshold = 65536
      };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.Script" /> clas.s
    /// </summary>
    public Script()
      : this(CoreModules.Preset_Default)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.Script" /> class.
    /// </summary>
    /// <param name="coreModules">The core modules to be pre-registered in the default global table.</param>
    public Script(CoreModules coreModules)
    {
      this.Options = new ScriptOptions(Script.DefaultOptions);
      this.PerformanceStats = new PerformanceStatistics();
      this.Registry = new Table(this);
      this.m_ByteCode = new ByteCode(this);
      this.m_MainProcessor = new Processor(this, this.m_GlobalTable, this.m_ByteCode);
      this.m_GlobalTable = new Table(this).RegisterCoreModules(coreModules);
    }

    /// <summary>
    /// Gets or sets the script loader which will be used as the value of the
    /// ScriptLoader property for all newly created scripts.
    /// </summary>
    public static ScriptOptions DefaultOptions { get; private set; }

    /// <summary>Gets access to the script options.</summary>
    public ScriptOptions Options { get; private set; }

    /// <summary>
    /// Gets the global options, that is options which cannot be customized per-script.
    /// </summary>
    public static ScriptGlobalOptions GlobalOptions { get; private set; }

    /// <summary>Gets access to performance statistics.</summary>
    public PerformanceStatistics PerformanceStats { get; private set; }

    /// <summary>
    /// Gets the default global table for this script. Unless a different table is intentionally passed (or setfenv has been used)
    /// execution uses this table.
    /// </summary>
    public Table Globals => this.m_GlobalTable;

    /// <summary>Loads a string containing a Lua/MoonSharp function.</summary>
    /// <param name="code">The code.</param>
    /// <param name="globalTable">The global table to bind to this chunk.</param>
    /// <param name="funcFriendlyName">Name of the function used to report errors, etc.</param>
    /// <returns>
    /// A DynValue containing a function which will execute the loaded code.
    /// </returns>
    public DynValue LoadFunction(string code, Table globalTable = null, string funcFriendlyName = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) globalTable);
      SourceCode source = new SourceCode(string.Format("libfunc_{0}", (object) (funcFriendlyName ?? this.m_Sources.Count.ToString())), code, this.m_Sources.Count, this);
      this.m_Sources.Add(source);
      int address = Loader_Fast.LoadFunction(this, source, this.m_ByteCode, globalTable != null || this.m_GlobalTable != null);
      this.SignalSourceCodeChange(source);
      this.SignalByteCodeChange();
      return this.MakeClosure(address, globalTable ?? this.m_GlobalTable);
    }

    private void SignalByteCodeChange()
    {
      if (this.m_Debugger == null)
        return;
      this.m_Debugger.SetByteCode(this.m_ByteCode.Code.Select<Instruction, string>((Func<Instruction, string>) (s => s.ToString())).ToArray<string>());
    }

    private void SignalSourceCodeChange(SourceCode source)
    {
      if (this.m_Debugger == null)
        return;
      this.m_Debugger.SetSourceCode(source);
    }

    /// <summary>Loads a string containing a Lua/MoonSharp script.</summary>
    /// <param name="code">The code.</param>
    /// <param name="globalTable">The global table to bind to this chunk.</param>
    /// <param name="codeFriendlyName">Name of the code - used to report errors, etc. Also used by debuggers to locate the original source file.</param>
    /// <returns>
    /// A DynValue containing a function which will execute the loaded code.
    /// </returns>
    public DynValue LoadString(string code, Table globalTable = null, string codeFriendlyName = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) globalTable);
      if (code.StartsWith("MoonSharp_dump_b64::"))
      {
        code = code.Substring("MoonSharp_dump_b64::".Length);
        using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(code)))
          return this.LoadStream((Stream) memoryStream, globalTable, codeFriendlyName);
      }
      else
      {
        string str = string.Format("{0}", (object) (codeFriendlyName ?? "chunk_" + this.m_Sources.Count.ToString()));
        SourceCode source = new SourceCode(codeFriendlyName ?? str, code, this.m_Sources.Count, this);
        this.m_Sources.Add(source);
        int address = Loader_Fast.LoadChunk(this, source, this.m_ByteCode);
        this.SignalSourceCodeChange(source);
        this.SignalByteCodeChange();
        return this.MakeClosure(address, globalTable ?? this.m_GlobalTable);
      }
    }

    /// <summary>
    /// Loads a Lua/MoonSharp script from a System.IO.Stream. NOTE: This will *NOT* close the stream!
    /// </summary>
    /// <param name="stream">The stream containing code.</param>
    /// <param name="globalTable">The global table to bind to this chunk.</param>
    /// <param name="codeFriendlyName">Name of the code - used to report errors, etc.</param>
    /// <returns>
    /// A DynValue containing a function which will execute the loaded code.
    /// </returns>
    public DynValue LoadStream(Stream stream, Table globalTable = null, string codeFriendlyName = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) globalTable);
      Stream stream1 = (Stream) new UndisposableStream(stream);
      if (!Processor.IsDumpStream(stream1))
      {
        using (StreamReader streamReader = new StreamReader(stream1))
          return this.LoadString(streamReader.ReadToEnd(), globalTable, codeFriendlyName);
      }
      else
      {
        string str = string.Format("{0}", (object) (codeFriendlyName ?? "dump_" + this.m_Sources.Count.ToString()));
        SourceCode source = new SourceCode(codeFriendlyName ?? str, string.Format("-- This script was decoded from a binary dump - dump_{0}", (object) this.m_Sources.Count), this.m_Sources.Count, this);
        this.m_Sources.Add(source);
        bool hasUpvalues;
        int address = this.m_MainProcessor.Undump(stream1, this.m_Sources.Count - 1, globalTable ?? this.m_GlobalTable, out hasUpvalues);
        this.SignalSourceCodeChange(source);
        this.SignalByteCodeChange();
        return hasUpvalues ? this.MakeClosure(address, globalTable ?? this.m_GlobalTable) : this.MakeClosure(address);
      }
    }

    /// <summary>Dumps on the specified stream.</summary>
    /// <param name="function">The function.</param>
    /// <param name="stream">The stream.</param>
    /// <exception cref="T:System.ArgumentException">
    /// function arg is not a function!
    /// or
    /// stream is readonly!
    /// or
    /// function arg has upvalues other than _ENV
    /// </exception>
    public void Dump(DynValue function, Stream stream)
    {
      this.CheckScriptOwnership(function);
      if (function.Type != DataType.Function)
        throw new ArgumentException("function arg is not a function!");
      if (!stream.CanWrite)
        throw new ArgumentException("stream is readonly!");
      Closure.UpvaluesType upvaluesType = function.Function.GetUpvaluesType();
      if (upvaluesType == Closure.UpvaluesType.Closure)
        throw new ArgumentException("function arg has upvalues other than _ENV");
      this.m_MainProcessor.Dump((Stream) new UndisposableStream(stream), function.Function.EntryPointByteCodeLocation, upvaluesType == Closure.UpvaluesType.Environment);
    }

    /// <summary>Loads a string containing a Lua/MoonSharp script.</summary>
    /// <param name="filename">The code.</param>
    /// <param name="globalContext">The global table to bind to this chunk.</param>
    /// <param name="friendlyFilename">The filename to be used in error messages.</param>
    /// <returns>
    /// A DynValue containing a function which will execute the loaded code.
    /// </returns>
    public DynValue LoadFile(string filename, Table globalContext = null, string friendlyFilename = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) globalContext);
      filename = this.Options.ScriptLoader.ResolveFileName(filename, globalContext ?? this.m_GlobalTable);
      object obj = this.Options.ScriptLoader.LoadFile(filename, globalContext ?? this.m_GlobalTable);
      switch (obj)
      {
        case string _:
          return this.LoadString((string) obj, globalContext, friendlyFilename ?? filename);
        case byte[] _:
          using (MemoryStream memoryStream = new MemoryStream((byte[]) obj))
            return this.LoadStream((Stream) memoryStream, globalContext, friendlyFilename ?? filename);
        case Stream _:
          try
          {
            return this.LoadStream((Stream) obj, globalContext, friendlyFilename ?? filename);
          }
          finally
          {
            ((Stream) obj).Dispose();
          }
        case null:
          throw new InvalidCastException("Unexpected null from IScriptLoader.LoadFile");
        default:
          throw new InvalidCastException(string.Format("Unsupported return type from IScriptLoader.LoadFile : {0}", (object) obj.GetType()));
      }
    }

    /// <summary>
    /// Loads and executes a string containing a Lua/MoonSharp script.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="globalContext">The global context.</param>
    /// <param name="codeFriendlyName">Name of the code - used to report errors, etc. Also used by debuggers to locate the original source file.</param>
    /// <returns>
    /// A DynValue containing the result of the processing of the loaded chunk.
    /// </returns>
    public DynValue DoString(string code, Table globalContext = null, string codeFriendlyName = null) => this.Call(this.LoadString(code, globalContext, codeFriendlyName));

    /// <summary>
    /// Loads and executes a stream containing a Lua/MoonSharp script.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="globalContext">The global context.</param>
    /// <param name="codeFriendlyName">Name of the code - used to report errors, etc. Also used by debuggers to locate the original source file.</param>
    /// <returns>
    /// A DynValue containing the result of the processing of the loaded chunk.
    /// </returns>
    public DynValue DoStream(Stream stream, Table globalContext = null, string codeFriendlyName = null) => this.Call(this.LoadStream(stream, globalContext, codeFriendlyName));

    /// <summary>
    /// Loads and executes a file containing a Lua/MoonSharp script.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// <param name="globalContext">The global context.</param>
    /// <param name="codeFriendlyName">Name of the code - used to report errors, etc. Also used by debuggers to locate the original source file.</param>
    /// <returns>
    /// A DynValue containing the result of the processing of the loaded chunk.
    /// </returns>
    public DynValue DoFile(string filename, Table globalContext = null, string codeFriendlyName = null) => this.Call(this.LoadFile(filename, globalContext, codeFriendlyName));

    /// <summary>
    /// Runs the specified file with all possible defaults for quick experimenting.
    /// </summary>
    /// <param name="filename">The filename.</param>
    /// 
    ///             A DynValue containing the result of the processing of the executed script.
    public static DynValue RunFile(string filename) => new Script().DoFile(filename);

    /// <summary>
    /// Runs the specified code with all possible defaults for quick experimenting.
    /// </summary>
    /// <param name="code">The Lua/MoonSharp code.</param>
    /// 
    ///             A DynValue containing the result of the processing of the executed script.
    public static DynValue RunString(string code) => new Script().DoString(code);

    /// <summary>Creates a closure from a bytecode address.</summary>
    /// <param name="address">The address.</param>
    /// <param name="envTable">The env table to create a 0-upvalue</param>
    /// <returns></returns>
    private DynValue MakeClosure(int address, Table envTable = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) envTable);
      Closure function;
      if (envTable == null)
      {
        Instruction meta = this.m_MainProcessor.FindMeta(ref address);
        if (meta != null && meta.NumVal2 == 0)
          function = new Closure(this, address, new SymbolRef[1]
          {
            SymbolRef.Upvalue("_ENV", 0)
          }, (IEnumerable<DynValue>) new DynValue[1]
          {
            meta.Value
          });
        else
          function = new Closure(this, address, new SymbolRef[0], (IEnumerable<DynValue>) new DynValue[0]);
      }
      else
      {
        SymbolRef[] symbols = new SymbolRef[1]
        {
          new SymbolRef()
          {
            i_Env = (SymbolRef) null,
            i_Index = 0,
            i_Name = "_ENV",
            i_Type = SymbolRefType.DefaultEnv
          }
        };
        DynValue[] resolvedLocals = new DynValue[1]
        {
          DynValue.NewTable(envTable)
        };
        function = new Closure(this, address, symbols, (IEnumerable<DynValue>) resolvedLocals);
      }
      return DynValue.NewClosure(function);
    }

    /// <summary>Calls the specified function.</summary>
    /// <param name="function">The Lua/MoonSharp function to be called</param>
    /// <returns>The return value(s) of the function call.</returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(DynValue function) => this.Call(function, new DynValue[0]);

    /// <summary>Calls the specified function.</summary>
    /// <param name="function">The Lua/MoonSharp function to be called</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The return value(s) of the function call.</returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(DynValue function, params DynValue[] args)
    {
      this.CheckScriptOwnership(function);
      this.CheckScriptOwnership(args);
      if (function.Type != DataType.Function && function.Type != DataType.ClrFunction)
      {
        DynValue metamethod = this.m_MainProcessor.GetMetamethod(function, "__call");
        if (metamethod == null)
          throw new ArgumentException("function is not a function and has no __call metamethod.");
        DynValue[] dynValueArray = new DynValue[args.Length + 1];
        dynValueArray[0] = function;
        for (int index = 0; index < args.Length; ++index)
          dynValueArray[index + 1] = args[index];
        function = metamethod;
        args = dynValueArray;
      }
      else if (function.Type == DataType.ClrFunction)
        return function.Callback.ClrCallback(this.CreateDynamicExecutionContext(function.Callback), new CallbackArguments((IList<DynValue>) args, false));
      return this.m_MainProcessor.Call(function, args);
    }

    /// <summary>Calls the specified function.</summary>
    /// <param name="function">The Lua/MoonSharp function to be called</param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns>The return value(s) of the function call.</returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(DynValue function, params object[] args)
    {
      DynValue[] dynValueArray = new DynValue[args.Length];
      for (int index = 0; index < dynValueArray.Length; ++index)
        dynValueArray[index] = DynValue.FromObject(this, args[index]);
      return this.Call(function, dynValueArray);
    }

    /// <summary>Calls the specified function.</summary>
    /// <param name="function">The Lua/MoonSharp function to be called</param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(object function) => this.Call(DynValue.FromObject(this, function));

    /// <summary>Calls the specified function.</summary>
    /// <param name="function">The Lua/MoonSharp function to be called </param>
    /// <param name="args">The arguments to pass to the function.</param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function</exception>
    public DynValue Call(object function, params object[] args) => this.Call(DynValue.FromObject(this, function), args);

    /// <summary>
    /// Creates a coroutine pointing at the specified function.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <returns>The coroutine handle.</returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function or DataType.ClrFunction</exception>
    public DynValue CreateCoroutine(DynValue function)
    {
      this.CheckScriptOwnership(function);
      if (function.Type == DataType.Function)
        return this.m_MainProcessor.Coroutine_Create(function.Function);
      return function.Type == DataType.ClrFunction ? DynValue.NewCoroutine(new Coroutine(function.Callback)) : throw new ArgumentException("function is not of DataType.Function or DataType.ClrFunction");
    }

    /// <summary>
    /// Creates a coroutine pointing at the specified function.
    /// </summary>
    /// <param name="function">The function.</param>
    /// <returns>The coroutine handle.</returns>
    /// <exception cref="T:System.ArgumentException">Thrown if function is not of DataType.Function or DataType.ClrFunction</exception>
    public DynValue CreateCoroutine(object function) => this.CreateCoroutine(DynValue.FromObject(this, function));

    /// <summary>
    /// Gets or sets a value indicating whether the debugger is enabled.
    /// Note that unless a debugger attached, this property returns a
    /// value which might not reflect the real status of the debugger.
    /// Use this property if you want to disable the debugger for some
    /// executions.
    /// </summary>
    public bool DebuggerEnabled
    {
      get => this.m_MainProcessor.DebuggerEnabled;
      set => this.m_MainProcessor.DebuggerEnabled = value;
    }

    /// <summary>
    /// Attaches a debugger. This usually should be called by the debugger itself and not by user code.
    /// </summary>
    /// <param name="debugger">The debugger object.</param>
    public void AttachDebugger(IDebugger debugger)
    {
      this.DebuggerEnabled = true;
      this.m_Debugger = debugger;
      this.m_MainProcessor.AttachDebugger(debugger);
      foreach (SourceCode source in this.m_Sources)
        this.SignalSourceCodeChange(source);
      this.SignalByteCodeChange();
    }

    /// <summary>Gets the source code.</summary>
    /// <param name="sourceCodeID">The source code identifier.</param>
    /// <returns></returns>
    public SourceCode GetSourceCode(int sourceCodeID) => this.m_Sources[sourceCodeID];

    /// <summary>Gets the source code count.</summary>
    /// <value>The source code count.</value>
    public int SourceCodeCount => this.m_Sources.Count;

    /// <summary>
    /// Loads a module as per the "require" Lua function. http://www.lua.org/pil/8.1.html
    /// </summary>
    /// <param name="modname">The module name</param>
    /// <param name="globalContext">The global context.</param>
    /// <returns></returns>
    /// <exception cref="T:MoonSharp.Interpreter.ScriptRuntimeException">Raised if module is not found</exception>
    public DynValue RequireModule(string modname, Table globalContext = null)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) globalContext);
      Table globalContext1 = globalContext ?? this.m_GlobalTable;
      string str = this.Options.ScriptLoader.ResolveModuleName(modname, globalContext1);
      return str != null ? this.LoadFile(str, globalContext, str) : throw new ScriptRuntimeException("module '{0}' not found", new object[1]
      {
        (object) modname
      });
    }

    /// <summary>Gets a type metatable.</summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public Table GetTypeMetatable(DataType type)
    {
      int index = (int) type;
      return index >= 0 && index < this.m_TypeMetatables.Length ? this.m_TypeMetatables[index] : (Table) null;
    }

    /// <summary>Sets a type metatable.</summary>
    /// <param name="type">The type. Must be Nil, Boolean, Number, String or Function</param>
    /// <param name="metatable">The metatable.</param>
    /// <exception cref="T:System.ArgumentException">Specified type not supported :  + type.ToString()</exception>
    public void SetTypeMetatable(DataType type, Table metatable)
    {
      this.CheckScriptOwnership((IScriptPrivateResource) metatable);
      int index = (int) type;
      if (index < 0 || index >= this.m_TypeMetatables.Length)
        throw new ArgumentException("Specified type not supported : " + type.ToString());
      this.m_TypeMetatables[index] = metatable;
    }

    /// <summary>
    /// Warms up the parser/lexer structures so that MoonSharp operations start faster.
    /// </summary>
    public static void WarmUp() => new Script(CoreModules.Basic).LoadString("return 1;");

    /// <summary>Creates a new dynamic expression.</summary>
    /// <param name="code">The code of the expression.</param>
    /// <returns></returns>
    public DynamicExpression CreateDynamicExpression(string code)
    {
      DynamicExprExpression expr = Loader_Fast.LoadDynamicExpr(this, new SourceCode("__dynamic", code, -1, this));
      return new DynamicExpression(this, code, expr);
    }

    /// <summary>
    /// Creates a new dynamic expression which is actually quite static, returning always the same constant value.
    /// </summary>
    /// <param name="code">The code of the not-so-dynamic expression.</param>
    /// <param name="constant">The constant to return.</param>
    /// <returns></returns>
    public DynamicExpression CreateConstantDynamicExpression(string code, DynValue constant)
    {
      this.CheckScriptOwnership(constant);
      return new DynamicExpression(this, code, constant);
    }

    /// <summary>
    /// Gets an execution context exposing only partial functionality, which should be used for
    /// those cases where the execution engine is not really running - for example for dynamic expression
    /// or calls from CLR to CLR callbacks
    /// </summary>
    internal ScriptExecutionContext CreateDynamicExecutionContext(CallbackFunction func = null) => new ScriptExecutionContext(this.m_MainProcessor, func, (SourceRef) null, true);

    /// <summary>
    /// MoonSharp (like Lua itself) provides a registry, a predefined table that can be used by any CLR code to
    /// store whatever Lua values it needs to store.
    /// Any CLR code can store data into this table, but it should take care to choose keys
    /// that are different from those used by other libraries, to avoid collisions.
    /// Typically, you should use as key a string GUID, a string containing your library name, or a
    /// userdata with the address of a CLR object in your code.
    /// </summary>
    public Table Registry { get; private set; }

    /// <summary>
    /// Gets a banner string with copyright info, link to website, version, etc.
    /// </summary>
    public static string GetBanner(string subproduct = null)
    {
      subproduct = subproduct != null ? subproduct + " " : "";
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine(string.Format("MoonSharp {0}{1} [{2}]", (object) subproduct, (object) "2.0.0.0", (object) Script.GlobalOptions.Platform.GetPlatformName()));
      stringBuilder.AppendLine("Copyright (C) 2014-2016 Marco Mastropaolo");
      stringBuilder.AppendLine("http://www.moonsharp.org");
      return stringBuilder.ToString();
    }

    Script IScriptPrivateResource.OwnerScript => this;
  }
}
