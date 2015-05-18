using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WA.Data.Entity.Migrations;

namespace WA.Data.Entity
{
    public class SeedAfterMigarateOnly<TContext, TMigrationsConfiguration> : MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private readonly DbMigrationsConfiguration _config;
        private readonly bool _useSuppliedContext;

        public SeedAfterMigarateOnly()
            : base(useSuppliedContext: false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MigrateAndSeedDbIfSchemaIsOutdated class specifying whether to
        /// use the connection information from the context that triggered initialization to perform the migration.
        /// </summary>
        /// <param name="useSuppliedContext">
        /// If set to <c>true</c> the initializer is run using the connection information from the context that 
        /// triggered initialization. Otherwise, the connection information will be taken from a context constructed 
        /// using the default constructor or registered factory if applicable. 
        /// </param>
        public SeedAfterMigarateOnly(bool useSuppliedContext)
            : base(useSuppliedContext, new TMigrationsConfiguration())
        {
        }

        /// <summary>
        /// Initializes a new instance of the MigrateAndSeedDbIfSchemaIsOutdated class specifying whether to
        /// use the connection information from the context that triggered initialization to perform the migration.
        /// Also allows specifying migrations configuration to use during initialization.
        /// </summary>
        /// <param name="useSuppliedContext">
        /// If set to <c>true</c> the initializer is run using the connection information from the context that
        /// triggered initialization. Otherwise, the connection information will be taken from a context constructed
        /// using the default constructor or registered factory if applicable.
        /// </param>
        /// <param name="configuration"> Migrations configuration to use during initialization. </param>
        public SeedAfterMigarateOnly(bool useSuppliedContext, TMigrationsConfiguration configuration)
            : base(useSuppliedContext, configuration)
        {
            _useSuppliedContext = useSuppliedContext;
            _config = configuration;
        }

        /// <summary>
        /// Initializes a new instance of the MigrateAndSeedDbIfSchemaIsOutdated class that will
        /// use a specific connection string from the configuration file to connect to
        /// the database to perform the migration.
        /// </summary>
        /// <param name="connectionStringName"> The name of the connection string to use for migration. </param>
        public SeedAfterMigarateOnly(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public override void InitializeDatabase(TContext context)
        {
            var migrator = new MigratorSeedIfSchemaIsChanged(_config);
            migrator.Update();
        }
    }
}
