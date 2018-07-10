using CacheManager.Core;
using EFSecondLevelCache.Core;
using EFSecondLevelCache.Core.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace DA
{

 /// <summary>
 /// Konfiguration für Second-Level-Cache
 /// </summary>
 public static class ConfigureServices
 {
  private static readonly Lazy<IServiceProvider> _serviceProviderBuilder =
      new Lazy<IServiceProvider>(getServiceProvider, LazyThreadSafetyMode.ExecutionAndPublication);

  /// <summary>
  /// lazy loaded thread-safe singleton
  /// </summary>
  public static IServiceProvider Instance { get; } = _serviceProviderBuilder.Value;

  public static IEFCacheServiceProvider GetEFCacheServiceProvider()
  {
   return Instance.GetService<IEFCacheServiceProvider>();
  }

  private static IServiceProvider getServiceProvider()
  {
   var services = new ServiceCollection();

   services.AddEntityFrameworkSqlServer()
           .AddDbContext<WWWingsContext>(ServiceLifetime.Scoped);

   services.AddEFSecondLevelCache();

   services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
   services.AddSingleton(typeof(ICacheManagerConfiguration),
       new CacheManager.Core.ConfigurationBuilder()
               .WithJsonSerializer()
               .WithMicrosoftMemoryCacheHandle()
               .WithExpiration(ExpirationMode.Absolute, TimeSpan.FromSeconds(5))
               .DisablePerformanceCounters()
               .DisableStatistics()
               .Build());

   var serviceProvider = services.BuildServiceProvider();
   EFServiceProvider.ApplicationServices = serviceProvider; // app.UseEFSecondLevelCache();

   return serviceProvider;
  }
 }
}