using BO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DA
{
 /// <summary>
 /// Configuration Class for Entity Class Flight
 /// EFCore >= 2.0
 /// </summary>
 class FlightETC : IEntityTypeConfiguration<Flight>
 {
  public void Configure(EntityTypeBuilder<Flight> f)
  {
   // ----------- PK
   f.HasKey(x => x.FlightNo);
   f.Property(x => x.FlightNo).ValueGeneratedNever();
   //// ----------- Length and null values
   f.Property(x => x.Memo).HasMaxLength(5000);
   f.Property(x => x.Seats).IsRequired();
   // ----------- Calculated column
   //f.Property(x => x.Utilization)
   // .HasComputedColumnSql("100.0-(([FreeSeats]*1.0)/[Seats])*100.0");

   // ----------- Default values
   f.Property(x => x.Price).HasDefaultValue(123.45m);
   //f.Property(x => x.Departure).HasDefaultValue("(not set)");
   //f.Property(x => x.Destination).HasDefaultValue("(not set)");
   //f.Property(x => x.Date).HasDefaultValueSql("getdate()");

   // ----------- Indexes
   // Index with one column
   f.HasIndex(x => x.FreeSeats).HasName("Index_FreeSeats");
   // Index with two columns
   f.HasIndex(x => new { x.Departure, x.Destination });
  }
 }
}
