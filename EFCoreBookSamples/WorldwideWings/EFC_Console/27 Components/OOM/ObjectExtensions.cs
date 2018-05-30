using System;
using System.Reflection;

namespace EFC_Console.OOM
{
 public static class ObjectExtensions
 {
  /// <summary>
  /// Copy the properties and fields of the same name to another, new object
  /// </summary>
  public static T CopyTo<T>(this object from)
   where T : new()
  {
   T to = new T();
   return CopyTo<T>(from, to);
  }

  /// <summary>
  /// Copy the properties and fields with the same name to another, existing object
  /// </summary>
  public static T CopyTo<T>(this object from, T to)
   where T : new()
  {
   Type fromType = from.GetType();
   Type toType = to.GetType();

   // Copy fields
   foreach (FieldInfo f in fromType.GetFields())
   {
    FieldInfo t = toType.GetField(f.Name);
    if (t != null)
    {
     t.SetValue(to, f.GetValue(from));
    }
   }

   // Copy properties
   foreach (PropertyInfo f in fromType.GetProperties())
   {
    object[] Empty = new object[0];
    PropertyInfo t = toType.GetProperty(f.Name);
    if (t != null)
    {
     t.SetValue(to, f.GetValue(from, Empty), Empty);
    }
   }
   return to;
  }
 }
}