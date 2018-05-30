using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EFC_MappingScenarios.TableSplitting
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_TableSplitting
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_TableSplitting));
   using (var ctx = new MyContext())
   {
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

    CUI.Headline("Master");
    var obj2 = new Master();
    foreach (var p in ctx.Entry(obj2).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.IsShadowProperty);
    }

    CUI.Headline("Detail");
    var obj1 = new Detail();
    foreach (var p in ctx.Entry(obj1).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.IsShadowProperty);
    }

    // not possible:
    //CUI.Headline("Split1");
    //var obj3 = new Split1();
    //foreach (var p in ctx.Entry(obj3).Properties)
    //{
    // Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.IsShadowProperty);
    //}


   }
  }
 }

 class MyContext : DbContext
 {
  public DbSet<Master> MasterSet { get; set; }
  public DbSet<Detail> DetailSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   // Set provider and connectring string
   string connstring = @"Server=.;Database=EFC_MappingScenarios_TableSplitting;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   // Define a composite key
   modelBuilder.Entity<Master>().HasKey(b => new { b.MasterId1, b.MasterId2 });

   // Define table splitting
   modelBuilder.Entity<Master>().OwnsOne(c => c.Split1);
   modelBuilder.Entity<Master>().OwnsOne(c => c.Split2);
   modelBuilder.Entity<Master>().OwnsOne(c => c.Split3);
  }
 }

 public class Master
 {
  public int MasterId1 { get; set; }
  public int MasterId2 { get; set; }
  public string Memo { get; set; }

  public List<Detail> DetailSet { get; set; }
  public Split1 Split1 { get; set; }
  public Split2 Split2 { get; set; }
  public Split3 Split3 { get; set; }
 }

 public class Detail
 {
  public int DetailId { get; set; }
  public string DetailMemo { get; set; }

  public Master Master { get; set; }
 }

 public class Split1
 {
  public string Memo1 { get; set; }
 }
 public class Split2
 {
  public string Memo2 { get; set; }
 }

 public class Split3
 {
  public string Memo3 { get; set; }
 }
}