using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EFC_WWWings1_Reverse_NETFX_
{
    public partial class WWWingsV1Context : DbContext
    {
        public virtual DbSet<Airport> Airport { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Flight> Flight { get; set; }
        public virtual DbSet<FlightPassenger> FlightPassenger { get; set; }
        public virtual DbSet<MigrationHistory> MigrationHistory { get; set; }
        public virtual DbSet<Passenger> Passenger { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Pilot> Pilot { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=.;Database=WWWingsV1;Trusted_Connection=True;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Airport>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("Airport", "Properties");

                entity.Property(e => e.Name)
                    .HasColumnType("nchar(30)")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Employee", "People");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HireDate).HasColumnType("datetime");

                entity.Property(e => e.SupervisorPersonId).HasColumnName("Supervisor_PersonID");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MI_Employee_PE_Person");

                entity.HasOne(d => d.SupervisorPerson)
                    .WithMany(p => p.InverseSupervisorPerson)
                    .HasForeignKey(d => d.SupervisorPersonId)
                    .HasConstraintName("FK_Employee_Employee");
            });

            modelBuilder.Entity<Flight>(entity =>
            {
                entity.HasKey(e => e.FlightNo);

                entity.ToTable("Flight", "Operation");

                entity.Property(e => e.FlightNo).ValueGeneratedNever();

                entity.Property(e => e.Airline).HasMaxLength(3);

                entity.Property(e => e.Departure)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Destination)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FlightDate).HasColumnType("datetime");

                entity.Property(e => e.Memo).IsUnicode(false);

                entity.Property(e => e.PilotPersonId).HasColumnName("Pilot_PersonID");

                entity.Property(e => e.Timestamp).IsRowVersion();

                entity.Property(e => e.Utilization).HasColumnName("Utilization ");

                entity.HasOne(d => d.PilotPerson)
                    .WithMany(p => p.Flight)
                    .HasForeignKey(d => d.PilotPersonId)
                    .HasConstraintName("FK_FL_Flight_PI_Pilot");
            });

            modelBuilder.Entity<FlightPassenger>(entity =>
            {
                entity.HasKey(e => new { e.FlightFlightNo, e.PassengerPersonId })
                    .ForSqlServerIsClustered(false);

                entity.ToTable("Flight_Passenger", "Operation");

                entity.Property(e => e.FlightFlightNo).HasColumnName("Flight_FlightNo");

                entity.Property(e => e.PassengerPersonId).HasColumnName("Passenger_PersonID");

                entity.HasOne(d => d.FlightFlightNoNavigation)
                    .WithMany(p => p.FlightPassenger)
                    .HasForeignKey(d => d.FlightFlightNo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Flight_Passenger_Flight");

                entity.HasOne(d => d.PassengerPerson)
                    .WithMany(p => p.FlightPassenger)
                    .HasForeignKey(d => d.PassengerPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Flight_Passenger_Passenger");
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => new { e.MigrationId, e.ContextKey });

                entity.ToTable("__MigrationHistory");

                entity.Property(e => e.MigrationId).HasMaxLength(150);

                entity.Property(e => e.ContextKey).HasMaxLength(300);

                entity.Property(e => e.Model).IsRequired();

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Passenger>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Passenger", "People");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CustomerSince).HasColumnType("datetime");

                entity.Property(e => e.PassengerStatus).HasColumnType("nchar(1)");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Passenger)
                    .HasForeignKey<Passenger>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PS_Passenger_PE_Person");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "People");

                entity.Property(e => e.PersonId).HasColumnName("PersonID");

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Country).HasMaxLength(2);

                entity.Property(e => e.Email)
                    .HasColumnName("EMail")
                    .HasMaxLength(50);

                entity.Property(e => e.GivenName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Memo).IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Pilot>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("Pilot", "People");

                entity.Property(e => e.PersonId)
                    .HasColumnName("PersonID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FlightSchool).HasMaxLength(50);

                entity.Property(e => e.Flightscheintyp).HasColumnType("nchar(1)");

                entity.Property(e => e.LicenseDate).HasColumnType("datetime");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.Pilot)
                    .HasForeignKey<Pilot>(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PI_Pilot_MI_Employee");
            });
        }
    }
}
