using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace VisionBoard.Models
{
    public partial class GoalTrackerContext : DbContext
    {
        public GoalTrackerContext(DbContextOptions<GoalTrackerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Measurement> Measurements { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<GoalTags> GoalTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json").Build();
                string connectionString = builder.GetSection("ConnectionStrings").GetSection("GoalTrackerConnection").Value.ToString();
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.EndingOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PictureUrl).HasMaxLength(2000);

                entity.Property(e => e.StartOn).HasColumnType("datetime");

                entity.HasOne(d => d.Measurement)
                    .WithOne(p => p.Goal)
                    .HasForeignKey<Measurement>(d => d.Id)
                    .HasConstraintName("FK_Goals_Mesurement");

                entity.HasOne(d => d.Reward)
                    .WithOne(p => p.Goal)
                    .HasForeignKey<Reward>(d => d.Id)
                    .HasConstraintName("FK_Goals_Reward");
            });

            modelBuilder.Entity<Measurement>(entity =>
            {
                entity.ToTable("Mesurement");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Goal)
                    .WithOne(p => p.Measurement)
                    .HasForeignKey<Goal>(d => d.MeasurementId)
                    .HasConstraintName("FK_Goals_Mesurement");

            });

            modelBuilder.Entity<Reward>(entity =>
            {
                entity.ToTable("Reward");

                entity.Property(e => e.Descrption)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PictureUrl)
                    .HasMaxLength(2000)
                    .IsFixedLength(true);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Goal)
                    .WithOne(p => p.Reward)
                    .HasForeignKey<Goal>(d => d.RewardId)
                    .HasConstraintName("FK_Goals_Reward");
            });

            modelBuilder.Entity<Step>(entity =>
            {
                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.Steps)
                    .HasForeignKey(d => d.GoalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Steps__GoalId__32E0915F");
            });

            modelBuilder.Entity<GoalTags>(entity =>
            {
                entity.HasKey(e => new { e.GoalId, e.TagId })
                    .HasName("PK_GT");

                entity.ToTable("GoalTags");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.GoalTags)
                    .HasForeignKey(d => d.GoalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Goal");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.GoalTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tag");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.Property(e => e.Colour)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
