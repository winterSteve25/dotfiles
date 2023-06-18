// Decompiled with JetBrains decompiler
// Type: MoonSharp.Interpreter.ScriptPrivateResource_Extension
// Assembly: MoonSharp.Interpreter, Version=2.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5DB4402A-7A62-477E-93F6-3E2C1F7A136B
// Assembly location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.dll
// XML documentation location: /home/cadenz/.nuget/packages/moonsharp/2.0.0/lib/netstandard1.6/MoonSharp.Interpreter.xml

namespace MoonSharp.Interpreter
{
  internal static class ScriptPrivateResource_Extension
  {
    public static void CheckScriptOwnership(
      this IScriptPrivateResource containingResource,
      DynValue[] values)
    {
      foreach (DynValue dynValue in values)
        containingResource.CheckScriptOwnership(dynValue);
    }

    public static void CheckScriptOwnership(
      this IScriptPrivateResource containingResource,
      DynValue value)
    {
      if (value == null)
        return;
      IScriptPrivateResource asPrivateResource = value.GetAsPrivateResource();
      if (asPrivateResource == null)
        return;
      containingResource.CheckScriptOwnership(asPrivateResource);
    }

    public static void CheckScriptOwnership(this IScriptPrivateResource resource, Script script)
    {
      if (resource.OwnerScript != null && resource.OwnerScript != script && script != null)
        throw new ScriptRuntimeException("Attempt to access a resource owned by a script, from another script");
    }

    public static void CheckScriptOwnership(
      this IScriptPrivateResource containingResource,
      IScriptPrivateResource itemResource)
    {
      if (itemResource == null)
        return;
      if (containingResource.OwnerScript != null && containingResource.OwnerScript != itemResource.OwnerScript && itemResource.OwnerScript != null)
        throw new ScriptRuntimeException("Attempt to perform operations with resources owned by different scripts.");
      if (containingResource.OwnerScript == null && itemResource.OwnerScript != null)
        throw new ScriptRuntimeException("Attempt to perform operations with a script private resource on a shared resource.");
    }
  }
}
