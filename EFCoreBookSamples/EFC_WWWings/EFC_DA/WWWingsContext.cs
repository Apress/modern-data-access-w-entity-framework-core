using BO;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using ITVisions;
using ITVisions.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using EFCExtensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DA
{


 /// <summary>
 /// EFCore context class for World Wings Wings database schema version 7.0
 /// </summary>
 // ReSharper disable once InconsistentNaming
 public class WWWingsContext : DbContext
 {


  public static string ConnectionString { get; set; } =
   @"Server=.;Database=WWWingsV2_EN-temp22;Trusted_Connection=True;MultipleActiveResultSets=True;App=Entityframework";

  #region Entities for tables 
  public DbSet<Airline> AirlineSet { get; set; }
  public DbSet<Flight> FlightSet { get; set; }
  public DbSet<Pilot> PilotSet { get; set; }
  public DbSet<Passenger> PassengerSet { get; set; }
  public DbSet<Booking> BookingSet { get; set; }
  public DbSet<AircraftType> AircraftTypeSet { get; set; }
  #endregion

  #region Pseudo-entities for grouping results
  public DbSet<DepartureGrouping> DepartureGroupSet { get; set; } // for grouping result
  #endregion

  #region Pseudo-entities for views
  public DbSet<DepartureStatistics> DepartureStatisticsSet { get; set; } // for view
  #endregion

  #region Query Views
  public DbQuery<DepartureGroup> DepartureGroup { get; set; } // Group Result
  public DbQuery<DepartureStatisticsView> DepartureStatisticsView { get; set; } // View
  public DbQuery<BO.FlightDTO> FlightDTO { get; set; } // view
  #endregion

  // Für Second Level Cache!
  private static readonly IEFCacheServiceProvider _efCacheServiceProvider = ConfigureServices.GetEFCacheServiceProvider();

  private static int count = 0;
  private int num = 0;
  private bool globalQueryFilter = false;
  static public string ShadowPropertyName = "LastChange";
  private static List<string> additionalColumnSet = null;
  public static List<string> AdditionalColumnSet
  {
   get { return additionalColumnSet; }
   set
   {
    if (count > 0) throw new ApplicationException("Cannot set AdditionalColumnSet as context has been used before!");
    additionalColumnSet = value;
   }
  }

  private DbConnection _DbConnection;

  public WWWingsContext(bool GlobalQueryFilter = false)
  {
   this.globalQueryFilter = GlobalQueryFilter;
   ctor();
  }

  public WWWingsContext(string shadowPropertyName)
  {
   if (count > 0) throw new ApplicationException("Cannot set ShadowPropertyName as context has been used before!");
   WWWingsContext.ShadowPropertyName = shadowPropertyName;
   ctor();
  }

  public WWWingsContext(List<string> additionalColumnSet)
  {
   if (count > 0) throw new ApplicationException("Cannot set additionalColumnSet as context has been used before!");

   WWWingsContext.AdditionalColumnSet = additionalColumnSet;
   ctor();
  }

  public WWWingsContext(DbContextOptions options) : base(options)
  {
   ctor();
  }

  public WWWingsContext()
  {
   ctor();
  }

  public WWWingsContext(DbConnection connection)
  {
   this._DbConnection = connection;
   ctor();
  }
  private void ctor()
  {
   count++;
   num = count;
   //CUI.Print("WWWingsContext # " + num + ": ctor", ConsoleColor.Blue);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {

   //CUI.Print($"WWWingsContext #{num:00}: OnConfiguring", ConsoleColor.Magenta);
   //CUI.Print("WWWingsContext # " + num + ": OnConfiguring", ConsoleColor.Blue);

   // Provider und Connectring String festlegen!
   //if (this._DbConnection != null) builder.UseSqlServer(this._DbConnection);
   //else builder.UseSqlServer(ConnectionString);

   
   if (_DbConnection != null) builder.UseSqlServer(_DbConnection);
   else builder.UseLazyLoadingProxies().UseSqlServer(ConnectionString); // 

   //builder.UseOracle(@"User ID=System; Password=sa+123; Direct=true; Host=localhost; SID=OraDoc; Port=1521;");

   // at client evaluation force exception!
   //builder.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

   builder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.AmbientTransactionWarning));

   // Optional: print extensions
   //foreach (var extension in builder.Options.Extensions)
   //{
   // Console.WriteLine("Aktive option: " + extension);
   //}

   //Other stores, e.g.
   //builder.UseInMemoryStore();
  }


  /// <summary>
  /// Manual configuration via Fluent API
  /// </summary>
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   CUI.Print($"WWWingsContext #{num:00}: OnModelCreating", ConsoleColor.Magenta);

   #region Trick for pseudo entities for grouping and Views
   // Trick: hide the view or grouping pseudo entities from the EF migration tool so it does not want to create a new table for it
   if (System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower() == "ef")
   {
    modelBuilder.Ignore<DepartureStatistics>();
    modelBuilder.Ignore<DepartureGrouping>();
   }

   #endregion

   #region EFC21 Query Types

   //modelBuilder
   //   .Query<DepartureGroup>().ToTable("View_BlogPostCounts")
   //   .Property(v => v.BlogName).HasColumnName("Name");
   #endregion

   #region EFC21 Value Converters: enum<->string 

   // Convert PilotLicenseType enum to / from string

   // Syntax 1: Predefinied Converter instance
   var converter = new EnumToStringConverter<PilotLicenseType>();
   modelBuilder
    .Entity<Pilot>()
    .Property(e => e.PilotLicenseType)
    .HasConversion(converter);

   //// Alternative Syntax 2: Explicit Converter instance
   //var cmh = new ConverterMappingHints(20);
   //var valueconverter = new ValueConverter<PilotLicenseType, string>(
   // v => v.ToString(),
   // v => (PilotLicenseType)Enum.Parse(typeof(PilotLicenseType), v), cmh);
   //modelBuilder
   //.Entity<Pilot>().Property(x => x.PilotLicenseType).
   //HasConversion(valueconverter);

   //// Alternative Syntax 3: Inline Converter
   //modelBuilder
   //   .Entity<Pilot>().Property(x => x.PilotLicenseType).
   //   HasConversion(x => x.ToString(),
   //         x => (PilotLicenseType)Enum.Parse(typeof(PilotLicenseType), x));
   #endregion

   #region EFC21 Value Converters: bool<->string 

   // FrequentFlyer String<->Bool Converter: Syntax 1: Explicit Converter instance
   var converterForFrequentFlyer = new ValueConverter<string, bool>(
    v => (v == "Yes" ? true : false),
    v => (v ? "Yes" : "No"));
   modelBuilder
      .Entity<Passenger>().Property(x => x.FrequentFlyer).
      HasConversion(converterForFrequentFlyer);

   // FrequentFlyer String<->Bool Converter: Alternative Syntax 2: Inline Converter
   //modelBuilder
   //   .Entity<Passenger>().Property(x => x.FrequentFlyer).
   //   HasConversion(v => (v == "Yes" ? true : false), v => (v ? "Yes" : "No"));

   #endregion

   #region EFC21 Value Converter Additional Samples
   // Syntax 1: Predefinied Converter instance: nvarchar(3)
   //var converterForFrequentFlyer1 = new BoolToStringConverter("No", "Yes");
   //modelBuilder
   //   .Entity<Passenger>().Property(x => x.FrequentFlyer).
   //   HasConversion(converterForFrequentFlyer1);

   ////// Alternative Syntax 2: Explicit Converter instance: nvarchar(3)
   //var cmh = new ConverterMappingHints(3);
   //var converterForFrequentFlyer2 = new ValueConverter<bool, string>(
   // v => (v ? "Yes" : "No"),
   // v => (v == "Yes" ? true : false), cmh);
   //modelBuilder
   //   .Entity<Passenger>().Property(x => x.FrequentFlyer).
   //   HasConversion(converterForFrequentFlyer2);

   //// Alternative Syntax 3: Inline Converter:  nvarchar(MAX)
   //modelBuilder
   //   .Entity<Passenger>().Property(x => x.FrequentFlyer).
   //   HasConversion(v => (v ? "Y" : "N"), v => (v == "Y" ? true : false));
   #endregion

   #region FluentAPI for Flight
   // ----------- PK
   modelBuilder.Entity<Flight>().HasKey(f => f.FlightNo);
   modelBuilder.Entity<Flight>().Property(b => b.FlightNo).ValueGeneratedNever();

   // ----------- Length and null values 
   modelBuilder.Entity<Flight>().Property(f => f.Memo).HasMaxLength(5000);
   modelBuilder.Entity<Flight>().Property(f => f.Seats).IsRequired();

   // ----------- Calculated column
   modelBuilder.Entity<Flight>().Property(p => p.Utilization)
            .HasComputedColumnSql("100.0-(([FreeSeats]*1.0)/[Seats])*100.0");

   // ----------- Default values
   modelBuilder.Entity<Flight>().Property(x => x.Price).HasDefaultValue(123.45m);
   modelBuilder.Entity<Flight>().Property(x => x.Departure).HasDefaultValue("(not set)");
   modelBuilder.Entity<Flight>().Property(x => x.Destination).HasDefaultValue("(not set)");
   modelBuilder.Entity<Flight>().Property(x => x.Date).HasDefaultValueSql("getdate()");

   //// ----------- Indexes
   //// Index with one column
   modelBuilder.Entity<Flight>().HasIndex(x => x.FreeSeats).HasName("Index_FreeSeats");
   //// Index with two columns
   modelBuilder.Entity<Flight>().HasIndex(f => new { f.Departure, f.Destination });

   // Syntax 2
   //modelBuilder.Entity<Flight>(f =>
   //{
   // // ----------- PK
   // f.HasKey(x => x.FlightNo);
   // f.Property(x => x.FlightNo).ValueGeneratedNever();
   // //// ----------- Length and null values
   // f.Property(x => x.Memo).HasMaxLength(5000);
   // f.Property(x => x.Seats).IsRequired();
   // // ----------- Calculated column
   // f.Property(x => x.Utilization)
   //            .HasComputedColumnSql("100.0-(([FreeSeats]*1.0)/[Seats])*100.0");

   // // ----------- Default values
   // f.Property(x => x.Price).HasDefaultValue(123.45m);
   // f.Property(x => x.Departure).HasDefaultValue("(not set)");
   // f.Property(x => x.Destination).HasDefaultValue("(not set)");
   // f.Property(x => x.Date).HasDefaultValueSql("getdate()");

   // // ----------- Indexes
   // // Index with one column
   // f.HasIndex(x => x.FreeSeats).HasName("Index_FreeSeats");
   // // Index with two columns
   // f.HasIndex(x => new { x.Departure, x.Destination });
   //});

   //// Syntax 3
   //modelBuilder.Entity<Flight>(ConfigureFlight);

   //// Syntax 4 (since EFC 2.0)
   //modelBuilder.ApplyConfiguration<Flight>(new FlightETC());


   #endregion

   #region EFC2_GlobalFilter
   if (globalQueryFilter)
   {
    CUI.PrintWarning("Activating Global Filters...!");
    modelBuilder.Entity<Flight>().HasQueryFilter(x => x.FreeSeats > 200);
    modelBuilder.Entity<Flight>().HasQueryFilter(x => x.FreeSeats > 0 && x.AirlineCode == "WWW");
    modelBuilder.Entity<Flight>().HasQueryFilter(x => x.FreeSeats > 0);
   }
   #endregion

   #region Keys for derived classes
   // Additional keys
   //builder.Entity<Person>().HasKey(x => x.PersonID);
   modelBuilder.Entity<Passenger>().HasKey(x => x.PersonID);
   //builder.Entity<Pilot>().HasKey(x => x.PersonID);
   modelBuilder.Entity<Employee>().HasKey(x => x.PersonID);
   #endregion

   #region BookingSet
   // --- Composite primary key
   modelBuilder.Entity<Booking>().HasKey(b => new { b.FlightNo, b.PassengerID });

   #endregion

   #region Mapping of a field
   modelBuilder.Entity<Employee>()
    .Property(x => x.PassportNumber)
    .HasField("_passportNumber")
    .UsePropertyAccessMode(PropertyAccessMode.Field);
   #endregion

   #region Shadow property
   if (!String.IsNullOrEmpty(ShadowPropertyName))
   {
    //modelBuilder.Entity<Flight>().Property<DateTime>("LastChange");
    modelBuilder.Entity<Flight>().Property<DateTime>(ShadowPropertyName);
   }

   if (AdditionalColumnSet != null)
   {
    foreach (string shadowProp in AdditionalColumnSet)
    {
     var splitted = shadowProp.Split(';');
     string entityclass = splitted[0];
     string columnname = splitted[1];
     string columntype = splitted[2];

     Type columntypeObj = Type.GetType(columntype);

     modelBuilder.Entity(entityclass).Property(columntypeObj, columnname);
    }
   }

   #endregion

   #region Bulk configuration via model class for all table names
   foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   {
    // All table names = class names (~ EF 6.x), 
    // except the classes that have a [Table] annotation
    var annotation = entity.ClrType?.GetCustomAttribute<TableAttribute>();
    if (annotation == null)
    {
     entity.Relational().TableName = entity.DisplayName();
    }
   }
   #endregion

   #region Bulk configuration: Columns ending in "No" become primary keys without auto increment
   foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   {
    //var propNr = entity.GetProperties().FirstOrDefault(x => x.Surname.EndsWith("No"));
    //if (propNr != null)
    //{
    // entity.SetPrimaryKey(propNr);
    // propNr.ValueGenerated = ValueGenerated.Never;
    //}
   }
   #endregion

   #region Bulk configuration via model class [ConcurrencyCheck]

   // Get all entity classes
   //foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   //{
   // // get all properties
   // foreach (var prop in entity.GetProperties())
   // {
   //   prop.IsConcurrencyToken = true;
   // }
   //}
   #endregion
   #region Bulk configuration via model class for a concurrency check of all properties except those annnotated with [ConcurrencyNoCheck]

   // Get all entity classes
   foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   {
    // get all properties
    //foreach (var prop in entity.GetProperties())
    //{
    // // Look for annotation [ConcurrencyNoCheck]
    // var annotation = prop.PropertyInfo?.GetCustomAttribute<ConcurrencyNoCheckAttribute>();
    // if (annotation == null)
    // {
    //  prop.IsConcurrencyToken = true;
    // }
    // else
    // {
    //  Console.WriteLine("No Concurrency Check for " + prop.Name);
    // }
    // if (prop.Name == "Timestamp")
    // {
    //  prop.ValueGenerated = ValueGenerated.OnAddOrUpdate;
    //  prop.IsConcurrencyToken = true;
    // }
    // foreach (var a in prop.GetAnnotations())
    // {
    //  Console.WriteLine(prop.Name + ":" + a.Name + "=" + a.Value);
    // }
    //}
   }
   #endregion
   #region Bulk configuration for Timestamp column

   // Get all entity classes
   foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   {
    // Get all properties
    foreach (var prop in entity.GetProperties())
    {
     if (prop.Name == "Timestamp")
     {
      prop.ValueGenerated = ValueGenerated.OnAddOrUpdate;
      prop.IsConcurrencyToken = true;
     }

    }
   }
   #endregion
   #region Print applied annotations
   foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
   {
    foreach (var prop in entity.GetProperties())
    {
     //foreach (var a in prop.GetAnnotations())
     //{

     // Console.WriteLine(prop.Surname + ":" + a.Surname + "=" + a.Value);
     //}

    }
   }
   #endregion


   // ----------- Timestamps/Concurrency
   //builder.Entity<Flight>().Property(x => x.Timestamp).ValueGeneratedOnAddOrUpdate().IsConcurrencyToken();
   //builder.Entity<Flight>().Property(x => x.FreeSeats).ConcurrencyToken();

   #region Relation Flight --> Pilot
   // One can apparently set cascading deletion only if relationship is bidirectional :-(
   modelBuilder.Entity<Pilot>().HasMany(p => p.FlightAsPilotSet).WithOne(p => p.Pilot).HasForeignKey(f => f.PilotId).OnDelete(DeleteBehavior.Restrict);

   modelBuilder.Entity<Pilot>().HasMany(p => p.FlightAsCopilotSet)
    .WithOne(p => p.Copilot).HasForeignKey(f => f.CopilotId).OnDelete(DeleteBehavior.Restrict);
   #endregion

   #region Relation Flight-->AircraftType
   //builder.Entity<Flight>().HasOne(p => p.Detail).WithOne(f => f.Flight).OnDelete(DeleteBehavior.Cascade);
   //-----------Beziehung Flight-- > AircraftTypeDetail
   modelBuilder.Entity<Flight>().HasOne(p => p.AircraftType).WithMany(f => f.FlightSet).OnDelete(DeleteBehavior.Restrict);
   //für Demo ggf: IsRequired()

   // Das hilft nicht :-(
   //var ent1 = builder.Model.GetEntityTypes().SingleOrDefault(x => x.Surname == "GO.Flight");
   //var prop1 = ent1.FindProperty("FlightNo");
   //var key1 = ent1.FindKey(prop1);
   //var ent2 = builder.Model.GetEntityTypes().SingleOrDefault(x => x.Surname == "GO.Flightdetail");
   //var prop2 = ent2.FindProperty("FlightNo");
   //var key2 = ent2.FindKey(prop2);
   //ent1.FindForeignKey(prop1, key2, ent2.AsEntityType()).DeleteBehavior = DeleteBehavior.Cascade;
   //ent2.FindForeignKey(prop2, key1, ent1.AsEntityType()).DeleteBehavior = DeleteBehavior.Cascade;
   #endregion

   #region Book relations
   // 1:N Relation Flight --> Booking
   modelBuilder.Entity<Passenger>().HasMany(p => p.BookingSet).WithOne(b => b.Passenger).HasForeignKey(p => p.PassengerID);

   // 1:N Relation Passenger --> Booking
   modelBuilder.Entity<Flight>().HasMany(f => f.BookingSet).WithOne(b => b.Flight).HasForeignKey(p => p.FlightNo);
   #endregion

   #region Indexes
   //// Index with one column
   modelBuilder.Entity<Flight>().HasIndex(x => x.FreeSeats).HasName("Index_FreeSeats");
   //// Index with two columns
   modelBuilder.Entity<Flight>().HasIndex(f => new { f.Departure, f.Destination });

   // Unique Index: Then there could be only one flight on each flight route ...
   //modelBuilder.Entity<Flight>().HasIndex(f => new { f.Departure, f.Destination }).IsUnique();

   // Unique index and clustered index: there can only be one CI per table (usually PK)
   //modelBuilder.Entity<Flight>().HasIndex(f => new { f.Departure, f.Destination }).IsUnique().ForSqlServerIsClustered();

   //or:  builder.Entity<Flight>().HasIndex("Departure", "Destination");
   //builder.Entity<Passenger>().HasIndex(x => x.PersonID).IsUnique();
   #endregion

   #region Sequences
   // Define sequence
   //builder.HasSequence<int>("FlightNo", "dbo")
   // .IncrementsBy(5)
   // .StartsAt(1000);

   // Apply sequence 
   //builder.Entity<Flight>()
   // .Property(x => x.FlightNo)
   // .ValueGeneratedOnAdd()
   // .ForSqlServerUseSequenceHiLo("FlightNo", "dbo");

   // Other syntax:
   //builder.Entity<Flight>(b =>
   //{
   //b.HasKey(e => e.FlightNo);
   //b.Property(x => x.FlightNo).HasColumnName("PK_FlightNo");
   //b.Property(e => e.FlightNo).ValueGeneratedOnAdd().ForSqlServerUseSequenceHiLo("FlightNo", "dbo");
   //});
   #endregion

   #region AlternateKeys
   // --- Alternate Keys (Unique Index)
   //builder.Entity<Person>()
   //     .HasAlternateKey("Country", "IDCard")
   //     .HasName("Contraint_LandIDCard");
   #endregion

   base.OnModelCreating(modelBuilder);
  }

  private void ConfigureFlight(EntityTypeBuilder<Flight> f)
  {
   // ----------- PK
   f.HasKey(x => x.FlightNo);
   f.Property(x => x.FlightNo).ValueGeneratedNever();
   //// ----------- Length and null values
   f.Property(x => x.Memo).HasMaxLength(5000);
   f.Property(x => x.Seats).IsRequired();
   // ----------- Calculated column
   f.Property(x => x.Utilization)
    .HasComputedColumnSql("100.0-(([FreeSeats]*1.0)/[Seats])*100.0");

   // ----------- Default values
   f.Property(x => x.Price).HasDefaultValue(123.45m);
   f.Property(x => x.Departure).HasDefaultValue("(offen)");
   f.Property(x => x.Destination).HasDefaultValue("(offen)");
   f.Property(x => x.Date).HasDefaultValueSql("getdate()");

   // ----------- Indexes
   // Index with one column
   f.HasIndex(x => x.FreeSeats).HasName("Index_FreeSeats");
   // Index with two columns
   f.HasIndex(x => new { x.Departure, x.Destination });
  }

  public int SaveChangesWithLog()
  {
   this.Log();
   var e = this.SaveChanges();
   this.ClearLog();
   return e;
  }

  /// <summary>
  /// Overridden method, sets shadow property, and clears second-level cache
  /// </summary>
  /// <returns></returns>
  public override int SaveChanges()
  {
   #region for Second Level Cache!
   string[] changedEntityNames = this.GetChangedEntityNames();
   _efCacheServiceProvider.InvalidateCacheDependencies(changedEntityNames);
   #endregion

   #region Shadow property changes

   // Detect changes
   this.ChangeTracker.DetectChanges();

   // Search all new and changed flights
   var entries = this.ChangeTracker.Entries<Flight>()
       .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

   if (!String.IsNullOrEmpty(ShadowPropertyName))
   {
    // set the Shadow State column "LastChange" for all of them
    foreach (var entry in entries)
    {
     entry.Property(ShadowPropertyName).CurrentValue = DateTime.Now;
    }
   }

   // Save changes (we do not need DetectChanges() to be called again!)
   this.ChangeTracker.AutoDetectChangesEnabled = false;
   var result = base.SaveChanges();   // Call base class now
   this.ChangeTracker.AutoDetectChangesEnabled = true;
   return result;
   #endregion

  }

 }

}