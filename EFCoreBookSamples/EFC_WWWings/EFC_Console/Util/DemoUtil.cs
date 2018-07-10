using DA;
using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EFC_Console
{

 public class DemoUtil
 {
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
     // Access one record for test. We don't want an error if database is empty, just if table is not accessible
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
 }
}
