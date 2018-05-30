using System;

namespace EFC_Console
{

 [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class)]
 class NotYetInTheBookAttribute : System.Attribute
 {
  public NotYetInTheBookAttribute()
  {

  }

 }
 [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class)]
 class EFCBookAttribute : System.Attribute
 {
  public EFCBookAttribute()
  {

  }
  public EFCBookAttribute(string Version)
  {

  }

  public EFCBookAttribute(string Version, string Bemerkung)
  {

  }
 }

 [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Class)]
 class ArticleAttribute : System.Attribute
 {
  public ArticleAttribute()
  {

  }
  public ArticleAttribute(string Version)
  {

  }

  public ArticleAttribute(string Version, string Bemerkung)
  {

  }
 }



}
