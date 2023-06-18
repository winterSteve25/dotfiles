// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Execution.VM.Processor
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.DataStructs;
using MoonSharp.Interpreter.Debugging;
using MoonSharp.Interpreter.Diagnostics;
using MoonSharp.Interpreter.Interop;
using MoonSharp.Interpreter.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MoonSharp.Interpreter.Execution.VM
{
  internal sealed class Processor
  {
    private ByteCode m_RootChunk;
    private FastStack<DynValue> m_ValueStack = new FastStack<DynValue>(131072);
    private FastStack<CallStackItem> m_ExecutionStack = new FastStack<CallStackItem>(131072);
    private List<Processor> m_CoroutinesStack;
    private Table m_GlobalTable;
    private Script m_Script;
    private Processor m_Parent;
    private CoroutineState m_State;
    private bool m_CanYield = true;
    private int m_SavedInstructionPtr = -1;
    private Processor.DebugContext m_Debug;
    private int m_OwningThreadID = -1;
    private int m_ExecutionNesting;
    private const ulong DUMP_CHUNK_MAGIC = 1877195438928383261;
    private const int DUMP_CHUNK_VERSION = 336;
    private const int YIELD_SPECIAL_TRAP = -99;
    internal long AutoYieldCounter;

    public Processor(Script script, Table globalContext, ByteCode byteCode)
    {
      this.m_CoroutinesStack = new List<Processor>();
      this.m_Debug = new Processor.DebugContext();
      this.m_RootChunk = byteCode;
      this.m_GlobalTable = globalContext;
      this.m_Script = script;
      this.m_State = CoroutineState.Main;
      DynValue.NewCoroutine(new Coroutine(this));
    }

    private Processor(Processor parentProcessor)
    {
      this.m_Debug = parentProcessor.m_Debug;
      this.m_RootChunk = parentProcessor.m_RootChunk;
      this.m_GlobalTable = parentProcessor.m_GlobalTable;
      this.m_Script = parentProcessor.m_Script;
      this.m_Parent = parentProcessor;
      this.m_State = CoroutineState.NotStarted;
    }

    public DynValue Call(DynValue function, DynValue[] args)
    {
      List<Processor> processorList = this.m_Parent != null ? this.m_Parent.m_CoroutinesStack : this.m_CoroutinesStack;
      if (processorList.Count > 0 && processorList[processorList.Count - 1] != this)
        return processorList[processorList.Count - 1].Call(function, args);
      this.EnterProcessor();
      try
      {
        IDisposable disposable = this.m_Script.PerformanceStats.StartStopwatch(PerformanceCounter.Execution);
        this.m_CanYield = false;
        try
        {
          return this.Processing_Loop(this.PushClrToScriptStackFrame(CallStackItemFlags.CallEntryPoint, function, args));
        }
        finally
        {
          this.m_CanYield = true;
          disposable?.Dispose();
        }
      }
      finally
      {
        this.LeaveProcessor();
      }
    }

    private int PushClrToScriptStackFrame(
      CallStackItemFlags flags,
      DynValue function,
      DynValue[] args)
    {
      if (function == null)
        function = this.m_ValueStack.Peek();
      else
        this.m_ValueStack.Push(function);
      args = this.Internal_AdjustTuple((IList<DynValue>) args);
      for (int index = 0; index < args.Length; ++index)
        this.m_ValueStack.Push(args[index]);
      this.m_ValueStack.Push(DynValue.NewNumber((double) args.Length));
      this.m_ExecutionStack.Push(new CallStackItem()
      {
        BasePointer = this.m_ValueStack.Count,
        Debug_EntryPoint = function.Function.EntryPointByteCodeLocation,
        ReturnAddress = -1,
        ClosureScope = function.Function.ClosureContext,
        CallingSourceRef = SourceRef.GetClrLocation(),
        Flags = flags
      });
      return function.Function.EntryPointByteCodeLocation;
    }

    private void LeaveProcessor()
    {
      --this.m_ExecutionNesting;
      this.m_OwningThreadID = -1;
      if (this.m_Parent != null)
        this.m_Parent.m_CoroutinesStack.RemoveAt(this.m_Parent.m_CoroutinesStack.Count - 1);
      if (this.m_ExecutionNesting != 0 || this.m_Debug == null || !this.m_Debug.DebuggerEnabled || this.m_Debug.DebuggerAttached == null)
        return;
      this.m_Debug.DebuggerAttached.SignalExecutionEnded();
    }

    private int GetThreadId() => 1;

    private void EnterProcessor()
    {
      int threadId = this.GetThreadId();
      if (this.m_OwningThreadID >= 0 && this.m_OwningThreadID != threadId && this.m_Script.Options.CheckThreadAccess)
        throw new InvalidOperationException(string.Format("Cannot enter the same MoonSharp processor from two different threads : {0} and {1}", (object) this.m_OwningThreadID, (object) threadId));
      this.m_OwningThreadID = threadId;
      ++this.m_ExecutionNesting;
      if (this.m_Parent == null)
        return;
      this.m_Parent.m_CoroutinesStack.Add(this);
    }

    internal SourceRef GetCoroutineSuspendedLocation() => this.GetCurrentSourceRef(this.m_SavedInstructionPtr);

    internal static bool IsDumpStream(Stream stream)
    {
      if (stream.Length < 8L)
        return false;
      using (BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8))
      {
        long num = (long) binaryReader.ReadUInt64();
        stream.Seek(-8L, SeekOrigin.Current);
        return num == 1877195438928383261L;
      }
    }

    internal int Dump(Stream stream, int baseAddress, bool hasUpvalues)
    {
      using (BinaryWriter binaryWriter = (BinaryWriter) new BinDumpBinaryWriter(stream, Encoding.UTF8))
      {
        Dictionary<SymbolRef, int> symbolMap = new Dictionary<SymbolRef, int>();
        Instruction meta = this.FindMeta(ref baseAddress);
        if (meta == null)
          throw new ArgumentException(nameof (baseAddress));
        binaryWriter.Write(1877195438928383261UL);
        binaryWriter.Write(336);
        binaryWriter.Write(hasUpvalues);
        binaryWriter.Write(meta.NumVal);
        for (int index = 0; index <= meta.NumVal; ++index)
        {
          SymbolRef[] symbolList;
          SymbolRef symbol;
          this.m_RootChunk.Code[baseAddress + index].GetSymbolReferences(out symbolList, out symbol);
          if (symbol != null)
            this.AddSymbolToMap(symbolMap, symbol);
          if (symbolList != null)
          {
            foreach (SymbolRef s in symbolList)
              this.AddSymbolToMap(symbolMap, s);
          }
        }
        foreach (SymbolRef symbolRef in symbolMap.Keys.ToArray<SymbolRef>())
        {
          if (symbolRef.i_Env != null)
            this.AddSymbolToMap(symbolMap, symbolRef.i_Env);
        }
        SymbolRef[] symbolRefArray = new SymbolRef[symbolMap.Count];
        foreach (KeyValuePair<SymbolRef, int> keyValuePair in symbolMap)
          symbolRefArray[keyValuePair.Value] = keyValuePair.Key;
        binaryWriter.Write(symbolMap.Count);
        foreach (SymbolRef symbolRef in symbolRefArray)
          symbolRef.WriteBinary(binaryWriter);
        foreach (SymbolRef symbolRef in symbolRefArray)
          symbolRef.WriteBinaryEnv(binaryWriter, symbolMap);
        for (int index = 0; index <= meta.NumVal; ++index)
          this.m_RootChunk.Code[baseAddress + index].WriteBinary(binaryWriter, baseAddress, symbolMap);
        return meta.NumVal + baseAddress + 1;
      }
    }

    private void AddSymbolToMap(Dictionary<SymbolRef, int> symbolMap, SymbolRef s)
    {
      if (symbolMap.ContainsKey(s))
        return;
      symbolMap.Add(s, symbolMap.Count);
    }

    internal int Undump(Stream stream, int sourceID, Table envTable, out bool hasUpvalues)
    {
      int count = this.m_RootChunk.Code.Count;
      SourceRef chunkRef = new SourceRef(sourceID, 0, 0, 0, 0, false);
      using (BinaryReader binaryReader = (BinaryReader) new BinDumpBinaryReader(stream, Encoding.UTF8))
      {
        if (binaryReader.ReadUInt64() != 1877195438928383261UL)
          throw new ArgumentException("Not a MoonSharp chunk");
        hasUpvalues = binaryReader.ReadInt32() == 336 ? binaryReader.ReadBoolean() : throw new ArgumentException("Invalid version");
        int num = binaryReader.ReadInt32();
        int length = binaryReader.ReadInt32();
        SymbolRef[] symbolRefArray = new SymbolRef[length];
        for (int index = 0; index < length; ++index)
          symbolRefArray[index] = SymbolRef.ReadBinary(binaryReader);
        for (int index = 0; index < length; ++index)
          symbolRefArray[index].ReadBinaryEnv(binaryReader, symbolRefArray);
        for (int index = 0; index <= num; ++index)
          this.m_RootChunk.Code.Add(Instruction.ReadBinary(chunkRef, binaryReader, count, envTable, symbolRefArray));
        return count;
      }
    }

    public DynValue Coroutine_Create(Closure closure)
    {
      Processor proc = new Processor(this);
      proc.m_ValueStack.Push(DynValue.NewClosure(closure));
      return DynValue.NewCoroutine(new Coroutine(proc));
    }

    public CoroutineState State => this.m_State;

    public Coroutine AssociatedCoroutine { get; set; }

    public DynValue Coroutine_Resume(DynValue[] args)
    {
      this.EnterProcessor();
      try
      {
        int instructionPtr = 0;
        if (this.m_State != CoroutineState.NotStarted && this.m_State != CoroutineState.Suspended && this.m_State != CoroutineState.ForceSuspended)
          throw ScriptRuntimeException.CannotResumeNotSuspended(this.m_State);
        if (this.m_State == CoroutineState.NotStarted)
          instructionPtr = this.PushClrToScriptStackFrame(CallStackItemFlags.ResumeEntryPoint, (DynValue) null, args);
        else if (this.m_State == CoroutineState.Suspended)
        {
          this.m_ValueStack.Push(DynValue.NewTuple(args));
          instructionPtr = this.m_SavedInstructionPtr;
        }
        else if (this.m_State == CoroutineState.ForceSuspended)
        {
          if (args != null && args.Length != 0)
            throw new ArgumentException("When resuming a force-suspended coroutine, args must be empty.");
          instructionPtr = this.m_SavedInstructionPtr;
        }
        this.m_State = CoroutineState.Running;
        DynValue dynValue = this.Processing_Loop(instructionPtr);
        if (dynValue.Type == DataType.YieldRequest)
        {
          if (dynValue.YieldRequest.Forced)
          {
            this.m_State = CoroutineState.ForceSuspended;
            return dynValue;
          }
          this.m_State = CoroutineState.Suspended;
          return DynValue.NewTuple(dynValue.YieldRequest.ReturnValues);
        }
        this.m_State = CoroutineState.Dead;
        return dynValue;
      }
      catch (Exception ex)
      {
        this.m_State = CoroutineState.Dead;
        throw;
      }
      finally
      {
        this.LeaveProcessor();
      }
    }

    internal Instruction FindMeta(ref int baseAddress)
    {
      Instruction instruction;
      for (instruction = this.m_RootChunk.Code[baseAddress]; instruction.OpCode == OpCode.Nop; instruction = this.m_RootChunk.Code[baseAddress])
        ++baseAddress;
      return instruction.OpCode != OpCode.Meta ? (Instruction) null : instruction;
    }

    internal void AttachDebugger(IDebugger debugger)
    {
      this.m_Debug.DebuggerAttached = debugger;
      this.m_Debug.LineBasedBreakPoints = (debugger.GetDebuggerCaps() & DebuggerCaps.HasLineBasedBreakpoints) != 0;
      debugger.SetDebugService(new DebugService(this.m_Script, this));
    }

    internal bool DebuggerEnabled
    {
      get => this.m_Debug.DebuggerEnabled;
      set => this.m_Debug.DebuggerEnabled = value;
    }

    private void ListenDebugger(Instruction instr, int instructionPtr)
    {
      bool flag = false;
      if (instr.SourceCodeRef != null && this.m_Debug.LastHlRef != null)
        flag = !this.m_Debug.LineBasedBreakPoints ? instr.SourceCodeRef != this.m_Debug.LastHlRef : instr.SourceCodeRef.SourceIdx != this.m_Debug.LastHlRef.SourceIdx || instr.SourceCodeRef.FromLine != this.m_Debug.LastHlRef.FromLine;
      else if (this.m_Debug.LastHlRef == null)
        flag = instr.SourceCodeRef != null;
      if (this.m_Debug.DebuggerAttached.IsPauseRequested() || ((instr.SourceCodeRef == null ? 0 : (instr.SourceCodeRef.Breakpoint ? 1 : 0)) & (flag ? 1 : 0)) != 0)
      {
        this.m_Debug.DebuggerCurrentAction = DebuggerAction.ActionType.None;
        this.m_Debug.DebuggerCurrentActionTarget = -1;
      }
      switch (this.m_Debug.DebuggerCurrentAction)
      {
        case DebuggerAction.ActionType.ByteCodeStepOver:
          if (this.m_Debug.DebuggerCurrentActionTarget != instructionPtr)
            return;
          break;
        case DebuggerAction.ActionType.ByteCodeStepOut:
        case DebuggerAction.ActionType.StepOut:
          if (this.m_ExecutionStack.Count >= this.m_Debug.ExStackDepthAtStep)
            return;
          break;
        case DebuggerAction.ActionType.StepIn:
          if (this.m_ExecutionStack.Count >= this.m_Debug.ExStackDepthAtStep && (instr.SourceCodeRef == null || instr.SourceCodeRef == this.m_Debug.LastHlRef))
            return;
          break;
        case DebuggerAction.ActionType.StepOver:
          if (instr.SourceCodeRef == null || instr.SourceCodeRef == this.m_Debug.LastHlRef || this.m_ExecutionStack.Count > this.m_Debug.ExStackDepthAtStep)
            return;
          break;
        case DebuggerAction.ActionType.Run:
          if (!this.m_Debug.LineBasedBreakPoints)
            return;
          this.m_Debug.LastHlRef = instr.SourceCodeRef;
          return;
      }
      this.RefreshDebugger(false, instructionPtr);
      DebuggerAction action;
      while (true)
      {
        action = this.m_Debug.DebuggerAttached.GetAction(instructionPtr, instr.SourceCodeRef);
        switch (action.Action)
        {
          case DebuggerAction.ActionType.ByteCodeStepIn:
            goto label_21;
          case DebuggerAction.ActionType.ByteCodeStepOver:
            goto label_22;
          case DebuggerAction.ActionType.ByteCodeStepOut:
          case DebuggerAction.ActionType.StepIn:
          case DebuggerAction.ActionType.StepOver:
          case DebuggerAction.ActionType.StepOut:
            goto label_20;
          case DebuggerAction.ActionType.Run:
            goto label_23;
          case DebuggerAction.ActionType.ToggleBreakpoint:
            this.ToggleBreakPoint(action, new bool?());
            this.RefreshDebugger(true, instructionPtr);
            continue;
          case DebuggerAction.ActionType.SetBreakpoint:
            this.ToggleBreakPoint(action, new bool?(true));
            this.RefreshDebugger(true, instructionPtr);
            continue;
          case DebuggerAction.ActionType.ClearBreakpoint:
            this.ToggleBreakPoint(action, new bool?(false));
            this.RefreshDebugger(true, instructionPtr);
            continue;
          case DebuggerAction.ActionType.ResetBreakpoints:
            this.ResetBreakPoints(action);
            this.RefreshDebugger(true, instructionPtr);
            continue;
          case DebuggerAction.ActionType.Refresh:
            this.RefreshDebugger(false, instructionPtr);
            continue;
          case DebuggerAction.ActionType.HardRefresh:
            this.RefreshDebugger(true, instructionPtr);
            continue;
          default:
            continue;
        }
      }
label_20:
      this.m_Debug.DebuggerCurrentAction = action.Action;
      this.m_Debug.LastHlRef = instr.SourceCodeRef;
      this.m_Debug.ExStackDepthAtStep = this.m_ExecutionStack.Count;
      return;
label_21:
      this.m_Debug.DebuggerCurrentAction = DebuggerAction.ActionType.ByteCodeStepIn;
      this.m_Debug.DebuggerCurrentActionTarget = -1;
      return;
label_22:
      this.m_Debug.DebuggerCurrentAction = DebuggerAction.ActionType.ByteCodeStepOver;
      this.m_Debug.DebuggerCurrentActionTarget = instructionPtr + 1;
      return;
label_23:
      this.m_Debug.DebuggerCurrentAction = DebuggerAction.ActionType.Run;
      this.m_Debug.LastHlRef = instr.SourceCodeRef;
      this.m_Debug.DebuggerCurrentActionTarget = -1;
    }

    private void ResetBreakPoints(DebuggerAction action) => this.ResetBreakPoints(this.m_Script.GetSourceCode(action.SourceID), new HashSet<int>((IEnumerable<int>) action.Lines));

    internal HashSet<int> ResetBreakPoints(SourceCode src, HashSet<int> lines)
    {
      HashSet<int> intSet = new HashSet<int>();
      foreach (SourceRef sourceRef in src.Refs)
      {
        if (!sourceRef.CannotBreakpoint)
        {
          sourceRef.Breakpoint = lines.Contains(sourceRef.FromLine);
          if (sourceRef.Breakpoint)
            intSet.Add(sourceRef.FromLine);
        }
      }
      return intSet;
    }

    private bool ToggleBreakPoint(DebuggerAction action, bool? state)
    {
      SourceCode sourceCode = this.m_Script.GetSourceCode(action.SourceID);
      bool flag = false;
      foreach (SourceRef sourceRef in sourceCode.Refs)
      {
        if (!sourceRef.CannotBreakpoint && sourceRef.IncludesLocation(action.SourceID, action.SourceLine, action.SourceCol))
        {
          flag = true;
          sourceRef.Breakpoint = state.HasValue ? state.Value : !sourceRef.Breakpoint;
          if (sourceRef.Breakpoint)
            this.m_Debug.BreakPoints.Add(sourceRef);
          else
            this.m_Debug.BreakPoints.Remove(sourceRef);
        }
      }
      if (flag)
        return true;
      int num = int.MaxValue;
      SourceRef sourceRef1 = (SourceRef) null;
      foreach (SourceRef sourceRef2 in sourceCode.Refs)
      {
        if (!sourceRef2.CannotBreakpoint)
        {
          int locationDistance = sourceRef2.GetLocationDistance(action.SourceID, action.SourceLine, action.SourceCol);
          if (locationDistance < num)
          {
            num = locationDistance;
            sourceRef1 = sourceRef2;
          }
        }
      }
      if (sourceRef1 == null)
        return false;
      sourceRef1.Breakpoint = state.HasValue ? state.Value : !sourceRef1.Breakpoint;
      if (sourceRef1.Breakpoint)
        this.m_Debug.BreakPoints.Add(sourceRef1);
      else
        this.m_Debug.BreakPoints.Remove(sourceRef1);
      return true;
    }

    private void RefreshDebugger(bool hard, int instructionPtr)
    {
      SourceRef currentSourceRef = this.GetCurrentSourceRef(instructionPtr);
      ScriptExecutionContext context = new ScriptExecutionContext(this, (CallbackFunction) null, currentSourceRef);
      List<DynamicExpression> watchItems = this.m_Debug.DebuggerAttached.GetWatchItems();
      List<WatchItem> callStack = this.Debugger_GetCallStack(currentSourceRef);
      List<WatchItem> items1 = this.Debugger_RefreshWatches(context, watchItems);
      List<WatchItem> items2 = this.Debugger_RefreshVStack();
      List<WatchItem> items3 = this.Debugger_RefreshLocals(context);
      List<WatchItem> items4 = this.Debugger_RefreshThreads(context);
      this.m_Debug.DebuggerAttached.Update(WatchType.CallStack, (IEnumerable<WatchItem>) callStack);
      this.m_Debug.DebuggerAttached.Update(WatchType.Watches, (IEnumerable<WatchItem>) items1);
      this.m_Debug.DebuggerAttached.Update(WatchType.VStack, (IEnumerable<WatchItem>) items2);
      this.m_Debug.DebuggerAttached.Update(WatchType.Locals, (IEnumerable<WatchItem>) items3);
      this.m_Debug.DebuggerAttached.Update(WatchType.Threads, (IEnumerable<WatchItem>) items4);
      if (!hard)
        return;
      this.m_Debug.DebuggerAttached.RefreshBreakpoints((IEnumerable<SourceRef>) this.m_Debug.BreakPoints);
    }

    private List<WatchItem> Debugger_RefreshThreads(ScriptExecutionContext context) => (this.m_Parent != null ? (IEnumerable<Processor>) this.m_Parent.m_CoroutinesStack : (IEnumerable<Processor>) this.m_CoroutinesStack).Select<Processor, WatchItem>((Func<Processor, WatchItem>) (c => new WatchItem()
    {
      Address = c.AssociatedCoroutine.ReferenceID,
      Name = "coroutine #" + c.AssociatedCoroutine.ReferenceID.ToString()
    })).ToList<WatchItem>();

    private List<WatchItem> Debugger_RefreshVStack()
    {
      List<WatchItem> watchItemList = new List<WatchItem>();
      for (int idxofs = 0; idxofs < Math.Min(32, this.m_ValueStack.Count); ++idxofs)
        watchItemList.Add(new WatchItem()
        {
          Address = idxofs,
          Value = this.m_ValueStack.Peek(idxofs)
        });
      return watchItemList;
    }

    private List<WatchItem> Debugger_RefreshWatches(
      ScriptExecutionContext context,
      List<DynamicExpression> watchList)
    {
      return watchList.Select<DynamicExpression, WatchItem>((Func<DynamicExpression, WatchItem>) (w => this.Debugger_RefreshWatch(context, w))).ToList<WatchItem>();
    }

    private List<WatchItem> Debugger_RefreshLocals(ScriptExecutionContext context)
    {
      List<WatchItem> watchItemList = new List<WatchItem>();
      CallStackItem callStackItem = this.m_ExecutionStack.Peek();
      if (callStackItem != null && callStackItem.Debug_Symbols != null && callStackItem.LocalScope != null)
      {
        int num = Math.Min(callStackItem.Debug_Symbols.Length, callStackItem.LocalScope.Length);
        for (int index = 0; index < num; ++index)
          watchItemList.Add(new WatchItem()
          {
            IsError = false,
            LValue = callStackItem.Debug_Symbols[index],
            Value = callStackItem.LocalScope[index],
            Name = callStackItem.Debug_Symbols[index].i_Name
          });
      }
      return watchItemList;
    }

    private WatchItem Debugger_RefreshWatch(
      ScriptExecutionContext context,
      DynamicExpression dynExpr)
    {
      try
      {
        SymbolRef symbol = dynExpr.FindSymbol(context);
        DynValue dynValue = dynExpr.Evaluate(context);
        return new WatchItem()
        {
          IsError = dynExpr.IsConstant(),
          LValue = symbol,
          Value = dynValue,
          Name = dynExpr.ExpressionCode
        };
      }
      catch (Exception ex)
      {
        return new WatchItem()
        {
          IsError = true,
          Value = DynValue.NewString(ex.Message),
          Name = dynExpr.ExpressionCode
        };
      }
    }

    internal List<WatchItem> Debugger_GetCallStack(SourceRef startingRef)
    {
      List<WatchItem> callStack = new List<WatchItem>();
      for (int idxofs = 0; idxofs < this.m_ExecutionStack.Count; ++idxofs)
      {
        CallStackItem callStackItem = this.m_ExecutionStack.Peek(idxofs);
        Instruction instruction = this.m_RootChunk.Code[callStackItem.Debug_EntryPoint];
        string name = instruction.OpCode == OpCode.Meta ? instruction.Name : (string) null;
        if (callStackItem.ClrFunction != null)
          callStack.Add(new WatchItem()
          {
            Address = -1,
            BasePtr = -1,
            RetAddress = callStackItem.ReturnAddress,
            Location = startingRef,
            Name = callStackItem.ClrFunction.Name
          });
        else
          callStack.Add(new WatchItem()
          {
            Address = callStackItem.Debug_EntryPoint,
            BasePtr = callStackItem.BasePointer,
            RetAddress = callStackItem.ReturnAddress,
            Name = name,
            Location = startingRef
          });
        startingRef = callStackItem.CallingSourceRef;
        if (callStackItem.Continuation != null)
          callStack.Add(new WatchItem()
          {
            Name = callStackItem.Continuation.Name,
            Location = SourceRef.GetClrLocation()
          });
      }
      return callStack;
    }

    private SourceRef GetCurrentSourceRef(int instructionPtr) => instructionPtr >= 0 && instructionPtr < this.m_RootChunk.Code.Count ? this.m_RootChunk.Code[instructionPtr].SourceCodeRef : (SourceRef) null;

    private void FillDebugData(InterpreterException ex, int ip)
    {
      if (ip == -99)
        ip = this.m_SavedInstructionPtr;
      else
        --ip;
      ex.InstructionPtr = ip;
      SourceRef currentSourceRef = this.GetCurrentSourceRef(ip);
      ex.DecorateMessage(this.m_Script, currentSourceRef, ip);
      ex.CallStack = (IList<WatchItem>) this.Debugger_GetCallStack(currentSourceRef);
    }

    internal Table GetMetatable(DynValue value)
    {
      if (value.Type == DataType.Table)
        return value.Table.MetaTable;
      return value.Type.CanHaveTypeMetatables() ? this.m_Script.GetTypeMetatable(value.Type) : (Table) null;
    }

    internal DynValue GetBinaryMetamethod(DynValue op1, DynValue op2, string eventName)
    {
      Table metatable1 = this.GetMetatable(op1);
      if (metatable1 != null)
      {
        DynValue binaryMetamethod = metatable1.RawGet(eventName);
        if (binaryMetamethod != null && binaryMetamethod.IsNotNil())
          return binaryMetamethod;
      }
      Table metatable2 = this.GetMetatable(op2);
      if (metatable2 != null)
      {
        DynValue binaryMetamethod = metatable2.RawGet(eventName);
        if (binaryMetamethod != null && binaryMetamethod.IsNotNil())
          return binaryMetamethod;
      }
      if (op1.Type == DataType.UserData)
      {
        DynValue binaryMetamethod = op1.UserData.Descriptor.MetaIndex(this.m_Script, op1.UserData.Object, eventName);
        if (binaryMetamethod != null)
          return binaryMetamethod;
      }
      if (op2.Type == DataType.UserData)
      {
        DynValue binaryMetamethod = op2.UserData.Descriptor.MetaIndex(this.m_Script, op2.UserData.Object, eventName);
        if (binaryMetamethod != null)
          return binaryMetamethod;
      }
      return (DynValue) null;
    }

    internal DynValue GetMetamethod(DynValue value, string metamethod)
    {
      if (value.Type == DataType.UserData)
      {
        DynValue metamethod1 = value.UserData.Descriptor.MetaIndex(this.m_Script, value.UserData.Object, metamethod);
        if (metamethod1 != null)
          return metamethod1;
      }
      return this.GetMetamethodRaw(value, metamethod);
    }

    internal DynValue GetMetamethodRaw(DynValue value, string metamethod)
    {
      Table metatable = this.GetMetatable(value);
      if (metatable == null)
        return (DynValue) null;
      DynValue dynValue = metatable.RawGet(metamethod);
      return dynValue == null || dynValue.IsNil() ? (DynValue) null : dynValue;
    }

    internal Script GetScript() => this.m_Script;

    private DynValue Processing_Loop(int instructionPtr)
    {
      long num = 0;
      bool flag = this.AutoYieldCounter > 0L && this.m_CanYield && this.State != 0;
label_1:
      try
      {
        Instruction instruction;
        do
        {
          instruction = this.m_RootChunk.Code[instructionPtr];
          if (this.m_Debug.DebuggerAttached != null)
            this.ListenDebugger(instruction, instructionPtr);
          ++num;
          if (flag && num > this.AutoYieldCounter)
          {
            this.m_SavedInstructionPtr = instructionPtr;
            return DynValue.NewForcedYieldReq();
          }
          ++instructionPtr;
          switch (instruction.OpCode)
          {
            case OpCode.Nop:
            case OpCode.Debug:
            case OpCode.Meta:
              continue;
            case OpCode.Pop:
              this.m_ValueStack.RemoveLast(instruction.NumVal);
              continue;
            case OpCode.Copy:
              this.m_ValueStack.Push(this.m_ValueStack.Peek(instruction.NumVal));
              continue;
            case OpCode.Swap:
              this.ExecSwap(instruction);
              continue;
            case OpCode.Literal:
              this.m_ValueStack.Push(instruction.Value);
              continue;
            case OpCode.Closure:
              this.ExecClosure(instruction);
              continue;
            case OpCode.NewTable:
              if (instruction.NumVal == 0)
              {
                this.m_ValueStack.Push(DynValue.NewTable(this.m_Script));
                continue;
              }
              this.m_ValueStack.Push(DynValue.NewPrimeTable());
              continue;
            case OpCode.TblInitN:
              this.ExecTblInitN(instruction);
              continue;
            case OpCode.TblInitI:
              this.ExecTblInitI(instruction);
              continue;
            case OpCode.StoreLcl:
              this.ExecStoreLcl(instruction);
              continue;
            case OpCode.Local:
              this.m_ValueStack.Push(this.m_ExecutionStack.Peek().LocalScope[instruction.Symbol.i_Index].AsReadOnly());
              continue;
            case OpCode.StoreUpv:
              this.ExecStoreUpv(instruction);
              continue;
            case OpCode.Upvalue:
              this.m_ValueStack.Push(this.m_ExecutionStack.Peek().ClosureScope[instruction.Symbol.i_Index].AsReadOnly());
              continue;
            case OpCode.IndexSet:
            case OpCode.IndexSetN:
            case OpCode.IndexSetL:
              instructionPtr = this.ExecIndexSet(instruction, instructionPtr);
              continue;
            case OpCode.Index:
            case OpCode.IndexN:
            case OpCode.IndexL:
              instructionPtr = this.ExecIndex(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Clean:
              this.ClearBlockData(instruction);
              continue;
            case OpCode.BeginFn:
              this.ExecBeginFn(instruction);
              continue;
            case OpCode.Args:
              this.ExecArgs(instruction);
              continue;
            case OpCode.Call:
            case OpCode.ThisCall:
              instructionPtr = this.Internal_ExecCall(instruction.NumVal, instructionPtr, thisCall: instruction.OpCode == OpCode.ThisCall, debugText: instruction.Name);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Ret:
              instructionPtr = this.ExecRet(instruction);
              if (instructionPtr != -99)
              {
                if (instructionPtr >= 0)
                  continue;
                goto label_82;
              }
              else
                goto label_60;
            case OpCode.Jump:
              instructionPtr = instruction.NumVal;
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Jf:
              instructionPtr = this.JumpBool(instruction, false, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.JNil:
              DynValue scalar = this.m_ValueStack.Pop().ToScalar();
              if (scalar.Type == DataType.Nil || scalar.Type == DataType.Void)
                instructionPtr = instruction.NumVal;
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.JFor:
              instructionPtr = this.ExecJFor(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.JtOrPop:
            case OpCode.JfOrPop:
              instructionPtr = this.ExecShortCircuitingOperator(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Concat:
              instructionPtr = this.ExecConcat(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.LessEq:
              instructionPtr = this.ExecLessEq(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Less:
              instructionPtr = this.ExecLess(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Eq:
              instructionPtr = this.ExecEq(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Add:
              instructionPtr = this.ExecAdd(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Sub:
              instructionPtr = this.ExecSub(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Mul:
              instructionPtr = this.ExecMul(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Div:
              instructionPtr = this.ExecDiv(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Mod:
              instructionPtr = this.ExecMod(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Not:
              this.ExecNot(instruction);
              continue;
            case OpCode.Len:
              instructionPtr = this.ExecLen(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Neg:
              instructionPtr = this.ExecNeg(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.Power:
              instructionPtr = this.ExecPower(instruction, instructionPtr);
              if (instructionPtr != -99)
                continue;
              goto label_60;
            case OpCode.CNot:
              this.ExecCNot(instruction);
              continue;
            case OpCode.MkTuple:
              this.ExecMkTuple(instruction);
              continue;
            case OpCode.Scalar:
              this.m_ValueStack.Push(this.m_ValueStack.Pop().ToScalar());
              continue;
            case OpCode.Incr:
              this.ExecIncr(instruction);
              continue;
            case OpCode.ToNum:
              this.ExecToNum(instruction);
              continue;
            case OpCode.ToBool:
              this.m_ValueStack.Push(DynValue.NewBoolean(this.m_ValueStack.Pop().ToScalar().CastToBool()));
              continue;
            case OpCode.ExpTuple:
              this.ExecExpTuple(instruction);
              continue;
            case OpCode.IterPrep:
              this.ExecIterPrep(instruction);
              continue;
            case OpCode.IterUpd:
              this.ExecIterUpd(instruction);
              continue;
            case OpCode.Invalid:
              goto label_58;
            default:
              goto label_59;
          }
        }
        while (instructionPtr != -99);
        goto label_60;
label_58:
        throw new NotImplementedException(string.Format("Invalid opcode : {0}", (object) instruction.Name));
label_59:
        throw new NotImplementedException(string.Format("Execution for {0} not implented yet!", (object) instruction.OpCode));
label_60:
        DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
        if (this.m_CanYield)
          return scalar1;
        if (this.State == CoroutineState.Main)
          throw ScriptRuntimeException.CannotYieldMain();
        throw ScriptRuntimeException.CannotYield();
      }
      catch (InterpreterException ex)
      {
        this.FillDebugData(ex, instructionPtr);
        if (!(ex is ScriptRuntimeException))
        {
          ex.Rethrow();
          throw;
        }
        else
        {
          if (this.m_Debug.DebuggerAttached != null && this.m_Debug.DebuggerAttached.SignalRuntimeException((ScriptRuntimeException) ex) && instructionPtr >= 0 && instructionPtr < this.m_RootChunk.Code.Count)
            this.ListenDebugger(this.m_RootChunk.Code[instructionPtr], instructionPtr);
          for (int idxofs = 0; idxofs < this.m_ExecutionStack.Count; ++idxofs)
          {
            CallStackItem callStackItem = this.m_ExecutionStack.Peek(idxofs);
            if (callStackItem.ErrorHandlerBeforeUnwind != null)
              ex.DecoratedMessage = this.PerformMessageDecorationBeforeUnwind(callStackItem.ErrorHandlerBeforeUnwind, ex.DecoratedMessage, this.GetCurrentSourceRef(instructionPtr));
          }
          while (this.m_ExecutionStack.Count > 0)
          {
            CallStackItem basePointer = this.PopToBasePointer();
            if (basePointer.ErrorHandler != null)
            {
              instructionPtr = basePointer.ReturnAddress;
              if (basePointer.ClrFunction == null)
                this.m_ValueStack.RemoveLast((int) this.m_ValueStack.Pop().Number + 1);
              DynValue[] args = new DynValue[1]
              {
                DynValue.NewString(ex.DecoratedMessage)
              };
              this.m_ValueStack.Push(basePointer.ErrorHandler.Invoke(new ScriptExecutionContext(this, basePointer.ErrorHandler, this.GetCurrentSourceRef(instructionPtr)), (IList<DynValue>) args));
              goto label_1;
            }
            else if ((basePointer.Flags & CallStackItemFlags.EntryPoint) != CallStackItemFlags.None)
            {
              ex.Rethrow();
              throw;
            }
          }
          ex.Rethrow();
          throw;
        }
      }
label_82:
      return this.m_ValueStack.Pop();
    }

    internal string PerformMessageDecorationBeforeUnwind(
      DynValue messageHandler,
      string decoratedMessage,
      SourceRef sourceRef)
    {
      try
      {
        DynValue[] args = new DynValue[1]
        {
          DynValue.NewString(decoratedMessage)
        };
        DynValue nil = DynValue.Nil;
        DynValue dynValue;
        if (messageHandler.Type == DataType.Function)
        {
          dynValue = this.Call(messageHandler, args);
        }
        else
        {
          if (messageHandler.Type != DataType.ClrFunction)
            throw new ScriptRuntimeException("error handler not set to a function");
          ScriptExecutionContext executionContext = new ScriptExecutionContext(this, messageHandler.Callback, sourceRef);
          dynValue = messageHandler.Callback.Invoke(executionContext, (IList<DynValue>) args);
        }
        string printString = dynValue.ToPrintString();
        if (printString != null)
          return printString;
      }
      catch (ScriptRuntimeException ex)
      {
        return ex.Message + "\n" + decoratedMessage;
      }
      return decoratedMessage;
    }

    private void AssignLocal(SymbolRef symref, DynValue value)
    {
      CallStackItem callStackItem = this.m_ExecutionStack.Peek();
      DynValue dynValue = callStackItem.LocalScope[symref.i_Index];
      if (dynValue == null)
        callStackItem.LocalScope[symref.i_Index] = dynValue = DynValue.NewNil();
      dynValue.Assign(value);
    }

    private void ExecStoreLcl(Instruction i)
    {
      DynValue storeValue = this.GetStoreValue(i);
      this.AssignLocal(i.Symbol, storeValue);
    }

    private void ExecStoreUpv(Instruction i)
    {
      DynValue storeValue = this.GetStoreValue(i);
      SymbolRef symbol = i.Symbol;
      CallStackItem callStackItem = this.m_ExecutionStack.Peek();
      DynValue dynValue = callStackItem.ClosureScope[symbol.i_Index];
      if (dynValue == null)
        callStackItem.ClosureScope[symbol.i_Index] = dynValue = DynValue.NewNil();
      dynValue.Assign(storeValue);
    }

    private void ExecSwap(Instruction i)
    {
      DynValue dynValue1 = this.m_ValueStack.Peek(i.NumVal);
      DynValue dynValue2 = this.m_ValueStack.Peek(i.NumVal2);
      this.m_ValueStack.Set(i.NumVal, dynValue2);
      this.m_ValueStack.Set(i.NumVal2, dynValue1);
    }

    private DynValue GetStoreValue(Instruction i)
    {
      int numVal = i.NumVal;
      int numVal2 = i.NumVal2;
      DynValue dynValue = this.m_ValueStack.Peek(numVal);
      return dynValue.Type == DataType.Tuple ? (numVal2 >= dynValue.Tuple.Length ? DynValue.NewNil() : dynValue.Tuple[numVal2]) : (numVal2 != 0 ? DynValue.NewNil() : dynValue);
    }

    private void ExecClosure(Instruction i) => this.m_ValueStack.Push(DynValue.NewClosure(new Closure(this.m_Script, i.NumVal, i.SymbolList, (IEnumerable<DynValue>) ((IEnumerable<SymbolRef>) i.SymbolList).Select<SymbolRef, DynValue>((Func<SymbolRef, DynValue>) (s => this.GetUpvalueSymbol(s))).ToList<DynValue>())));

    private DynValue GetUpvalueSymbol(SymbolRef s)
    {
      if (s.Type == SymbolRefType.Local)
        return this.m_ExecutionStack.Peek().LocalScope[s.i_Index];
      if (s.Type == SymbolRefType.Upvalue)
        return this.m_ExecutionStack.Peek().ClosureScope[s.i_Index];
      throw new Exception("unsupported symbol type");
    }

    private void ExecMkTuple(Instruction i)
    {
      DynValue[] dynValueArray = this.Internal_AdjustTuple((IList<DynValue>) new Slice<DynValue>((IList<DynValue>) this.m_ValueStack, this.m_ValueStack.Count - i.NumVal, i.NumVal, false));
      this.m_ValueStack.RemoveLast(i.NumVal);
      this.m_ValueStack.Push(DynValue.NewTuple(dynValueArray));
    }

    private void ExecToNum(Instruction i) => this.m_ValueStack.Push(DynValue.NewNumber((this.m_ValueStack.Pop().ToScalar().CastToNumber() ?? throw ScriptRuntimeException.ConvertToNumberFailed(i.NumVal)).Value));

    private void ExecIterUpd(Instruction i)
    {
      DynValue dynValue = this.m_ValueStack.Peek();
      this.m_ValueStack.Peek(1).Tuple[2] = dynValue;
    }

    private void ExecExpTuple(Instruction i)
    {
      DynValue dynValue = this.m_ValueStack.Peek(i.NumVal);
      if (dynValue.Type == DataType.Tuple)
      {
        for (int index = 0; index < dynValue.Tuple.Length; ++index)
          this.m_ValueStack.Push(dynValue.Tuple[index]);
      }
      else
        this.m_ValueStack.Push(dynValue);
    }

    private void ExecIterPrep(Instruction i)
    {
      DynValue dynValue1 = this.m_ValueStack.Pop();
      if (dynValue1.Type != DataType.Tuple)
        dynValue1 = DynValue.NewTuple(dynValue1, DynValue.Nil, DynValue.Nil);
      DynValue dynValue2 = dynValue1.Tuple.Length >= 1 ? dynValue1.Tuple[0] : DynValue.Nil;
      DynValue dynValue3 = dynValue1.Tuple.Length >= 2 ? dynValue1.Tuple[1] : DynValue.Nil;
      DynValue dynValue4 = dynValue1.Tuple.Length >= 3 ? dynValue1.Tuple[2] : DynValue.Nil;
      if (dynValue2.Type != DataType.Function && dynValue2.Type != DataType.ClrFunction)
      {
        DynValue metamethod1 = this.GetMetamethod(dynValue2, "__iterator");
        if (metamethod1 != null && !metamethod1.IsNil())
        {
          DynValue dynValue5;
          if (metamethod1.Type != DataType.Tuple)
            dynValue5 = this.GetScript().Call(metamethod1, new DynValue[3]
            {
              dynValue2,
              dynValue3,
              dynValue4
            });
          else
            dynValue5 = metamethod1;
          dynValue2 = dynValue5.Tuple.Length >= 1 ? dynValue5.Tuple[0] : DynValue.Nil;
          dynValue3 = dynValue5.Tuple.Length >= 2 ? dynValue5.Tuple[1] : DynValue.Nil;
          dynValue4 = dynValue5.Tuple.Length >= 3 ? dynValue5.Tuple[2] : DynValue.Nil;
          this.m_ValueStack.Push(DynValue.NewTuple(dynValue2, dynValue3, dynValue4));
        }
        else if (dynValue2.Type == DataType.Table)
        {
          DynValue metamethod2 = this.GetMetamethod(dynValue2, "__call");
          if (metamethod2 == null || metamethod2.IsNil())
            this.m_ValueStack.Push(EnumerableWrapper.ConvertTable(dynValue2.Table));
        }
      }
      this.m_ValueStack.Push(DynValue.NewTuple(dynValue2, dynValue3, dynValue4));
    }

    private int ExecJFor(Instruction i, int instructionPtr)
    {
      double number1 = this.m_ValueStack.Peek().Number;
      double number2 = this.m_ValueStack.Peek(1).Number;
      double number3 = this.m_ValueStack.Peek(2).Number;
      return (number2 > 0.0 ? (number1 <= number3 ? 1 : 0) : (number1 >= number3 ? 1 : 0)) == 0 ? i.NumVal : instructionPtr;
    }

    private void ExecIncr(Instruction i)
    {
      DynValue dynValue1 = this.m_ValueStack.Peek();
      DynValue dynValue2 = this.m_ValueStack.Peek(i.NumVal);
      if (dynValue1.ReadOnly)
      {
        this.m_ValueStack.Pop();
        if (dynValue1.ReadOnly)
          dynValue1 = dynValue1.CloneAsWritable();
        this.m_ValueStack.Push(dynValue1);
      }
      dynValue1.AssignNumber(dynValue1.Number + dynValue2.Number);
    }

    private void ExecCNot(Instruction i)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      if (scalar2.Type != DataType.Boolean)
        throw new InternalErrorException("CNOT had non-bool arg");
      if (scalar2.CastToBool())
        this.m_ValueStack.Push(DynValue.NewBoolean(!scalar1.CastToBool()));
      else
        this.m_ValueStack.Push(DynValue.NewBoolean(scalar1.CastToBool()));
    }

    private void ExecNot(Instruction i) => this.m_ValueStack.Push(DynValue.NewBoolean(!this.m_ValueStack.Pop().ToScalar().CastToBool()));

    private void ExecBeginFn(Instruction i)
    {
      CallStackItem callStackItem = this.m_ExecutionStack.Peek();
      callStackItem.Debug_Symbols = i.SymbolList;
      callStackItem.LocalScope = new DynValue[i.NumVal];
      this.ClearBlockData(i);
    }

    private CallStackItem PopToBasePointer()
    {
      CallStackItem basePointer = this.m_ExecutionStack.Pop();
      if (basePointer.BasePointer >= 0)
        this.m_ValueStack.CropAtCount(basePointer.BasePointer);
      return basePointer;
    }

    private int PopExecStackAndCheckVStack(int vstackguard)
    {
      CallStackItem callStackItem = this.m_ExecutionStack.Pop();
      if (vstackguard != callStackItem.BasePointer)
        throw new InternalErrorException("StackGuard violation");
      return callStackItem.ReturnAddress;
    }

    private IList<DynValue> CreateArgsListForFunctionCall(int numargs, int offsFromTop)
    {
      if (numargs == 0)
        return (IList<DynValue>) new DynValue[0];
      DynValue dynValue = this.m_ValueStack.Peek(offsFromTop);
      if (dynValue.Type != DataType.Tuple || dynValue.Tuple.Length <= 1)
        return (IList<DynValue>) new Slice<DynValue>((IList<DynValue>) this.m_ValueStack, this.m_ValueStack.Count - numargs - offsFromTop, numargs, false);
      List<DynValue> listForFunctionCall = new List<DynValue>();
      for (int index = 0; index < numargs - 1; ++index)
        listForFunctionCall.Add(this.m_ValueStack.Peek(numargs - index - 1 + offsFromTop));
      for (int index = 0; index < dynValue.Tuple.Length; ++index)
        listForFunctionCall.Add(dynValue.Tuple[index]);
      return (IList<DynValue>) listForFunctionCall;
    }

    private void ExecArgs(Instruction I)
    {
      IList<DynValue> listForFunctionCall = this.CreateArgsListForFunctionCall((int) this.m_ValueStack.Peek().Number, 1);
      for (int index1 = 0; index1 < I.SymbolList.Length; ++index1)
      {
        if (index1 >= listForFunctionCall.Count)
          this.AssignLocal(I.SymbolList[index1], DynValue.NewNil());
        else if (index1 == I.SymbolList.Length - 1 && I.SymbolList[index1].i_Name == "...")
        {
          int length = listForFunctionCall.Count - index1;
          DynValue[] values = new DynValue[length];
          int index2 = 0;
          while (index2 < length)
          {
            values[index2] = listForFunctionCall[index1].ToScalar().CloneAsWritable();
            ++index2;
            ++index1;
          }
          this.AssignLocal(I.SymbolList[I.SymbolList.Length - 1], DynValue.NewTuple(this.Internal_AdjustTuple((IList<DynValue>) values)));
        }
        else
          this.AssignLocal(I.SymbolList[index1], listForFunctionCall[index1].ToScalar().CloneAsWritable());
      }
    }

    private int Internal_ExecCall(
      int argsCount,
      int instructionPtr,
      CallbackFunction handler = null,
      CallbackFunction continuation = null,
      bool thisCall = false,
      string debugText = null,
      DynValue unwindHandler = null)
    {
      DynValue dynValue1 = this.m_ValueStack.Peek(argsCount);
      CallStackItemFlags callStackItemFlags = thisCall ? CallStackItemFlags.MethodCall : CallStackItemFlags.None;
      if ((this.m_ExecutionStack.Count > this.m_Script.Options.TailCallOptimizationThreshold && this.m_ExecutionStack.Count > 1 || this.m_ValueStack.Count > this.m_Script.Options.TailCallOptimizationThreshold && this.m_ValueStack.Count > 1) && instructionPtr >= 0 && instructionPtr < this.m_RootChunk.Code.Count)
      {
        Instruction instruction = this.m_RootChunk.Code[instructionPtr];
        if (instruction.OpCode == OpCode.Ret && instruction.NumVal == 1)
        {
          CallStackItem callStackItem = this.m_ExecutionStack.Peek();
          if (callStackItem.ClrFunction == null && callStackItem.Continuation == null && callStackItem.ErrorHandler == null && callStackItem.ErrorHandlerBeforeUnwind == null && continuation == null && unwindHandler == null && handler == null)
          {
            instructionPtr = this.PerformTCO(instructionPtr, argsCount);
            callStackItemFlags |= CallStackItemFlags.TailCall;
          }
        }
      }
      if (dynValue1.Type == DataType.ClrFunction)
      {
        IList<DynValue> listForFunctionCall = this.CreateArgsListForFunctionCall(argsCount, 0);
        SourceRef currentSourceRef = this.GetCurrentSourceRef(instructionPtr);
        this.m_ExecutionStack.Push(new CallStackItem()
        {
          ClrFunction = dynValue1.Callback,
          ReturnAddress = instructionPtr,
          CallingSourceRef = currentSourceRef,
          BasePointer = -1,
          ErrorHandler = handler,
          Continuation = continuation,
          ErrorHandlerBeforeUnwind = unwindHandler,
          Flags = callStackItemFlags
        });
        DynValue dynValue2 = dynValue1.Callback.Invoke(new ScriptExecutionContext(this, dynValue1.Callback, currentSourceRef), listForFunctionCall, thisCall);
        this.m_ValueStack.RemoveLast(argsCount + 1);
        this.m_ValueStack.Push(dynValue2);
        this.m_ExecutionStack.Pop();
        return this.Internal_CheckForTailRequests((Instruction) null, instructionPtr);
      }
      if (dynValue1.Type == DataType.Function)
      {
        this.m_ValueStack.Push(DynValue.NewNumber((double) argsCount));
        this.m_ExecutionStack.Push(new CallStackItem()
        {
          BasePointer = this.m_ValueStack.Count,
          ReturnAddress = instructionPtr,
          Debug_EntryPoint = dynValue1.Function.EntryPointByteCodeLocation,
          CallingSourceRef = this.GetCurrentSourceRef(instructionPtr),
          ClosureScope = dynValue1.Function.ClosureContext,
          ErrorHandler = handler,
          Continuation = continuation,
          ErrorHandlerBeforeUnwind = unwindHandler,
          Flags = callStackItemFlags
        });
        return dynValue1.Function.EntryPointByteCodeLocation;
      }
      DynValue metamethod = this.GetMetamethod(dynValue1, "__call");
      if (metamethod == null || !metamethod.IsNotNil())
        throw ScriptRuntimeException.AttemptToCallNonFunc(dynValue1.Type, debugText);
      DynValue[] dynValueArray = new DynValue[argsCount + 1];
      for (int index = 0; index < argsCount + 1; ++index)
        dynValueArray[index] = this.m_ValueStack.Pop();
      this.m_ValueStack.Push(metamethod);
      for (int index = argsCount; index >= 0; --index)
        this.m_ValueStack.Push(dynValueArray[index]);
      return this.Internal_ExecCall(argsCount + 1, instructionPtr, handler, continuation);
    }

    private int PerformTCO(int instructionPtr, int argsCount)
    {
      DynValue[] dynValueArray = new DynValue[argsCount + 1];
      for (int index = 0; index <= argsCount; ++index)
        dynValueArray[index] = this.m_ValueStack.Pop();
      int returnAddress = this.PopToBasePointer().ReturnAddress;
      this.m_ValueStack.RemoveLast((int) this.m_ValueStack.Pop().Number + 1);
      for (int index = argsCount; index >= 0; --index)
        this.m_ValueStack.Push(dynValueArray[index]);
      return returnAddress;
    }

    private int ExecRet(Instruction i)
    {
      CallStackItem basePointer;
      int num;
      if (i.NumVal == 0)
      {
        basePointer = this.PopToBasePointer();
        num = basePointer.ReturnAddress;
        this.m_ValueStack.RemoveLast((int) this.m_ValueStack.Pop().Number + 1);
        this.m_ValueStack.Push(DynValue.Void);
      }
      else
      {
        if (i.NumVal != 1)
          throw new InternalErrorException("RET supports only 0 and 1 ret val scenarios");
        DynValue dynValue = this.m_ValueStack.Pop();
        basePointer = this.PopToBasePointer();
        int returnAddress = basePointer.ReturnAddress;
        this.m_ValueStack.RemoveLast((int) this.m_ValueStack.Pop().Number + 1);
        this.m_ValueStack.Push(dynValue);
        num = this.Internal_CheckForTailRequests(i, returnAddress);
      }
      if (basePointer.Continuation != null)
        this.m_ValueStack.Push(basePointer.Continuation.Invoke(new ScriptExecutionContext(this, basePointer.Continuation, i.SourceCodeRef), (IList<DynValue>) new DynValue[1]
        {
          this.m_ValueStack.Pop()
        }));
      return num;
    }

    private int Internal_CheckForTailRequests(Instruction i, int instructionPtr)
    {
      DynValue dynValue = this.m_ValueStack.Peek();
      if (dynValue.Type == DataType.TailCallRequest)
      {
        this.m_ValueStack.Pop();
        TailCallData tailCallData = dynValue.TailCallData;
        this.m_ValueStack.Push(tailCallData.Function);
        for (int index = 0; index < tailCallData.Args.Length; ++index)
          this.m_ValueStack.Push(tailCallData.Args[index]);
        return this.Internal_ExecCall(tailCallData.Args.Length, instructionPtr, tailCallData.ErrorHandler, tailCallData.Continuation, unwindHandler: tailCallData.ErrorHandlerBeforeUnwind);
      }
      if (dynValue.Type != DataType.YieldRequest)
        return instructionPtr;
      this.m_SavedInstructionPtr = instructionPtr;
      return -99;
    }

    private int JumpBool(Instruction i, bool expectedValueForJump, int instructionPtr) => this.m_ValueStack.Pop().ToScalar().CastToBool() == expectedValueForJump ? i.NumVal : instructionPtr;

    private int ExecShortCircuitingOperator(Instruction i, int instructionPtr)
    {
      bool flag = i.OpCode == OpCode.JtOrPop;
      if (this.m_ValueStack.Peek().ToScalar().CastToBool() == flag)
        return i.NumVal;
      this.m_ValueStack.Pop();
      return instructionPtr;
    }

    private int ExecAdd(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(number2.Value + number1.Value));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__add", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecSub(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(number2.Value - number1.Value));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__sub", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecMul(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(number2.Value * number1.Value));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__mul", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecMod(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        double num = Math.IEEERemainder(number2.Value, number1.Value);
        if (num < 0.0)
          num += number1.Value;
        this.m_ValueStack.Push(DynValue.NewNumber(num));
        return instructionPtr;
      }
      int num1 = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__mod", instructionPtr);
      return num1 >= 0 ? num1 : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecDiv(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(number2.Value / number1.Value));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__div", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecPower(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      double? number1 = scalar1.CastToNumber();
      double? number2 = scalar2.CastToNumber();
      if (number2.HasValue && number1.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(Math.Pow(number2.Value, number1.Value)));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__pow", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar2, scalar1);
    }

    private int ExecNeg(Instruction i, int instructionPtr)
    {
      DynValue scalar = this.m_ValueStack.Pop().ToScalar();
      double? number = scalar.CastToNumber();
      if (number.HasValue)
      {
        this.m_ValueStack.Push(DynValue.NewNumber(-number.Value));
        return instructionPtr;
      }
      int num = this.Internal_InvokeUnaryMetaMethod(scalar, "__unm", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ArithmeticOnNonNumber(scalar);
    }

    private int ExecEq(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      if (scalar1 == scalar2)
      {
        this.m_ValueStack.Push(DynValue.True);
        return instructionPtr;
      }
      if (scalar2.Type == DataType.UserData || scalar1.Type == DataType.UserData)
      {
        int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__eq", instructionPtr);
        if (num >= 0)
          return num;
      }
      if (scalar1.Type != scalar2.Type)
      {
        if (scalar2.Type == DataType.Nil && scalar1.Type == DataType.Void || scalar2.Type == DataType.Void && scalar1.Type == DataType.Nil)
          this.m_ValueStack.Push(DynValue.True);
        else
          this.m_ValueStack.Push(DynValue.False);
        return instructionPtr;
      }
      if (scalar2.Type == DataType.Table && this.GetMetatable(scalar2) != null && this.GetMetatable(scalar2) == this.GetMetatable(scalar1))
      {
        int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__eq", instructionPtr);
        if (num >= 0)
          return num;
      }
      this.m_ValueStack.Push(DynValue.NewBoolean(scalar1.Equals((object) scalar2)));
      return instructionPtr;
    }

    private int ExecLess(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      if (scalar2.Type == DataType.Number && scalar1.Type == DataType.Number)
        this.m_ValueStack.Push(DynValue.NewBoolean(scalar2.Number < scalar1.Number));
      else if (scalar2.Type == DataType.String && scalar1.Type == DataType.String)
      {
        this.m_ValueStack.Push(DynValue.NewBoolean(scalar2.String.CompareTo(scalar1.String) < 0));
      }
      else
      {
        int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__lt", instructionPtr);
        return num >= 0 ? num : throw ScriptRuntimeException.CompareInvalidType(scalar2, scalar1);
      }
      return instructionPtr;
    }

    private int ExecLessEq(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      if (scalar2.Type == DataType.Number && scalar1.Type == DataType.Number)
      {
        this.m_ValueStack.Push(DynValue.False);
        this.m_ValueStack.Push(DynValue.NewBoolean(scalar2.Number <= scalar1.Number));
      }
      else if (scalar2.Type == DataType.String && scalar1.Type == DataType.String)
      {
        this.m_ValueStack.Push(DynValue.False);
        this.m_ValueStack.Push(DynValue.NewBoolean(scalar2.String.CompareTo(scalar1.String) <= 0));
      }
      else
      {
        int num1 = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__le", instructionPtr, DynValue.False);
        if (num1 >= 0)
          return num1;
        int num2 = this.Internal_InvokeBinaryMetaMethod(scalar1, scalar2, "__lt", instructionPtr, DynValue.True);
        return num2 >= 0 ? num2 : throw ScriptRuntimeException.CompareInvalidType(scalar2, scalar1);
      }
      return instructionPtr;
    }

    private int ExecLen(Instruction i, int instructionPtr)
    {
      DynValue scalar = this.m_ValueStack.Pop().ToScalar();
      if (scalar.Type == DataType.String)
      {
        this.m_ValueStack.Push(DynValue.NewNumber((double) scalar.String.Length));
      }
      else
      {
        int num = this.Internal_InvokeUnaryMetaMethod(scalar, "__len", instructionPtr);
        if (num >= 0)
          return num;
        if (scalar.Type != DataType.Table)
          throw ScriptRuntimeException.LenOnInvalidType(scalar);
        this.m_ValueStack.Push(DynValue.NewNumber((double) scalar.Table.Length));
      }
      return instructionPtr;
    }

    private int ExecConcat(Instruction i, int instructionPtr)
    {
      DynValue scalar1 = this.m_ValueStack.Pop().ToScalar();
      DynValue scalar2 = this.m_ValueStack.Pop().ToScalar();
      string str1 = scalar1.CastToString();
      string str2 = scalar2.CastToString();
      if (str1 != null && str2 != null)
      {
        this.m_ValueStack.Push(DynValue.NewString(str2 + str1));
        return instructionPtr;
      }
      int num = this.Internal_InvokeBinaryMetaMethod(scalar2, scalar1, "__concat", instructionPtr);
      return num >= 0 ? num : throw ScriptRuntimeException.ConcatOnNonString(scalar2, scalar1);
    }

    private void ExecTblInitI(Instruction i)
    {
      DynValue val = this.m_ValueStack.Pop();
      DynValue dynValue = this.m_ValueStack.Peek();
      if (dynValue.Type != DataType.Table)
        throw new InternalErrorException("Unexpected type in table ctor : {0}", new object[1]
        {
          (object) dynValue
        });
      dynValue.Table.InitNextArrayKeys(val, i.NumVal != 0);
    }

    private void ExecTblInitN(Instruction i)
    {
      DynValue dynValue1 = this.m_ValueStack.Pop();
      DynValue key = this.m_ValueStack.Pop();
      DynValue dynValue2 = this.m_ValueStack.Peek();
      if (dynValue2.Type != DataType.Table)
        throw new InternalErrorException("Unexpected type in table ctor : {0}", new object[1]
        {
          (object) dynValue2
        });
      dynValue2.Table.Set(key, dynValue1.ToScalar());
    }

    private int ExecIndexSet(Instruction i, int instructionPtr)
    {
      int num = 100;
      bool isDirectIndexing = i.OpCode == OpCode.IndexSetN;
      bool flag = i.OpCode == OpCode.IndexSetL;
      DynValue index = i.Value ?? this.m_ValueStack.Pop();
      DynValue scalar = index.ToScalar();
      DynValue dynValue1 = this.m_ValueStack.Pop().ToScalar();
      DynValue storeValue = this.GetStoreValue(i);
      DynValue dynValue2 = (DynValue) null;
      while (num > 0)
      {
        --num;
        DynValue metamethodRaw;
        if (dynValue1.Type == DataType.Table)
        {
          if (!flag && !dynValue1.Table.Get(scalar).IsNil())
          {
            dynValue1.Table.Set(scalar, storeValue);
            return instructionPtr;
          }
          metamethodRaw = this.GetMetamethodRaw(dynValue1, "__newindex");
          if (metamethodRaw == null || metamethodRaw.IsNil())
          {
            if (flag)
              throw new ScriptRuntimeException("cannot multi-index a table. userdata expected");
            dynValue1.Table.Set(scalar, storeValue);
            return instructionPtr;
          }
        }
        else
        {
          if (dynValue1.Type == DataType.UserData)
          {
            UserData userData = dynValue1.UserData;
            if (!userData.Descriptor.SetIndex(this.GetScript(), userData.Object, index, storeValue, isDirectIndexing))
              throw ScriptRuntimeException.UserDataMissingField(userData.Descriptor.Name, scalar.String);
            return instructionPtr;
          }
          metamethodRaw = this.GetMetamethodRaw(dynValue1, "__newindex");
          if (metamethodRaw == null || metamethodRaw.IsNil())
            throw ScriptRuntimeException.IndexType(dynValue1);
        }
        if (metamethodRaw.Type == DataType.Function || metamethodRaw.Type == DataType.ClrFunction)
        {
          if (flag)
            throw new ScriptRuntimeException("cannot multi-index through metamethods. userdata expected");
          this.m_ValueStack.Pop();
          this.m_ValueStack.Push(metamethodRaw);
          this.m_ValueStack.Push(dynValue1);
          this.m_ValueStack.Push(scalar);
          this.m_ValueStack.Push(storeValue);
          return this.Internal_ExecCall(3, instructionPtr);
        }
        dynValue1 = metamethodRaw;
        dynValue2 = (DynValue) null;
      }
      throw ScriptRuntimeException.LoopInNewIndex();
    }

    private int ExecIndex(Instruction i, int instructionPtr)
    {
      int num = 100;
      bool isDirectIndexing = i.OpCode == OpCode.IndexN;
      bool flag = i.OpCode == OpCode.IndexL;
      DynValue index = i.Value ?? this.m_ValueStack.Pop();
      DynValue scalar = index.ToScalar();
      DynValue dynValue1 = this.m_ValueStack.Pop().ToScalar();
      DynValue dynValue2 = (DynValue) null;
      while (num > 0)
      {
        --num;
        DynValue metamethodRaw;
        if (dynValue1.Type == DataType.Table)
        {
          if (!flag)
          {
            DynValue dynValue3 = dynValue1.Table.Get(scalar);
            if (!dynValue3.IsNil())
            {
              this.m_ValueStack.Push(dynValue3.AsReadOnly());
              return instructionPtr;
            }
          }
          metamethodRaw = this.GetMetamethodRaw(dynValue1, "__index");
          if (metamethodRaw == null || metamethodRaw.IsNil())
          {
            if (flag)
              throw new ScriptRuntimeException("cannot multi-index a table. userdata expected");
            this.m_ValueStack.Push(DynValue.Nil);
            return instructionPtr;
          }
        }
        else
        {
          if (dynValue1.Type == DataType.UserData)
          {
            UserData userData = dynValue1.UserData;
            this.m_ValueStack.Push((userData.Descriptor.Index(this.GetScript(), userData.Object, index, isDirectIndexing) ?? throw ScriptRuntimeException.UserDataMissingField(userData.Descriptor.Name, scalar.String)).AsReadOnly());
            return instructionPtr;
          }
          metamethodRaw = this.GetMetamethodRaw(dynValue1, "__index");
          if (metamethodRaw == null || metamethodRaw.IsNil())
            throw ScriptRuntimeException.IndexType(dynValue1);
        }
        if (metamethodRaw.Type == DataType.Function || metamethodRaw.Type == DataType.ClrFunction)
        {
          if (flag)
            throw new ScriptRuntimeException("cannot multi-index through metamethods. userdata expected");
          this.m_ValueStack.Push(metamethodRaw);
          this.m_ValueStack.Push(dynValue1);
          this.m_ValueStack.Push(scalar);
          return this.Internal_ExecCall(2, instructionPtr);
        }
        dynValue1 = metamethodRaw;
        dynValue2 = (DynValue) null;
      }
      throw ScriptRuntimeException.LoopInIndex();
    }

    private void ClearBlockData(Instruction I)
    {
      int numVal = I.NumVal;
      int numVal2 = I.NumVal2;
      DynValue[] localScope = this.m_ExecutionStack.Peek().LocalScope;
      if (numVal2 < 0 || numVal < 0 || numVal2 < numVal)
        return;
      Array.Clear((Array) localScope, numVal, numVal2 - numVal + 1);
    }

    public DynValue GetGenericSymbol(SymbolRef symref)
    {
      switch (symref.i_Type)
      {
        case SymbolRefType.Local:
          return this.GetTopNonClrFunction().LocalScope[symref.i_Index];
        case SymbolRefType.Upvalue:
          return this.GetTopNonClrFunction().ClosureScope[symref.i_Index];
        case SymbolRefType.Global:
          return this.GetGlobalSymbol(this.GetGenericSymbol(symref.i_Env), symref.i_Name);
        case SymbolRefType.DefaultEnv:
          return DynValue.NewTable(this.GetScript().Globals);
        default:
          throw new InternalErrorException("Unexpected {0} LRef at resolution: {1}", new object[2]
          {
            (object) symref.i_Type,
            (object) symref.i_Name
          });
      }
    }

    private DynValue GetGlobalSymbol(DynValue dynValue, string name)
    {
      if (dynValue.Type != DataType.Table)
        throw new InvalidOperationException(string.Format("_ENV is not a table but a {0}", (object) dynValue.Type));
      return dynValue.Table.Get(name);
    }

    private void SetGlobalSymbol(DynValue dynValue, string name, DynValue value)
    {
      if (dynValue.Type != DataType.Table)
        throw new InvalidOperationException(string.Format("_ENV is not a table but a {0}", (object) dynValue.Type));
      dynValue.Table.Set(name, value ?? DynValue.Nil);
    }

    public void AssignGenericSymbol(SymbolRef symref, DynValue value)
    {
      switch (symref.i_Type)
      {
        case SymbolRefType.Local:
          CallStackItem topNonClrFunction1 = this.GetTopNonClrFunction();
          DynValue dynValue1 = topNonClrFunction1.LocalScope[symref.i_Index];
          if (dynValue1 == null)
            topNonClrFunction1.LocalScope[symref.i_Index] = dynValue1 = DynValue.NewNil();
          dynValue1.Assign(value);
          break;
        case SymbolRefType.Upvalue:
          CallStackItem topNonClrFunction2 = this.GetTopNonClrFunction();
          DynValue dynValue2 = topNonClrFunction2.ClosureScope[symref.i_Index];
          if (dynValue2 == null)
            topNonClrFunction2.ClosureScope[symref.i_Index] = dynValue2 = DynValue.NewNil();
          dynValue2.Assign(value);
          break;
        case SymbolRefType.Global:
          this.SetGlobalSymbol(this.GetGenericSymbol(symref.i_Env), symref.i_Name, value);
          break;
        case SymbolRefType.DefaultEnv:
          throw new ArgumentException("Can't AssignGenericSymbol on a DefaultEnv symbol");
        default:
          throw new InternalErrorException("Unexpected {0} LRef at resolution: {1}", new object[2]
          {
            (object) symref.i_Type,
            (object) symref.i_Name
          });
      }
    }

    private CallStackItem GetTopNonClrFunction()
    {
      CallStackItem topNonClrFunction = (CallStackItem) null;
      for (int idxofs = 0; idxofs < this.m_ExecutionStack.Count; ++idxofs)
      {
        topNonClrFunction = this.m_ExecutionStack.Peek(idxofs);
        if (topNonClrFunction.ClrFunction == null)
          break;
      }
      return topNonClrFunction;
    }

    public SymbolRef FindSymbolByName(string name)
    {
      if (this.m_ExecutionStack.Count > 0)
      {
        CallStackItem topNonClrFunction = this.GetTopNonClrFunction();
        if (topNonClrFunction != null)
        {
          if (topNonClrFunction.Debug_Symbols != null)
          {
            for (int index = topNonClrFunction.Debug_Symbols.Length - 1; index >= 0; --index)
            {
              SymbolRef debugSymbol = topNonClrFunction.Debug_Symbols[index];
              if (debugSymbol.i_Name == name && topNonClrFunction.LocalScope[index] != null)
                return debugSymbol;
            }
          }
          ClosureContext closureScope = topNonClrFunction.ClosureScope;
          if (closureScope != null)
          {
            for (int index = 0; index < closureScope.Symbols.Length; ++index)
            {
              if (closureScope.Symbols[index] == name)
                return SymbolRef.Upvalue(name, index);
            }
          }
        }
      }
      if (!(name != "_ENV"))
        return SymbolRef.DefaultEnv;
      SymbolRef symbolByName = this.FindSymbolByName("_ENV");
      return SymbolRef.Global(name, symbolByName);
    }

    private DynValue[] Internal_AdjustTuple(IList<DynValue> values)
    {
      if (values == null || values.Count == 0)
        return new DynValue[0];
      if (values[values.Count - 1].Type == DataType.Tuple)
      {
        DynValue[] values1 = new DynValue[values.Count - 1 + values[values.Count - 1].Tuple.Length];
        for (int index = 0; index < values.Count - 1; ++index)
          values1[index] = values[index].ToScalar();
        for (int index = 0; index < values[values.Count - 1].Tuple.Length; ++index)
          values1[values.Count + index - 1] = values[values.Count - 1].Tuple[index];
        return values1[values1.Length - 1].Type == DataType.Tuple ? this.Internal_AdjustTuple((IList<DynValue>) values1) : values1;
      }
      DynValue[] dynValueArray = new DynValue[values.Count];
      for (int index = 0; index < values.Count; ++index)
        dynValueArray[index] = values[index].ToScalar();
      return dynValueArray;
    }

    private int Internal_InvokeUnaryMetaMethod(DynValue op1, string eventName, int instructionPtr)
    {
      DynValue dynValue1 = (DynValue) null;
      if (op1.Type == DataType.UserData)
        dynValue1 = op1.UserData.Descriptor.MetaIndex(this.m_Script, op1.UserData.Object, eventName);
      if (dynValue1 == null)
      {
        Table metatable = this.GetMetatable(op1);
        if (metatable != null)
        {
          DynValue dynValue2 = metatable.RawGet(eventName);
          if (dynValue2 != null && dynValue2.IsNotNil())
            dynValue1 = dynValue2;
        }
      }
      if (dynValue1 == null)
        return -1;
      this.m_ValueStack.Push(dynValue1);
      this.m_ValueStack.Push(op1);
      return this.Internal_ExecCall(1, instructionPtr);
    }

    private int Internal_InvokeBinaryMetaMethod(
      DynValue l,
      DynValue r,
      string eventName,
      int instructionPtr,
      DynValue extraPush = null)
    {
      DynValue binaryMetamethod = this.GetBinaryMetamethod(l, r, eventName);
      if (binaryMetamethod == null)
        return -1;
      if (extraPush != null)
        this.m_ValueStack.Push(extraPush);
      this.m_ValueStack.Push(binaryMetamethod);
      this.m_ValueStack.Push(l);
      this.m_ValueStack.Push(r);
      return this.Internal_ExecCall(2, instructionPtr);
    }

    private DynValue[] StackTopToArray(int items, bool pop)
    {
      DynValue[] array = new DynValue[items];
      if (pop)
      {
        for (int index = 0; index < items; ++index)
          array[index] = this.m_ValueStack.Pop();
      }
      else
      {
        for (int index = 0; index < items; ++index)
          array[index] = this.m_ValueStack[this.m_ValueStack.Count - 1 - index];
      }
      return array;
    }

    private DynValue[] StackTopToArrayReverse(int items, bool pop)
    {
      DynValue[] arrayReverse = new DynValue[items];
      if (pop)
      {
        for (int index = 0; index < items; ++index)
          arrayReverse[items - 1 - index] = this.m_ValueStack.Pop();
      }
      else
      {
        for (int index = 0; index < items; ++index)
          arrayReverse[items - 1 - index] = this.m_ValueStack[this.m_ValueStack.Count - 1 - index];
      }
      return arrayReverse;
    }

    private class DebugContext
    {
      public bool DebuggerEnabled = true;
      public IDebugger DebuggerAttached;
      public DebuggerAction.ActionType DebuggerCurrentAction = DebuggerAction.ActionType.None;
      public int DebuggerCurrentActionTarget = -1;
      public SourceRef LastHlRef;
      public int ExStackDepthAtStep = -1;
      public List<SourceRef> BreakPoints = new List<SourceRef>();
      public bool LineBasedBreakPoints;
    }
  }
}
