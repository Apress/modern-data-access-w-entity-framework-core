using System;

namespace EFC_Console.AutoMapper
{
 class Person
 {
  public string GivenName { get; set; }
  public string Surname { get; set; }
  public DateTime Birthday { get; set; }
 }

 class Man : Person
 {
  public int NumberOfCars { get; set; }
 }

 class Woman : Person
 {
  public int NumberOfShoes { get; set; }
 }

 class PersonDTO
 {
  public string Name { get; set; }
  public int YearOfBirth { get; set; }
 }

 class ManDTO : PersonDTO
 {
  public byte NumberOfCars { get; set; }
 }

 class WomanDTO : PersonDTO
 {
  public byte NumberOfShoes { get; set; }
 }

 // see https://europa.eu/youreurope/citizens/family/couple/registered-partners/index_en.htm
 class RegisteredPartnership<T1, T2>
  where T1 : Person
  where T2 : Person
 {
  public T1 Partner1 { get; set; }
  public T2 Partner2 { get; set; }
  public DateTime Date { get; set; }
 }

 class Marriage<T1, T2>
  where T1 : Person // or: PersonDTO
  where T2 : Person // or: PersonDTO
 {
  public T1 Partner1 { get; set; }
  public T2 Partner2 { get; set; }
  public DateTime Date { get; set; }
 }

}