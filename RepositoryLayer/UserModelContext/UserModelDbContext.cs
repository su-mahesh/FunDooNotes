using System;
using System.Collections.Generic;
using System.Text;
using CommonLayer.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace RepositoryLayer.UserModelContext
{
    public class UserModelDbContext : DbContext
    {

        public UserModelDbContext(DbContextOptions<UserModelDbContext> options)
        : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().HasIndex(u => u.Email).IsUnique();
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
