using Hx.Sdk.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace Hx.Sdk.Test.Entity
{
    [AppDbContext("MySqlConnectionString", DbProvider.MySqlOfficial)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}