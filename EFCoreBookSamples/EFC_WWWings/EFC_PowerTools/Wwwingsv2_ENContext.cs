using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFC_PowerTools
{
    public partial class Wwwingsv2_ENContext : DbContext
    {
        public virtual DbSet<AircraftType> AircraftType { get; set; }
        public virtual DbSet<AircraftTypeDetail> AircraftTypeDetail { get; set; }
        public virtual DbSet<Airline> Airline { get; set; }
        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<Passenger> Passenger { get; set; }
        public virtual DbSet<Persondetail> Persondetail { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Data Source=E66;Initial Catalog=WWWingsV2_EN;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AircraftType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");
            });

            modelBuilder.Entity<AircraftTypeDetail>(entity =>
            {
                entity.HasKey(e => e.AircraftTypeId);

                entity.Property(e => e.AircraftTypeId).HasColumnName("AircraftTypeID");

                entity.HasOne(d => d.AircraftType)
                    .WithOne(p => p.AircraftTypeDetail)
                    .HasForeignKey<AircraftTypeDetail>(d => d.AircraftTypeId);
            });

            modelBuilder.Entity<Airline>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(3)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => new { e.FlightNo, e.PassengerId });

                entity.HasIndex(e => e.PassengerId);

                entity.Property(e => e.PassengerId).HasColumnName("PassengerID");

                entity.HasOne(d => d.FlightNoNavigation)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.FlightNo);

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.PassengerId);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.HasIndex(e => e.DetailId);

                entity.HasIndex(e => e.SupervisorPersonId);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.DetailId).HasColumnName("DetailID");

                entity.Property(e => e.Discriminator).IsRequired();

                entity.Property(e => e.Email).HasColumnName("EMail");

                entity.Property(e => e.FlightSchool).HasMaxLength(50);

                entity.Property(e => e.SupervisorPersonId).HasColumnName("SupervisorPersonID");

                entity.HasOne(d => d.Detail)
                    .WithMany(p => p.Employee)
                    .HasForeignKey(d => d.DetailId);

                entity.HasOne(d => d.SupervisorPerson)
                    .WithMany(p => p.InverseSupervisorPerson)
                    .HasForeignKey(d => d.SupervisorPersonId);
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(e => e.FlightNo);

                entity.HasIndex(e => e.AircraftTypeId);

                entity.HasIndex(e => e.AirlineCode);

                entity.HasIndex(e => e.CopilotId);

                entity.HasIndex(e => e.FreeSeats)
                    .HasName("Index_FreeSeats");

                entity.HasIndex(e => e.PilotId);

                entity.HasIndex(e => new { e.Departure, e.Destination });

                entity.Property(e => e.FlightNo).ValueGeneratedNever();

                entity.Property(e => e.AircraftTypeId).HasColumnName("AircraftTypeID");

                entity.Property(e => e.AirlineCode).HasMaxLength(3);

                entity.Property(e => e.Departure)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'(offen)')");

                entity.Property(e => e.Destination)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'(offen)')");

                entity.Property(e => e.FlightDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Price).HasDefaultValueSql("((123.45))");

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.Property(e => e.Utilization)
                    .HasColumnType("numeric(20, 8)")
                    .HasComputedColumnSql("((100.0)-(([FreeSeats]*(1.0))/[Seats])*(100.0))");

                entity.HasOne(d => d.AircraftType)
                    .WithMany(p => p.Flight)
                    .HasForeignKey(d => d.AircraftTypeId);

                entity.HasOne(d => d.AirlineCodeNavigation)
                    .WithMany(p => p.Flight)
                    .HasForeignKey(d => d.AirlineCode);

                entity.HasOne(d => d.Copilot)
                    .WithMany(p => p.FlightCopilot)
                    .HasForeignKey(d => d.CopilotId);

                entity.HasOne(d => d.Pilot)
                    .WithMany(p => p.FlightPilot)
                    .HasForeignKey(d => d.PilotId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.HasIndex(e => e.DetailId);

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.DetailId).HasColumnName("DetailID");

                entity.Property(e => e.Email).HasColumnName("EMail");

                entity.Property(e => e.Status).HasMaxLength(1);

                entity.HasOne(d => d.Detail)
                    .WithMany(p => p.Passenger)
                    .HasForeignKey(d => d.DetailId);
            });

            modelBuilder.Entity<Persondetail>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(3);

                entity.Property(e => e.Postcode).HasMaxLength(8);

                entity.Property(e => e.Street).HasMaxLength(30);
            });
        }
    }
}
