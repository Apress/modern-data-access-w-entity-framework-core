
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace EFC_Xamarin
{
 /// <summary>
 /// Entity Framework context
 /// </summary>
 public class EFContext : DbContext
 {

  static public string Path { get; set; }
  public DbSet<Task> TaskSet { get; set; }
  public DbSet<TaskDetail> TaskDetailSet { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
   EFContext.Path = System.IO.Path.Combine(DependencyService.Get<IEnv>().GetDbFolder(), "miraclelist.db");
   // set provider and database file path
   optionsBuilder.UseSqlite($"Filename={  EFContext.Path}");
  }
 }
}
