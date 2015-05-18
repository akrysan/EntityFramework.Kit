using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Migrations;

namespace WaveAccess.Data.Entity
{
    public class MigrateDatabaseWithSpecialSeed<TContext, TMigrationsConfiguration> : MigrateDatabaseToLatestVersion<TContext, TMigrationsConfiguration>
        where TContext : DbContext
        where TMigrationsConfiguration : DbMigrationsConfiguration<TContext>, new()
    {
        private readonly DbMigrationsConfiguration _config;
        private readonly bool _useSuppliedContext;

        public MigrateDatabaseWithSpecialSeed()
            : this(useSuppliedContext: false)
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
        public MigrateDatabaseWithSpecialSeed(bool useSuppliedContext)
            : this(useSuppliedContext, new TMigrationsConfiguration())
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
        public MigrateDatabaseWithSpecialSeed(bool useSuppliedContext, TMigrationsConfiguration configuration):base(useSuppliedContext, configuration)
        {
            _useSuppliedContext = useSuppliedContext;
            _config = configuration;
        }


        public override void InitializeDatabase(TContext context)
        {
            var migrator = new SpecialSeedMigrator(_config);
            migrator.Update();
        }
    }
}
