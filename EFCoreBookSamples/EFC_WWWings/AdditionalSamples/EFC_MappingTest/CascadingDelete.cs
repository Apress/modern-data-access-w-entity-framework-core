using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_MappingScenarios.CascadingDelete
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_CascasdingDelete
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_CascasdingDelete));
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

    CUI.Headline("Metadata of Master");
    var obj2 = new Master();
    foreach (var p in ctx.Entry(obj2).Properties)
    {
     Console.WriteLine(p.Metadata.Name + ": ");
    }
    foreach (var p in ctx.Entry(obj2).Navigations)
    {
     Console.WriteLine(p.Metadata.Name + ": " + p.Metadata);
    }

    CUI.Headline("Clean database");
    ctx.Database.ExecuteSqlCommand("Delete from Detail1Set");
    ctx.Database.ExecuteSqlCommand("Delete from Detail2Set");
    ctx.Database.ExecuteSqlCommand("Delete from Detail3Set");
    ctx.Database.ExecuteSqlCommand("Delete from Detail4Set");
    ctx.Database.ExecuteSqlCommand("Delete from MasterSet");

    CUI.Headline("Create one Master with three details");
    var d1 = new Detail1();
    var d2 = new Detail2();
    var d3 = new Detail3();
    var d4 = new Detail4();
    var m = new Master();
    m.Detail1Set.Add(d1);
    m.Detail2Set.Add(d2);
    m.Detail3Set.Add(d3);
  //  m.Detail4Set.Add(d4); // Code will fail with this
    ctx.MasterSet.Add(m);
    var count1 = ctx.SaveChanges();
    Console.WriteLine("Saved changes: " + count1);

    PrintStatusDB();
   }

   CUI.Headline("Delete Master object...");
   using (var ctx = new MyContext())
   {
    var m = ctx.MasterSet.Include(x => x.Detail1Set).Include(x => x.Detail2Set).Include(x=>x.Detail3Set).Include(x => x.Detail4Set).FirstOrDefault();
    PrintStatusRAM(m);
    ctx.Log();
    ctx.Remove(m);
    var count2 = ctx.SaveChanges();
    DbContextExtensionLogging.DoLogging = false;
    Console.WriteLine("Saved changes: " + count2);
    PrintStatusDB();
    PrintStatusRAM(m);
   }
  }

  private static void PrintStatusRAM(Master m)
  {
   Console.WriteLine("h.Detail1=" + m.Detail1Set.Count + " / Detail1.FK=" + (m.Detail1Set.Count > 0 ? m.Detail1Set.ElementAt(0)?.MasterId.ToString() : "--"));
   Console.WriteLine("h.Detail2=" + m.Detail2Set.Count + " / Detail2.FK=" + (m.Detail2Set.Count > 0 ? m.Detail2Set.ElementAt(0)?.MasterId.ToString() : "--"));
   Console.WriteLine("h.Detail3=" + m.Detail3Set.Count + " / Detail3.FK=" + (m.Detail3Set.Count > 0 ? m.Detail3Set.ElementAt(0)?.MasterId.ToString() : "--"));
   Console.WriteLine("h.Detail4=" + m.Detail4Set.Count + " / Detail4.FK=" + (m.Detail4Set.Count > 0 ? m.Detail4Set.ElementAt(0)?.MasterId.ToString() : "--"));
  }

  private static void PrintStatusDB()
  {
   using (var ctx = new MyContext())
   {
    Console.WriteLine("DB Mastern: " + ctx.MasterSet.Count());
    Console.WriteLine("DB Detail1: " + ctx.Detail1Set.Count());
    Console.WriteLine("DB Detail2: " + ctx.Detail2Set.Count());
    Console.WriteLine("DB Detail3: " + ctx.Detail3Set.Count());
    Console.WriteLine("DB Detail4: " + ctx.Detail4Set.Count());
   }
  }
 }

 class MyContext : DbContext
 {
  public DbSet<Master> MasterSet { get; set; }
  public DbSet<Detail1> Detail1Set { get; set; }
  public DbSet<Detail2> Detail2Set { get; set; }
  public DbSet<Detail3> Detail3Set { get; set; }
  public DbSet<Detail4> Detail4Set { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   // Set provider and connectring string
   string connstring = @"Server=.;Database=EFC_MappingScenarios_CascadingDelete;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   modelBuilder.Entity<Detail1>()
   .HasOne(f => f.Master)
   .WithMany(t => t.Detail1Set)
   .HasForeignKey(x => x.MasterId)
   .OnDelete(DeleteBehavior.Cascade);

   modelBuilder.Entity<Detail2>()
   .HasOne(f => f.Master)
   .WithMany(t => t.Detail2Set)
   .HasForeignKey(x => x.MasterId)
   .OnDelete(DeleteBehavior.ClientSetNull);

   modelBuilder.Entity<Detail3>()
    .HasOne(f => f.Master)
    .WithMany(t => t.Detail3Set)
    .HasForeignKey(x => x.MasterId)
    .OnDelete(DeleteBehavior.SetNull);

   modelBuilder.Entity<Detail4>()
    .HasOne(f => f.Master)
    .WithMany(t => t.Detail4Set)
    .HasForeignKey(x => x.MasterId)
    .OnDelete(DeleteBehavior.Restrict);
  }
 }

 public class Master
 {
  public int MasterId { get; set; }
  public string Memo { get; set; }

  public List<Detail1> Detail1Set { get; set; } = new List<Detail1>();
  public List<Detail2> Detail2Set { get; set; } = new List<Detail2>();
  public List<Detail3> Detail3Set { get; set; } = new List<Detail3>();
  public List<Detail4> Detail4Set { get; set; } = new List<Detail4>();
 }

 public class Detail1
 {
  public int Detail1Id { get; set; }
  public string DetailMemo { get; set; }
  public Master Master { get; set; }
  public int? MasterId { get; set; }
 }

 public class Detail2
 {
  public int Detail2Id { get; set; }
  public string DetailMemo { get; set; }
  public Master Master { get; set; }
  public int? MasterId { get; set; }
 }

 public class Detail3
 {
  public int Detail3Id { get; set; }
  public string DetailMemo { get; set; }
  public Master Master { get; set; }
  public int? MasterId { get; set; }
 }

 public class Detail4
 {
  public int Detail4Id { get; set; }
  public string DetailMemo { get; set; }
  public Master Master { get; set; }
  public int? MasterId { get; set; }
 }
}