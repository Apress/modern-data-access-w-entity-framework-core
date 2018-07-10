using DA;
using ITVisions;
using Microsoft.EntityFrameworkCore;

namespace EFC_Console
{
 class CreateDatabaseAtRuntime
 {
  public static void Create()
  {
   CUI.MainHeadline("----------- Create Database at runtime");
   using (var ctx = new WWWingsContext())
   {
    // GetDbConnection() requires using Microsoft.EntityFrameworkCore !
    CUI.Print("Database: " + ctx.Database.GetDbConnection().ConnectionString);
    var e = ctx.Database.EnsureCreated();
    if (e)
    {
     CUI.Print("Database has been created");
    }
    else
    {
     CUI.Print("Database exists!");
    }
   }
  }
 }
}
