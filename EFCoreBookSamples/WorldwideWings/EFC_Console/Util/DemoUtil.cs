using System;
using System.Linq;
using ITVisions;
using DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using ITVisions.EFCore;
//using Microsoft.EntityFrameworkCore.InMemory;
//using Microsoft.EntityFrameworkCore.SqlServer;

//using Microsoft.Extensions.Logging.Console;

namespace EFC_Console
{

 public class DemoUtil
 {

  public static void NoLog(string s) { }
  public static string TestConnection()
  {

   using (var ctx = new WWWingsContext())
   {

    ctx.Database.EnsureCreated();

    try
    {
     var conn = ctx.Database.GetDbConnection();
     conn.Open();
     CUI.Print("Database: " + conn.Database);
     CUI.Print("Database server: " + conn.DataSource);
     CUI.Print("Database server version: " + conn.ServerVersion);
     var f = ctx.PilotSet.FirstOrDefault();
     CUI.PrintSuccess("OK!");
     return "";
    }
    catch (Exception ex)
    {
     CUI.PrintError(ex.Message);
     return ex.Message;
    }
   }

  }

  public static int PasCount = 0;


 }
}
