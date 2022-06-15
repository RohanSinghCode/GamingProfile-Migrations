namespace GamingProfileMigrations
{
    using FluentMigrator.Runner;
    using FluentMigrator.Runner.Conventions;
    using FluentMigrator.Runner.Processors;
    using GamingProfile_Migrations.Entites;
    using System.Reflection;

    class Program
    {
        private static void Main(string[] args)
        {
            var down = args[0].ToLower().Trim();
            var workingDirectory = args[1].ToLower().Trim();
            var appSettings = GetAppsettings("development", workingDirectory);
            Console.WriteLine("Migration for Gaming Profile is starting");
            var isMigrateDown = bool.Parse(down);
            var failedMigrations = ExecuteMigration(isMigrateDown, appSettings, workingDirectory);
            if (failedMigrations != null)
            {
                Console.WriteLine($"Error while migrating {failedMigrations.Exception}");
            }
        }

        private static Appsettings GetAppsettings(string environmentName,string workingDirectory)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(workingDirectory)
            .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appSettings.{environmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

            var configuration = builder.Build();
            var appsettings = new Appsettings();
            configuration.GetSection("AppSettings").Bind(appsettings);
            return appsettings;

        }

        static dynamic ExecuteMigration(bool isDown, Appsettings appsettings, string workingDirectory)
        {
            try
            {
                var dllPath = workingDirectory + "bin\\release\\net6.0\\GamingProfile-Migrations.dll";
                var assembly = Assembly.LoadFrom(dllPath);
                var conventionSet = new DefaultConventionSet(defaultSchemaName: null, workingDirectory: workingDirectory);
                var serviceProvider = new ServiceCollection()
                    .AddLogging(lb => lb.AddFluentMigratorConsole())
                    .AddFluentMigratorCore()
                    .ConfigureRunner(
                        fluentMigrationBuilder => fluentMigrationBuilder
                            // Use Sql Server
                            .AddSqlServer()
                            // The connection string
                            .WithGlobalConnectionString(appsettings.ConnectionString)
                            // Specify the assembly with the migrations
                            .WithMigrationsIn(assembly))
                    .Configure<FluentMigratorLoggerOptions>(cfg => {
                        cfg.ShowSql = true;
                    })
                    .Configure<ProcessorOptions>(cfg => {
                        cfg.Timeout = TimeSpan.FromSeconds(100);
                    })
                    .AddSingleton<IConventionSet>(conventionSet)
                    .BuildServiceProvider(false);

                using (var scope = serviceProvider.CreateScope())
                {
                    var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
                    if (isDown)
                    {
                        migrationRunner.MigrateDown(1);
                    }
                    else
                    {
                        migrationRunner.MigrateUp();
                    }
                }
            } catch (Exception ex)
            {
                return new {Exception = ex};
            }

            return null;
        }
    }
}