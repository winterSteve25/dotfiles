// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Interop.Converters.ClrToScriptConversions
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace MoonSharp.Interpreter.Interop.Converters
{
  internal static class ClrToScriptConversions
  {
    /// <summary>
    /// Tries to convert a CLR object to a MoonSharp value, using "trivial" logic.
    /// Skips on custom conversions, etc.
    /// Does NOT throw on failure.
    /// </summary>
    internal static DynValue TryObjectToTrivialDynValue(Script script, object obj)
    {
      if (obj == null)
        return DynValue.Nil;
      if (obj is DynValue)
        return (DynValue) obj;
      Type type = obj.GetType();
      if (obj is bool v)
        return DynValue.NewBoolean(v);
      if (obj is string || obj is StringBuilder || obj is char)
        return DynValue.NewString(obj.ToString());
      if (NumericConversions.NumericTypes.Contains(type))
        return DynValue.NewNumber(NumericConversions.TypeToDouble(type, obj));
      return obj is Table ? DynValue.NewTable((Table) obj) : (DynValue) null;
    }

    /// <summary>
    /// Tries to convert a CLR object to a MoonSharp value, using "simple" logic.
    /// Does NOT throw on failure.
    /// </summary>
    internal static DynValue TryObjectToSimpleDynValue(Script script, object obj)
    {
      if (obj == null)
        return DynValue.Nil;
      if (obj is DynValue)
        return (DynValue) obj;
      Func<Script, object, DynValue> customConversion = Script.GlobalOptions.CustomConverters.GetClrToScriptCustomConversion(obj.GetType());
      if (customConversion != null)
      {
        DynValue simpleDynValue = customConversion(script, obj);
        if (simpleDynValue != null)
          return simpleDynValue;
      }
      Type type = obj.GetType();
      switch (obj)
      {
        case bool v:
          return DynValue.NewBoolean(v);
        case string _:
        case StringBuilder _:
        case char _:
          return DynValue.NewString(obj.ToString());
        case Closure _:
          return DynValue.NewClosure((Closure) obj);
        default:
          if (NumericConversions.NumericTypes.Contains(type))
            return DynValue.NewNumber(NumericConversions.TypeToDouble(type, obj));
          if (obj is Table)
            return DynValue.NewTable((Table) obj);
          if (obj is CallbackFunction)
            return DynValue.NewCallback((CallbackFunction) obj);
          if ((object) (obj as Delegate) != null)
          {
            Delegate @delegate = (Delegate) obj;
            if (CallbackFunction.CheckCallbackSignature(@delegate.GetMethodInfo(), false))
              return DynValue.NewCallback((Func<ScriptExecutionContext, CallbackArguments, DynValue>) @delegate);
          }
          return (DynValue) null;
      }
    }

    /// <summary>
    /// Tries to convert a CLR object to a MoonSharp value, using more in-depth analysis
    /// </summary>
    internal static DynValue ObjectToDynValue(Script script, object obj)
    {
      DynValue simpleDynValue = ClrToScriptConversions.TryObjectToSimpleDynValue(script, obj);
      if (simpleDynValue != null)
        return simpleDynValue;
      DynValue dynValue = UserData.Create(obj);
      if (dynValue != null)
        return dynValue;
      if ((object) (obj as Type) != null)
        dynValue = UserData.CreateStatic(obj as Type);
      if (obj is Enum)
        return DynValue.NewNumber(NumericConversions.TypeToDouble(Enum.GetUnderlyingType(obj.GetType()), obj));
      if (dynValue != null)
        return dynValue;
      if ((object) (obj as Delegate) != null)
        return DynValue.NewCallback(CallbackFunction.FromDelegate(script, (Delegate) obj));
      if ((object) (obj as MethodInfo) != null)
      {
        MethodInfo mi = (MethodInfo) obj;
        if (mi.IsStatic)
          return DynValue.NewCallback(CallbackFunction.FromMethodInfo(script, mi));
      }
      switch (obj)
      {
        case IList _:
          return DynValue.NewTable(TableConversions.ConvertIListToTable(script, (IList) obj));
        case IDictionary _:
          return DynValue.NewTable(TableConversions.ConvertIDictionaryToTable(script, (IDictionary) obj));
        default:
          return ClrToScriptConversions.EnumerationToDynValue(script, obj) ?? throw ScriptRuntimeException.ConvertObjectFailed(obj);
      }
    }

    /// <summary>Converts an IEnumerable or IEnumerator to a DynValue</summary>
    /// <param name="script">The script.</param>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static DynValue EnumerationToDynValue(Script script, object obj)
    {
      switch (obj)
      {
        case IEnumerable _:
          IEnumerable enumerable = (IEnumerable) obj;
          return EnumerableWrapper.ConvertIterator(script, enumerable.GetEnumerator());
        case IEnumerator _:
          IEnumerator enumerator = (IEnumerator) obj;
          return EnumerableWrapper.ConvertIterator(script, enumerator);
        default:
          return (DynValue) null;
      }
    }
  }
}
