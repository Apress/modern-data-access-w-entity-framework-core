using System;
using EFC_Console;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
//**** NOTE: This sample is not in the English book. Therefore it has not been translated!
static internal class Conventions
{

 [NotYetInTheBook]
 public static void ListConventions()
 {

  var conventionSet = new CoreConventionSetBuilder(null).CreateConventionSet();


  Console.WriteLine("----------------- PropertyAddedConventions");
  foreach (var con in conventionSet.PropertyAddedConventions)
  {
   Console.WriteLine(con);

  }

  Console.WriteLine("----------------- KeyAddedConventions");
  foreach (var con in conventionSet.KeyAddedConventions)
  {
   Console.WriteLine(con);
  }


  Console.WriteLine("----------------- ModelBuiltConventions");
  foreach (var con in conventionSet.ModelBuiltConventions)
  {
   Console.WriteLine(con);
  }


  Console.WriteLine("----------------- NavigationAddedConventions");
  foreach (var con in conventionSet.NavigationAddedConventions)
  {
   Console.WriteLine(con);
  }


  Console.WriteLine("----------------- ForeignKeyRemovedConventions");
  foreach (var con in conventionSet.ForeignKeyRemovedConventions)
  {
   Console.WriteLine(con);
  }
 }
}