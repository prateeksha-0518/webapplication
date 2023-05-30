using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using code.Models;

namespace code.context
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext() : base("DefaultConnection") { }

        public DbSet<Student> Students { get; set; }

    }
}