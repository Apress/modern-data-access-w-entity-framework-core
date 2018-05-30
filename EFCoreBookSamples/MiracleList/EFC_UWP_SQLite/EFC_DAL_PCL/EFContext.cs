using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EFC_UWP_SQLite
{
 public class EFContext : DbContext
 {
  public DbSet<Eintrag> EintragSet { get; set; }
  public DbSet<Unterpunkt> UnterpunktSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
   optionsBuilder.UseSqlite("Filename=EinfacherMerkzettel2.db");
  }
 }

 public class Eintrag
 {
  public int EintragId { get; set; }
  public string Titel { get; set; }
  public DateTime Zeitpunkt { get; set; }
  public List<Unterpunkt> UnterpunktSet { get; set; }

  public string Ansicht { get { return Zeitpunkt.ToString() + ": " + Titel; } }
 }

 public class Unterpunkt
 {
  public int UnterpunktId { get; set; }
  public string Text { get; set; }
  public int EintragId { get; set; }
  public Eintrag Eintrag { get; set; }
 }
}