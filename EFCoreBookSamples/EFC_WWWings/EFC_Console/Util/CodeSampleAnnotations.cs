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
  public EFCBookAttribute(string BookVersion)
  {

  }
  public EFCBookAttribute(string BookVersion, string EFCVersion)
  {

  }
  public EFCBookAttribute(string BookVersion, string EFCVersion, string Comment)
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
