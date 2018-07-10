using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace EFC_MappingScenarios.AlternateKeys
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_AlternateKeys
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_AlternateKeys));
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

    CUI.MainHeadline("Metadata");
    CUI.Headline("Detail");
    var obj1 = new Detail();
    foreach (var p in ctx.Entry(obj1).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": Key=" + p.Metadata.IsKey() + " PrimaryKey=" + p.Metadata.IsPrimaryKey());
    }

    CUI.Headline("Master");
    var obj2 = new Master();
    foreach (var p in ctx.Entry(obj2).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": Key=" + p.Metadata.IsKey() + " PrimaryKey=" + p.Metadata.IsPrimaryKey());
    }

    CUI.MainHeadline("Two new objects...");
    var h = new Master();
    h.Guid = Guid.NewGuid().ToString();
    var d = new Detail();
    d.Guid = Guid.NewGuid().ToString();
    d.Area = "AB";
    h.DetailSet.Add(d);
    ctx.MasterSet.Add(h);
    var count = ctx.SaveChanges();
    if (count > 0)
    {
     CUI.PrintSuccess(count + " Saved changes!");
     CUI.Headline("Master object");
     Console.WriteLine(h.ToNameValueString());
     CUI.Headline("Detail object");
     Console.WriteLine(d.ToNameValueString());
    }
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
   string connstring = @"Server=.;Database=EFC_MappingScenarios_AlternateKey;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   // Alternative key with one column
   modelBuilder.Entity<Detail>()
    .HasAlternateKey(c => c.Guid).HasName("UniqueContraint_GuidOnly"); ;
   // Alternative key with two columns
   modelBuilder.Entity<Detail>()
 .HasAlternateKey(c => new { c.Guid, Bereich = c.Area }).HasName("UniqueContraint_GuidAndArea");

   // The Entity Framework Core automatically generates an alternate key if, in a relationship definition, you do not create the relationship between foreign key and primary key, but use a different column of the parent class instead of the primary key.
   modelBuilder.Entity<Detail>()
          .HasOne(p => p.Master)
          .WithMany(b => b.DetailSet)
          .HasForeignKey(p => p.MasterGuid)
          .HasPrincipalKey(b => b.Guid);
  }
 }

 public class Master
 {
  public int MasterID { get; set; }
  public string Guid { get; set; }
  public string Memo { get; set; }
  public List<Detail> DetailSet { get; set; } = new List<Detail>();
 }

 public class Detail
 {
  public string DetailID { get; set; }
  public string DetailMemo { get; set; }
  public string Guid { get; set; }
  public string Area { get; set; }
  public string MasterGuid { get; set; }
  public Master Master { get; set; }
 }
}