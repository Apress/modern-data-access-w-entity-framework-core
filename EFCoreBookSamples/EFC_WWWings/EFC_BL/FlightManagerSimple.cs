using System;
using System.Linq;
using BO;
using DA;

namespace BL
{
 /// <summary>
 /// A repository class whose method returns an IQuerably<Flight>
 /// EFCBook
 /// </summary>
 class FlightManagerSimple : IDisposable
 {
  private WWWingsContext ctx = new WWWingsContext();
  public IQueryable<Flight> GetBaseQuery()
  {
   var query = (from x in ctx.FlightSet
                  where x.FreeSeats > 0
                  select x);
   return query;
  }

  public void Dispose()
  {
   ctx.Dispose();
  }
 }
}
