using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

class Border
{
 // foreign key for IncomingCountry
 public int Country_Id { get; set; }
 // foreign key for OutgoingCountry
 public int Country_Id1 { get; set; }

 public virtual Country IncomingCountry { get; set; } 
 public virtual Country OutgoingCountry { get; set; } 
}

class Country
{
 public int Id { get; set; }
 public string Name { get; set; }

 // N-M relationship via Borders
 public virtual ICollection<Border> IncomingBorders { get; set; } = new List<Border>();
 public virtual ICollection<Border> OutgoingBorders { get; set; } = new List<Border>();

 public void AddBorderToCounty(Country c)
 {
  var b = new Border() {Country_Id = this.Id, Country_Id1 = c.Id};
  this.OutgoingBorders.Add(b);
 }
}

class WorldContext : DbContext
{
 public DbSet<Country> Countries { get; set; }
 public DbSet<Country> Borders { get; set; }
 protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 {
  optionsBuilder.UseSqlServer(@"Server=.;Database=EFC_NMSelf;Trusted_Connection=True;MultipleActiveResultSets=True");
 }

 protected override void OnModelCreating(ModelBuilder modelBuilder)
 {
  // Configure primary key
  modelBuilder.Entity<Border>().HasKey(x => new {x.Country_Id, x.Country_Id1});
  // Configure relationships and foreign keys
  modelBuilder.Entity<Border>().HasOne<Country>(x => x.IncomingCountry).WithMany(x => x.IncomingBorders).HasForeignKey(x=>x.Country_Id1).OnDelete(DeleteBehavior.Restrict);
  modelBuilder.Entity<Border>().HasOne<Country>(x => x.OutgoingCountry).WithMany(x => x.OutgoingBorders).HasForeignKey(x => x.Country_Id).OnDelete(DeleteBehavior.Restrict); ;
 }

 /// <summary>
 /// Get all neighbors by the union of the two sets
 /// </summary>
 /// <param name="countryId"></param>
 /// <returns></returns>
 public IEnumerable<Country> GetNeigbours(int countryId)
 {
  var borders1 = this.Countries.Where(x => x.IncomingBorders.Any(y => y.Country_Id == countryId)).ToList();
  var borders2 = this.Countries.Where(x => x.OutgoingBorders.Any(y => y.Country_Id1 == countryId)).ToList();
  var allborders = borders1.Union(borders2).OrderBy(x=>x.Name);
  return allborders;
 }
}