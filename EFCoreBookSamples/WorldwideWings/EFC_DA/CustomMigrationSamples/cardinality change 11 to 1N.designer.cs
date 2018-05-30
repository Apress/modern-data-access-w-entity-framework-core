using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Kontext;

namespace EFC_Kontext.Migrations
{
    [DbContext(typeof(WWWingsContext))]
    [Migration("Kardinaliaet11wird1N")]
    partial class Kardinaliaet11wird1N
 {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GO.Buchung", b =>
                {
                    b.Property<int>("FlugNr");

                    b.Property<int>("PassagierId");

                    b.HasKey("FlugNr", "PassagierId");

                    b.HasAlternateKey("FlugNr");

                    b.HasIndex("FlugNr");

                    b.HasIndex("PassagierId");

                    b.ToTable("Buchung");
                });

            modelBuilder.Entity("GO.Flug", b =>
                {
                    b.Property<int>("FlugNr");

                    b.Property<string>("departure")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("(offen)")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("Bestreikt");

                    b.Property<int?>("CopilotId");

                    b.Property<DateTime>("Datum")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("FlugDatum")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("Fluggesellschaft");

                    b.Property<byte?>("FlugzeugTypNr");

                    b.Property<short?>("FreeSeats")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("LastChange");

                    b.Property<string>("Memo")
                        .HasAnnotation("MaxLength", 5000);

                    b.Property<bool>("NichtRaucherFlug");

                    b.Property<int>("PilotId");

                    b.Property<short?>("Plaetze")
                        .IsRequired();

                    b.Property<decimal>("Preis")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(123.45m);

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<string>("Zielort")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("(offen)")
                        .HasAnnotation("MaxLength", 50);

                    b.HasKey("FlugNr");

                    b.HasIndex("CopilotId");

                    b.HasIndex("FlugNr");

                    b.HasIndex("FlugzeugTypNr");

                    b.HasIndex("PilotId");

                    b.HasIndex("Plaetze");

                    b.HasIndex("departure", "Zielort");

                    b.ToTable("Flug");
                });

            modelBuilder.Entity("GO.Flugdetail", b =>
                {
                    b.Property<int>("FlugNr");

                    b.Property<DateTime?>("Ankunftszeit");

                    b.Property<string>("Gate")
                        .HasAnnotation("MaxLength", 3);

                    b.HasKey("FlugNr");

                    b.ToTable("Flugdetail");
                });

            modelBuilder.Entity("GO.Flughafen", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Land")
                        .HasAnnotation("MaxLength", 40);

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<string>("Ort")
                        .HasAnnotation("MaxLength", 40);

                    b.HasKey("Id");

                    b.ToTable("Flughafen");
                });

            modelBuilder.Entity("GO.Flugzeugtyp", b =>
                {
                    b.Property<byte>("TypID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Hersteller");

                    b.Property<string>("Name");

                    b.HasKey("TypID");

                    b.ToTable("Flugzeugtyp");
                });

            modelBuilder.Entity("GO.Passagier", b =>
                {
                    b.Property<int>("PersonID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EMail");

                    b.Property<DateTime?>("Geburtsdatum");

                    b.Property<DateTime?>("KundeSeit");

                    b.Property<string>("Name");

                    b.Property<string>("PassagierStatus")
                        .HasAnnotation("MaxLength", 1);

                    b.Property<string>("Vorname");

                    b.HasKey("PersonID");

                    b.HasIndex("PersonID")
                        .IsUnique();

                    b.ToTable("Passagier");
                });

            modelBuilder.Entity("GO.Persondetail", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Foto");

                    b.Property<string>("Land")
                        .HasAnnotation("MaxLength", 3);

                    b.Property<string>("Memo");

                    b.Property<int?>("PassagierPersonID");

                    b.Property<int?>("PilotPersonID");

                    b.Property<string>("Planet");

                    b.Property<string>("Plz")
                        .HasAnnotation("MaxLength", 5);

                    b.Property<string>("Stadt")
                        .HasAnnotation("MaxLength", 30);

                    b.Property<string>("Strasse")
                        .HasAnnotation("MaxLength", 30);

                    b.HasKey("ID");

                    b.HasIndex("PassagierPersonID");

                    b.HasIndex("PilotPersonID");

                    b.ToTable("Persondetail");
                });

            modelBuilder.Entity("GO.Pilot", b =>
                {
                    b.Property<int>("PersonID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EMail");

                    b.Property<DateTime>("FlugscheinSeit");

                    b.Property<string>("Flugscheintyp");

                    b.Property<string>("Flugschule");

                    b.Property<int?>("Flugstunden");

                    b.Property<DateTime?>("Geburtsdatum");

                    b.Property<string>("Name");

                    b.Property<string>("Vorname");

                    b.HasKey("PersonID");

                    b.ToTable("Pilot");
                });

            modelBuilder.Entity("GO.Test", b =>
                {
                    b.Property<byte>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("TBool");

                    b.Property<byte[]>("TByteArray");

                    b.Property<Guid>("TGuid");

                    b.Property<string>("TString");

                    b.Property<decimal>("ZDecimal");

                    b.Property<double>("ZDouble");

                    b.Property<float>("ZFloat");

                    b.Property<int>("ZInt");

                    b.Property<long>("ZLong");

                    b.Property<short>("ZShort");

                    b.Property<byte[]>("a1");

                    b.HasKey("ID");

                    b.ToTable("Test");
                });

            modelBuilder.Entity("GO.Buchung", b =>
                {
                    b.HasOne("GO.Flug", "Flug")
                        .WithMany("Buchungen")
                        .HasForeignKey("FlugNr")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GO.Passagier", "Passagier")
                        .WithMany("Buchungen")
                        .HasForeignKey("PassagierId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GO.Flug", b =>
                {
                    b.HasOne("GO.Pilot", "Copilot")
                        .WithMany("FluegeAlsCopilot")
                        .HasForeignKey("CopilotId");

                    b.HasOne("GO.Flugdetail", "Detail")
                        .WithMany()
                        .HasForeignKey("FlugNr")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GO.Flugzeugtyp", "Flugzeugtyp")
                        .WithMany("flightSet")
                        .HasForeignKey("FlugzeugTypNr");

                    b.HasOne("GO.Pilot", "Pilot")
                        .WithMany("FluegeAlsPilot")
                        .HasForeignKey("PilotId");
                });

            modelBuilder.Entity("GO.Persondetail", b =>
                {
                    b.HasOne("GO.Passagier")
                        .WithMany("Detail")
                        .HasForeignKey("PassagierPersonID");

                    b.HasOne("GO.Pilot")
                        .WithMany("Detail")
                        .HasForeignKey("PilotPersonID");
                });
        }
    }
}
