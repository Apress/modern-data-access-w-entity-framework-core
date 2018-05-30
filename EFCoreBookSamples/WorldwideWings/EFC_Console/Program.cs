using ITVisions;
using ITVisions.CLR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using EFC_Console.AutoMapper;
using EFC_Console.OOM;

namespace EFC_Console
{

 /// <summary>
 /// Entity Framework Core Examples
 /// (C) Dr. Holger Schwichtenberg 2016-2018, www.EFCore.net
 /// In Main() call the samples you like. 
 /// </summary>
 class Program
 {
  public static object LINQ_ListAsync { get; private set; }
  public static object Demo_BeziehungenAnlegen { get; private set; }

  public static string CONNSTRING = @"Server=.;Database=WWWingsV2_EN;Trusted_Connection=True;MultipleActiveResultSets=True;App=EFCoreDemos";

  static void Main(string[] args)
  {
   DA.WWWingsContext.ConnectionString = Program.CONNSTRING;


   CUI.Print("Entity Framework Core Samples", ConsoleColor.Blue, ConsoleColor.White);
   CUI.Print("(C) Dr. Holger Schwichtenberg 2016-2018, www.EFCore.net", ConsoleColor.Blue, ConsoleColor.White);
   var assembly = Assembly.GetAssembly(typeof(Microsoft.EntityFrameworkCore.DbContext));
   var informalVersion = assembly.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).OfType<AssemblyInformationalVersionAttribute>().FirstOrDefault()?.InformationalVersion;

   CUI.Print($"Application compiled for: {CLRInfo.GetCLRVersionCompiledFor()}");
   CUI.Print($"Application running on: {CLRInfo.GetCLRVersionRunningOn()}");
   CUI.Print("EF Core version: " + assembly.GetName().Version.ToString() + "/" + informalVersion);

   ShadowState.ColumnsAddedAfterCompilation();


   // Test connection to database and generate data if necessary
   if (DemoUtil.TestConnection() != "") End();
   //EFC_WWWingsV1_Reverse.DataGenerator.Run(true);
   DataGenerator.Run(false);



   //FirstLevelCache.ClearCache();

   // Connect to EFProfiler 
   HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();


   CUI.Print("-------------------- START ------------- ", ConsoleColor.Blue, ConsoleColor.White);
   //QueriesDynamic.ExpressionTreeTwoConditions();
   End();

   #region Samples from the book"
   // ------------ Read data mit LINQ 
   SimpleQueries.LINQ_List();
   SimpleQueries.LINQ_QueryWithPaging();
   SimpleQueries.LINQ_RepositoryPattern();
   SimpleQueries.Projection_Read();
   SimpleQueries.LINQ_SingleOrDefault();
   SimpleQueries.LINQ_Find();
   Queries.GroupBy();
   Queries.LINQ_RAMQueries();


   // ------------ Object relationships and loading strategies
   LoadingStrategies.Demo_LazyLoading(); // 1 SELECT, no Details :-(
   LoadingStrategies.Demo_EagerLoading(); // 4 SELECT
   LoadingStrategies.Demo_ExplizitLoading(); // many SELECT :-(
   LoadingStrategies.Demo_PreLoading(); // 5 SELECT
   LoadingStrategies.Demo_PreLoadingPilotsCaching();

   // ------------ CUD 
   Conflicts.ChangeFlightOneProperty();
   Updates.ChangeFlightAndPilot();
   Updates.AddFlight();
   Updates.RemoveFlight();
   Updates.RemoveFlightWithKey();
   RelationChange.Demo_CreateRelatedObjects();
   Transactions.ExplicitTransactionTwoContextInstances();
   ChangeTrackerDemos.ChangeTracking_OneObject();
   ChangeTrackerDemos.ChangeTracking_MultipleObjects();
   ContradictoryRelationships.Demo_ContradictoryRelationships();

   // ----------------- Data conflicts
   Conflicts.ConflictWhileChangingFlight();
   ReadLock.UpdateWithReadLock();

   // ----------------- async
   Updates.Demo_ForeachProblem();
   AsyncOperations.ReadDataAsync();
   AsyncOperations.ChangeDataAsync();
   AsyncOperations.AsyncForeach();
   AsyncOperations.LINQ_MiscAsync();

   // ---------- Dynamic LINQ
   QueriesDynamic.LINQComposition();
   QueriesDynamic.ExpressionTreeTwoConditions();
   QueriesDynamic.ExpressionTreeNumerousConditions();
   QueriesDynamic.DynamicLINQNumerousCondition();

   // ----------------- SQL, SP, TVF 
   SQLSPTVF.Demo_SQLDirect1();
   SQLSPTVF.Demo_SQLDirect2();
   SQLSPTVF.Demo_SQLDirect3();
   SQLSPTVF.Demo_SQLDirect4();
   SQLSPTVF.Demo_SQLDirectAndLINQComposition();
   SQLSPTVF.Demo_SP();
   SQLSPTVF.Demo_TVF();
   SQLSPTVF.Demo_SQLDirect_Projection();

   // ------------------- Performance / Tracking
   TrackingModes.TrackingMode_AsNoTracking();
   TrackingModes.TrackingMode_NoTracking_Attach();
   TrackingModes.TrackingMode_QueryTrackingBehavior();
   TrackingModes.TrackingMode_Performance();

   // ------------------- Tips
   EFC2_GlobalFilters.GlobalFilter();
   ShadowState.ReadAndChangeShadowProperty();
   CalculatedColumns.ComputedColumnWithFormula();
   CalculatedColumns.DefaultValues();

   // ------------------- Bulk operations
   BulkOperations.BulkDelete_Prepare();
   BulkOperations.BulkDeleteEFCAPIwithoutBatching();
   BulkOperations.BulkDelete_Prepare();
   BulkOperations.BulkDeleteEFCAPIwithBatching();
   BulkOperations.BulkDelete_Prepare();
   BulkOperations.BulkUpdateEFPlus();

   // ------------------- Performance / Caching
   Caching.Demo_MemoryCache();
   Caching.Demo_CacheManager();
   Caching.Demo_SecondLevelCache();

   // ------------------- Performance / Beziehungszuweisung
   ForeignKeyAssociations.ChangePilotUsingFK();
   ForeignKeyAssociations.ChangePilotUsingObjectAssignment();
   ForeignKeyAssociations.ChangePilotUsingFKShadowProperty();

   // ------------------- OOM / Automapper
   ReflectionMapping.Run();
   AutoMapper_HelloWorld.RunSimpleDemo();
   AutoMapperBasics.Demo_SingleObject();
   AutoMapperBasics.Demo_ListMapping();
   AutoMapperAdvanced.Inheritance();
   AutoMapperAdvanced.GenericDemo();
   AutoMapperAdvanced.GenericHomogeneousList();
   AutoMapperAdvanced.BeforeAfterDemo();
   AutoMapperAdvanced.GenericHeterogeneousList();

   #endregion
   End();

   #region ++++++++++++++++++ Additional samples, not yet in the book

   Timestamps.ShowUpdatedTimeStamp();
   CachingRelFixup.Demo_RelFixup();
   CachingRelFixup.Demo_FirstLevelCaching();
   ContextEvents.EreignisFolge();
   Threading.EF_MT();

   Queries.Demo_Projection_OneFlight();
   Queries.GeneratedSQL();
   RelationshipFixup.Demo1_PilotBleibtImRAM();
   RelationshipFixup.Demo2_RueckwartigeBeziehung();
   DeleteCascading.EinfachesUpdate();
   FieldMapping.UsePrivateField();

   Updates.EF_ChangeTracking();
   Transactions.TransactionScopeDemo();
   Graph.TrackGraph();
   RelationChange.Demo_FlightAddAndRead();
   Queries.GroupBy_SQL_NonEntityType();
   FirstLevelCache.LocalClear();

   Updates.AddBatch();
   Updates.Batching_Change10Flights();
   ChangeTrackerDemos.EF_ChangeTrackerAuswerten();
   LoadingStrategies.Demo_PreLoading1();
   LoadingStrategies.Demo_EagerLoadingPilotDetails();
   LoadingStrategies.Demo_ExplizitLoadingCustom();
   #endregion
  }


  /// <summary>
  /// Let main program warit for key and than exit
  /// </summary>
  public static void End()
  {
   Console.WriteLine("");
   CUI.Print("!!!!!!!!!!!!!!!!!!! Main program is waiting for keypress !!!!!!!!!!!!!!!!", ConsoleColor.Blue, ConsoleColor.White);
   Console.ReadLine();
   Environment.Exit(0);
  }
 }
}
