using ITVisions;
using System;

// A collection of Entity Framework Core samples outside of the World Wide Wings scenario
// (C) Dr. Holger Schwichtenberg 2016 - 2018


// NUGET: install-package Microsoft.EntityFrameworkCore.Sqlite
// NUGET: install-package Microsoft.EntityFrameworkCore.SqlServer
// NUGET: install-package System.Data.Sqlclient
// NUGET: Install-Package Devart.Data.Oracle.EFCore

namespace EFC_MappingScenarios
{

 class Program
 {
  static void Main(string[] args)
  {

   //TableSplitting.DEMO_TableSplitting.Run();
   //Sequences.DEMO_SequencesDemos.Run();
   //AlternateKeys.DEMO_AlternateKeys.Run();

   //CascadingDelete.DEMO_CascasdingDelete.Run();
   //GuidPKDemo.Run();

   ColumnTypes.DEMO_ColumnTypes.Run();


   CUI.PrintSuccess("==== DONE!");
   Console.ReadLine();
  }
 }
}


