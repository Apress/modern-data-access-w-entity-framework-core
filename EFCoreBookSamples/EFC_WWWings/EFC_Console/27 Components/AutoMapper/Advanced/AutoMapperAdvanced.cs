using AutoMapper;
using ITVisions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFC_Console.AutoMapper
{
 
 class AutoMapperAdvanced
 {

  [EFCBook]
  public static void Inheritance()
  {

   CUI.Headline(nameof(Inheritance));

   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Person, PersonDTO>()
       .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
       .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
    cfg.CreateMap<Man, ManDTO>()
        .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
        .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
    cfg.CreateMap<Woman, WomanDTO>()
        .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
        .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
   });

   // or shorter using include()
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Person, PersonDTO>()
          .Include<Man, ManDTO>()
          .Include<Woman, WomanDTO>()
          .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
          .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
    cfg.CreateMap<Man, ManDTO>();
    cfg.CreateMap<Woman, WomanDTO>();
   });

   var m = new Man()
   {
    GivenName = "John",
    Surname = "Doe",
    Birthday = new DateTime(1980, 10, 1),
    NumberOfCars = 40
   };

   PersonDTO mDTO1 = Mapper.Map<PersonDTO>(m);
   Console.WriteLine(mDTO1.Name + " *" + mDTO1.YearOfBirth);

  ManDTO mDTO1b = Mapper.Map<ManDTO>(m);
   Console.WriteLine(mDTO1b.Name + " *" + mDTO1b.YearOfBirth);

   ManDTO mDTO2 = (ManDTO)Mapper.Map(m, m.GetType(), typeof(ManDTO));
   Console.WriteLine(mDTO2.Name + " *" + mDTO2.YearOfBirth + " owns " + mDTO2.NumberOfCars + " cars.");

   ManDTO mDTO3 = Mapper.Map<ManDTO>(m);
   Console.WriteLine(mDTO3.Name + " *" + mDTO3.YearOfBirth + " owns " + mDTO3.NumberOfCars + " cars.");

   // gender transformation: man -> woman
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Man, Woman>()
          .ForMember(z => z.NumberOfShoes, map => map.MapFrom(q => q.NumberOfCars * 10));
   });

   Woman f = Mapper.Map<Woman>(m);
   Console.WriteLine(f.GivenName + " " + f.Surname + " *" + f.Birthday + " owns " + f.NumberOfShoes + " shoes.");
  }

  [EFCBook]
  public static void GenericHomogeneousList()
  {
   CUI.Headline(nameof(GenericHomogeneousList));

   var PersonSet = new List<Person>();
   for (int i = 0; i < 100; i++)
   {
    PersonSet.Add(new Person() { GivenName="John", Surname="Doe"});
   }

   // define Mapping
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Person, PersonDTO>()
.ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
.ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
   });

   // Convert list
   var PersonDTOSet = Mapper.Map<List<PersonDTO>>(PersonSet);

   Console.WriteLine(PersonDTOSet.Count());
   foreach (var p in PersonDTOSet.Take(5))
   {
    Console.WriteLine(p.Name + ": "+ p.YearOfBirth);
   }
  }

  /// <summary>
  ///  TODO: Das geht  nicht. siehe https://github.com/AutoMapper/AutoMapper/wiki/Lists-and-arrays 
  /// </summary>
  [NotYetInTheBook]
  public static void GenericHeterogeneousList()
  {
   var f = new Woman();
   var m = new Man();
   var u = new Uri("http://www.IT-Visions.de");
   // Generische Liste erzeugen und befüllen
   var set = new Person[] { f, m };

   // Mappingsdefinieren
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Person, PersonDTO>()
          .Include<Man, ManDTO>()
          .Include<Woman, WomanDTO>()
          .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
          .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
    cfg.CreateMap<Man, ManDTO>();
    cfg.CreateMap<Woman, WomanDTO>();
   });

   //Mapper.CreateMap<System.Uri, string>().ConvertUsing<UriToStringConverter>();

   // Einzelmapping zum test
   //string s = Mapper.Map<string>(u);

   var set2 = Mapper.Map<Person[], PersonDTO[]>(set);


   Console.WriteLine(((ManDTO)set2[0]).NumberOfCars);
   Console.WriteLine(((WomanDTO)set2[1]).NumberOfShoes);
  }

  public class UriToStringConverter : ITypeConverter<System.Uri, string>
  {
   public string Convert(System.Uri uri, string s, ResolutionContext context)
   {
    if (uri == null)
    {
     return String.Empty;
    }

    return uri.AbsoluteUri;
   }
  }

  [EFCBook]
  public static void GenericDemo()
  {
   CUI.MainHeadline(nameof(GenericDemo));

   // A registered partnership between two men
   var m1 = new Man() { GivenName = "Heinz", Surname = "Müller" };
   var m2 = new Man() { GivenName = "Gerd", Surname = "Meier" };
   var ep = new RegisteredPartnership<Man, Man>() { Partner1 = m1, Partner2 = m2, Date = new DateTime(2015, 5, 28) };

   // Define Mapping from RegisteredPartnership to Marriage
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap(typeof(RegisteredPartnership<,>), typeof(Marriage<,>));
   });

   // Do mapping
   Marriage<Man, Man> marriage = Mapper.Map<Marriage<Man, Man>>(ep);
   Console.WriteLine(marriage.Partner1.GivenName + " + " + marriage.Partner2.GivenName + ": " + marriage.Date.ToShortDateString());

   // An additional mapping of the generic parameters is possible with additional mapping for the type of the parameter

   //Mapper.Initialize(cfg =>
   //{
   // cfg.CreateMap(typeof(RegisteredPartnership<,>), typeof(Marriage<,>));
   // cfg.CreateMap<Man, ManDTO>()
   //  .ForMember(z => z.NumberOfCars, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
   //  .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year));
   //});

   //Marriage<ManDTO, ManDTO> marriageDTO = Mapper.Map<Marriage<ManDTO, ManDTO>>(ep);
   //Console.WriteLine(marriageDTO.Partner1.Name + " + " + marriageDTO.Partner2.Name + ": " + marriage.Date.ToShortDateString());
  }

  [EFCBook]
  public static void BeforeAfterDemo()
  {
   CUI.Headline(nameof(BeforeAfterDemo));

   var PersonSet = new List<Person>();
   for (int i = 0; i < 10; i++)
   {
    PersonSet.Add(new Person()
    {
     GivenName = "John",
     Surname = "Doe",
     Birthday = new DateTime(1980, 10, 1),
    });
   }

   // Define mapping
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Person, PersonDTO>()
       .ForMember(z => z.Name, map => map.MapFrom(q => q.GivenName + " " + q.Surname))
       .ForMember(z => z.YearOfBirth, map => map.MapFrom(q => q.Birthday.Year))
       .BeforeMap((q, z) => q.GivenName = (String.IsNullOrEmpty(q.GivenName) ? q.GivenName = "?" : q.GivenName))
       .BeforeMap((q, z) => q.Surname = (String.IsNullOrEmpty(q.Surname) ? q.Surname = "?" : q.Surname))
       .AfterMap((q, z) => z.Name = GetName(z.Name, z.YearOfBirth));
    cfg.CreateMap<DateTime, Int32>().ConvertUsing(ConvertDateTimeToInt);
   });

   // Map list
   var PersonDTOSet = Mapper.Map<List<PersonDTO>>(PersonSet);

   foreach (var p in PersonDTOSet)
   {
    Console.WriteLine(p.Name + " in born in year " + p.YearOfBirth);
   }
  }

  /// <summary>
  /// Converges DateTime into integer (only extracts year)
  /// </summary>
  /// <returns></returns>
  public static Int32 ConvertDateTimeToInt(DateTime d)
  {
   return d.Year;
  }

  /// <summary>
  /// Method called as part of AfterMap()
  /// </summary>
  /// <param name="n">Surname</param>
  /// <param name="yearOfBirth">YearOfBirth</param>
  /// <returns></returns>
  public static string GetName(string name, int yearOfBirth)
  {
   if (yearOfBirth == 0) return name;
   if (yearOfBirth <= 1980) return name + " (too young)";
   return name + " (" + yearOfBirth +")";
  }
 }
}