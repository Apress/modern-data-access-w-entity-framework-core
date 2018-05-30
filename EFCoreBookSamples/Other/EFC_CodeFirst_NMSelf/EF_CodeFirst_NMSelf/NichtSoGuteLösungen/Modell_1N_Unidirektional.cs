//// mit dieser einfachen Lösung mit unidirektionalen n:m-Mapping kann man zwar alle Beziehungen abspeichern, aber man müsste jede Beziehung doppelt abspeichern, um alle Beziehungen sehen zu können.
//// Beispiel: Wenn man anlegt, dass DE an NL grenzt, weiß EF bei unidirektionaler Beziehung nicht, dass NL auch DE grenzt. Weder Zugriff auf Borders noch die nachstehende LINQ-Abfrage liefert nicht alle Ergebnisse.
////var borders = ctx.Countries.All(x => x.Borders.Any(y => y.Id == c.Id));


//using System.Collections.Generic;
//using System.Data.Entity;

//class Country
//{
// public int Id { get; set; }
// public string Name { get; set; }

// // N-M-Beziehung
// public virtual ICollection<Country> Borders { get; set; } = new List<Country>();


// public void AddBorderToCounty(Country c)
// {
//  this.Borders.Add(c);
// }

//}


//class WorldContext1 : DbContext
//{
// public DbSet<Country> Countries { get; set; }

// protected override void OnModelCreating(DbModelBuilder modelBuilder)
// {

//  
// }
//}
