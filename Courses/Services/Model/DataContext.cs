using Entitis.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses
{
    public class DataContext : IdentityDbContext<Users>
    {
        public DataContext(DbContextOptions<DataContext> options) 
            : base(options)
        {

        }

        public DbSet<Users> User { get; set; }
        public DbSet<Application_intro> Application_intros { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<Course_specialty> Course_specialties { get; set; }
        public DbSet<Entitis.Models.Courses> Courses { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Faqs> Faqs { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Instructors> Instructors { get; set; }
        public DbSet<Soical> Soicals { get; set; }
        public DbSet<NewsLetter> NewsLetters { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Login_attempts> login_Attempts { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application_intro>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Blogs>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Comments>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Course_specialty>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Entitis.Models.Courses>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Events>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Faqs>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Gallery>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Instructors>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Soical>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<NewsLetter>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Setting>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Booking>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Notifications>()
                .Property(x => x.Id).HasDefaultValueSql("NEWID()");
        }
    }
}
