// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.Table
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.DataStructs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MoonSharp.Interpreter
{
  /// <summary>A class representing a Lua table.</summary>
  public class Table : RefIdObject, IScriptPrivateResource
  {
    private readonly LinkedList<TablePair> m_Values;
    private readonly LinkedListIndex<DynValue, TablePair> m_ValueMap;
    private readonly LinkedListIndex<string, TablePair> m_StringMap;
    private readonly LinkedListIndex<int, TablePair> m_ArrayMap;
    private readonly Script m_Owner;
    private int m_InitArray;
    private int m_CachedLength = -1;
    private bool m_ContainsNilEntries;
    private Table m_MetaTable;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.Table" /> class.
    /// </summary>
    /// <param name="owner">The owner script.</param>
    public Table(Script owner)
    {
      this.m_Values = new LinkedList<TablePair>();
      this.m_StringMap = new LinkedListIndex<string, TablePair>(this.m_Values);
      this.m_ArrayMap = new LinkedListIndex<int, TablePair>(this.m_Values);
      this.m_ValueMap = new LinkedListIndex<DynValue, TablePair>(this.m_Values);
      this.m_Owner = owner;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:MoonSharp.Interpreter.Table" /> class.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <param name="arrayValues">The values for the "array-like" part of the table.</param>
    public Table(Script owner, params DynValue[] arrayValues)
      : this(owner)
    {
      for (int index = 0; index < arrayValues.Length; ++index)
        this.Set(DynValue.NewNumber((double) (index + 1)), arrayValues[index]);
    }

    /// <summary>Gets the script owning this resource.</summary>
    public Script OwnerScript => this.m_Owner;

    /// <summary>Removes all items from the Table.</summary>
    public void Clear()
    {
      this.m_Values.Clear();
      this.m_StringMap.Clear();
      this.m_ArrayMap.Clear();
      this.m_ValueMap.Clear();
      this.m_CachedLength = -1;
    }

    /// <summary>Gets the integral key from a double.</summary>
    private int GetIntegralKey(double d)
    {
      int num = (int) d;
      return d >= 1.0 && d == (double) num ? num : -1;
    }

    /// <summary>
    /// Gets or sets the
    /// <see cref="T:System.Object" /> with the specified key(s).
    /// This will marshall CLR and MoonSharp objects in the best possible way.
    /// Multiple keys can be used to access subtables.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Object" />.
    /// </value>
    /// <param name="keys">The keys to access the table and subtables</param>
    public object this[params object[] keys]
    {
      get => this.Get(keys).ToObject();
      set => this.Set(keys, DynValue.FromObject(this.OwnerScript, value));
    }

    /// <summary>
    /// Gets or sets the <see cref="T:System.Object" /> with the specified key(s).
    /// This will marshall CLR and MoonSharp objects in the best possible way.
    /// </summary>
    /// <value>
    /// The <see cref="T:System.Object" />.
    /// </value>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public object this[object key]
    {
      get => this.Get(key).ToObject();
      set => this.Set(key, DynValue.FromObject(this.OwnerScript, value));
    }

    private Table ResolveMultipleKeys(object[] keys, out object key)
    {
      Table table = this;
      key = keys.Length != 0 ? keys[0] : (object) null;
      for (int index = 1; index < keys.Length; ++index)
      {
        DynValue dynValue = table.RawGet(key);
        if (dynValue == null)
          throw new ScriptRuntimeException("Key '{0}' did not point to anything");
        table = dynValue.Type == DataType.Table ? dynValue.Table : throw new ScriptRuntimeException("Key '{0}' did not point to a table");
        key = keys[index];
      }
      return table;
    }

    /// <summary>
    /// Append the value to the table using the next available integer index.
    /// </summary>
    /// <param name="value">The value.</param>
    public void Append(DynValue value)
    {
      this.CheckScriptOwnership(value);
      this.PerformTableSet<int>(this.m_ArrayMap, this.Length + 1, DynValue.NewNumber((double) (this.Length + 1)), value, true, this.Length + 1);
    }

    private void PerformTableSet<T>(
      LinkedListIndex<T, TablePair> listIndex,
      T key,
      DynValue keyDynValue,
      DynValue value,
      bool isNumber,
      int appendKey)
    {
      TablePair tablePair = listIndex.Set(key, new TablePair(keyDynValue, value));
      if (this.m_ContainsNilEntries && value.IsNotNil() && (tablePair.Value == null || tablePair.Value.IsNil()))
        this.CollectDeadKeys();
      else if (value.IsNil())
      {
        this.m_ContainsNilEntries = true;
        if (!isNumber)
          return;
        this.m_CachedLength = -1;
      }
      else
      {
        if (!isNumber || tablePair.Value != null && !tablePair.Value.IsNilOrNan())
          return;
        if (appendKey >= 0)
        {
          LinkedListNode<TablePair> linkedListNode = this.m_ArrayMap.Find(appendKey + 1);
          if (linkedListNode == null || linkedListNode.Value.Value == null || linkedListNode.Value.Value.IsNil())
            ++this.m_CachedLength;
          else
            this.m_CachedLength = -1;
        }
        else
          this.m_CachedLength = -1;
      }
    }

    /// <summary>Sets the value associated to the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(string key, DynValue value)
    {
      if (key == null)
        throw ScriptRuntimeException.TableIndexIsNil();
      this.CheckScriptOwnership(value);
      this.PerformTableSet<string>(this.m_StringMap, key, DynValue.NewString(key), value, false, -1);
    }

    /// <summary>Sets the value associated to the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(int key, DynValue value)
    {
      this.CheckScriptOwnership(value);
      this.PerformTableSet<int>(this.m_ArrayMap, key, DynValue.NewNumber((double) key), value, true, -1);
    }

    /// <summary>Sets the value associated to the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(DynValue key, DynValue value)
    {
      if (key.IsNilOrNan())
      {
        if (key.IsNil())
          throw ScriptRuntimeException.TableIndexIsNil();
        throw ScriptRuntimeException.TableIndexIsNaN();
      }
      if (key.Type == DataType.String)
      {
        this.Set(key.String, value);
      }
      else
      {
        if (key.Type == DataType.Number)
        {
          int integralKey = this.GetIntegralKey(key.Number);
          if (integralKey > 0)
          {
            this.Set(integralKey, value);
            return;
          }
        }
        this.CheckScriptOwnership(key);
        this.CheckScriptOwnership(value);
        this.PerformTableSet<DynValue>(this.m_ValueMap, key, key, value, false, -1);
      }
    }

    /// <summary>Sets the value associated with the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public void Set(object key, DynValue value)
    {
      switch (key)
      {
        case null:
          throw ScriptRuntimeException.TableIndexIsNil();
        case string _:
          this.Set((string) key, value);
          break;
        case int key1:
          this.Set(key1, value);
          break;
        default:
          this.Set(DynValue.FromObject(this.OwnerScript, key), value);
          break;
      }
    }

    /// <summary>
    /// Sets the value associated with the specified keys.
    /// Multiple keys can be used to access subtables.
    /// </summary>
    /// <param name="key">The keys.</param>
    /// <param name="value">The value.</param>
    public void Set(object[] keys, DynValue value)
    {
      if (keys == null || keys.Length == 0)
        throw ScriptRuntimeException.TableIndexIsNil();
      object key;
      this.ResolveMultipleKeys(keys, out key).Set(key, value);
    }

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <param name="key">The key.</param>
    public DynValue Get(string key) => this.RawGet(key) ?? DynValue.Nil;

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <param name="key">The key.</param>
    public DynValue Get(int key) => this.RawGet(key) ?? DynValue.Nil;

    /// <summary>Gets the value associated with the specified key.</summary>
    /// <param name="key">The key.</param>
    public DynValue Get(DynValue key) => this.RawGet(key) ?? DynValue.Nil;

    /// <summary>
    /// Gets the value associated with the specified key.
    /// (expressed as a <see cref="T:System.Object" />).
    /// </summary>
    /// <param name="key">The key.</param>
    public DynValue Get(object key) => this.RawGet(key) ?? DynValue.Nil;

    /// <summary>
    /// Gets the value associated with the specified keys (expressed as an
    /// array of <see cref="T:System.Object" />).
    /// This will marshall CLR and MoonSharp objects in the best possible way.
    /// Multiple keys can be used to access subtables.
    /// </summary>
    /// <param name="keys">The keys to access the table and subtables</param>
    public DynValue Get(params object[] keys) => this.RawGet(keys) ?? DynValue.Nil;

    private static DynValue RawGetValue(LinkedListNode<TablePair> linkedListNode) => linkedListNode?.Value.Value;

    /// <summary>
    /// Gets the value associated with the specified key,
    /// without bringing to Nil the non-existant values.
    /// </summary>
    /// <param name="key">The key.</param>
    public DynValue RawGet(string key) => Table.RawGetValue(this.m_StringMap.Find(key));

    /// <summary>
    /// Gets the value associated with the specified key,
    /// without bringing to Nil the non-existant values.
    /// </summary>
    /// <param name="key">The key.</param>
    public DynValue RawGet(int key) => Table.RawGetValue(this.m_ArrayMap.Find(key));

    /// <summary>
    /// Gets the value associated with the specified key,
    /// without bringing to Nil the non-existant values.
    /// </summary>
    /// <param name="key">The key.</param>
    public DynValue RawGet(DynValue key)
    {
      if (key.Type == DataType.String)
        return this.RawGet(key.String);
      if (key.Type == DataType.Number)
      {
        int integralKey = this.GetIntegralKey(key.Number);
        if (integralKey > 0)
          return this.RawGet(integralKey);
      }
      return Table.RawGetValue(this.m_ValueMap.Find(key));
    }

    /// <summary>
    /// Gets the value associated with the specified key,
    /// without bringing to Nil the non-existant values.
    /// </summary>
    /// <param name="key">The key.</param>
    public DynValue RawGet(object key)
    {
      switch (key)
      {
        case null:
          return (DynValue) null;
        case string _:
          return this.RawGet((string) key);
        case int key1:
          return this.RawGet(key1);
        default:
          return this.RawGet(DynValue.FromObject(this.OwnerScript, key));
      }
    }

    /// <summary>
    /// Gets the value associated with the specified keys (expressed as an
    /// array of <see cref="T:System.Object" />).
    /// This will marshall CLR and MoonSharp objects in the best possible way.
    /// Multiple keys can be used to access subtables.
    /// </summary>
    /// <param name="keys">The keys to access the table and subtables</param>
    public DynValue RawGet(params object[] keys)
    {
      object key;
      return keys == null || keys.Length == 0 ? (DynValue) null : this.ResolveMultipleKeys(keys, out key).RawGet(key);
    }

    private bool PerformTableRemove<T>(
      LinkedListIndex<T, TablePair> listIndex,
      T key,
      bool isNumber)
    {
      int num = listIndex.Remove(key) ? 1 : 0;
      if ((num & (isNumber ? 1 : 0)) == 0)
        return num != 0;
      this.m_CachedLength = -1;
      return num != 0;
    }

    /// <summary>
    /// Remove the value associated with the specified key from the table.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if values was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(string key) => this.PerformTableRemove<string>(this.m_StringMap, key, false);

    /// <summary>
    /// Remove the value associated with the specified key from the table.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if values was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(int key) => this.PerformTableRemove<int>(this.m_ArrayMap, key, true);

    /// <summary>
    /// Remove the value associated with the specified key from the table.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if values was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(DynValue key)
    {
      if (key.Type == DataType.String)
        return this.Remove(key.String);
      if (key.Type == DataType.Number)
      {
        int integralKey = this.GetIntegralKey(key.Number);
        if (integralKey > 0)
          return this.Remove(integralKey);
      }
      return this.PerformTableRemove<DynValue>(this.m_ValueMap, key, false);
    }

    /// <summary>
    /// Remove the value associated with the specified key from the table.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if values was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(object key)
    {
      switch (key)
      {
        case string _:
          return this.Remove((string) key);
        case int key1:
          return this.Remove(key1);
        default:
          return this.Remove(DynValue.FromObject(this.OwnerScript, key));
      }
    }

    /// <summary>
    /// Remove the value associated with the specified keys from the table.
    /// Multiple keys can be used to access subtables.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns><c>true</c> if values was successfully removed; otherwise, <c>false</c>.</returns>
    public bool Remove(params object[] keys)
    {
      object key;
      return keys != null && keys.Length != 0 && this.ResolveMultipleKeys(keys, out key).Remove(key);
    }

    /// <summary>
    /// Collects the dead keys. This frees up memory but invalidates pending iterators.
    /// It's called automatically internally when the semantics of Lua tables allow, but can be forced
    /// externally if it's known that no iterators are pending.
    /// </summary>
    public void CollectDeadKeys()
    {
      for (LinkedListNode<TablePair> linkedListNode = this.m_Values.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
      {
        TablePair tablePair = linkedListNode.Value;
        if (tablePair.Value.IsNil())
        {
          tablePair = linkedListNode.Value;
          this.Remove(tablePair.Key);
        }
      }
      this.m_ContainsNilEntries = false;
      this.m_CachedLength = -1;
    }

    /// <summary>Returns the next pair from a value</summary>
    public TablePair? NextKey(DynValue v)
    {
      if (v.IsNil())
      {
        LinkedListNode<TablePair> first = this.m_Values.First;
        if (first == null)
          return new TablePair?(TablePair.Nil);
        return first.Value.Value.IsNil() ? this.NextKey(first.Value.Key) : new TablePair?(first.Value);
      }
      if (v.Type == DataType.String)
        return this.GetNextOf(this.m_StringMap.Find(v.String));
      if (v.Type == DataType.Number)
      {
        int integralKey = this.GetIntegralKey(v.Number);
        if (integralKey > 0)
          return this.GetNextOf(this.m_ArrayMap.Find(integralKey));
      }
      return this.GetNextOf(this.m_ValueMap.Find(v));
    }

    private TablePair? GetNextOf(LinkedListNode<TablePair> linkedListNode)
    {
      while (linkedListNode != null)
      {
        if (linkedListNode.Next == null)
          return new TablePair?(TablePair.Nil);
        linkedListNode = linkedListNode.Next;
        if (!linkedListNode.Value.Value.IsNil())
          return new TablePair?(linkedListNode.Value);
      }
      return new TablePair?();
    }

    /// <summary>Gets the length of the "array part".</summary>
    public int Length
    {
      get
      {
        if (this.m_CachedLength < 0)
        {
          this.m_CachedLength = 0;
          for (int key = 1; this.m_ArrayMap.ContainsKey(key) && !this.m_ArrayMap.Find(key).Value.Value.IsNil(); ++key)
            this.m_CachedLength = key;
        }
        return this.m_CachedLength;
      }
    }

    internal void InitNextArrayKeys(DynValue val, bool lastpos)
    {
      if (val.Type == DataType.Tuple & lastpos)
      {
        foreach (DynValue val1 in val.Tuple)
          this.InitNextArrayKeys(val1, true);
      }
      else
        this.Set(++this.m_InitArray, val.ToScalar());
    }

    /// <summary>Gets the meta-table associated with this instance.</summary>
    public Table MetaTable
    {
      get => this.m_MetaTable;
      set
      {
        this.CheckScriptOwnership((IScriptPrivateResource) this.m_MetaTable);
        this.m_MetaTable = value;
      }
    }

    /// <summary>Enumerates the key/value pairs.</summary>
    /// <returns></returns>
    public IEnumerable<TablePair> Pairs => this.m_Values.Select<TablePair, TablePair>((Func<TablePair, TablePair>) (n => new TablePair(n.Key, n.Value)));

    /// <summary>Enumerates the keys.</summary>
    /// <returns></returns>
    public IEnumerable<DynValue> Keys => this.m_Values.Select<TablePair, DynValue>((Func<TablePair, DynValue>) (n => n.Key));

    /// <summary>Enumerates the values</summary>
    /// <returns></returns>
    public IEnumerable<DynValue> Values => this.m_Values.Select<TablePair, DynValue>((Func<TablePair, DynValue>) (n => n.Value));
  }
}
