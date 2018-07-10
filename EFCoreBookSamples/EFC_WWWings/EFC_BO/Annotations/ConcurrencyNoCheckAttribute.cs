using System;

namespace EFCExtensions
{

 /// <summary>
 /// Annotation for EFCore entity classes and properties for which EFCore should not run a concurrency check
 /// </summary>
 [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
 public class ConcurrencyNoCheckAttribute : Attribute
 {
 }
}
