using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using ITVisions;

namespace EFC_MappingScenarios.ColumnTypes
{
 /// <summary>
 /// In this example, several classes are deliberately implemented in one file, so that the example is clearer.
 /// </summary>
 class DEMO_ColumnTypes
 {

  public static void Run()
  {
   CUI.MainHeadline(nameof(DEMO_ColumnTypes));
   using (var ctx = new Context())
   {
    CUI.Print("Database: " + ctx.Database.GetDbConnection().ConnectionString);
    ctx.Database.EnsureDeleted();
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

    var obj1 = new EntityClassDataTypes();
    foreach (var p in ctx.Entry(obj1).Properties)
    {
     var r = p.Metadata.Relational();
     Console.WriteLine(p.Metadata.Name + ": " + r.ColumnType);
    }

    for (byte i = 1; i < 100; i++)
    {
     var obj = new EntityClassDataTypes();
     obj.ID = i;
     ctx.EntityClassDataTypes.Add(obj);
     ctx.SaveChanges();
    }


   }
  }

  class Context : DbContext
  {
   public DbSet<EntityClassDataTypes> EntityClassDataTypes { get; set; }

   protected override void OnConfiguring(DbContextOptionsBuilder builder)
   {
    builder.EnableSensitiveDataLogging(true);

    // Set provider and connectring string
    builder.UseSqlServer(@"Server=.;Database=EFC_MappingScenarios_DataTypes4;Trusted_Connection=True;MultipleActiveResultSets=True;");
    // builder.UseSqlite("Filename=EFC_MappingScenarios_DataTypes.db");
    //builder.UseOracle(@"User ID=System; Password=sa+123; Direct=true; Host=localhost; SID=OraDoc; Port=1521;");


   }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
    modelBuilder.Entity<EntityClassDataTypes>()
       .Property(p => p.Timestamp).IsRowVersion();
   }
  }

  public class EntityClassDataTypes
  {
   public byte ID { get; set; }

   public short Byte { get; set; }
   public short SByte { get; set; }
   public short Short { get; set; }
   public int Int { get; set; }
   public long Long { get; set; }
   public float Float { get; set; }
   public double Double { get; set; }
   public decimal Decimal { get; set; }

   public bool Bool { get; set; }
   public Guid Guid { get; set; }

   public DateTime DateTime { get; set; }
   public DateTimeOffset DateTimeOffset { get; set; }
   public TimeSpan TimeSpan { get; set; }

   public string String { get; set; }
   [StringLength(10)]
   public string String10 { get; set; }

   [NotMapped] public char Char { get; set; } // Not supported in SQL Server (Error: "The property 'xy' is of type 'char' which is not supported by current database provider. Either change the property CLR type or ignore the property using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.")
   public byte[] ByteArray { get; set; }

   [NotMapped] public short[] ShortArray { get; set; } //  Not supported (Error: "The property 'EntityClass.ShortArray' could not be mapped, because it is of type 'Int16[]' which is not a supported primitive type or a valid entity type. Either explicitly map this property, or ignore it using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'.")
   [NotMapped] public int[] IntArray { get; set; }
   [NotMapped] public string[] StringArray { get; set; }

   //[NotMapped] public DbGeography DbGeography { get; set; }
   //[NotMapped] public DbGeometry DbGeometry { get; set; }


   [NotMapped] public XmlDocument XmlDocument { get; set; } //  Not supported(Error: " 'The entity type 'XmlSchemaCompilationSettings' requires a primary key to be defined.'")

   [Timestamp]
   public byte[] Timestamp { get; set; }

   /// <summary>
   /// the version that goes to the client
   /// </summary>
   public ulong RowVersionAsInt => Timestamp != null ? BitConverter.ToUInt64(Timestamp.Reverse().ToArray(), 0) : 0;
  }
 }
}