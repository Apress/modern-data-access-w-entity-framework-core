using ITVisions;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace EFC_MappingScenarios.Sequences
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_SequencesDemos
 {
  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_SequencesDemos));
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

    // This will fail, because we consume more IDs that the sequence defines!
    for (int i = 0; i < 30; i++)
    {
     var obj1 = new EntityClass1();
     ctx.EntityClass1Set.Add(obj1);

     CUI.Headline("EntityClass1");
     Console.WriteLine($"BEFORE: PK={obj1.EntityClass1Id}");
     var count1 = ctx.SaveChanges();
     Console.WriteLine($"Saved changes: {count1} / PK={obj1.EntityClass1Id}");
     CUI.Headline("EntityClass2");
     var obj2 = new EntityClass2();
     ctx.EntityClass2Set.Add(obj2);
     Console.WriteLine($"BEFORE: PK={obj2.EntityClass2Id} Value={obj2.Value}");
     var count2 = ctx.SaveChanges();
     Console.WriteLine($"Saved changes: {count2} / PK={obj2.EntityClass2Id} Value={obj2.Value}");
     CUI.Headline("EntityClass3");
     var obj3 = new EntityClass3();
     ctx.EntityClass3Set.Add(obj3);
     Console.WriteLine($"BEFORE: PK={obj3.EntityClass3Id1}|{obj3.EntityClass3Id2}");
     var count3 = ctx.SaveChanges();
     Console.WriteLine($"Saved changes: {count3} / PK={obj3.EntityClass3Id1}|{obj3.EntityClass3Id2}");
    }
   }
  }
 }
 class Context : DbContext
 {
  public DbSet<EntityClass1> EntityClass1Set { get; set; }
  public DbSet<EntityClass2> EntityClass2Set { get; set; }
  public DbSet<EntityClass3> EntityClass3Set { get; set; }
  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   // Set provider and connectring string
   string connstring = @"Server=.;Database=EFC_MappingScenarios_Sequences2;Trusted_Connection=True;MultipleActiveResultSets=True;";
   builder.UseSqlServer(connstring);
   builder.EnableSensitiveDataLogging(true);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   // cyclic sequence between 1000 and 1300, step 10, starting at 1100
   modelBuilder.HasSequence<int>("Setp10IDs", schema: "demo")
   .StartsAt(1100).IncrementsBy(10).HasMin(1000).HasMax(1300).IsCyclic();

   // Sequence used for primary key (Data type: short)
   modelBuilder.Entity<EntityClass1>()
            .Property(o => o.EntityClass1Id)
            .HasDefaultValueSql("NEXT VALUE FOR demo.Setp10IDs");
   // Sequence used for normal column (Data type: decimal)
   modelBuilder.Entity<EntityClass2>()
         .Property(o => o.Value)
         .HasDefaultValueSql("NEXT VALUE FOR demo.Setp10IDs");

   // Sequence used for part of a composite key (Data type: int)
   modelBuilder.Entity<EntityClass3>().HasKey(b => new { b.EntityClass3Id1, b.EntityClass3Id2 });
   modelBuilder.Entity<EntityClass3>()
      .Property(o => o.EntityClass3Id1)
      .HasDefaultValueSql("NEXT VALUE FOR demo.Setp10IDs");
  }
 }

 public class EntityClass1
 {
  public short EntityClass1Id { get; set; }
  [Timestamp]
  public byte[] Timestamp { get; set; }
 }

 public class EntityClass2
 {
  public int EntityClass2Id { get; set; }
  public decimal Value { get; set; }
 }

 public class EntityClass3
 {
  /// Composite PK
  public int EntityClass3Id1 { get; set; }
  public int EntityClass3Id2 { get; set; }
 }
}