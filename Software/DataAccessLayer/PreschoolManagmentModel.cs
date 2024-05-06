using EntityLayer.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataAccessLayer
{
    public partial class PreschoolManagmentModel : DbContext
    {
        public PreschoolManagmentModel()
            : base("name=PreschoolManagmentModel")
        {
        }

        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<Child> Children { get; set; }
        public virtual DbSet<DailyActivity> DailyActivities { get; set; }
        public virtual DbSet<Day> Days { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<PreeschoolYear> PreeschoolYears { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<WeeklySchedule> WeeklySchedules { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Child>()
                .Property(e => e.PIN)
                .IsUnicode(false);

            modelBuilder.Entity<Child>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.Sex)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.Adress)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.Nationality)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.DevelopmentStatus)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.MedicalInformation)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .Property(e => e.BirthPlace)
                .IsFixedLength();

            modelBuilder.Entity<Child>()
                .HasMany(e => e.Attendances)
                .WithOptional(e => e.Child)
                .HasForeignKey(e => e.Id_Child);

            modelBuilder.Entity<Child>()
                .HasMany(e => e.Notes)
                .WithOptional(e => e.Child)
                .HasForeignKey(e => e.Id_child);

            modelBuilder.Entity<Child>()
                .HasMany(e => e.Parents)
                .WithMany(e => e.Children)
                .Map(m => m.ToTable("Parents_Children").MapLeftKey("Id_Children").MapRightKey("Id_Parents"));

            modelBuilder.Entity<DailyActivity>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DailyActivity>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<DailyActivity>()
                .Property(e => e.StartTime)
                .IsUnicode(false);

            modelBuilder.Entity<DailyActivity>()
                .Property(e => e.EndTime)
                .IsUnicode(false);

            modelBuilder.Entity<DailyActivity>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<DailyActivity>()
                .HasMany(e => e.Resources)
                .WithMany(e => e.DailyActivities)
                .Map(m => m.ToTable("Resources_DailyActivities").MapLeftKey("Id_DailyActivity").MapRightKey("Id_Resource"));

            modelBuilder.Entity<Day>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Day>()
                .HasMany(e => e.DailyActivities)
                .WithMany(e => e.Days)
                .Map(m => m.ToTable("DailyActivities_Days").MapLeftKey("Id_Day").MapRightKey("Id_DailyActivity"));

            modelBuilder.Entity<Day>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Days)
                .Map(m => m.ToTable("Days_Users").MapLeftKey("Id_Day").MapRightKey("Id_User"));

            modelBuilder.Entity<Group>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Group>()
                .Property(e => e.Age)
                .IsUnicode(false);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Children)
                .WithOptional(e => e.Group)
                .HasForeignKey(e => e.Id_Group);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Group)
                .HasForeignKey(e => e.Id_Group);

            modelBuilder.Entity<Group>()
                .HasMany(e => e.PreeschoolYears)
                .WithMany(e => e.Groups)
                .Map(m => m.ToTable("Groups_PreeschoolYears").MapLeftKey("Id_Group").MapRightKey("Id_PreeschoolYear"));

            modelBuilder.Entity<Note>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Note>()
                .Property(e => e.Behaviour)
                .IsFixedLength();

            modelBuilder.Entity<Parent>()
                .Property(e => e.PIN)
                .IsUnicode(false);

            modelBuilder.Entity<Parent>()
                .Property(e => e.FirstName)
                .IsFixedLength();

            modelBuilder.Entity<Parent>()
                .Property(e => e.LastName)
                .IsFixedLength();

            modelBuilder.Entity<Parent>()
                .Property(e => e.Sex)
                .IsFixedLength();

            modelBuilder.Entity<Parent>()
                .Property(e => e.Email)
                .IsFixedLength();

            modelBuilder.Entity<Parent>()
                .Property(e => e.Telephone)
                .IsFixedLength();

            modelBuilder.Entity<PreeschoolYear>()
                .Property(e => e.Year)
                .IsUnicode(false);

            modelBuilder.Entity<Resource>()
                .Property(e => e.Name)
                .IsFixedLength();

            modelBuilder.Entity<Resource>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<Role>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithOptional(e => e.Role)
                .HasForeignKey(e => e.Id_role);

            modelBuilder.Entity<User>()
                .Property(e => e.PIN)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Sex)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Telephone)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Username)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Attendances)
                .WithOptional(e => e.User)
                .HasForeignKey(e => e.Id_User);

            modelBuilder.Entity<WeeklySchedule>()
                .HasMany(e => e.Days)
                .WithOptional(e => e.WeeklySchedule)
                .HasForeignKey(e => e.Id_WeeklySchedule);
        }
    }
}
