using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace EFC_Console.AutoMapper
{
 public class Source
 {
  public int Value { get; set; }
 }

 public class Destination
 {
  public int Value { get; set; }
 }

 public class Foo
 {
  public int baz;
 }

 public class Bar
 {
  public uint baz;
 }

 public class OuterSource
 {
  public int Value { get; set; }
  public InnerSource Inner { get; set; }
 }

 public class InnerSource
 {
  public int OtherValue { get; set; }
 }

 public class OuterDest
 {
  public int Value { get; set; }
  public InnerDest Inner { get; set; }
 }

 public class InnerDest
 {
  public int OtherValue { get; set; }
 }

 public class AutoMapper_HelloWorld
 {
  public static void RunSimpleDemo()
  {
   Mapper.Initialize(cfg =>
   {
    cfg.CreateMap<Foo, Bar>().ForMember(dest => dest.baz, opt => opt.Condition(src => (src.baz >= 0)));
   });

   var foo1 = new Foo { baz = 1 };
   var bar1 = Mapper.Map<Bar>(foo1);

   Console.WriteLine("bar1.baz={0}", bar1.baz);

   var foo2 = new Foo { baz = 100 };
   var bar2 = Mapper.Map<Bar>(foo2);


   Console.WriteLine("bar2.baz={0}", bar2.baz);

   Mapper.Initialize(cfg => cfg.CreateMap<Source, Destination>());

   var sources = new[]
   {
    new Source { Value = 5 },
    new Source { Value = 6 },
    new Source { Value = 7 }
   };

   IEnumerable<Destination> ienumerableDest = Mapper.Map<Source[], IEnumerable<Destination>>(sources);
   ICollection<Destination> icollectionDest = Mapper.Map<Source[], ICollection<Destination>>(sources);
   IList<Destination> ilistDest = Mapper.Map<Source[], IList<Destination>>(sources);
   List<Destination> listDest = Mapper.Map<Source[], List<Destination>>(sources);
   Destination[] arrayDest = Mapper.Map<Source[], Destination[]>(sources);

   Console.WriteLine(arrayDest.Count());

   var config = new MapperConfiguration(cfg => {
    cfg.CreateMap<OuterSource, OuterDest>();
    cfg.CreateMap<InnerSource, InnerDest>();
   });
   config.AssertConfigurationIsValid();

   var source = new OuterSource
   {
    Value = 5,
    Inner = new InnerSource { OtherValue = 15 }
   };
   var mapper = config.CreateMapper();
   var dest2 = mapper.Map<OuterSource, OuterDest>(source);

   Console.WriteLine(dest2.Inner.OtherValue);
  }
 }

}