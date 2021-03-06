﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using WaveAccess.Data.Entity.Hint;
using WaveAccess.Data.Entity.Test.Module1.Models;
using WaveAccess.Data.Entity.Test.Module2.Models;

namespace WaveAccess.Data.Entity.Test
{
    public class GenericContext : DbContext
    {
        static GenericContext()
        {
            Database.SetInitializer<GenericContext>(new NullDatabaseInitializer<GenericContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(typeof(User).Assembly);
            modelBuilder.Configurations.AddFromAssembly(typeof(Task).Assembly);
        }
    }
}
