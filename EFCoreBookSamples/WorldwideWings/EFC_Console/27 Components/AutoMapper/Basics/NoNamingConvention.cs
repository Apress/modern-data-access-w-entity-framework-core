using AutoMapper;
using System.Text.RegularExpressions;

namespace EFC_Console.AutoMapper
{
 /// <summary>
 /// No use of underscores when mapping
 /// </summary>
 class NoNamingConvention : INamingConvention
 {
  #region INamingConvention Members
  public string ReplaceValue(Match match)
  {
   return ""; 
  }

  public string SeparatorCharacter
  {
   get { return ""; }
  }
  public Regex SplittingExpression
  {
   get { return new Regex(""); }
  }
  #endregion
 }
}
