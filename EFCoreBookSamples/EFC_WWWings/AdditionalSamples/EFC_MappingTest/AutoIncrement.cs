using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace EFC_MappingScenarios.AutoIncrement
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_AutoIncrement
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_AutoIncrement));
   using (var ctx = new Context())
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
    var obj2 = new Master1();
    foreach (var p in ctx.Entry(obj2).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata.ValueGenerated);
    }
   }
  }
 }

 class Context : DbContext
 {
  public DbSet<Master1> Master1Set { get; set; }
  public DbSet<Master2> Master2Set { get; set; }
  public DbSet<Master3> Master3Set { get; set; }
  public DbSet<Master4> Master4Set { get; set; }
  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   // Set provider and connectring string
   string connstring = @"Server=.;Database=EFC_MappingScenarios_AutoIncrement;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Master4>().HasKey(b => new { b.Master4Id1, b.Master4Id2 });

  }
 }

 public class Master1
 {
  public short Master1Id { get; set; }
  [Timestamp]
  public byte[] Timestamp { get; set; }

 }

 public class Master4
 {
  public int Master4Id1 { get; set; }
  public int Master4Id2 { get; set; }


 }

 public class Master2
 {
  public decimal Master2Id { get; set; }



 }

 public class Master3
 {
  public string Master3Id { get; set; }



 }


}


