// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.List`1
// Assembly: System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: C962EF2C-6BF1-4BE3-9FE1-C91B10C6800C
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/7.0.5/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/7.0.5/ref/net7.0/System.Collections.xml

using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;


#nullable enable
namespace System.Collections.Generic
{
  /// <summary>Represents a strongly typed list of objects that can be accessed by index. Provides methods to search, sort, and manipulate lists.</summary>
  /// <typeparam name="T">The type of elements in the list.</typeparam>
  [DebuggerTypeProxy(typeof (ICollectionDebugView<>))]
  [DebuggerDisplay("Count = {Count}")]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class List<T> : 
    IList<T>,
    ICollection<T>,
    IEnumerable<T>,
    IEnumerable,
    IList,
    ICollection,
    IReadOnlyList<T>,
    IReadOnlyCollection<T>
  {

    #nullable disable
    internal T[] _items;
    internal int _size;
    private int _version;
    private static readonly T[] s_emptyArray = new T[0];

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the default initial capacity.</summary>
    public List() => this._items = List<T>.s_emptyArray;

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that is empty and has the specified initial capacity.</summary>
    /// <param name="capacity">The number of elements that the new list can initially store.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than 0.</exception>
    public List(int capacity)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (capacity == 0)
        this._items = List<T>.s_emptyArray;
      else
        this._items = new T[capacity];
    }


    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
    /// <param name="collection">The collection whose elements are copied to the new list.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    public List(IEnumerable<T> collection)
    {
      if (collection == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
      if (collection is ICollection<T> objs)
      {
        int count = objs.Count;
        if (count == 0)
        {
          this._items = List<T>.s_emptyArray;
        }
        else
        {
          this._items = new T[count];
          objs.CopyTo(this._items, 0);
          this._size = count;
        }
      }
      else
      {
        this._items = List<T>.s_emptyArray;
        foreach (T obj in collection)
          this.Add(obj);
      }
    }

    /// <summary>Gets or sets the total number of elements the internal data structure can hold without resizing.</summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <see cref="P:System.Collections.Generic.List`1.Capacity" /> is set to a value that is less than <see cref="P:System.Collections.Generic.List`1.Count" />.</exception>
    /// <exception cref="T:System.OutOfMemoryException">There is not enough memory available on the system.</exception>
    /// <returns>The number of elements that the <see cref="T:System.Collections.Generic.List`1" /> can contain before resizing is required.</returns>
    public int Capacity
    {
      get => this._items.Length;
      set
      {
        if (value < this._size)
          ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.value, ExceptionResource.ArgumentOutOfRange_SmallCapacity);
        if (value == this._items.Length)
          return;
        if (value > 0)
        {
          T[] destinationArray = new T[value];
          if (this._size > 0)
            Array.Copy((Array) this._items, (Array) destinationArray, this._size);
          this._items = destinationArray;
        }
        else
          this._items = List<T>.s_emptyArray;
      }
    }

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public int Count => this._size;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> has a fixed size.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> has a fixed size; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns <see langword="false" />.</returns>
    bool IList.IsFixedSize => false;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns <see langword="false" />.</returns>
    bool ICollection<
    #nullable disable
    T>.IsReadOnly => false;

    /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.IList" /> is read-only.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.IList" /> is read-only; otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns <see langword="false" />.</returns>
    bool IList.IsReadOnly => false;

    /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
    /// <returns>
    /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns <see langword="false" />.</returns>
    bool ICollection.IsSynchronized => false;


    #nullable enable
    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.List`1" />, this property always returns the current instance.</returns>
    object ICollection.SyncRoot => (object) this;

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Generic.List`1.Count" />.</exception>
    /// <returns>The element at the specified index.</returns>
    public T this[int index]
    {
      get
      {
        if ((uint) index >= (uint) this._size)
          ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
        return this._items[index];
      }
      set
      {
        if ((uint) index >= (uint) this._size)
          ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
        this._items[index] = value;
        ++this._version;
      }
    }


    #nullable disable
    private static bool IsCompatibleObject(object value)
    {
      if (value is T)
        return true;
      return value == null && (object) default (T) == null;
    }


    #nullable enable
    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.ArgumentException">The property is set and <paramref name="value" /> is of a type that is not assignable to the <see cref="T:System.Collections.IList" />.</exception>
    /// <returns>The element at the specified index.</returns>
    object? IList.this[int index]
    {
      get => (object) this[index];
      set
      {
        ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(value, ExceptionArgument.value);
        try
        {
          this[index] = (T) value;
        }
        catch (InvalidCastException ex)
        {
          ThrowHelper.ThrowWrongValueTypeArgumentException<object>(value, typeof (T));
        }
      }
    }

    /// <summary>Adds an object to the end of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="item">The object to be added to the end of the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
      ++this._version;
      T[] items = this._items;
      int size = this._size;
      if ((uint) size < (uint) items.Length)
      {
        this._size = size + 1;
        items[size] = item;
      }
      else
        this.AddWithResize(item);
    }


    #nullable disable
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void AddWithResize(T item)
    {
      int size = this._size;
      this.Grow(size + 1);
      this._size = size + 1;
      this._items[size] = item;
    }

    /// <summary>Adds an item to the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="item">The <see cref="T:System.Object" /> to add to the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="item" /> is of a type that is not assignable to the <see cref="T:System.Collections.IList" />.</exception>
    /// <returns>The position into which the new element was inserted.</returns>
    int IList.Add(object item)
    {
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
      try
      {
        this.Add((T) item);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException<object>(item, typeof (T));
      }
      return this.Count - 1;
    }


    #nullable enable
    /// <summary>Adds the elements of the specified collection to the end of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="collection">The collection whose elements should be added to the end of the <see cref="T:System.Collections.Generic.List`1" />. The collection itself cannot be <see langword="null" />, but it can contain elements that are <see langword="null" />, if type <paramref name="T" /> is a reference type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    public void AddRange(IEnumerable<T> collection) => this.InsertRange(this._size, collection);

    /// <summary>Returns a read-only <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> wrapper for the current collection.</summary>
    /// <returns>An object that acts as a read-only wrapper around the current <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public ReadOnlyCollection<T> AsReadOnly() => new ReadOnlyCollection<T>((IList<T>) this);

    /// <summary>Searches a range of elements in the sorted <see cref="T:System.Collections.Generic.List`1" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
    /// <param name="index">The zero-based starting index of the range to search.</param>
    /// <param name="count">The length of the range to search.</param>
    /// <param name="item">The object to locate. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements, or <see langword="null" /> to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find an implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    /// <returns>The zero-based index of <paramref name="item" /> in the sorted <see cref="T:System.Collections.Generic.List`1" />, if <paramref name="item" /> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <paramref name="item" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.Generic.List`1.Count" />.</returns>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
    {
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      return Array.BinarySearch<T>(this._items, index, count, item, comparer);
    }

    /// <summary>Searches the entire sorted <see cref="T:System.Collections.Generic.List`1" /> for an element using the default comparer and returns the zero-based index of the element.</summary>
    /// <param name="item">The object to locate. The value can be <see langword="null" /> for reference types.</param>
    /// <exception cref="T:System.InvalidOperationException">The default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find an implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    /// <returns>The zero-based index of <paramref name="item" /> in the sorted <see cref="T:System.Collections.Generic.List`1" />, if <paramref name="item" /> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <paramref name="item" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.Generic.List`1.Count" />.</returns>
    public int BinarySearch(T item) => this.BinarySearch(0, this.Count, item, (IComparer<T>) null);

    /// <summary>Searches the entire sorted <see cref="T:System.Collections.Generic.List`1" /> for an element using the specified comparer and returns the zero-based index of the element.</summary>
    /// <param name="item">The object to locate. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements.
    /// 
    /// -or-
    /// 
    /// <see langword="null" /> to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />.</param>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find an implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    /// <returns>The zero-based index of <paramref name="item" /> in the sorted <see cref="T:System.Collections.Generic.List`1" />, if <paramref name="item" /> is found; otherwise, a negative number that is the bitwise complement of the index of the next element that is larger than <paramref name="item" /> or, if there is no larger element, the bitwise complement of <see cref="P:System.Collections.Generic.List`1.Count" />.</returns>
    public int BinarySearch(T item, IComparer<T>? comparer) => this.BinarySearch(0, this.Count, item, comparer);

    /// <summary>Removes all elements from the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Clear()
    {
      ++this._version;
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
      {
        int size = this._size;
        this._size = 0;
        if (size <= 0)
          return;
        Array.Clear((Array) this._items, 0, size);
      }
      else
        this._size = 0;
    }

    /// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.List`1" />; otherwise, <see langword="false" />.</returns>
    public bool Contains(T item) => this._size != 0 && this.IndexOf(item) >= 0;


    #nullable disable
    /// <summary>Determines whether the <see cref="T:System.Collections.IList" /> contains a specific value.</summary>
    /// <param name="item">The <see cref="T:System.Object" /> to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.IList" />; otherwise, <see langword="false" />.</returns>
    bool IList.Contains(object item) => List<T>.IsCompatibleObject(item) && this.Contains((T) item);


    #nullable enable
    /// <summary>Converts the elements in the current <see cref="T:System.Collections.Generic.List`1" /> to another type, and returns a list containing the converted elements.</summary>
    /// <param name="converter">A <see cref="T:System.Converter`2" /> delegate that converts each element from one type to another type.</param>
    /// <typeparam name="TOutput">The type of the elements of the target array.</typeparam>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="converter" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Collections.Generic.List`1" /> of the target type containing the converted elements from the current <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
    {
      if (converter == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.converter);
      List<TOutput> outputList = new List<TOutput>(this._size);
      for (int index = 0; index < this._size; ++index)
        outputList._items[index] = converter(this._items[index]);
      outputList._size = this._size;
      return outputList;
    }

    /// <summary>Copies the entire <see cref="T:System.Collections.Generic.List`1" /> to a compatible one-dimensional array, starting at the beginning of the target array.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.List`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.List`1" /> is greater than the number of elements that the destination <paramref name="array" /> can contain.</exception>
    public void CopyTo(T[] array) => this.CopyTo(array, 0);


    #nullable disable
    /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// <paramref name="array" /> does not have zero-based indexing.
    /// 
    /// -or-
    /// 
    /// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    void ICollection.CopyTo(Array array, int arrayIndex)
    {
      if (array != null && array.Rank != 1)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Arg_RankMultiDimNotSupported);
      try
      {
        Array.Copy((Array) this._items, 0, array, arrayIndex, this._size);
      }
      catch (ArrayTypeMismatchException ex)
      {
        ThrowHelper.ThrowArgumentException_Argument_InvalidArrayType();
      }
    }


    #nullable enable
    /// <summary>Copies a range of elements from the <see cref="T:System.Collections.Generic.List`1" /> to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
    /// <param name="index">The zero-based index in the source <see cref="T:System.Collections.Generic.List`1" /> at which copying begins.</param>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.List`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <param name="count">The number of elements to copy.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="arrayIndex" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> is equal to or greater than the <see cref="P:System.Collections.Generic.List`1.Count" /> of the source <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// The number of elements from <paramref name="index" /> to the end of the source <see cref="T:System.Collections.Generic.List`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
    public void CopyTo(int index, T[] array, int arrayIndex, int count)
    {
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      Array.Copy((Array) this._items, index, (Array) array, arrayIndex, count);
    }

    /// <summary>Copies the entire <see cref="T:System.Collections.Generic.List`1" /> to a compatible one-dimensional array, starting at the specified index of the target array.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.List`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.List`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
    public void CopyTo(T[] array, int arrayIndex) => Array.Copy((Array) this._items, 0, (Array) array, arrayIndex, this._size);

    /// <summary>Ensures that the capacity of this list is at least the specified <paramref name="capacity" />. If the current capacity is less than <paramref name="capacity" />, it is successively increased to twice the current capacity until it is at least the specified <paramref name="capacity" />.</summary>
    /// <param name="capacity">The minimum capacity to ensure.</param>
    /// <returns>The new capacity of this list.</returns>
    public int EnsureCapacity(int capacity)
    {
      if (capacity < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.capacity, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._items.Length < capacity)
      {
        this.Grow(capacity);
        ++this._version;
      }
      return this._items.Length;
    }

    private void Grow(int capacity)
    {
      int num = this._items.Length == 0 ? 4 : 2 * this._items.Length;
      if ((uint) num > 2147483591U)
        num = 2147483591;
      if (num < capacity)
        num = capacity;
      this.Capacity = num;
    }

    /// <summary>Determines whether the <see cref="T:System.Collections.Generic.List`1" /> contains elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Collections.Generic.List`1" /> contains one or more elements that match the conditions defined by the specified predicate; otherwise, <see langword="false" />.</returns>
    public bool Exists(Predicate<T> match) => this.FindIndex(match) != -1;

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
    public T? Find(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < this._size; ++index)
      {
        if (match(this._items[index]))
          return this._items[index];
      }
      return default (T);
    }

    /// <summary>Retrieves all the elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Collections.Generic.List`1" /> containing all the elements that match the conditions defined by the specified predicate, if found; otherwise, an empty <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public List<T> FindAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      List<T> all = new List<T>();
      for (int index = 0; index < this._size; ++index)
      {
        if (match(this._items[index]))
          all.Add(this._items[index]);
      }
      return all;
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindIndex(Predicate<T> match) => this.FindIndex(0, this._size, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the specified index to the last element.</summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindIndex(int startIndex, Predicate<T> match) => this.FindIndex(startIndex, this._size - startIndex, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that starts at the specified index and contains the specified number of elements.</summary>
    /// <param name="startIndex">The zero-based starting index of the search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindIndex(int startIndex, int count, Predicate<T> match)
    {
      if ((uint) startIndex > (uint) this._size)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLessOrEqual();
      if (count < 0 || startIndex > this._size - count)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int num = startIndex + count;
      for (int index = startIndex; index < num; ++index)
      {
        if (match(this._items[index]))
          return index;
      }
      return -1;
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the last occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The last element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type <paramref name="T" />.</returns>
    public T? FindLast(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = this._size - 1; index >= 0; --index)
      {
        if (match(this._items[index]))
          return this._items[index];
      }
      return default (T);
    }

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindLastIndex(Predicate<T> match) => this.FindLastIndex(this._size - 1, this._size, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the first element to the specified index.</summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindLastIndex(int startIndex, Predicate<T> match) => this.FindLastIndex(startIndex, startIndex + 1, match);

    /// <summary>Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="startIndex">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the element to search for.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="startIndex" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="startIndex" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the last occurrence of an element that matches the conditions defined by <paramref name="match" />, if found; otherwise, -1.</returns>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      if (this._size == 0)
      {
        if (startIndex != -1)
          ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess();
      }
      else if ((uint) startIndex >= (uint) this._size)
        ThrowHelper.ThrowStartIndexArgumentOutOfRange_ArgumentOutOfRange_IndexMustBeLess();
      if (count < 0 || startIndex - count + 1 < 0)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      int num = startIndex - count;
      for (int lastIndex = startIndex; lastIndex > num; --lastIndex)
      {
        if (match(this._items[lastIndex]))
          return lastIndex;
      }
      return -1;
    }

    /// <summary>Performs the specified action on each element of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="action">The <see cref="T:System.Action`1" /> delegate to perform on each element of the <see cref="T:System.Collections.Generic.List`1" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="action" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">An element in the collection has been modified.</exception>
    public void ForEach(Action<T> action)
    {
      if (action == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.action);
      int version = this._version;
      for (int index = 0; index < this._size && version == this._version; ++index)
        action(this._items[index]);
      if (version == this._version)
        return;
      ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
    }

    /// <summary>Returns an enumerator that iterates through the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <returns>A <see cref="T:System.Collections.Generic.List`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public List<
    #nullable disable
    T>.Enumerator GetEnumerator() => new List<T>.Enumerator(this);

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>) new List<T>.Enumerator(this);

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) new List<T>.Enumerator(this);


    #nullable enable
    /// <summary>Creates a shallow copy of a range of elements in the source <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="index">The zero-based <see cref="T:System.Collections.Generic.List`1" /> index at which the range starts.</param>
    /// <param name="count">The number of elements in the range.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>A shallow copy of a range of elements in the source <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public List<T> GetRange(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      List<T> range = new List<T>(count);
      Array.Copy((Array) this._items, index, (Array) range._items, 0, count);
      range._size = count;
      return range;
    }

    /// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the entire <see cref="T:System.Collections.Generic.List`1" />, if found; otherwise, -1.</returns>
    public int IndexOf(T item) => Array.IndexOf<T>(this._items, item, 0, this._size);


    #nullable disable
    /// <summary>Determines the index of a specific item in the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="item" /> is of a type that is not assignable to the <see cref="T:System.Collections.IList" />.</exception>
    /// <returns>The index of <paramref name="item" /> if found in the list; otherwise, -1.</returns>
    int IList.IndexOf(object item) => List<T>.IsCompatibleObject(item) ? this.IndexOf((T) item) : -1;


    #nullable enable
    /// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the specified index to the last element.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from <paramref name="index" /> to the last element, if found; otherwise, -1.</returns>
    public int IndexOf(T item, int index)
    {
      if (index > this._size)
        ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();
      return Array.IndexOf<T>(this._items, item, index, this._size - index);
    }

    /// <summary>Searches for the specified object and returns the zero-based index of the first occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that starts at the specified index and contains the specified number of elements.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the first occurrence of <paramref name="item" /> within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that starts at <paramref name="index" /> and contains <paramref name="count" /> number of elements, if found; otherwise, -1.</returns>
    public int IndexOf(T item, int index, int count)
    {
      if (index > this._size)
        ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();
      if (count < 0 || index > this._size - count)
        ThrowHelper.ThrowCountArgumentOutOfRange_ArgumentOutOfRange_Count();
      return Array.IndexOf<T>(this._items, item, index, count);
    }

    /// <summary>Inserts an element into the <see cref="T:System.Collections.Generic.List`1" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The object to insert. The value can be <see langword="null" /> for reference types.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is greater than <see cref="P:System.Collections.Generic.List`1.Count" />.</exception>
    public void Insert(int index, T item)
    {
      if ((uint) index > (uint) this._size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_ListInsert);
      if (this._size == this._items.Length)
        this.Grow(this._size + 1);
      if (index < this._size)
        Array.Copy((Array) this._items, index, (Array) this._items, index + 1, this._size - index);
      this._items[index] = item;
      ++this._size;
      ++this._version;
    }


    #nullable disable
    /// <summary>Inserts an item to the <see cref="T:System.Collections.IList" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which <paramref name="item" /> should be inserted.</param>
    /// <param name="item">The object to insert into the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is not a valid index in the <see cref="T:System.Collections.IList" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="item" /> is of a type that is not assignable to the <see cref="T:System.Collections.IList" />.</exception>
    void IList.Insert(int index, object item)
    {
      ThrowHelper.IfNullAndNullsAreIllegalThenThrow<T>(item, ExceptionArgument.item);
      try
      {
        this.Insert(index, (T) item);
      }
      catch (InvalidCastException ex)
      {
        ThrowHelper.ThrowWrongValueTypeArgumentException<object>(item, typeof (T));
      }
    }


    #nullable enable
    /// <summary>Inserts the elements of a collection into the <see cref="T:System.Collections.Generic.List`1" /> at the specified index.</summary>
    /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
    /// <param name="collection">The collection whose elements should be inserted into the <see cref="T:System.Collections.Generic.List`1" />. The collection itself cannot be <see langword="null" />, but it can contain elements that are <see langword="null" />, if type <paramref name="T" /> is a reference type.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is greater than <see cref="P:System.Collections.Generic.List`1.Count" />.</exception>
    public void InsertRange(int index, IEnumerable<T> collection)
    {
      if (collection == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.collection);
      if ((uint) index > (uint) this._size)
        ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessOrEqualException();
      if (collection is ICollection<T> objs)
      {
        int count = objs.Count;
        if (count > 0)
        {
          if (this._items.Length - this._size < count)
            this.Grow(this._size + count);
          if (index < this._size)
            Array.Copy((Array) this._items, index, (Array) this._items, index + count, this._size - index);
          if (this == objs)
          {
            Array.Copy((Array) this._items, 0, (Array) this._items, index, index);
            Array.Copy((Array) this._items, index + count, (Array) this._items, index * 2, this._size - index);
          }
          else
            objs.CopyTo(this._items, index);
          this._size += count;
        }
      }
      else
      {
        foreach (T obj in collection)
          this.Insert(index++, obj);
      }
      ++this._version;
    }

    /// <summary>Searches for the specified object and returns the zero-based index of the last occurrence within the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>The zero-based index of the last occurrence of <paramref name="item" /> within the entire the <see cref="T:System.Collections.Generic.List`1" />, if found; otherwise, -1.</returns>
    public int LastIndexOf(T item) => this._size == 0 ? -1 : this.LastIndexOf(item, this._size - 1, this._size);

    /// <summary>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the first element to the specified index.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="index">The zero-based starting index of the backward search.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the last occurrence of <paramref name="item" /> within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that extends from the first element to <paramref name="index" />, if found; otherwise, -1.</returns>
    public int LastIndexOf(T item, int index)
    {
      if (index >= this._size)
        ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
      return this.LastIndexOf(item, index, index + 1);
    }

    /// <summary>Searches for the specified object and returns the zero-based index of the last occurrence within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that contains the specified number of elements and ends at the specified index.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <param name="index">The zero-based starting index of the backward search.</param>
    /// <param name="count">The number of elements in the section to search.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is outside the range of valid indexes for the <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> and <paramref name="count" /> do not specify a valid section in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    /// <returns>The zero-based index of the last occurrence of <paramref name="item" /> within the range of elements in the <see cref="T:System.Collections.Generic.List`1" /> that contains <paramref name="count" /> number of elements and ends at <paramref name="index" />, if found; otherwise, -1.</returns>
    public int LastIndexOf(T item, int index, int count)
    {
      if (this.Count != 0 && index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (this.Count != 0 && count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size == 0)
        return -1;
      if (index >= this._size)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
      if (count > index + 1)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_BiggerThanCollection);
      return Array.LastIndexOf<T>(this._items, item, index, count);
    }

    /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.List`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is successfully removed; otherwise, <see langword="false" />.  This method also returns <see langword="false" /> if <paramref name="item" /> was not found in the <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public bool Remove(T item)
    {
      int index = this.IndexOf(item);
      if (index < 0)
        return false;
      this.RemoveAt(index);
      return true;
    }


    #nullable disable
    /// <summary>Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList" />.</summary>
    /// <param name="item">The object to remove from the <see cref="T:System.Collections.IList" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="item" /> is of a type that is not assignable to the <see cref="T:System.Collections.IList" />.</exception>
    void IList.Remove(object item)
    {
      if (!List<T>.IsCompatibleObject(item))
        return;
      this.Remove((T) item);
    }


    #nullable enable
    /// <summary>Removes all the elements that match the conditions defined by the specified predicate.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions of the elements to remove.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>The number of elements removed from the <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public int RemoveAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      int index1 = 0;
      while (index1 < this._size && !match(this._items[index1]))
        ++index1;
      if (index1 >= this._size)
        return 0;
      int index2 = index1 + 1;
      while (index2 < this._size)
      {
        while (index2 < this._size && match(this._items[index2]))
          ++index2;
        if (index2 < this._size)
          this._items[index1++] = this._items[index2++];
      }
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        Array.Clear((Array) this._items, index1, this._size - index1);
      int num = this._size - index1;
      this._size = index1;
      ++this._version;
      return num;
    }

    /// <summary>Removes the element at the specified index of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="index" /> is equal to or greater than <see cref="P:System.Collections.Generic.List`1.Count" />.</exception>
    public void RemoveAt(int index)
    {
      if ((uint) index >= (uint) this._size)
        ThrowHelper.ThrowArgumentOutOfRange_IndexMustBeLessException();
      --this._size;
      if (index < this._size)
        Array.Copy((Array) this._items, index + 1, (Array) this._items, index, this._size - index);
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        this._items[this._size] = default (T);
      ++this._version;
    }

    /// <summary>Removes a range of elements from the <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
    /// <param name="count">The number of elements to remove.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public void RemoveRange(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (count <= 0)
        return;
      this._size -= count;
      if (index < this._size)
        Array.Copy((Array) this._items, index + count, (Array) this._items, index, this._size - index);
      ++this._version;
      if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        return;
      Array.Clear((Array) this._items, this._size, count);
    }

    /// <summary>Reverses the order of the elements in the entire <see cref="T:System.Collections.Generic.List`1" />.</summary>
    public void Reverse() => this.Reverse(0, this.Count);

    /// <summary>Reverses the order of the elements in the specified range.</summary>
    /// <param name="index">The zero-based starting index of the range to reverse.</param>
    /// <param name="count">The number of elements in the range to reverse.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="index" /> and <paramref name="count" /> do not denote a valid range of elements in the <see cref="T:System.Collections.Generic.List`1" />.</exception>
    public void Reverse(int index, int count)
    {
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (count > 1)
        Array.Reverse<T>(this._items, index, count);
      ++this._version;
    }

    /// <summary>Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the default comparer.</summary>
    /// <exception cref="T:System.InvalidOperationException">The default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find an implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    public void Sort() => this.Sort(0, this.Count, (IComparer<T>) null);

    /// <summary>Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the specified comparer.</summary>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements, or <see langword="null" /> to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />.</param>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    /// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    public void Sort(IComparer<T>? comparer) => this.Sort(0, this.Count, comparer);

    /// <summary>Sorts the elements in a range of elements in <see cref="T:System.Collections.Generic.List`1" /> using the specified comparer.</summary>
    /// <param name="index">The zero-based starting index of the range to sort.</param>
    /// <param name="count">The length of the range to sort.</param>
    /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1" /> implementation to use when comparing elements, or <see langword="null" /> to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" />.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="index" /> is less than 0.
    /// 
    /// -or-
    /// 
    /// <paramref name="count" /> is less than 0.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="index" /> and <paramref name="count" /> do not specify a valid range in the <see cref="T:System.Collections.Generic.List`1" />.
    /// 
    /// -or-
    /// 
    /// The implementation of <paramref name="comparer" /> caused an error during the sort. For example, <paramref name="comparer" /> might not return 0 when comparing an item with itself.</exception>
    /// <exception cref="T:System.InvalidOperationException">
    /// <paramref name="comparer" /> is <see langword="null" />, and the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default" /> cannot find implementation of the <see cref="T:System.IComparable`1" /> generic interface or the <see cref="T:System.IComparable" /> interface for type <paramref name="T" />.</exception>
    public void Sort(int index, int count, IComparer<T>? comparer)
    {
      if (index < 0)
        ThrowHelper.ThrowIndexArgumentOutOfRange_NeedNonNegNumException();
      if (count < 0)
        ThrowHelper.ThrowArgumentOutOfRangeException(ExceptionArgument.count, ExceptionResource.ArgumentOutOfRange_NeedNonNegNum);
      if (this._size - index < count)
        ThrowHelper.ThrowArgumentException(ExceptionResource.Argument_InvalidOffLen);
      if (count > 1)
        Array.Sort<T>(this._items, index, count, comparer);
      ++this._version;
    }

    /// <summary>Sorts the elements in the entire <see cref="T:System.Collections.Generic.List`1" /> using the specified <see cref="T:System.Comparison`1" />.</summary>
    /// <param name="comparison">The <see cref="T:System.Comparison`1" /> to use when comparing elements.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="comparison" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">The implementation of <paramref name="comparison" /> caused an error during the sort. For example, <paramref name="comparison" /> might not return 0 when comparing an item with itself.</exception>
    public void Sort(Comparison<T> comparison)
    {
      if (comparison == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.comparison);
      if (this._size > 1)
        ArraySortHelper<T>.Sort(new Span<T>(this._items, 0, this._size), comparison);
      ++this._version;
    }

    /// <summary>Copies the elements of the <see cref="T:System.Collections.Generic.List`1" /> to a new array.</summary>
    /// <returns>An array containing copies of the elements of the <see cref="T:System.Collections.Generic.List`1" />.</returns>
    public T[] ToArray()
    {
      if (this._size == 0)
        return List<T>.s_emptyArray;
      T[] destinationArray = new T[this._size];
      Array.Copy((Array) this._items, (Array) destinationArray, this._size);
      return destinationArray;
    }

    /// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Generic.List`1" />, if that number is less than a threshold value.</summary>
    public void TrimExcess()
    {
      if (this._size >= (int) ((double) this._items.Length * 0.9))
        return;
      this.Capacity = this._size;
    }

    /// <summary>Determines whether every element in the <see cref="T:System.Collections.Generic.List`1" /> matches the conditions defined by the specified predicate.</summary>
    /// <param name="match">The <see cref="T:System.Predicate`1" /> delegate that defines the conditions to check against the elements.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="match" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if every element in the <see cref="T:System.Collections.Generic.List`1" /> matches the conditions defined by the specified predicate; otherwise, <see langword="false" />. If the list has no elements, the return value is <see langword="true" />.</returns>
    public bool TrueForAll(Predicate<T> match)
    {
      if (match == null)
        ThrowHelper.ThrowArgumentNullException(ExceptionArgument.match);
      for (int index = 0; index < this._size; ++index)
      {
        if (!match(this._items[index]))
          return false;
      }
      return true;
    }

    /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.List`1" />.</summary>
    /// <typeparam name="T" />
    public struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
    {

      #nullable disable
      private readonly List<T> _list;
      private int _index;
      private readonly int _version;
      private T _current;

      internal Enumerator(List<T> list)
      {
        this._list = list;
        this._index = 0;
        this._version = list._version;
        this._current = default (T);
      }

      /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.List`1.Enumerator" />.</summary>
      public void Dispose()
      {
      }

      /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.List`1" />.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      /// <returns>
      /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
      public bool MoveNext()
      {
        List<T> list = this._list;
        if (this._version != list._version || (uint) this._index >= (uint) list._size)
          return this.MoveNextRare();
        this._current = list._items[this._index];
        ++this._index;
        return true;
      }

      private bool MoveNextRare()
      {
        if (this._version != this._list._version)
          ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        this._index = this._list._size + 1;
        this._current = default (T);
        return false;
      }


      #nullable enable
      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <returns>The element in the <see cref="T:System.Collections.Generic.List`1" /> at the current position of the enumerator.</returns>
      public T Current => this._current;

      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The element in the <see cref="T:System.Collections.Generic.List`1" /> at the current position of the enumerator.</returns>
      object? IEnumerator.Current
      {
        get
        {
          if (this._index == 0 || this._index == this._list._size + 1)
            ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumOpCantHappen();
          return (object) this.Current;
        }
      }

      /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      void IEnumerator.Reset()
      {
        if (this._version != this._list._version)
          ThrowHelper.ThrowInvalidOperationException_InvalidOperation_EnumFailedVersion();
        this._index = 0;
        this._current = default (T);
      }
    }
  }
}
