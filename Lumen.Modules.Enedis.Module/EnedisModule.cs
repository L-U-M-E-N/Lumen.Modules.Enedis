using Lumen.Modules.Enedis.Business.Implementations;
using Lumen.Modules.Enedis.Business.Interfaces;
using Lumen.Modules.Enedis.Data;
using Lumen.Modules.Sdk;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.Enedis.Module {
    public class EnedisModule(IEnumerable<ConfigEntry> configEntries, ILogger<LumenModuleBase> logger, IServiceProvider provider) : LumenModuleBase(configEntries, logger, provider) {
        public const string PERSON_NUMBER = nameof(PERSON_NUMBER);
        public const string PRM_NUMBER = nameof(PRM_NUMBER);

        public static string PersonNumber = null!;
        public static string PrmNumber = null!;

        private string GetPersonNumberFromConfig() {
            var configEntry = configEntries.FirstOrDefault(x => x.ConfigKey == PERSON_NUMBER);
            if (configEntry is null || configEntry.ConfigValue is null) {
                logger.LogError($"[{nameof(PERSON_NUMBER)}] Config key \"{PERSON_NUMBER}\" is missing!");
            }

            return configEntry.ConfigValue;
        }

        private string GetPrmNumberFromConfig() {
            var configEntry = configEntries.FirstOrDefault(x => x.ConfigKey == PRM_NUMBER);
            if (configEntry is null || configEntry.ConfigValue is null) {
                logger.LogError($"[{nameof(PRM_NUMBER)}] Config key \"{PRM_NUMBER}\" is missing!");
            }

            return configEntry.ConfigValue;
        }

        public override Task InitAsync(LumenModuleRunsOnFlag currentEnv) {
            PersonNumber = GetPersonNumberFromConfig();
            PrmNumber = GetPrmNumberFromConfig();

            return Task.CompletedTask;
        }

        public override async Task RunAsync(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            try {
                logger.LogTrace($"[{nameof(EnedisModule)}] Running tasks ...");
                throw new NotImplementedException();
            } catch (Exception ex) {
                logger.LogError(ex, $"[{nameof(EnedisModule)}] Error when running tasks.");
            }
        }

        public override bool ShouldRunNow(LumenModuleRunsOnFlag currentEnv, DateTime date) {
            return false;
        }

        public override Task ShutdownAsync() {
            // Nothing to do
            return Task.CompletedTask;
        }

        public static new void SetupServices(LumenModuleRunsOnFlag currentEnv, IServiceCollection serviceCollection, string? postgresConnectionString) {
            if (currentEnv == LumenModuleRunsOnFlag.API) {
                serviceCollection.AddDbContext<EnedisContext>(o => o.UseNpgsql(postgresConnectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", EnedisContext.SCHEMA_NAME)));
                serviceCollection.AddTransient<IEnedisApi, EnedisApi>();
                serviceCollection.AddHttpClient();
            }
        }

        public override Type GetDatabaseContextType() {
            return typeof(EnedisContext);
        }
    }
}
