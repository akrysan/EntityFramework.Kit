﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaveAccess.Data.Entity.Test.Migrations;


namespace WaveAccess.Data.Entity.Test.Models {
    public class SimpleContext:DbContext {
        static SimpleContext() {
            Database.SetInitializer<SimpleContext>(new MigrateDatabaseToLatestVersion<SimpleContext, Configuration>());            
        }
        public DbSet<SimpleEntity> SimpleEntities { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
        }
    }
}
