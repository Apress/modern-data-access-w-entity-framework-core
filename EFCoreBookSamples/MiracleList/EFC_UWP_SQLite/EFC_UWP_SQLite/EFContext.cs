using Microsoft.EntityFrameworkCore;

namespace EFC_UWP_SQLite
{
 /// <summary>
 /// Entity Framework core context
 /// </summary>
 public class EFContext : DbContext
 {
  public static string FileName = "MiracleList.db";

  public DbSet<Task> TaskSet { get; set; }
  public DbSet<TaskDetail> TaskDetailSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
   // Set provider and database filename
   optionsBuilder.UseSqlite($"Filename={FileName}");
  }
 }
}