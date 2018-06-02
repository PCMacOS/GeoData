namespace GeoData.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Country : DbContext
    {
        public Country()
            : base("name=Country")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

        public virtual DbSet<Data> CountryDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);
        }
    }

    public class Data
    {
        public string DataId { get; set; }
        public string ImageFlag { get; set; }
        public string Capital { get; set; }
        public string CoordinatesLant { get; set; }
        public string CoordinatesLont { get; set; }
        public string AreaKm2 { get; set; }
        public string PopulationEstimate { get; set; }
        public string GdpNominalPerCapita { get; set; }
        public string Gini { get; set; }
        public string Hdi { get; set; }
        public string Title { get; set; }

        //public string UserId { get; set; }
        //public virtual AspNetUser User { get; set; }
    }
}
