using Quartz;

namespace OutOfSchool.WebApi.Extensions.Startup;

public static class QuartzExtension
{
    /// <summary>
    /// Adds default Quartz.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">App configuration.</param>
    /// <param name="quartzConnectionString">Connection string key for Quartz.</param>
    /// <param name="configureJobs">Expose Quartz Configurator to Configure Jobs.</param>
    /// <returns><see cref="IServiceCollection"/> instance.</returns>
    /// <exception cref="ArgumentNullException">Whenever the services collection is null.</exception>
    public static IServiceCollection AddDefaultQuartz(
        this IServiceCollection services,
        IConfiguration configuration,
        string quartzConnectionString = "QuartzConnection",
        Action<IServiceCollectionQuartzConfigurator> configureJobs = null)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.AddQuartz(q =>
        {
            q.SchedulerId = DefaultQuartzConfig.DefaultId;
            q.SchedulerName = DefaultQuartzConfig.DefaultName;

            q.UseMicrosoftDependencyInjectionJobFactory();
            q.UseTimeZoneConverter();

            configureJobs?.Invoke(q);
        });

        services.AddQuartzServer(options => { options.WaitForJobsToComplete = true; });

        return services;
    }
}