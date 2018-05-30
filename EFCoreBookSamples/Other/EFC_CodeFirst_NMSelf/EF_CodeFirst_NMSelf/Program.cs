using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EF_CodeFirst_NMSelf
{
 class Program
 {
  static void Main(string[] args)
  {
   var ctx = new WorldContext();
   ctx.Database.EnsureCreated();

   // Reset example
   ctx.Database.ExecuteSqlCommand("delete from Border");
   ctx.Database.ExecuteSqlCommand("delete from country");

   // Create countries
   var de = new Country();
   de.Name = "Germany";
   ctx.Countries.Add(de);
   ctx.SaveChanges();

   var nl = new Country();
   nl.Name = "Netherlands";
   ctx.Countries.Add(nl);
   nl.AddBorderToCounty(de);
   ctx.SaveChanges();

   var dk = new Country();
   dk.Name = "Denmark";
   ctx.Countries.Add(dk);
   dk.AddBorderToCounty(de);
   ctx.SaveChanges();

   var be = new Country();
   be.Name = "Belgium";
   ctx.Countries.Add(be);
   be.AddBorderToCounty(de);
   be.AddBorderToCounty(nl);
   ctx.SaveChanges();

   var fr = new Country();
   fr.Name = "France";
   ctx.Countries.Add(fr);
   fr.AddBorderToCounty(de);
   ctx.SaveChanges();

   var cz = new Country();
   cz.Name = "Czech Republic";
   ctx.Countries.Add(cz);
   cz.AddBorderToCounty(de);
   ctx.SaveChanges();

   var lu = new Country();
   lu.Name = "Luxembourg";
   ctx.Countries.Add(lu);
   lu.AddBorderToCounty(de);
   lu.AddBorderToCounty(fr);
   lu.AddBorderToCounty(be);
   ctx.SaveChanges();

   var pl = new Country();
   pl.Name = "Poland";
   ctx.Countries.Add(pl);
   pl.AddBorderToCounty(de);
   pl.AddBorderToCounty(cz);
   ctx.SaveChanges();

   var at = new Country();
   at.Name = "Austria";
   ctx.Countries.Add(at);
   at.AddBorderToCounty(de);
   at.AddBorderToCounty(cz);
   ctx.SaveChanges();

   var ch = new Country();
   ch.Name = "Switzerland";
   ctx.Countries.Add(ch);
   ch.AddBorderToCounty(de);
   ch.AddBorderToCounty(fr);
   ch.AddBorderToCounty(at);
   ctx.SaveChanges();

   Console.WriteLine("All countries with their borders");
   foreach (var country in ctx.Countries)
   {
    Console.WriteLine("--------- " + country.Name);

    // now explicitly load the neighboring countries, as Lazy Loading in EFC does not work
    //var borders1 = ctx.Countries.Where(x => x.IncomingBorders.Any(y => y.Country_Id == country.Id)).ToList();
    //var borders2 = ctx.Countries.Where(x => x.OutgoingBorders.Any(y => y.Country_Id1 == country.Id)).ToList();
    //var allborders = borders1.Union(borders2).OrderBy(x=>x.Name);;

    // better: encapsulated in the context class:
    var allborders = ctx.GetNeigbours(country.Id);

    foreach (var neighbour in allborders)
    {
     Console.WriteLine(neighbour.Name);
    }
   }

   Console.WriteLine("=== DONE!");
   Console.ReadLine();
  }
 }
}
