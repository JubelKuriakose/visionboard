using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        public virtual DbSet<Mesurement> Mesurements { get; set; }
        public virtual DbSet<Reward> Rewards { get; set; }
        public virtual DbSet<Step> Steps { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

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

                entity.HasOne(d => d.Reward)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.RewardId)
                    .HasConstraintName("FK__Goals__RewardId__300424B4");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("FK__Goals__TagId__2F10007B");
            });

            modelBuilder.Entity<Mesurement>(entity =>
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
                    .WithMany(p => p.Mesurements)
                    .HasForeignKey(d => d.GoalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Mesuremen__GoalI__30F848ED");
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
                    .WithMany(p => p.Rewards)
                    .HasForeignKey(d => d.GoalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Reward__GoalId__31EC6D26");
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
