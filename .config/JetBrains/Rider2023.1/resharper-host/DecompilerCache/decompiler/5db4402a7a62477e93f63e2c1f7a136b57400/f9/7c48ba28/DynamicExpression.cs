// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.DynamicExpression
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

using MoonSharp.Interpreter.Tree.Expressions;

namespace MoonSharp.Interpreter
{
  /// <summary>Represents a dynamic expression in the script</summary>
  public class DynamicExpression : IScriptPrivateResource
  {
    private DynamicExprExpression m_Exp;
    private DynValue m_Constant;
    /// <summary>The code which generated this expression</summary>
    public readonly string ExpressionCode;

    internal DynamicExpression(Script S, string strExpr, DynamicExprExpression expr)
    {
      this.ExpressionCode = strExpr;
      this.OwnerScript = S;
      this.m_Exp = expr;
    }

    internal DynamicExpression(Script S, string strExpr, DynValue constant)
    {
      this.ExpressionCode = strExpr;
      this.OwnerScript = S;
      this.m_Constant = constant;
    }

    /// <summary>Evaluates the expression</summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public DynValue Evaluate(ScriptExecutionContext context = null)
    {
      context = context ?? this.OwnerScript.CreateDynamicExecutionContext();
      ScriptPrivateResource_Extension.CheckScriptOwnership(this, context.GetScript());
      return this.m_Constant != null ? this.m_Constant : this.m_Exp.Eval(context);
    }

    /// <summary>Finds a symbol in the expression</summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public SymbolRef FindSymbol(ScriptExecutionContext context)
    {
      ScriptPrivateResource_Extension.CheckScriptOwnership(this, context.GetScript());
      return this.m_Exp != null ? this.m_Exp.FindDynamic(context) : (SymbolRef) null;
    }

    /// <summary>Gets the script owning this resource.</summary>
    /// <value>The script owning this resource.</value>
    public Script OwnerScript { get; private set; }

    /// <summary>
    /// Determines whether this instance is a constant expression
    /// </summary>
    /// <returns></returns>
    public bool IsConstant() => this.m_Constant != null;

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
    /// </returns>
    public override int GetHashCode() => this.ExpressionCode.GetHashCode();

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object" />, is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="T:System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj) => obj is DynamicExpression dynamicExpression && dynamicExpression.ExpressionCode == this.ExpressionCode;
  }
}
