using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFC_MappingScenarios.Sequences;
using ITVisions;
using Microsoft.EntityFrameworkCore;

namespace EFC_MappingScenarios.GuidPK
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class GuidPKDemo
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(GuidPKDemo));
   using (var ctx = new GuidPK.Context())
   {
    //ctx.Log();
    CUI.Print("Database: " + ctx.Database.GetDbConnection().ConnectionString);
    var e = ctx.Database.EnsureCreated();
    if (e)
    {
     CUI.Print("Database has been created!");
    }
    else
    {
     CUI.Print("Database exists!");
    }

    for (int i = 0; i < 30; i++)
    {
     var obj1 = new EntityClassWithGuidPK();
     obj1.Name = "Test";
     ctx.EntityClassWithGuidPKSet.Add(obj1);
     var c = ctx.SaveChanges();
     Console.WriteLine(obj1.Id);
     Console.WriteLine($"Number of saved changes: {c}");
    }

    CUI.PrintSuccess("Done!");
   }
  }
 }

 class Context : DbContext
 {
  public DbSet<EntityClassWithGuidPK> EntityClassWithGuidPKSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   // Set provider and connectring string
   string connstring = @"Server=.;Database=EFC_MappingScenarios_GuidPK;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
   builder.EnableSensitiveDataLogging(true);
  }

 }

 public class EntityClassWithGuidPK
 {
  public Guid Id { get; set; }
  public string Name { get; set; }

 }


}
