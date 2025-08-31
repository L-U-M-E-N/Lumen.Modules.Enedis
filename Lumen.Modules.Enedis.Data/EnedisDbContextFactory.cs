using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Lumen.Modules.Enedis.Data {
    public class EnedisDbContextFactory : IDesignTimeDbContextFactory<EnedisContext> {
        public EnedisContext CreateDbContext(string[] args) {
            var optionsBuilder = new DbContextOptionsBuilder<EnedisContext>();
            optionsBuilder.UseNpgsql();

            return new EnedisContext(optionsBuilder.Options);
        }
    }
}
