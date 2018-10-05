using System;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Utilize.Identity.Provider.Models;


namespace Utilize.Identity.Provider.DataSources
{
    public class AuthDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Licence> Licences { get; set; }

        public DbSet<PermissionScheme> PermissionSchemes { get; set; }

        public AuthDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            NamesToSnakeCase(builder);

            base.OnModelCreating(builder);


            builder.Entity<PermissionScheme>().HasData(new PermissionScheme()
            {
                Id = Helpers.Hasher.GetHash("Default Permission Scheme"),
                Name = "Default Permission Scheme",
                Tenant = null
            });
        }

        private static void NamesToSnakeCase(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Replace table names
                entity.Relational().TableName = ToSnakeCase(entity.Relational().TableName);

                // Replace column names            
                foreach (var property in entity.GetProperties())
                {
                    property.Relational().ColumnName = ToSnakeCase(property.Name);
                }

                foreach (var key in entity.GetKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var key in entity.GetForeignKeys())
                {
                    key.Relational().Name = ToSnakeCase(key.Relational().Name);
                }

                foreach (var index in entity.GetIndexes())
                {
                    index.Relational().Name = ToSnakeCase(index.Relational().Name);
                }
            }
        }

        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }
            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}