using Abacuza.Services.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Abacuza.Services.Identity.Data
{
    public class ApplicationDbContext : IdentityDbContext<AbacuzaAppUser, AbacuzaAppRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public virtual DbSet<AbacuzaAppGroup> Groups { get; set; }
        
        public virtual DbSet<AbacuzaAppUserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            // Defines AppGroup entity
            builder.Entity<AbacuzaAppGroup>()
                .ToTable("AspNetGroups")
                .HasKey(x => x.Id);
            builder.Entity<AbacuzaAppGroup>()
                .HasIndex(x => x.Name).HasDatabaseName("GroupNameIndex").IsUnique(true);
            builder.Entity<AbacuzaAppGroup>()
                .Property(x => x.Name).IsUnicode().HasMaxLength(64);
            builder.Entity<AbacuzaAppGroup>()
                .Property(x => x.Description).IsUnicode().HasMaxLength(255);
            
            // Extends AppUser entity
            builder.Entity<AbacuzaAppUser>()
                .HasMany<AbacuzaAppUserGroup>()
                .WithOne()
                .HasForeignKey(ug => ug.UserId).IsRequired(true);

            // Extends AppGroup Entity
            builder.Entity<AbacuzaAppGroup>()
                .HasMany<AbacuzaAppUserGroup>()
                .WithOne()
                .HasForeignKey(ug => ug.GroupId).IsRequired(true);
            
            // Defines UserGroup Entity
            builder.Entity<AbacuzaAppUserGroup>()
                .ToTable("AspNetUserGroups")
                .HasKey(g => new
                {
                    g.UserId,
                    g.GroupId
                });
        }
    }
}
