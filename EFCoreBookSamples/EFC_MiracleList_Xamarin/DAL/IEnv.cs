namespace EFC_Xamarin
{
 /// <summary>
 /// Custom Interface for getting the OS specific folder for the DB file
 /// </summary>
 public interface IEnv
 {
  string GetDbFolder();
 }
}