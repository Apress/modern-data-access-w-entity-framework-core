//using System;

//namespace EF_CodeFirst_NMSelf
//{
// class Program
// {
//  static void Main(string[] args)
//  {

//   var ctx = new WorldContext2();

//   Console.WriteLine(ctx.Database.Connection.ConnectionString);

//   // Beispiel zurücksetzen
//   ctx.Database.ExecuteSqlCommand("delete from Borders");
//   ctx.Database.ExecuteSqlCommand("delete from countries");
   
//   // Länder anlegen
//   var de = new Country();
//   de.Name = "Deutschland";
//   ctx.Countries.Add(de);
//   ctx.SaveChanges();

//   var nl = new Country();
//   nl.Name = "Niederlande";
//   ctx.Countries.Add(nl);
//   nl.Borders.Add(de);
//   ctx.SaveChanges();

//   var dk = new Country();
  
//   dk.Name = "Dänemark";
//   ctx.Countries.Add(dk);
//   dk.Borders.Add(de);
//   ctx.SaveChanges();

//   var fr = new Country();
//   fr.Name = "Frankreich";
//   ctx.Countries.Add(fr);
//   fr.Borders.Add(de);
//   fr.Borders.Add(nl);
//   ctx.SaveChanges();

//   // kontrollausgabe
//   Console.WriteLine("Alle Länder mit Ihren Grenzen");
//   foreach (var country in ctx.Countries)
//   {
//    Console.WriteLine("--------- " + country.Name);
//    foreach (var neighbour in country.Borders)
//    {
//     Console.WriteLine(neighbour.Name);
//    }

//   }

//   Console.ReadLine();
//  }
// }
//}
