using Microsoft.EntityFrameworkCore;

namespace EFC_UWP.DAL
{
 public enum Provider
 {
  SQLite, SQLServer
 }
 /// <summary>
 /// Entity Framework Core context
 /// </summary>
 public class EFContext : DbContext
 {
  public static Provider Provider { get; set; } = Provider.SQLServer;
  public static string FileName = "MiracleList.db";

  public DbSet<Task> TaskSet { get; set; }
  public DbSet<TaskDetail> TaskDetailSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
   switch (EFContext.Provider)
   {
    case Provider.SQLite:
     // Set provider and database filename
     optionsBuilder.UseSqlite($"Filename={FileName}");
     break;
    case Provider.SQLServer:
     // TODO: Aus Konfiguration auslesen
    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MiracleListLight_UWP;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
     break;
    default:
     throw new System.Exception("Provider " + EFContext.Provider + " not supported!");
   }
  }

  /// <summary>
  /// Information about the location of the database for display in the UI
  /// </summary>
  public string DBInfo
  {
   get
   {
    if (this.Database.ProviderName.Contains("SqlServer"))
    {
     return this.Database.GetDbConnection().Database + "@" + this.Database.GetDbConnection().DataSource;
    }
    else // SQLite
    {
     return  this.Database.GetDbConnection().DataSource; // ApplicationData.Current.LocalFolder.Path  + @"\" +
    }
   }
  }
 }
}