using MySql.Data.Entity;
using DocumentCenter.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DocumentCenter.DB
{
    //[DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class DocumentDbContext : DbContext
    {
        public DocumentDbContext():base("Default")
        {

        }

        public DbSet<DocumentInfo> DocumentInfos { get; set; }
        public DbSet<DocumentHistory> DocumentHistories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FileUser> FileUsers { get; set; }
        public DbSet<PersonalFile> PersonalFiles { get; set; }
    }
}