using BO;
using Microsoft.EntityFrameworkCore;

namespace DA
{

 /// <summary>
 /// EFCore context class for World Wings Wings database schema version 7.0
 /// </summary>
 public class WWWingsContext : DbContext
 {

  #region Tables
  public DbSet<Flight> FlightSet { get; set; }
  public DbSet<Passenger> PassengerSet { get; set; }
  public DbSet<Pilot> PilotSet { get; set; }
  public DbSet<Booking> BookingSet { get; set; }
  #endregion


  public static string ConnectionString { get; set; } =
@"Server=.;Database=WWWings70_EN_Step1;Trusted_Connection=True;MultipleActiveResultSets=True;App=Entityframework";

  public WWWingsContext() { }

  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {
   builder.UseSqlServer(ConnectionString);
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
   #region Configure the double relation between Flight and Pilot
   // fix for problem:  "Unable to determine the relationship represented by navigation property Flug.Pilot' of type 'Pilot'. Either manually configure the relationship, or ignore this property using the '[NotMapped]' attribute or by using 'EntityTypeBuilder.Ignore' in 'OnModelCreating'."
   modelBuilder.Entity<Pilot>().HasMany(p => p.FlightAsPilotSet).WithOne(p => p.Pilot).HasForeignKey(f => f.PilotId).OnDelete(DeleteBehavior.Restrict);
   modelBuilder.Entity<Pilot>().HasMany(p => p.FlightAsCopilotSet)
    .WithOne(p => p.Copilot).HasForeignKey(f => f.CopilotId).OnDelete(DeleteBehavior.Restrict); ;
   #endregion

   #region Composite key for BookingSet
   // fix for problem: 'The entity type 'Booking' requires a primary key to be defined.'
   modelBuilder.Entity<Booking>().HasKey(b => new { FlightNo = b.FlightNo, PassengerID = b.PassengerID });
   #endregion

   #region Other Primary Keys
   // fix for problem: 'The entity type 'Employee' requires a primary key to be defined.'
   modelBuilder.Entity<Employee>().HasKey(x => x.PersonID);

   // fix for problem: 'The entity type 'Flight' requires a primary key to be defined.'
   modelBuilder.Entity<Flight>().HasKey(x => x.FlightNo);

   // fix for problem: 'The entity type 'Passenger' requires a primary key to be defined.'
   modelBuilder.Entity<Passenger>().HasKey(x => x.PersonID);
   #endregion

   base.OnModelCreating(modelBuilder);
  }
 }
}