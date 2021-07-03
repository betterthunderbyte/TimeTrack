using System;
using Microsoft.EntityFrameworkCore;
using TimeTrack.Core.Model;

namespace TimeTrack.UseCase
{
    public class TimeTrackDbContext : DbContext, IDbContext
    {
        public TimeTrackDbContext() { }

        public TimeTrackDbContext(DbContextOptions<TimeTrackDbContext> options) : base(options) { }

        /// <summary>
        /// Die Datenbank vorbereiten sowie Standardwerte setzen
        /// </summary>
        public void Setup(bool connectToDatabase = false)
        {
            if (connectToDatabase)
            {
                Database.OpenConnection();
            }
            
            if (Database.EnsureCreated())
            {
                CreateDefaultData();
            }
        }

        public void CreateDefaultData()
        {
            ActivityTypes.Add(new ActivityTypeEntity() {Title = "Undefiniert"});
            Customers.Add(new CustomerEntity(){Name = "Undefiniert"});
            Projects.Add(new ProjectEntity(){Name = "Undefiniert"});
            
            var admin = new MemberEntity()
            {
                Mail = "admin@timetrack.dev",
                RenewPassword = true,
                MailConfirmed = true,
                GivenName = "Jon",
                Surname = "Doe",
                Role = MemberEntity.MemberRole.Admin,
                Active = true
            };
            admin.SetPassword("123asd!");
            Members.Add(admin);
            
            var moderator = new MemberEntity()
            {
                Mail = "moderator@timetrack.dev",
                RenewPassword = true,
                MailConfirmed = true,
                GivenName = "Jon",
                Surname = "Doe",
                Role = MemberEntity.MemberRole.Moderator
            };
            moderator.SetPassword("123asd!");
            Members.Add(moderator);
            
            var user = new MemberEntity()
            {
                Mail = "user@timetrack.dev",
                RenewPassword = true,
                MailConfirmed = true,
                GivenName = "Jon",
                Surname = "Doe",
                Role = MemberEntity.MemberRole.User
            };
            user.SetPassword("123asd!");
            Members.Add(user);
            
            SaveChanges();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("DataSource=:memory:");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<ProjectEntity>()
                .HasMany<ActivityEntity>(x => x.Activities)
                .WithOne(x => x.Project)
                .HasForeignKey(x => x.ProjectFk);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany<ActivityEntity>(x => x.Activities)
                .WithOne(x => x.Customer)
                .HasForeignKey(x => x.CustomerFk);

            modelBuilder.Entity<MemberEntity>()
                .HasMany<ActivityEntity>(x => x.Activities)
                .WithOne(x => x.Owner)
                .HasForeignKey(x => x.OwnerFk);
            
            modelBuilder.Entity<ActivityTypeEntity>()
                .HasMany<ActivityEntity>(x => x.Activities)
                .WithOne(x => x.ActivityType)
                .HasForeignKey(x => x.ActivityTypeFk);
        }

        public DbSet<ActivityEntity> Activities { get; set; }
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<ActivityTypeEntity> ActivityTypes { get; set; }

        public DbSet<MemberEntity> Members { get; set; }


    }
}
