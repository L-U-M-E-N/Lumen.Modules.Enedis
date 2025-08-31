using Lumen.Modules.Enedis.Common.Models;

using Microsoft.EntityFrameworkCore;

namespace Lumen.Modules.Enedis.Data {
    public class EnedisContext : DbContext {
        public const string SCHEMA_NAME = "Enedis";

        public EnedisContext(DbContextOptions<EnedisContext> options) : base(options) {
        }

        public DbSet<EnedisPointInTime> Enedis { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasDefaultSchema(SCHEMA_NAME);

            var EnedisModelBuilder = modelBuilder.Entity<EnedisPointInTime>();
            EnedisModelBuilder.HasKey(x => x.DateDebut);
        }
    }
}
