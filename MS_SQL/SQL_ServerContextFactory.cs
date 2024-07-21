using Classes.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MS_SQL
{
    public class SQL_ServerContextFactory : IDesignTimeDbContextFactory<datacontext>
    {
        private const string DbContextString = "Server=localhost,1433;Database=Library;User ID=sa;Password=<YourStrong@Passw0rd>;MultipleActiveResultSets=true;TrustServerCertificate=True";
        public datacontext CreateDbContext(string[] args)
        {
            var optionBuilder = new DbContextOptionsBuilder<datacontext>();

            optionBuilder.UseSqlServer(DbContextString, b => b.MigrationsAssembly(typeof(SQL_ServerContextFactory).Namespace));

            return new datacontext(optionBuilder.Options);

        }
    }
}
