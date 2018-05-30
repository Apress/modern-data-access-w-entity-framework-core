using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;


namespace EF_CodeFirst_NMSelf.Migrations
{
    [DbContext(typeof(WorldContext))]
    partial class WorldContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Border", b =>
                {
                    b.Property<int>("Country_Id");

                    b.Property<int>("Country_Id1");

                    b.HasKey("Country_Id", "Country_Id1");

                    b.HasIndex("Country_Id");

                    b.HasIndex("Country_Id1");

                    b.ToTable("Border");
                });

            modelBuilder.Entity("Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("Border", b =>
                {
                    b.HasOne("Country", "IncomingCountry")
                        .WithMany("IncomingBorders")
                        .HasForeignKey("Country_Id");

                    b.HasOne("Country", "OutgoingCountry")
                        .WithMany("OutgoingBorders")
                        .HasForeignKey("Country_Id1");
                });
        }
    }
}
