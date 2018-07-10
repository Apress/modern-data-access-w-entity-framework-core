using System;
using System.Data.SqlClient;
using DA;
using EFC_Console;

static internal class ConnectionManagement
{
 /// <summary>
 /// Use external connection, must not be opened!
 /// </summary>
 public static void ExterneConnectionDemo()
 {
  using (var connection = new SqlConnection(Program.CONNSTRING))
  {
   using (var ctx = new WWWingsContext(connection))
   {
    // Load flight
    var f = ctx.FlightSet.Find(101);
    Console.WriteLine(f);
   }
  }
 }
}